﻿# QuickNV.HikvisionISUPSDK
* Avaliable as [nuget](https://www.nuget.org/packages/QuickNV.HikvisionISUPSDK/) 

* [![NuGet Downloads](https://img.shields.io/nuget/dt/QuickNV.HikvisionISUPSDK.svg)](https://www.nuget.org/packages/QuickNV.HikvisionISUPSDK/)

* QuickNV.HikvisionISUPSDK.

说明
```
SDK定义类：QuickNV.HikvisionISUPSDK.Defines
SDK方法类：QuickNV.HikvisionISUPSDK.Methods
SDK错误枚举：QuickNV.HikvisionISUPSDK.Errors
```

示例
```
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using static QuickNV.HikvisionISUPSDK.Defines;
using static QuickNV.HikvisionISUPSDK.Methods;

var listenIPAddress = "0.0.0.0";
short listenPort = 7660;

try
{
    INIT_NATIVE_FILES();
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

            //Message mes = new Message();
            //mes.Msg = ISUPDemo.WM_DEL_DEV;
            //mes.LParam = ptrTemp;
            //g_deviceTree.ProDevStatu(mes);

            //PostMessage(m_treeHandle, ISUPDemo.WM_DEL_DEV, ptrTemp, ptrTemp);
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
    Encoding.Default.GetBytes(listenIPAddress, 0, listenIPAddress.Length, cmd_listen_param.struAddress.szIP, 0);
    cmd_listen_param.struAddress.wPort = listenPort;
    cmd_listen_param.fnCB = device_register_cb;
    cmd_listen_param.byRes = new byte[32];

    Console.WriteLine($"正在监听：{listenIPAddress}:{listenPort}...");
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
```