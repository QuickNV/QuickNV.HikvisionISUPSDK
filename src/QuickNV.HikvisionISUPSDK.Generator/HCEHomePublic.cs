using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ISUPDemo
{
    public class HCEHomePublic
    {
        /*******************全局错误码 begin**********************/


        //音对讲库错误码
        public const int  NET_AUDIOINTERCOM_OK              =     600; //无错误
        public const int  NET_AUDIOINTECOM_ERR_NOTSUPORT    =     601; //不支持
        public const int  NET_AUDIOINTECOM_ERR_ALLOC_MEMERY =     602; //内存申请错误
        public const int  NET_AUDIOINTECOM_ERR_PARAMETER    =     603; //参数错误
        public const int  NET_AUDIOINTECOM_ERR_CALL_ORDER   =     604; //调用次序错误
        public const int  NET_AUDIOINTECOM_ERR_FIND_DEVICE  =     605;//未发现设备
        public const int  NET_AUDIOINTECOM_ERR_OPEN_DEVICE  =     606; //不能打开设备诶
        public const int  NET_AUDIOINTECOM_ERR_NO_CONTEXT   =     607; //设备上下文出错
        public const int  NET_AUDIOINTECOM_ERR_NO_WAVFILE   =     608; //WAV文件出错
        public const int  NET_AUDIOINTECOM_ERR_INVALID_TYPE =     609; //无效的WAV参数类型
        public const int  NET_AUDIOINTECOM_ERR_ENCODE_FAIL  =     610; //编码失败
        public const int  NET_AUDIOINTECOM_ERR_DECODE_FAIL  =     611; //解码失败
        public const int  NET_AUDIOINTECOM_ERR_NO_PLAYBACK  =     612; //播放失败
        public const int  NET_AUDIOINTECOM_ERR_DENOISE_FAIL =     613; //降噪失败
        public const int  NET_AUDIOINTECOM_ERR_UNKOWN       =     619; //未知错误
        /*******************全局错误码 end**********************/    

        public const int ALARM_INFO_T = 0;
        public const int OPERATION_SUCC_T = 1;
        public const int OPERATION_FAIL_T = 2;
        public const int PLAY_SUCC_T = 3;
        public const int PLAY_FAIL_T = 4;

        public const int MAX_PASSWD_LEN    =  32;
        public const int NAME_LEN          =  32;      //用户名长度
        public const int MAX_DEVICE_ID_LEN  =  256;     //设备ID长度
        public const int NET_EHOME_SERIAL_LEN = 12;
        public const int MAX_MASTER_KEY_LEN = 16;

        public const int MAX_URL_LEN_SS = 4096;//图片服务器回调UPL长度
        
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_IPADDRESS
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.U1)]
            public char[] szIP;
            public Int16  wPort;     //端口
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byRes;
            public void Init()
            {
                wPort = 0;
                szIP = new char[128];
                byRes = new byte[2];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_DEV_SESSIONKEY
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.MAX_DEVICE_ID_LEN, ArraySubType = UnmanagedType.U1)]
            public byte[] sDeviceID;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.MAX_MASTER_KEY_LEN, ArraySubType = UnmanagedType.U1)]
            public byte[] sSessionKey;

            public void Init()
            {
                sDeviceID = new byte[HCEHomePublic.MAX_DEVICE_ID_LEN];
                sSessionKey = new byte[HCEHomePublic.MAX_MASTER_KEY_LEN];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_ZONE
        {
            Int32 dwX;          //X轴坐标
            Int32 dwY;          //Y轴坐标
            Int32 dwWidth;      //宽度
            Int32 dwHeight;     //高度
        }
        //本地配置
        
        public enum NET_EHOME_LOCAL_CFG_TYPE
        {
            UNDEFINE = -1,   //暂时没有具体的定义
            ACTIVE_ACCESS_SECURITY = 0, //设备主动接入的安全性
            AMS_ADDRESS = 1,            //报警服务器本地回环地址
            SEND_PARAM = 2,             //发送参数配置
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_LOCAL_ACCESS_SECURITY
        {
            public UInt32  dwSize;
            public byte    byAccessSecurity;    //0-兼容模式（允许任意版本的协议接入），1-普通模式（只支持4.0以下版本，不支持协议安全的版本接入） 2-安全模式（只允许4.0以上版本，支持协议安全的版本接入）
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 127)]
            public byte[]  byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_AMS_ADDRESS
        {
            public Int32 dwSize;
            public byte  byEnable;  //0-关闭CMS接收报警功能，1-开启CMS接收报警功能
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] byRes1;
            public NET_EHOME_IPADDRESS struAddress;    //AMS本地回环地址
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] byRes2;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_SEND_PARAM
        {
            public Int32 dwSize;
            public Int32 dwRecvTimeOut;    //接收超时时间，单位毫秒
            public byte  bySendTimes;      //报文发送次数，为了应对网络环境较差的情况下，丢包的情况，默认一次，最大3次
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 127)]
            public byte[] byRes2;
        }

        public struct LOCAL_LOG_INFO
        {
            public int    iLogType;
            public string strTime;
            public string strLogInfo;
            public string strDevInfo;
            public string strErrInfo;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_PT_PARAM
        {
            public NET_EHOME_IPADDRESS struIP;
            public byte byProtocolType;
            public byte byProxyType;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byRes;

            public void Init()
            {
                struIP = new NET_EHOME_IPADDRESS();
                struIP.Init();
                byRes = new byte[2];
            }
        }

        public enum NET_CMS_ENUM_PROXY_TYPE
        {
            ENUM_PROXY_TYPE_NETSDK = 0,	//NetSDK代理
            ENUM_PROXY_TYPE_HTTP	    //HTTP代理
        }
    }
}
