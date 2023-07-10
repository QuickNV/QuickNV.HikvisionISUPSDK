using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ISUPDemo
{
    public class HCEHomeCMS
    {

        //开启关闭监听
        public const int    ENUM_UNKNOWN = -1;
        public const int    ENUM_DEV_ON = 0;             //设备上线回调
        public const int    ENUM_DEV_OFF = 1;               //设备下线回调
        public const int    ENUM_DEV_ADDRESS_CHANGED = 2;     //设备地址发生变化
        public const int    ENUM_DEV_AUTH = 3;       //Ehome5.0设备认证回调
        public const int    ENUM_DEV_SESSIONKEY = 4;    //Ehome5.0设备Sessionkey回调
        public const int    ENUM_DEV_DAS_REQ = 5;      //Ehome5.0设备重定向请求回调
        public const int    ENUM_DEV_SESSIONKEY_REQ = 6;//EHome5.0设备sessionkey请求回调
        public const int    ENUM_DEV_DAS_REREGISTER = 7;//设备重注册回调
        public const int    MAX_DEVNAME_LEN=32;

        //OpenSSL路径
        public const int NET_EHOME_CMS_INIT_CFG_LIBEAY_PATH = 0;   //设置OpenSSL的libeay32.dll/libcrypto.so所在路径
        public const int NET_EHOME_CMS_INIT_CFG_SSLEAY_PATH = 1;   //设置OpenSSL的ssleay32.dll/libssl.so所在

        //通道类型
        public const int    DEMO_CHANNEL_TYPE_INVALID = -1;
        public const int    DEMO_CHANNEL_TYPE_ANALOG = 0;
        public const int    DEMO_CHANNEL_TYPE_IP = 1;
        public const int    DEMO_CHANNEL_TYPE_ZERO = 2; //零通道
       
        public const int    MAX_SERIALNO_LEN = 128;    //序列号最大长度
        public const int    MAX_PHOMENUM_LEN = 32;     //手机号码最大长度
        public const int    MAX_DEVICE_NAME_LEN = 32;  //设备名称长度
        public const int NET_EHOME_GET_GPS_CFG = 20; //获取GPS参数
        public const int NET_EHOME_SET_GPS_CFG = 21; //设置GPS参数
        public const int NET_EHOME_GET_PIC_CFG = 22; //获取OSD显示参数
        public const int NET_EHOME_SET_PIC_CFG = 23; //设置OSD显示参数
        public const int MAX_EHOME_PROTOCOL_LEN = 1500;
        public const int IPADDRESS_LENGTH = 128;//IP地址数组的长度

        public const String CONFIG_GET_PARAM_XML = "<Params>\r\n<ConfigCmd>{0}</ConfigCmd>\r\n<ConfigParam1>{1}</ConfigParam1>\r\n<ConfigParam2>{2}</ConfigParam2>\r\n<ConfigParam3>{3}</ConfigParam3>\r\n<ConfigParam4>{4}</ConfigParam4>\r\n</Params>\r\n";
        public const String CONFIG_SET_PARAM_XML = "<Params>\r\n<ConfigCmd>{0}</ConfigCmd>\r\n<ConfigParam1>{1}</ConfigParam1>\r\n<ConfigParam2>{2}</ConfigParam2>\r\n<ConfigParam3>{3}</ConfigParam3>\r\n<ConfigParam4>{4}</ConfigParam4>\r\n<ConfigXML>{5}</ConfigXML>\r\n</Params>\r\n";

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_CONFIG
        {
            public IntPtr pCondBuf;    //[in]，条件数据指针，如表示通道号等
            public uint dwCondSize; //[in]，pCondBuf指向的数据大小
            public IntPtr pInBuf;        //[in]，设置时需要用到，指向结构体的指针
            public uint dwInSize;    //[in], pInBuf指向的数据大小
            public IntPtr pOutBuf;        //[out]，获取时需要用到，指向结构体的指针，内存由上层分配
            public uint dwOutSize;    //[in]，获取时需要用到，表示pOutBuf指向的内存大小， 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 40, ArraySubType = UnmanagedType.U1)]
            public byte[] byRes;    //保留
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_DEVICE_INFO
        {
            public int dwSize;                //结构体大小
            public uint dwChannelNumber;     //模拟视频通道个数
            public uint dwChannelAmount;    //总视频通道数（模拟+IP）
            public uint dwDevType;            //设备类型，1-DVR，3-DVS，30-IPC，40-IPDOME，其他值参考海康NetSdk设备类型号定义值
            public uint dwDiskNumber;        //设备当前硬盘数
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_SERIALNO_LEN)]
            public byte[] sSerialNumber;        //设备序列号
            public uint dwAlarmInPortNum;    //模拟通道报警输入个数
            public uint dwAlarmInAmount;    //设备总报警输入个数
            public uint dwAlarmOutPortNum;    //模拟通道报警输出个数
            public uint dwAlarmOutAmount;    //设备总报警输出个数
            public uint dwStartChannel;        //视频起始通道号
            public uint dwAudioChanNum;    //语音对讲通道个数
            public uint dwMaxDigitChannelNum;    //设备支持的最大数字通道路数
            public uint dwAudioEncType;        //语音对讲音频格式，0- OggVorbis，1-G711U，2-G711A，3-G726，4-AAC，5-MP2L2,6-PCM
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_SERIALNO_LEN)]
            public byte[] sSIMCardSN;    //车载设备扩展：SIM卡序列号
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_PHOMENUM_LEN)]
            public byte[] sSIMCardPhoneNum;    //车载扩展：SIM卡手机号码
            public uint dwSupportZeroChan;    // SupportZeroChan:支持零通道的个数：0-不支持，1-支持1路，2-支持2路，以此类推
            public uint dwStartZeroChan;        //零通道起始编号，默认10000
            public uint dwSmartType;            //智能类型，0-smart，1-专业智能；默认0-smart
            public ushort wDevClass;            //设备的大类
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 158)]
            public byte[] byRes;            //保留
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_DEV_REG_INFO
        {
            public int  dwSize; 
            public int  dwNetUnitType;            //根据EHomeSDK协议预留，目前没有意义
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.MAX_DEVICE_ID_LEN)]
            public byte[] byDeviceID; //设备ID
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] byFirmwareVersion;    //固件版本
            public      HCEHomePublic.NET_EHOME_IPADDRESS struDevAdd;         //设备注册上来是，设备的本地地址
            public int  dwDevType;                  //设备类型
            public int  dwManufacture;              //设备厂家代码
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] byPassWord;             //设备登陆CMS的密码，由用户自行根据需求进行验证
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.NET_EHOME_SERIAL_LEN/*12*/)]
            public byte[] sDeviceSerial;    //设备序列号，数字序列号
            public byte byReliableTransmission;
            public byte byWebSocketTransmission;
            public byte bySupportRedirect;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] byDevProtocolVersion;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.MAX_MASTER_KEY_LEN)]
            public byte[] bySessionKey;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 27)]
            public byte[] byRes;
            public void Init()
            {
                byDeviceID = new byte[HCEHomePublic.MAX_DEVICE_ID_LEN];
                byFirmwareVersion = new byte[24];
                byPassWord = new byte[32];
                sDeviceSerial = new byte[HCEHomePublic.NET_EHOME_SERIAL_LEN];
                byDevProtocolVersion = new byte[6];
                bySessionKey = new byte[HCEHomePublic.MAX_MASTER_KEY_LEN];
                byRes = new byte[27];
            }
        }

        public struct NET_EHOME_IPADDRESS
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.U1)]
            public char[] szIP;
            public Int16 wPort;     //端口
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
        public struct NET_EHOME_DEV_REG_INFO_V12
        {
            public HCEHomeCMS.NET_EHOME_DEV_REG_INFO struRegInfo;
            public HCEHomeCMS.NET_EHOME_IPADDRESS struRegAddr;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_DEVNAME_LEN)]
            public byte[] sDevName;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 224)]
            public byte[] byRes;
            public void Init()
            {
                struRegInfo.Init();
                struRegAddr.Init();
                sDevName = new byte[MAX_DEVNAME_LEN];
                byRes = new byte[224];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_BLACKLIST_SEVER
        {
            public HCEHomePublic.NET_EHOME_IPADDRESS struAdd;         //服务器地址
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.NAME_LEN/*32*/)]
            public byte[]    byServerName;                            //服务器名称
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.NAME_LEN/*32*/)]
            public byte[]    byUserName;                              //用户名
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = HCEHomePublic.NAME_LEN/*32*/)]
            public byte[]    byPassWord;                              //密码
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64/*64*/)]
            public byte[]   byRes;
            public void Init()
            {
                struAdd.Init();
                byServerName = new byte[HCEHomePublic.NAME_LEN];
                byUserName = new byte[HCEHomePublic.NAME_LEN];
                byPassWord = new byte[HCEHomePublic.NAME_LEN];
                byRes = new byte[64];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_SERVER_INFO
        {
            public int                  dwSize;
            public int                  dwKeepAliveSec;            //心跳间隔（单位：秒,0:默认为15S）
            public int                  dwTimeOutCount;         //心跳超时次数（0：默认为6）
            public HCEHomePublic.NET_EHOME_IPADDRESS         struTCPAlarmSever;      //报警服务器地址（TCP协议）
            public HCEHomePublic.NET_EHOME_IPADDRESS         struUDPAlarmSever;        //报警服务器地址（UDP协议）
            public int                  dwAlarmServerType;        //报警服务器类型0-只支持UDP协议上报，1-支持UDP、TCP两种协议上报
            public HCEHomePublic.NET_EHOME_IPADDRESS         struNTPSever;            //NTP服务器地址
            public int                  dwNTPInterval;            //NTP校时间隔（单位：秒）
            public HCEHomePublic.NET_EHOME_IPADDRESS         struPictureSever;       //图片服务器地址
            public int                  dwPicServerType;        //图片服务器类型图片服务器类型，1-VRB图片服务器，0-Tomcat图片服务
            public NET_EHOME_BLACKLIST_SEVER   struBlackListServer;    //黑名单服务器
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128/*128*/)]
            public byte[]                   byRes;
            public void Init()
            {
                struTCPAlarmSever.Init();
                struUDPAlarmSever.Init();
                struNTPSever.Init();
                struPictureSever.Init();
                struBlackListServer.Init();
                byRes = new byte[128];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_SERVER_INFO_V50
        {
            public int dwSize;
            public int dwKeepAliveSec;            //心跳间隔（单位：秒,0:默认为15S）
            public int dwTimeOutCount;         //心跳超时次数（0：默认为6）
            public HCEHomePublic.NET_EHOME_IPADDRESS struTCPAlarmSever;      //报警服务器地址（TCP协议）
            public HCEHomePublic.NET_EHOME_IPADDRESS struUDPAlarmSever;        //报警服务器地址（UDP协议）
            public int dwAlarmServerType;        //报警服务器类型0-只支持UDP协议上报，1-支持UDP、TCP两种协议上报
            public HCEHomePublic.NET_EHOME_IPADDRESS struNTPSever;            //NTP服务器地址
            public int dwNTPInterval;            //NTP校时间隔（单位：秒）
            public HCEHomePublic.NET_EHOME_IPADDRESS struPictureSever;       //图片服务器地址
            public int dwPicServerType;        //图片服务器类型图片服务器类型，1-VRB图片服务器，0-Tomcat图片服务,2-云存储3,3-KMS
            public NET_EHOME_BLACKLIST_SEVER struBlackListServer;    //黑名单服务器
            public HCEHomePublic.NET_EHOME_IPADDRESS struRedirectSever;       //Redirect Server
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] byClouldAccessKey; //云存储AK
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] byClouldSecretKey; //云存储SK
            public byte byClouldHttps;//云存储HTTPS使能 1-HTTPS 0-HTTP
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 383)]
            public byte[] byRes;

            public void Init()
            {
                struTCPAlarmSever.Init();
                struUDPAlarmSever.Init();
                struNTPSever.Init();
                struPictureSever.Init();
                struBlackListServer.Init();
                struRedirectSever.Init();
                byClouldAccessKey = new byte[64];
                byClouldSecretKey = new byte[64];
                byRes = new byte[383];
            }
        }


        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_CMS_LISTEN_PARAM
        {
           public HCEHomePublic.NET_EHOME_IPADDRESS struAddress; //Local Listen Information, when IP is 0.0.0.0，it is local address by default; when with multiple NICs, it is the first one got from the operating system by default 
           public DEVICE_REGISTER_CB fnCB; //Device Register Callback Function 
           public IntPtr pUserData;   // User Data 
           [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32)]
           public byte[] byRes; 
        }

                //预览请求
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_PREVIEWINFO_IN
        {
            public int iChannel;                        //通道号 
            public int dwStreamType;                    // 码流类型，0-主码流，1-子码流, 2-第三码流
            public int dwLinkMode;                        // 0：TCP方式,1：UDP方式,2: HRUDP方式
            public HCEHomePublic.NET_EHOME_IPADDRESS struStreamSever;     //流媒体地址
            public void Init()
            {
                struStreamSever.Init();
            }
        }
     
        public struct NET_EHOME_PREVIEWINFO_IN_V11
        {
            public int iChannel;
            public int dwStreamType;
            public int dwLinkMode;
            public HCEHomePublic.NET_EHOME_IPADDRESS struStreamSever;
            public byte byDelayPreview;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 31)]
            public byte[] byRes;

            public void Init()
            {
                struStreamSever.Init();
                byRes = new byte[31];
            }

        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_PREVIEWINFO_OUT
        {
            public int lSessionID; 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_PUSHSTREAM_IN
        {
            public int dwSize;
            public int lSessionID; 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
            public void Init()
            {
                byRes = new byte[128];
            }
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_PUSHSTREAM_OUT
        {
            public int dwSize;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
            public void Init()
            {
                byRes = new byte[128];
            }
        }

        public delegate bool DEVICE_REGISTER_CB(int lUserID, int dwDataType, IntPtr pOutBuffer, uint dwOutLen,
                                                 IntPtr pInBuffer, uint dwInLen, IntPtr pUser);


        //-----------------------------------------------------------------------------------------------------------
        //语音对讲
        public delegate void fVoiceDataCallBack(Int32 iVoiceHandle, char[] pRecvDataBuffer, uint dwBufSize, uint dwEncodeType, byte byAudioFlag, IntPtr pUser);
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_VOICETALK_PARA
        {
            public int bNeedCBNoEncData; //需要回调的语音类型：0-编码后语音(编码数据)，1-编码前语音(PCM数据)（语音转发时不支持）
            public fVoiceDataCallBack cbVoiceDataCallBack; //用于回调音频数据的回调函数
            public uint dwEncodeType;         //SDK赋值,SDK的语音编码类型,0- OggVorbis，1-G711U，2-G711A，3-G726，4-AAC，5-MP2L2，6-PCM
            public IntPtr pUser;               //用户参数
            public byte byVoiceTalk;    //0-语音对讲,1-语音转发
            public byte byDevAudioEnc;  //输出参数，设备的音频编码方式 0- OggVorbis，1-G711U，2-G711A，3-G726，4-AAC，5-MP2L2，6-PCM
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 62)]
            public byte[] byRes;//Reserved, set as 0. 0
        } 

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_VOICE_TALK_IN
        {
            public uint dwVoiceChan;                   //通道号
            public HCEHomePublic.NET_EHOME_IPADDRESS struStreamSever; //流媒体地址
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_VOICE_TALK_OUT
        {
            public Int32 lSessionID;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_PUSHVOICE_IN
        {
            public uint dwSize;
            public Int32 lSessionID; 
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_PUSHVOICE_OUT
        {
            public uint dwSize;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_XML_CFG 
        {
            public IntPtr pCmdBuf;    //字符串格式命令
            public uint dwCmdLen;   //pCmdBuf长度
            public IntPtr pInBuf;     //输入数据
            public uint dwInSize;   //输入数据长度
            public IntPtr pOutBuf;    //输出缓冲
            public uint dwOutSize;  //输出缓冲区长度
            public uint dwSendTimeOut;  //数据发送超时时间,单位ms，默认5s
            public uint dwRecvTimeOut;  //数据接收超时时间,单位ms，默认5s
            public IntPtr pStatusBuf;     //返回的状态参数(XML格式),如果不需要,可以置NULL
            public uint dwStatusSize;   //状态缓冲区大小(内存大小)
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_XML_REMOTE_CTRL_PARAM
        {
            public uint dwSize;
            public IntPtr lpInbuffer;          //input param buffer
            public uint  dwInBufferSize;      //size of input param buffer
            public uint  dwSendTimeOut;  //send time out,unit ms，default 5s
            public uint  dwRecvTimeOut;  //receive time out,unit ms，default 5s
            public IntPtr lpOutBuffer;     //output buffer
            public uint dwOutBufferSize;  //size of output buffer
            public IntPtr lpStatusBuffer;   //status buffer,if not user can set NULL
            public uint dwStatusBufferSize;  //status buffer size
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[]byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_PTXML_PARAM
        {
            public IntPtr pRequestUrl;
            public uint   dwRequestUrlLen;
            public IntPtr pCondBuffer;
            public uint   dwCondSize;
            public IntPtr pInBuffer;
            public uint   dwInSize;
            public IntPtr pOutBuffer;
            public uint   dwOutSize;
            public uint   dwReturnedXMLLen;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] byRes;
        }

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct NET_EHOME_REMOTE_CTRL_PARAM
        {
            uint   dwSize;
            IntPtr lpCondBuffer;        //条件参数缓冲区
            uint   dwCondBufferSize;    //条件参数缓冲区长度
            IntPtr lpInbuffer;          //控制参数缓冲区
            uint   dwInBufferSize;      //控制参数缓冲区长度
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32)]
            byte[]   byRes;
        }

        //时间参数
        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_TIME
        {
            public Int16 wYear;      //年
            public byte byMonth;    //月
            public byte byDay;      //日
            public byte byHour;     //时
            public byte byMinute;   //分
            public byte bySecond;   //秒
            public byte byRes1;
            public Int16 wMSecond;   //毫秒
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byRes;            //保留  
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_REC_FILE_COND
        {
            public Int32 dwChannel;          //通道号，从1开始
            public Int32 dwRecType;
            public NET_EHOME_TIME struStartTime;      //开始时间
            public NET_EHOME_TIME struStopTime;       //结束时间
            public Int32 dwStartIndex;       //查询起始位置
            public Int32 dwMaxFileCountPer;  //单次搜索最大文件个数，最大文件个数，需要确定实际网络环境，建议最大个数为8
            public byte byLocalOrUTC;       //0-struStartTime和struStopTime中，表示的是设备本地时间，即设备OSD时间  1-struStartTime和struStopTime中，表示的是UTC时间
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 63)]
            public byte[] byRes;            //保留            
        }


        //查询接口
        public const int MAX_FILE_NAME_LEN = 100;
        //录像文件信息
        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_REC_FILE
        {
            public Int32 dwSize;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_FILE_NAME_LEN)]
            public char[] sFileName;   //文件名
            public NET_EHOME_TIME struStartTime;                  //文件的开始时间
            public NET_EHOME_TIME struStopTime;                   //文件的结束时间
            public Int32 dwFileSize;                     //文件的大小
            public Int32 dwFileMainType;                 //录像文件主类型
            public Int32 dwFileSubType;                  //录像文件子类型
            public Int32 dwFileIndex;                    //录像文件索引
            public byte[] byTimeDiffH;                    //struStartTime、struStopTime与国际标准时间（UTC）的时差（小时），-12 ... +14,0xff表示无效
            public byte[] byTimeDiffM;                    //struStartTime、struStopTime与国际标准时间（UTC）的时差（分钟），-30,0, 30, 45, 0xff表示无效
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 126)]
            public byte[] byRes;
        }

        public struct NET_EHOME_STOPPLAYBACK_PARAM
        {
            public Int32  lSessionID;
            public Int32  lHandle;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 120)]
            public byte[]   byRes;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=512)]
        public struct PLAYBACK_BY_NAME
        {
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = MAX_FILE_NAME_LEN,ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] szFileName;   //回放的文件名
            public Int32 dwSeekType;                      //0-按字节长度计算偏移量  1-按时间（秒数）计算偏移量
            public Int32 dwFileOffset;                    //文件偏移量，从哪个位置开始下载，如果dwSeekType为0，偏移则以字节计算，为1则以秒数计算
            public Int32 dwFileSpan;                      //下载的文件大小，为0时，表示下载直到该文件结束为止，如果dwSeekType为0，大小则以字节计算，为1则以秒数计算
            public void Init()
            {
                szFileName = new char[MAX_FILE_NAME_LEN];
                dwSeekType = 0;
                dwFileOffset = 0;
                dwFileSpan = 0 ;
            }
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Size=512)]
        public struct PLAYBACK_BY_TIME
        {
            public NET_EHOME_TIME struStartTime;  // 按时间回放的开始时间
            public NET_EHOME_TIME struStopTime;   // 按时间回放的结束时间
            public byte byLocalOrUTC;           //0-设备本地时间，即设备OSD时间  1-UTC时间
            public byte byDuplicateSegment;     //byLocalOrUTC为1时无效 0-重复时间段的前段 1-重复时间段后端
            public void Init()
            {
                byLocalOrUTC = 0;
                byDuplicateSegment = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PLAYBACK_INFO_IN
        {
            public Int32 dwSize;
            public Int32 dwChannel;                    //回放的通道号
            public byte byPlayBackMode;               //回放下载模式 0－按名字 1－按时间
            public byte byStreamPackage;              //回放码流类型，设备端发出的码流格式 0－PS（默认） 1－RTP
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byRes;
            public unionPlayBackMode uPlayBackMode;
            public NET_EHOME_IPADDRESS struStreamSever;    //流媒体地址

            public void Init()
            {
                dwSize = 0;
                dwChannel = 0;
                byPlayBackMode = 0;
                byStreamPackage = 0;
                byRes = new byte[2];
                uPlayBackMode.Init();
                struStreamSever.Init();
            }
        }

        [StructLayoutAttribute(LayoutKind.Explicit, Size = 512)]
        public struct unionPlayBackMode
        {
            [FieldOffset(0)]
            public PLAYBACK_BY_NAME struPlayBackbyName;
            [FieldOffset(0)]
            public PLAYBACK_BY_TIME struPlayBackbyTime;
            public void Init()
            {
                struPlayBackbyName.Init();
                struPlayBackbyTime.Init();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PLAYBACK_INFO_IN_NAME
        {
            public Int32 dwSize;
            public Int32 dwChannel;                    //回放的通道号
            public byte byPlayBackMode;               //回放下载模式 0－按名字 1－按时间
            public byte byStreamPackage;              //回放码流类型，设备端发出的码流格式 0－PS（默认） 1－RTP
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byRes;
            public PLAYBACK_BY_NAME struPlayBackbyName;            
            public NET_EHOME_IPADDRESS struStreamSever;    //流媒体地址

            public void Init()
            {
                dwSize = 0;
                dwChannel = 0;
                byPlayBackMode = 0;
                byStreamPackage = 0;
                byRes = new byte[2];
                struPlayBackbyName.Init();
                struStreamSever.Init();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PLAYBACK_INFO_IN_TIME
        {
            public Int32 dwSize;
            public Int32 dwChannel;                    //回放的通道号
            public byte byPlayBackMode;               //回放下载模式 0－按名字 1－按时间
            public byte byStreamPackage;              //回放码流类型，设备端发出的码流格式 0－PS（默认） 1－RTP
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] byRes;
            public PLAYBACK_BY_TIME struPlayBackbyTime;
            public NET_EHOME_IPADDRESS struStreamSever;    //流媒体地址

            public void Init()
            {
                dwSize = 0;
                dwChannel = 0;
                byPlayBackMode = 0;
                byStreamPackage = 0;
                byRes = new byte[2];
                struPlayBackbyTime.Init();
                struStreamSever.Init();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PUSHPLAYBACK_IN
        {
            public Int32 dwSize;
            public Int32 lSessionID; 
             [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] byKeyMD5;//码流加密秘钥,两次MD5
             [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 96)]
            public byte[]  byRes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PUSHPLAYBACK_OUT
        {
            public Int32 dwSize;
            public Int32  lHandle;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 124)]
            public byte[]  byRes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NET_EHOME_PLAYBACK_INFO_OUT
        {
            public Int32 lSessionID;     //目前协议不支持，返回-1
            public Int32 lHandle;  //设置了回放异步回调之后，该值为消息句柄，回调中用于标识
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 124)]
            public byte[] byRes;
        }

        public const int ENUM_SEARCH_TYPE_ERR = -1;
        public const int ENUM_SEARCH_RECORD_FILE = 0;   //查找录像文件
        public const int ENUM_SEARCH_PICTURE_FILE = 1;    //查找图片文件
        public const int ENUM_SEARCH_FLOW_INFO = 2;    //流量查询
        public const int ENUM_SEARCH_DEV_LOG = 3;   //设备日志查询
        public const int ENUM_SEARCH_ALARM_HOST_LOG = 4;    //报警主机日志查询

        public const int ENUM_GET_NEXT_STATUS_SUCCESS = 1000; //成功读取到一条数据，处理完本次数据后需要再次调用FindNext获取下一条数据
        public const int ENUM_GET_NETX_STATUS_NO_FILE = 1001;          //没有找到一条数据
        public const int ENUM_GET_NETX_STATUS_NEED_WAIT = 1002;         //数据还未就绪，需等待，继续调用FindNext函数
        public const int ENUM_GET_NEXT_STATUS_FINISH = 1003;           //数据全部取完
        public const int ENUM_GET_NEXT_STATUS_FAILED = 1004;           //出现异常
        public const int ENUM_GET_NEXT_STATUS_NOT_SUPPORT = 1005;       //设备不支持该操作，不支持的查询类型




        #region HCISUPCMS.dll function definition

        //SDK初始化
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_Init();

        //SDK反初始化
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_Fini();

        [DllImport(@"HCISUPCMS.dll")]
        public static extern int NET_ECMS_GetLastError();

        [DllImport("HCISUPCMS.dll")]
        public static extern long NET_ECMS_GetBuildVersion();

        [DllImport("HCISUPCMS.dll")]
        public static extern bool NET_ECMS_SetSDKInitCfg(Int32 enumType, IntPtr lpInBuff);

        [DllImport("HCISUPCMS.dll")]
        public static extern int NET_ECMS_StartListen(ref NET_EHOME_CMS_LISTEN_PARAM lpCMSListenPara);//

        [DllImport("HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StopListen(Int32 iHandle);

        [DllImport("HCISUPCMS.dll")]
        public static extern bool NET_ECMS_ForceLogout(Int32 lUserID);

        [DllImport(@"HCISUPCMS.dll")]//, CallingConvention = CallingConvention.Cdecl)
        public static extern bool NET_ECMS_SetLogToFile(Int32 iLogLevel, char[] szLogDir, bool bAutoDel);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_SetSDKLocalCfg(HCEHomePublic.NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr pInBuff);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_GetSDKLocalCfg(HCEHomePublic.NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr POutBuff);
        
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StartGetRealStream(Int32 lUserID, ref HCEHomeCMS.NET_EHOME_PREVIEWINFO_IN pPreviewInfoIn, ref HCEHomeCMS.NET_EHOME_PREVIEWINFO_OUT pPreviewInfoOut); //lUserID由SDK分配的用户ID，由设备注册回调时fDeviceRegisterCallBack返回
        
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StartGetRealStreamV11(Int32 lUserID, ref NET_EHOME_PREVIEWINFO_IN_V11 pPreviewInfoIn, ref NET_EHOME_PREVIEWINFO_OUT pPreviewInfoOut);//iUserID由SDK分配的用户ID，由设备注册回调时fDeviceRegisterCallBack返回
        
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StopGetRealStream(Int32 lUserID, Int32 lSessionID);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StartPushRealStream(Int32 lUserID, ref NET_EHOME_PUSHSTREAM_IN pPushInfoIn, ref NET_EHOME_PUSHSTREAM_OUT pPushInfoOut); 
        
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_GetDevConfig(int lUserID, uint dwCommand, ref HCEHomeCMS.NET_EHOME_CONFIG lpConfig, uint dwConfigSize);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_SetDevConfig(Int32 lUserID, Int32 dwCommand, ref NET_EHOME_CONFIG lpConfig, Int32 dwConfigSize);


        [DllImport(@"HCISUPCMS.dll")]
        public static extern int  NET_ECMS_StartVoiceTalk(Int32 lUserID, uint dwVoiceChan, ref NET_EHOME_VOICETALK_PARA pVoiceTalkPara);
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool  NET_ECMS_StartVoiceWithStmServer(Int32 lUserID, ref NET_EHOME_VOICE_TALK_IN lpVoiceTalkIn,ref NET_EHOME_VOICE_TALK_OUT lpVoiceTalkOut);
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool  NET_ECMS_StartPushVoiceStream(Int32 lUserID,ref NET_EHOME_PUSHVOICE_IN lpPushParamIn, ref NET_EHOME_PUSHVOICE_OUT lpPushParamOut);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool  NET_ECMS_StopVoiceTalk(Int32 iVoiceHandle);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool   NET_ECMS_StopVoiceTalkWithStmServer(Int32 lUserID, Int32 lSessionID);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool  NET_ECMS_SendVoiceTransData(Int32 iVoiceHandle, char[] pSendBuf, uint dwBufSize);
   
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_XMLConfig(Int32 lUserID, IntPtr pXmlCfg, Int32 dwConfigSize);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_RemoteControl(Int32 lUserID, Int32 dwCommand, ref NET_EHOME_REMOTE_CTRL_PARAM lpCtrlParam);

         [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_XMLRemoteControl(Int32 lUserID, IntPtr lpCtrlParam, Int32 dwCtrlSize);
        

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_SetDeviceSessionKey(ref HCEHomePublic.NET_EHOME_DEV_SESSIONKEY pDeviceKey);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_GetDeviceSessionKey(ref HCEHomePublic.NET_EHOME_DEV_SESSIONKEY pDeviceKey);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern int NET_ECMS_StartListenProxy(ref HCEHomePublic.NET_EHOME_PT_PARAM pStruProxyParam);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StopListenProxy(Int32 lListenHandle, Int32 dwProxyType);

        //回放
        [DllImport("HCISUPCMS.dll")]
        public static extern Int32 NET_ECMS_StartFindFile_V11(Int32 lUserID, Int32 lSearchType, IntPtr pFindCond, Int32 dwCondSize);

        [DllImport("HCISUPCMS.dll")]
        public static extern Int32 NET_ECMS_FindNextFile_V11(Int32 lHandle, IntPtr pFindData, Int32 dwDataSize);

        [DllImport("HCISUPCMS.dll")]
        public static extern Int32 NET_ECMS_StopFindFile(Int32 lHandle);

        [DllImport("HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StartPlayBack(Int32 lUserID, ref NET_EHOME_PLAYBACK_INFO_IN_NAME pPlayBackInfoIn, ref NET_EHOME_PLAYBACK_INFO_OUT pPlayBackInfoOut);
        [DllImport("HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StartPlayBack(Int32 lUserID, ref NET_EHOME_PLAYBACK_INFO_IN_TIME pPlayBackInfoIn, ref NET_EHOME_PLAYBACK_INFO_OUT pPlayBackInfoOut);

        [DllImport("HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StartPushPlayBack(Int32 lUserID, ref NET_EHOME_PUSHPLAYBACK_IN pPushInfoIn, ref NET_EHOME_PUSHPLAYBACK_OUT pPushInfoOut);

        [DllImport("HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StopPlayBack(Int32 iUserID, IntPtr pStopParam);

        [DllImport("HCISUPCMS.dll")]
        public static extern bool NET_ECMS_StopPlayBackEx(Int32 iUserID, IntPtr pStopParam);

        //ISAPI 
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_GetPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_PutPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_PostPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam);

        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_DeletePTXMLConfig(Int32 lUserID, IntPtr lpPTXMLParam);
        [DllImport(@"HCISUPCMS.dll")]
        public static extern bool NET_ECMS_ISAPIPassThrough(Int32 lUserID, IntPtr lpParam);

        #endregion

    }
}
