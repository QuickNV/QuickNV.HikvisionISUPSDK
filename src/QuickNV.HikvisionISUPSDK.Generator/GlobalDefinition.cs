using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ISUPDemo
{
    public class GlobalDefinition
    {
        public const int MAX_DEVICES = 1024;    //max device number
        public const int MAX_CHAN_NUM_DEMO = 256;    //max channels
        public const int MAX_OUTPUTS = 64; //预览画面数
        public const int MAX_LISTEN_NUM = 10;  //最大监听个数
        public struct LISTEN_INFO
        {
            public HCEHomePublic.NET_EHOME_IPADDRESS struIP;
            public Int32 iLinkType;  //0-TCP , 1-UDP
            public Int32 lHandle;
        }
       

        public const int TREE_ALL  = 0;    //device list    
        public const int DEVICE_LOGOUT = 1;   //device not log in
        public const int DEVICE_LOGIN   = 2;   //devic3e login
        public const int DEVICE_FORTIFY = 3;   //on guard
        public const int DEVICE_ALARM   = 4;   //alarm on device
        public const int DEVICE_FORTIFY_ALARM = 5;    //onguard & alarm on device

        public const int CHAN_ORIGINAL    =     6;   //no preview, no record
        public const int CHAN_PLAY        =     7;   //preview
        public const int CHAN_RECORD      =     8;   //record
        public const int CHAN_PLAY_RECORD =     9;   //preview and record

        //device list config
        public const int TREE_ALL_T  = 0;        //root node
        public const int DEVICETYPE  = 1;          //device
        public const int CHANNELTYPE = 2;          //channel    

        //demo 消息
        public const int WM_USER = 0x0400;
        public const int WM_ADD_LOG  =  WM_USER + 1;    //add log 
        public const int WM_ADD_DEV  = WM_USER + 2; //add device
        public const int WM_DEL_DEV = WM_USER + 3; //delete device
        public const int WM_CHANGE_CHANNEL_ITEM_IMAGE  =  WM_USER + 4;     //change channel node icon
        public const int WM_PROC_EXCEPTION  = WM_USER + 5;    //process exception
        public const int WM_CHANGE_IP = WM_USER + 6; //ip address changed

        //local log type
        public const int ALARM_INFO_T     = 0;    //alarm
        public const int OPERATION_SUCC_T = 1;    //operation succeed
        public const int OPERATION_FAIL_T = 2;    //operation fail
        public const int PLAY_SUCC_T      = 3;    //player succeed
        public const int PLAY_FAIL_T = 4;    //player fail

        public enum DEMO_CHANNEL_TYPE
        {
            DEMO_CHANNEL_TYPE_INVALID = -1,
            DEMO_CHANNEL_TYPE_ANALOG  = 0,
            DEMO_CHANNEL_TYPE_IP = 1,
            DEMO_CHANNEL_TYPE_ZERO //零通道
        };

        public struct STRU_CHANNEL_INFO
        {
            public int iDeviceIndex;        //device index
            public int iChanIndex;            //channel index
            public int iSessionID;
            public int dwStreamType;       //码流类型，0-主码流，1-子码流，2-码流3，
            public int dwLinkMode;         //码流连接方式: 0：TCP方式,1：UDP方式
            public int lPreviewHandle;     //preview handle
            public DEMO_CHANNEL_TYPE iChanType;
            public int iChannelNO;
            public bool bEnable;            //enable
            public int dwImageType;
            public HCEHomePublic.NET_EHOME_IPADDRESS struIP;
            public int iPlayWndIndex;          //播放窗口,通过这个参数，来找到对应的CDlgOutput对象
            public IntPtr m_hWnd;              //播放窗口句柄
            public bool bPlay;
            public void Init()
            {
                iDeviceIndex        = -1;
                iChanIndex          = -1;
                iSessionID          = -1;
                dwStreamType        = 0;
                dwLinkMode          = 0;
                lPreviewHandle      = -1;
                iChannelNO          = -1;
                bEnable             = false;
                iChanType           = DEMO_CHANNEL_TYPE.DEMO_CHANNEL_TYPE_INVALID;
                dwImageType         = CHAN_ORIGINAL;
                struIP.szIP = new char[128];
                struIP.byRes = new byte[32];
                struIP.wPort = 8000;
                iPlayWndIndex = -1;
                m_hWnd = IntPtr.Zero;
                bPlay = false;
            }
        }

        public struct LOCAL_DEVICE_INFO
        {
            public int iDeviceIndex;
            public int iLoginID;                //ID
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.U1)]
            public byte[] byDeviceID;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U1)]
            public byte[] byPassword;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U1)]
            public byte[] byFirmwareVersion;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 128, ArraySubType = UnmanagedType.U1)]
            public byte[] byDevLocalIP;
            public Int16 wDevLocalPort;
            public Int16 wManuFacture;
            public uint dwDevType;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 50, ArraySubType = UnmanagedType.U1)]
            public byte[] bySerialNumber;        //SN
            public uint dwDeviceChanNum;          //channel numder  (analog + ip)
            public uint dwStartChan;              //start channel number    
            public uint dwDiskNum;                //HD number    
            public uint dwAlarmInNum;             //alarm in number
            public uint dwAlarmOutNum;            //alarm out number
            public uint dwAudioNum;               //voice talk number
            public uint dwAnalogChanNum;          //analog channel number
            public uint dwIPChanNum;              //IP channel number
            public uint dwZeroChanNum;            //零通道个数
            public uint dwZeroChanStart;          //零通道起始个数
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType =UnmanagedType.U1)]
            public byte[] sDeviceSerial;    //设备序列号，数字序列号
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType =UnmanagedType.U1)]
            public byte[] sIdentifyCode;    //设备验证码，出厂时，设备固件中写入的随机码。默认为abcdef
            public bool bPlayDevice;
            public uint dwVersion;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_CHAN_NUM_DEMO, ArraySubType = UnmanagedType.Struct)]
            public STRU_CHANNEL_INFO[] struChanInfo;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 32, ArraySubType = UnmanagedType.U1)]
            public byte[] byEhomeKey;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U1)]
            public byte[] bySessionKey;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 64, ArraySubType = UnmanagedType.U1)]
            public byte[] byClouldSecretKey;
            public void Init()
            {
                iDeviceIndex = -1;
                iLoginID = -1; 
                byDeviceID = new byte[256];   
                byPassword = new byte[32];
                byFirmwareVersion = new byte[64];
                byDevLocalIP = new byte[128];
                wDevLocalPort = 0;
                wManuFacture = 0;
                dwDevType = 0;
                bySerialNumber = new byte[52];
                dwDeviceChanNum = 0;
                dwStartChan = 0;
                dwDiskNum = 0;
                dwAlarmInNum = 0;
                dwAlarmOutNum = 0;
                dwAudioNum = 0;
                dwAnalogChanNum = 0;
                dwIPChanNum = 0;
                dwZeroChanNum = 0;
                dwZeroChanStart = 0;
                bPlayDevice = false;
                sDeviceSerial = new byte[12];
                sIdentifyCode = new byte[8];
                dwVersion = 0;
                struChanInfo = new STRU_CHANNEL_INFO[MAX_CHAN_NUM_DEMO];
                byEhomeKey = new byte[32];
                bySessionKey = new byte[16];
                byClouldSecretKey = new byte[64];
            }
        }

        public string convEncode(string write, string fromEncode, string toEncode)
        {

            //声明字符集

            Encoding From, To;

            From = Encoding.GetEncoding(fromEncode);

            To = Encoding.GetEncoding(toEncode);

            byte[] temp = From.GetBytes(write);

            byte[] temp1 = Encoding.Convert(From, To, temp);

            //返回转换后的字符   

            return To.GetString(temp1);
        }

        private HCEHomeCMS.NET_EHOME_PTXML_PARAM struPTXML = new HCEHomeCMS.NET_EHOME_PTXML_PARAM();
        String m_strInputXml;
        String m_strOutputXml;
        private byte[] m_szInputBuffer = new byte[1500];
        private byte[] m_szOutBuffer = new byte[1024 * 10];
        private byte[] m_szUrl = new byte[1024];
        private int m_iDeviceIndex;

        public void ConfigMethod(ref IntPtr ptrCfg, String strTemp, String m_strInputXml, int OutBufferLen)
        {
            byte[] m_szInputBuffer = new byte[strTemp.Length];
            m_szUrl = Encoding.Default.GetBytes(strTemp);
            struPTXML.pRequestUrl = Marshal.AllocHGlobal(300);
            Marshal.Copy(m_szUrl, 0, struPTXML.pRequestUrl, m_szUrl.Length);
            struPTXML.dwRequestUrlLen = (uint)m_szUrl.Length;
            strTemp = m_strInputXml;
            if ("" == strTemp)
            {
                struPTXML.pInBuffer = IntPtr.Zero;
                struPTXML.dwInSize = 0;
            }
            else
            {
                m_szInputBuffer = Encoding.UTF8.GetBytes(strTemp);
                struPTXML.pInBuffer = Marshal.AllocHGlobal(strTemp.Length);
                Marshal.Copy(m_szInputBuffer, 0, struPTXML.pInBuffer, m_szInputBuffer.Length);
                struPTXML.dwInSize = (uint)m_szInputBuffer.Length;
            }
            struPTXML.pOutBuffer = Marshal.AllocHGlobal(OutBufferLen);
            for (int i = 0; i < OutBufferLen; i++)
            {
                Marshal.WriteByte(struPTXML.pOutBuffer, i, 0);
            }
            struPTXML.dwOutSize = (uint)OutBufferLen;
            struPTXML.byRes = new byte[32];
            try
            {
                ptrCfg = Marshal.AllocHGlobal(Marshal.SizeOf(struPTXML));
                Marshal.StructureToPtr(struPTXML, ptrCfg, false);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Memory Exception");
                Marshal.FreeHGlobal(struPTXML.pRequestUrl);
                Marshal.FreeHGlobal(struPTXML.pInBuffer);
                Marshal.FreeHGlobal(struPTXML.pOutBuffer);
                Marshal.FreeHGlobal(ptrCfg);
                return;
            }
        }




  }
}
