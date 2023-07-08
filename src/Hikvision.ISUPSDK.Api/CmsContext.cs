using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Hikvision.ISUPSDK.Defines;
using static Hikvision.ISUPSDK.Methods;

namespace Hikvision.ISUPSDK.Api
{
    public class CmsContext
    {
        private CmsContextOptions options;
        private int listenHandle;
        private Dictionary<int, DeviceContext> deviceDict = new Dictionary<int, DeviceContext>();
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
            //设置访问安全
            var m_struAccessSecure = new NET_EHOME_LOCAL_ACCESS_SECURITY();
            m_struAccessSecure.dwSize = Marshal.SizeOf(m_struAccessSecure);
            m_struAccessSecure.byAccessSecurity = (byte)options.AccessSecurity;
            IntPtr ptrAccessSecure = Marshal.AllocHGlobal((int)m_struAccessSecure.dwSize);
            try
            {
                Marshal.StructureToPtr(m_struAccessSecure, ptrAccessSecure, false);
                Invoke(NET_ECMS_SetSDKLocalCfg(NET_EHOME_LOCAL_CFG_TYPE.ACTIVE_ACCESS_SECURITY, ptrAccessSecure));
            }
            catch
            {
                Marshal.FreeHGlobal(ptrAccessSecure);
                throw;
            }
            //开始监听
            var cmd_listen_param = new NET_EHOME_CMS_LISTEN_PARAM();
            cmd_listen_param.struAddress.Init();
            options.Encoding.GetBytes(options.ListenIPAddress, 0, options.ListenIPAddress.Length, cmd_listen_param.struAddress.szIP, 0);
            cmd_listen_param.struAddress.wPort = Convert.ToInt16(options.ListenPort);
            cmd_listen_param.fnCB = onDEVICE_REGISTER_CB;
            cmd_listen_param.byRes = new byte[32];
            listenHandle = Invoke(NET_ECMS_StartListen(ref cmd_listen_param));
        }

        public void Stop()
        {
            NET_ECMS_StopListen(listenHandle);
        }

        public event EventHandler<DeviceContext> DeviceOnline;
        public event EventHandler<DeviceContext> DeviceOffline;

        private bool onDEVICE_REGISTER_CB(int iUserID, int dwDataType, IntPtr pOutBuffer, int dwOutLen,
                                                 IntPtr pInBuffer, int dwInLen, IntPtr pUser)
        {
            NET_EHOME_DEV_REG_INFO_V12 struDevInfo = new NET_EHOME_DEV_REG_INFO_V12();
            struDevInfo.Init();
            switch (dwDataType)
            {
                case ENUM_DEV_ON:
                case ENUM_DEV_AUTH:
                case ENUM_DEV_SESSIONKEY:
                case ENUM_DEV_ADDRESS_CHANGED:
                    if (pOutBuffer != IntPtr.Zero)
                        struDevInfo = (NET_EHOME_DEV_REG_INFO_V12)Marshal.PtrToStructure(pOutBuffer, typeof(NET_EHOME_DEV_REG_INFO_V12));
                    break;
            }

            //如果是设备上线回调
            if (ENUM_DEV_ON == dwDataType)
            {
                var device = new DeviceContext(options, iUserID, struDevInfo);
                //1秒后获取设备信息
                Task.Delay(1000).ContinueWith(t =>
                {
                    try
                    {
                        device.Load();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    lock (deviceDict)
                        deviceDict[device.LoginID] = device;
                    //通知设备上线
                    DeviceOnline?.Invoke(this, device);
                });
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
                DeviceContext deviceInfo = null;
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

            }
            return true;
        }
    }
}