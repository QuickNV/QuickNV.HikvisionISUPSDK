using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using static Hikvision.ISUPSDK.Defines;
using static Hikvision.ISUPSDK.Methods;

namespace Hikvision.ISUPSDK.Api
{
    public class CmsContext
    {
        private CmsContextOptions options;
        private int listenHandle;
        private Dictionary<int, DeviceInfo> deviceDict = new Dictionary<int, DeviceInfo>();
        public CmsContext(CmsContextOptions options)
        {
            this.options = options;
        }

        public static void Init()
        {
            INIT_NATIVE_FILES();
            Invoke(NET_ECMS_Init());
        }

        public void Start()
        {
            lock (deviceDict)
                deviceDict.Clear();
            var cmd_listen_param = new NET_EHOME_CMS_LISTEN_PARAM();
            cmd_listen_param.struAddress.Init();
            options.ListenIPAddress.CopyTo(0, cmd_listen_param.struAddress.szIP, 0, options.ListenIPAddress.Length);
            cmd_listen_param.struAddress.wPort = Convert.ToInt16(options.ListenPort);
            cmd_listen_param.fnCB = onDEVICE_REGISTER_CB;
            cmd_listen_param.byRes = new byte[32];
            listenHandle = Invoke(NET_ECMS_StartListen(ref cmd_listen_param));
        }

        public void Stop()
        {
            NET_ECMS_StopListen(listenHandle);
        }

        public event EventHandler<DeviceInfo> DeviceOnline;
        public event EventHandler<DeviceInfo> DeviceOffline;

        private string ByteArray2String(byte[] buffer)
        {
            return Encoding.Default.GetString(buffer).Trim(char.MinValue);
        }

        private bool onDEVICE_REGISTER_CB(int iUserID, int dwDataType, IntPtr pOutBuffer, uint dwOutLen,
                                                 IntPtr pInBuffer, uint dwInLen, IntPtr pUser)
        {
            NET_EHOME_DEV_REG_INFO_V12 struDevInfo = new NET_EHOME_DEV_REG_INFO_V12();
            struDevInfo.Init();
            if (pOutBuffer != IntPtr.Zero)
            {
                if (dwDataType == ENUM_DEV_ON || ENUM_DEV_AUTH == dwDataType || ENUM_DEV_SESSIONKEY == dwDataType || ENUM_DEV_ADDRESS_CHANGED == dwDataType)
                {
                    struDevInfo = (NET_EHOME_DEV_REG_INFO_V12)Marshal.PtrToStructure(pOutBuffer, typeof(NET_EHOME_DEV_REG_INFO_V12));
                }
            }
            else
            {
                Console.WriteLine("pOutBuffer is NULL");
            }

            //如果是设备上线回调
            if (ENUM_DEV_ON == dwDataType)
            {
                var deviceInfo = new DeviceInfo();
                deviceInfo.LoginID = iUserID;
                deviceInfo.Id = ByteArray2String(struDevInfo.struRegInfo.byDeviceID);
                deviceInfo.SessionKey = ByteArray2String(struDevInfo.struRegInfo.bySessionKey);
                deviceInfo.Serial = ByteArray2String(struDevInfo.struRegInfo.sDeviceSerial);
                deviceInfo.FirmwareVersion = ByteArray2String(struDevInfo.struRegInfo.byFirmwareVersion);
                deviceInfo.IPAddress = new string(struDevInfo.struRegInfo.struDevAdd.szIP).Trim(char.MinValue);
                deviceInfo.Port = struDevInfo.struRegInfo.struDevAdd.wPort;
                deviceInfo.Type = struDevInfo.struRegInfo.dwDevType;
                deviceInfo.ProtocolVersion = ByteArray2String(struDevInfo.struRegInfo.byDevProtocolVersion);
                lock (deviceDict)
                    deviceDict[deviceInfo.LoginID] = deviceInfo;
                DeviceOnline?.Invoke(this, deviceInfo);

                if (pInBuffer == IntPtr.Zero)
                {
                    return false;
                }

                //返回服务端信息
                NET_EHOME_SERVER_INFO_V50 struServInfo = new NET_EHOME_SERVER_INFO_V50();
                struServInfo.Init();
                struServInfo = (NET_EHOME_SERVER_INFO_V50)Marshal.PtrToStructure(pInBuffer, typeof(NET_EHOME_SERVER_INFO_V50));

                struServInfo.dwKeepAliveSec = 15;
                struServInfo.dwTimeOutCount = 6;
                struServInfo.dwNTPInterval = 3600;

                int TdwSize = Marshal.SizeOf(struServInfo);
                IntPtr ptrStruS = Marshal.AllocHGlobal(TdwSize);
                Marshal.StructureToPtr(struServInfo, ptrStruS, false);
                Marshal.StructureToPtr(struServInfo, pInBuffer, false);
                return true;
            }
            //如果是设备下线回调
            else if (ENUM_DEV_OFF == dwDataType)
            {
                DeviceInfo deviceInfo = null;
                lock (deviceDict)
                {
                    if (!deviceDict.ContainsKey(iUserID))
                        return true;
                    deviceInfo = deviceDict[iUserID];
                    deviceDict.Remove(iUserID);
                }
                DeviceOffline?.Invoke(this, deviceInfo);
                return false;
            }
            //如果是Ehome5.0设备认证回调
            else if (ENUM_DEV_AUTH == dwDataType)
            {

            }
            //如果是Ehome5.0设备Sessionkey回调
            else if (ENUM_DEV_SESSIONKEY == dwDataType)
            {
                NET_EHOME_DEV_SESSIONKEY devSessionkey = new NET_EHOME_DEV_SESSIONKEY();
                devSessionkey.Init();
                struDevInfo.struRegInfo.byDeviceID.CopyTo(devSessionkey.sDeviceID, 0);
                struDevInfo.struRegInfo.bySessionKey.CopyTo(devSessionkey.sSessionKey, 0);
                NET_ECMS_SetDeviceSessionKey(ref devSessionkey);
            }
            //如果是Ehome5.0设备重定向请求回调
            else if (ENUM_DEV_DAS_REQ == dwDataType)
            {
                string szLocalIP = "";
                //IntPtr ptrLocalIP = Marshal.AllocHGlobal(128);
                //ptrLocalIP=Marshal.StringToHGlobalAnsi(szLocalIP);
                //szLocalIP = SelectIP.IpAddressList[0].ToString();
                IntPtr ptrLocalIP = IntPtr.Zero;
                int dwPort = 0;
                //GetAddressByType(3, 0, ref ptrLocalIP, 128, ref dwPort, 4);
                szLocalIP = Marshal.PtrToStringAnsi(ptrLocalIP);
                //if (0 == dwPort)
                //{
                //    dwPort = m_nPort;
                //}
                string portTemp = dwPort.ToString();
                string strInBuffer = "{\"Type\":\"DAS\",\"DasInfo\":{\"Address\":\"" + szLocalIP + "\"," +
                "\"Domain\":\"test.ys7.com\",\"ServerID\":\"das_" + szLocalIP + "_" + portTemp + "\",\"Port\":" + portTemp + ",\"UdpPort\":" + portTemp + "}}";

                byte[] byTemp = System.Text.Encoding.Default.GetBytes(strInBuffer);
                Marshal.Copy(byTemp, 0, pInBuffer, byTemp.Length);

            }
            //如果是设备地址发生变化回调
            else if (ENUM_DEV_ADDRESS_CHANGED == dwDataType)
            {
                //Marshal.StructureToPtr(struTemp, ptrTemp, false);

                ////m_ConvertModel.UTF82A(struDevInfo.struRegInfo.byDeviceID, struDevInfo.struRegInfo.byDeviceID, MAX_DEVICE_ID_LEN, ref iOutLen);
                ////m_ConvertModel.UTF82A(struDevInfo.struRegInfo.sDeviceSerial, struDevInfo.struRegInfo.sDeviceSerial, NET_EHOME_SERIAL_LEN, ref iOutLen);

                //struDevInfo.struRegInfo.byDeviceID.CopyTo(struTemp.byDeviceID, 0);
                //struTemp.iLoginID = iUserID;
                //struDevInfo.struRegInfo.sDeviceSerial.CopyTo(struTemp.sDeviceSerial, 0);


                //byte[] szDeviceSerial = new byte[NET_EHOME_SERIAL_LEN + 1];
                //struDevInfo.struRegInfo.sDeviceSerial.CopyTo(szDeviceSerial, 0);
                //if (2 == struDevInfo.struRegInfo.byDevProtocolVersion[0])
                //{
                //    struTemp.dwVersion = 2;
                //}
                //else if (4 == struDevInfo.struRegInfo.byDevProtocolVersion[0])
                //{
                //    struTemp.dwVersion = 4;
                //}
                //else
                //{
                //    struTemp.dwVersion = 5;
                //}
            }
            return true;
        }
    }
}