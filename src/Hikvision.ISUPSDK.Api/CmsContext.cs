using Hikvision.ISUPSDK.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private DEVICE_REGISTER_CB fnDEVICE_REGISTER_CB;
        private Dictionary<int, DeviceContext> loginIdDeviceDict = new Dictionary<int, DeviceContext>();
        private Dictionary<string, DeviceContext> deviceDict = new Dictionary<string, DeviceContext>();
        public DeviceContext[] Devices { get; private set; } = new DeviceContext[0];
        public DeviceContext GetDevice(string deviceId)
        {
            lock (deviceDict)
            {
                if (deviceDict.TryGetValue(deviceId, out var deviceContext))
                    return deviceContext;
            }
            return null;
        }

        public CmsContext(CmsContextOptions options)
        {
            this.options = options;
            fnDEVICE_REGISTER_CB = new DEVICE_REGISTER_CB(onDEVICE_REGISTER_CB);
        }

        public static void Init()
        {
            SdkManager.Init();
            Invoke(NET_ECMS_Init());
        }

        public void Start()
        {
            lock (loginIdDeviceDict)
                loginIdDeviceDict.Clear();
            lock (deviceDict)
                deviceDict.Clear();
            Devices = new DeviceContext[0];

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
            StringUtils.String2ByteArray(options.ListenIPAddress, cmd_listen_param.struAddress.szIP);
            cmd_listen_param.struAddress.wPort = Convert.ToUInt16(options.ListenPort);
            cmd_listen_param.fnCB = fnDEVICE_REGISTER_CB;
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
                    catch
                    {
                        //如果读取设备信息出错，则强制退出登录
                        NET_ECMS_ForceLogout(iUserID);
                        return;
                    }
                    lock (loginIdDeviceDict)
                        loginIdDeviceDict[device.LoginID] = device;
                    lock (deviceDict)
                    {
                        deviceDict[device.Id] = device;
                        Devices = deviceDict.Values.ToArray();
                    }
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
            }
            //如果是设备下线回调
            else if (ENUM_DEV_OFF == dwDataType)
            {
                DeviceContext deviceInfo = null;
                lock (loginIdDeviceDict)
                {
                    if (!loginIdDeviceDict.TryGetValue(iUserID, out deviceInfo))
                        return true;
                    loginIdDeviceDict.Remove(iUserID);
                }
                lock (deviceDict)
                {
                    if (deviceDict.ContainsKey(deviceInfo.Id))
                    {
                        deviceDict.Remove(deviceInfo.Id);
                        Devices = deviceDict.Values.ToArray();
                    }
                }
                DeviceOffline?.Invoke(this, deviceInfo);
            }
            //如果是Ehome5.0设备认证回调
            else if (ENUM_DEV_AUTH == dwDataType)
            {
                var buffer = Encoding.ASCII.GetBytes(options.Password.PadRight(32, char.MinValue));
                Marshal.Copy(buffer, 0, pInBuffer, buffer.Length);
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
                var port = options.ServerPort;
                if (port == null)
                    port = options.ListenPort;
                var rep = new DeviceDasResponse()
                {
                    Type = "DAS",
                    DasInfo = new DeviceDasResponse_DasInfo()
                    {
                        Address = options.ServerHost,
                        Domain = "test.ys7.com",
                        ServerID = $"das_{options.ServerHost}_{port}",
                        Port = port.Value,
                        UdpPort = port.Value
                    }
                };
                var dasInfoString = System.Text.Json.JsonSerializer.Serialize(rep);
                var dasInfoBuffer = Encoding.Default.GetBytes(dasInfoString);
                Marshal.Copy(dasInfoBuffer, 0, pInBuffer, dasInfoBuffer.Length);
            }
            //如果是设备地址发生变化回调
            else if (ENUM_DEV_ADDRESS_CHANGED == dwDataType)
            {
            }
            return true;
        }

        private class DeviceDasResponse_DasInfo
        {
            public string Address { get; set; }
            public string Domain { get; set; }
            public string ServerID { get; set; }
            public int Port { get; set; }
            public int UdpPort { get; set; }
        }
        private class DeviceDasResponse
        {
            public string Type { get; set; }
            public DeviceDasResponse_DasInfo DasInfo { get; set; }
        }
    }
}