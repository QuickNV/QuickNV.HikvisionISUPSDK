﻿using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using static QuickNV.HikvisionISUPSDK.Defines;
using static QuickNV.HikvisionISUPSDK.Methods;

var smsServerIpAddrss = "127.0.0.1";
var smsListenIpAddress = "0.0.0.0";
var smsPassword = "123456";
ushort smsListenPort = 7660;

try
{
    INIT_NATIVE_DIR();
    Invoke(NET_ECMS_Init());
    Console.WriteLine("初始化成功！");

    DEVICE_REGISTER_CB device_register_cb = (iUserID, dwDataType, pOutBuffer, dwOutLen, pInBuffer, dwInLen, pUser) =>
    {
        int dwSize = 0;
        LOCAL_DEVICE_INFO struTemp = new LOCAL_DEVICE_INFO();
        struTemp.Init();
        dwSize = Marshal.SizeOf(typeof(LOCAL_DEVICE_INFO));
        IntPtr ptrTemp = Marshal.AllocHGlobal(dwSize);

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
            Console.WriteLine("设备上线！");
            Console.WriteLine("struDevInfo: " + JsonConvert.SerializeObject(struDevInfo, Formatting.Indented));

            if (pInBuffer == IntPtr.Zero)
            {
                return false;
            }

            struDevInfo.struRegInfo.byDeviceID.CopyTo(struTemp.byDeviceID, 0);
            struTemp.iLoginID = iUserID;
            struDevInfo.struRegInfo.sDeviceSerial.CopyTo(struTemp.sDeviceSerial, 0);


            byte[] szDeviceSerial = new byte[NET_EHOME_SERIAL_LEN + 1];
            struDevInfo.struRegInfo.sDeviceSerial.CopyTo(szDeviceSerial, 0);
            byte[] szSessionKey = new byte[MAX_MASTER_KEY_LEN + 1];
            struDevInfo.struRegInfo.bySessionKey.CopyTo(szSessionKey, 0);
            if (struDevInfo.struRegInfo.byDevProtocolVersion[0] == '2')
            {
                struTemp.dwVersion = 2;
            }
            else if ('4' == struDevInfo.struRegInfo.byDevProtocolVersion[0])
            {
                struTemp.dwVersion = 4;
            }
            else
            {
                struTemp.dwVersion = 5;
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
            Console.WriteLine("设备下线！");
            Marshal.StructureToPtr(iUserID, ptrTemp, false);
            return false;
        }
        //如果是Ehome5.0设备认证回调
        else if (ENUM_DEV_AUTH == dwDataType)
        {
            Console.WriteLine("Ehome5.0设备认证回调");
            var buffer = Encoding.ASCII.GetBytes(smsPassword.PadRight(32, char.MinValue));
            Marshal.Copy(buffer, 0, pInBuffer, buffer.Length);
            return true;
        }
        //如果是Ehome5.0设备Sessionkey回调
        else if (ENUM_DEV_SESSIONKEY == dwDataType)
        {
            Console.WriteLine("Ehome5.0设备Sessionkey回调");
            NET_EHOME_DEV_SESSIONKEY devSessionkey = new NET_EHOME_DEV_SESSIONKEY();
            devSessionkey.Init();
            struDevInfo.struRegInfo.byDeviceID.CopyTo(devSessionkey.sDeviceID, 0);
            struDevInfo.struRegInfo.bySessionKey.CopyTo(devSessionkey.sSessionKey, 0);
            NET_ECMS_SetDeviceSessionKey(ref devSessionkey);
        }
        //如果是Ehome5.0设备重定向请求回调
        else if (ENUM_DEV_DAS_REQ == dwDataType)
        {
            Console.WriteLine("Ehome5.0设备重定向请求回调");
            string strInBuffer = "{\"Type\":\"DAS\",\"DasInfo\":{\"Address\":\"" + smsServerIpAddrss + "\"," +
            "\"Domain\":\"test.ys7.com\",\"ServerID\":\"das_" + smsServerIpAddrss + "_" + smsListenPort + "\",\"Port\":" + smsListenPort + ",\"UdpPort\":" + smsListenPort + "}}";
            byte[] byTemp = Encoding.Default.GetBytes(strInBuffer);
            Marshal.Copy(byTemp, 0, pInBuffer, byTemp.Length);
        }
        //如果是设备地址发生变化回调
        else if (ENUM_DEV_ADDRESS_CHANGED == dwDataType)
        {
            Console.WriteLine("设备地址发生变化回调");
            Marshal.StructureToPtr(struTemp, ptrTemp, false);

            struDevInfo.struRegInfo.byDeviceID.CopyTo(struTemp.byDeviceID, 0);
            struTemp.iLoginID = iUserID;
            struDevInfo.struRegInfo.sDeviceSerial.CopyTo(struTemp.sDeviceSerial, 0);

            byte[] szDeviceSerial = new byte[NET_EHOME_SERIAL_LEN + 1];
            struDevInfo.struRegInfo.sDeviceSerial.CopyTo(szDeviceSerial, 0);
            if (2 == struDevInfo.struRegInfo.byDevProtocolVersion[0])
            {
                struTemp.dwVersion = 2;
            }
            else if (4 == struDevInfo.struRegInfo.byDevProtocolVersion[0])
            {
                struTemp.dwVersion = 4;
            }
            else
            {
                struTemp.dwVersion = 5;
            }
        }

        return true;
    };
    var cmd_listen_param = new NET_EHOME_CMS_LISTEN_PARAM();
    cmd_listen_param.struAddress.Init();
    Encoding.Default.GetBytes(smsListenIpAddress, 0, smsListenIpAddress.Length, cmd_listen_param.struAddress.szIP, 0);
    cmd_listen_param.struAddress.wPort = smsListenPort;
    cmd_listen_param.fnCB = device_register_cb;
    cmd_listen_param.byRes = new byte[32];

    Console.WriteLine($"正在监听：{smsListenIpAddress}:{smsListenPort}...");
    var listenHandle = Invoke(NET_ECMS_StartListen(ref cmd_listen_param));
    Console.WriteLine("开始监听！");

    Console.ReadLine();
    NET_ECMS_StopListen(listenHandle);
    Console.WriteLine("已停止监听");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}