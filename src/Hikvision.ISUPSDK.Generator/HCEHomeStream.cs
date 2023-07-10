using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace ISUPDemo
{
    public class HCEHomeStream
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_NEWLINK_CB_MSG
        {
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.MAX_DEVICE_ID_LEN)]
           public byte[]  szDeviceID;   //设备标示符    
           public long    iSessionID;     //设备分配给该取流会话的ID
           public long    dwChannelNo;    //设备通道号
           public byte    byStreamType;   //0-主码流，1-子码流
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
           public byte[]  byRes1;
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.NET_EHOME_SERIAL_LEN)]
           public byte[] sDeviceSerial;    //设备序列号，数字序列号
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 112)]
           public byte[]  byRes;
           public void Init()
           {
               szDeviceID = new byte[HCEHomePublic.MAX_DEVICE_ID_LEN];
               byRes1 = new byte[3];
               sDeviceSerial = new byte[HCEHomePublic.NET_EHOME_SERIAL_LEN];
               byRes = new byte[112];
           }

        }

        public delegate bool PREVIEW_NEWLINK_CB(int lLinkHandle, ref NET_EHOME_NEWLINK_CB_MSG pNewLinkCBMsg, IntPtr pUserData);

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_LISTEN_PREVIEW_CFG
        {
           public HCEHomePublic.NET_EHOME_IPADDRESS struIPAdress; //本地监听信息，IP为0.0.0.0的情况下，默认为本地地址，多个网卡的情况下，默认为从操作系统获取到的第一个
           public PREVIEW_NEWLINK_CB                fnNewLinkCB; //预览请求回调函数，当收到预览连接请求后，SDK会回调该回调函数。
           public IntPtr                            pUser;        // 用户参数，在fnNewLinkCB中返回出来
           public byte                              byLinkMode;   //0：TCP，1：UDP 2: HRUDP方式
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 127)]
           public byte[]                            byRes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PREVIEW_CB_MSG
        {
           public byte     byDataType;      
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3)]
           public byte[]   byRes1;
           public IntPtr   pRecvdata;      //码流头或者数据
           public int      dwDataLen;      //数据长度
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
           public byte[]   byRes2;
        }
        public delegate void PREVIEW_DATA_CB(int iPreviewHandle, ref NET_EHOME_PREVIEW_CB_MSG pPreviewCBMsg, IntPtr pUserData);
       
        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PREVIEW_DATA_CB_PARAM
        {
           public PREVIEW_DATA_CB   fnPreviewDataCB;    //数据回调函数
           public IntPtr            pUserData;          //用户参数, 在fnPreviewDataCB回调出来
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
           public byte[]       byRes;                   //保留
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PLAYBACK_NEWLINK_CB_INFO
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.MAX_DEVICE_ID_LEN)]
            public byte[] szDeviceID;   //设备标示符
            public Int32 lSessionID;     //设备分配给该回放会话的ID，0表示无效
            public Int32 dwChannelNo;    //设备通道号，0表示无效
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.NET_EHOME_SERIAL_LEN)]
            public byte[] sDeviceSerial;	/*12*///设备序列号，数字序列号
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 108)]
            public byte[] byRes;
        }


        public delegate bool PLAYBACK_NEWLINK_CB(Int32 lPlayBackLinkHandle, ref NET_EHOME_PLAYBACK_NEWLINK_CB_INFO pNewLinkCBInfo, IntPtr pUserData);
        
        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PLAYBACK_LISTEN_PARAM
        {
            public HCEHomePublic.NET_EHOME_IPADDRESS struIPAdress;   //本地监听信息，IP为0.0.0.0的情况下，默认为本地地址，
            //多个网卡的情况下，默认为从操作系统获取到的第一个
            public PLAYBACK_NEWLINK_CB fnNewLinkCB;    //回放新连接回调函数
            public IntPtr                pUserData;        //用户参数，在fnNewLinkCB中返回出来
            public byte                  byLinkMode;     //0：TCP，1：UDP (UDP保留)
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 127)]
            public byte[]                byRes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PLAYBACK_DATA_CB_INFO
        {
            public Int32       dwType;                    //类型 0-头信息 1-码流数据
            public IntPtr      pData;                    //数据指针
            public Int32       dwDataLen;                //数据长度
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[]      byRes;                //保留
        }


        public delegate bool PLAYBACK_DATA_CB(Int32 iPlayBackLinkHandle, ref NET_EHOME_PLAYBACK_DATA_CB_INFO pDataCBInfo, IntPtr pUserData);
       
        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PLAYBACK_DATA_CB_PARAM
        {
            public PLAYBACK_DATA_CB    fnPlayBackDataCB;        //数据回调函数
            public IntPtr              pUserData;               //用户参数, 在fnPlayBackDataCB回调出来
            public byte                byStreamFormat;          //码流封装格式：0-PS 1-RTP 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 127)]
            public byte[]              byRes;                   //保留
        }

        public const int  EHOME_PREVIEW_EXCEPTION = 0x102;     //预览取流异常
        public const int  EHOME_PLAYBACK_EXCEPTION = 0x103;    //回放取流异常
        public const int  EHOME_AUDIOTALK_EXCEPTION = 0x104;   //语音对讲取流异常
        public const int  NET_EHOME_DEVICEID_LEN    = 256;     //设备ID长度
        public const int NET_EHOME_SERIAL_LEN       = 12;

        public const int MAX_FILE_NAME_LEN          = 100;


        public const int    NET_EHOME_SYSHEAD          = 1;     //系统头数据 
        public const int    NET_EHOME_STREAMDATA       = 2;      //视频流数据
        public const int    NET_EHOME_STREAMEND        = 3;      //视频流结束标记
       
        //------------------------------------------------------------------------------------------------------------
        //语音对讲
        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_VOICETALK_NEWLINK_CB_INFO
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NET_EHOME_DEVICEID_LEN)]
            public byte[] szDeviceID; /*256*/      //设备标示符    
            public Int32 dwEncodeType;            //SDK赋值,当前对讲设备的语音编码类型,0- OggVorbis，1-G711U，2-G711A，3-G726，4-AAC，5-MP2L2，6-PCM
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = NET_EHOME_SERIAL_LEN)]
            public char[] sDeviceSerial;/*12*/     //设备序列号，数字序列号
            public Int32 dwAudioChan; //对讲通道
            public Int32 lSessionID;     //设备分配给该回放会话的ID，0表示无效
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
        }


        public delegate bool VOICETALK_NEWLINK_CB(Int32 lHandle, ref NET_EHOME_VOICETALK_NEWLINK_CB_INFO pNewLinkCBInfo, IntPtr pUserData);


        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_LISTEN_VOICETALK_CFG
        {
            public HCEHomePublic.NET_EHOME_IPADDRESS struIPAdress;   //本地监听信息，IP为0.0.0.0的情况下，默认为本地地址，
            //多个网卡的情况下，默认为从操作系统获取到的第一个
            public VOICETALK_NEWLINK_CB fnNewLinkCB;    //新连接回调函数
            public IntPtr pUser;        //用户参数，在fnNewLinkCB中返回出来
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_VOICETALK_DATA_CB_INFO
        {
            public byte[] pData;               //数据指针
            public Int32 dwDataLen;            //数据长度
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;                //保留
        }

        public delegate bool VOICETALK_DATA_CB(Int32 lHandle,ref NET_EHOME_VOICETALK_DATA_CB_INFO pDataCBInfo, IntPtr pUserData);

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_VOICETALK_DATA_CB_PARAM
        {
           public VOICETALK_DATA_CB    fnVoiceTalkDataCB;    //数据回调函数
           public IntPtr               pUserData;         //用户参数, 在fnVoiceTalkDataCB回调出来
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
           public byte[]               byRes;          //保留
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_VOICETALK_DATA
        {
           public byte[]       pSendBuf;            //音频数据缓冲区
           public uint        dwDataLen;            //音频数据长度
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
           public byte[]         byRes;            //保留
        }

        public delegate void fExceptionCallBack(int dwType, int iUserID, int iHandle, IntPtr pUser);

        #region HCISUPStream.dll function definition
        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_Init();

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_Fini();

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_GetLastError();

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_SetExceptionCallBack(Int32 dwMessage, Int32 hWnd, fExceptionCallBack fExCB, IntPtr pUser);
        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_SetLogToFile(Int32 iLogLevel, String strLogDir, bool bAutoDel );

        //获取版本号
        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_GetBuildVersion();

        [DllImport("HCISUPStream.dll")]
        public static extern Int32 NET_ESTREAM_StartListenPreview(ref NET_EHOME_LISTEN_PREVIEW_CFG pListenParam);//ref NET_EHOME_LISTEN_PREVIEW_CFG pListenParam

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_StopListenPreview(Int32 iListenHandle);

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_StopPreview(Int32 iPreviewHandle);

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_SetPreviewDataCB(Int32 iHandle, /*ref NET_EHOME_PREVIEW_DATA_CB_PARAM*/IntPtr pStruCBParam);
       
        //语音对讲部分
        [DllImport("HCISUPStream.dll")]
        public static extern Int32 NET_ESTREAM_StartListenVoiceTalk(ref NET_EHOME_LISTEN_VOICETALK_CFG pListenParam);
        
        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_StopListenVoiceTalk(Int32 lListenHandle);

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_SetVoiceTalkDataCB(Int32 lHandle, ref NET_EHOME_VOICETALK_DATA_CB_PARAM pStruCBParam);

        [DllImport("HCISUPStream.dll")]
        public static extern int NET_ESTREAM_SendVoiceTalkData (Int32 lHandle, ref NET_EHOME_VOICETALK_DATA pVoicTalkData);

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_StopVoiceTalk(Int32 lHandle);

        //回放接口
        [DllImport("HCISUPStream.dll")]
        public static extern Int32 NET_ESTREAM_StartListenPlayBack(ref NET_EHOME_PLAYBACK_LISTEN_PARAM pListenParam);

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_SetPlayBackDataCB(Int32 iPlayBackLinkHandle, IntPtr ptrDataCBParam);

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_StopPlayBack(Int32 iPlayBackLinkHandle);

        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_StopListenPlayBack(Int32 iPlaybackListenHandle);

        //日志
        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_SetSDKLocalCfg(EhomeSDK.NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr lpInBuff);
        
        [DllImport("HCISUPStream.dll")]
        public static extern bool NET_ESTREAM_GetSDKLocalCfg(EhomeSDK.NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr lpOutBuff);
        #endregion


    }
}
