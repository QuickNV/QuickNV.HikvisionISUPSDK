using Hikvision.ISUPSDK.Api.Utils;
using System;
using System.Runtime.InteropServices;
using System.Text;
using static Hikvision.ISUPSDK.Defines;
using static Hikvision.ISUPSDK.Methods;

namespace Hikvision.ISUPSDK.Api
{
    public class DeviceContext
    {
        private CmsContextOptions options;

        /// <summary>
        /// 登录编号
        /// </summary>
        public int LoginID { get; private set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string Id { get; private set; }
        public string Name { get; private set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public string Serial { get; private set; }
        /// <summary>
        /// SIM卡电话号码
        /// </summary>
        public string SIMCardPhoneNum { get; private set; }
        /// <summary>
        /// SIM卡序列号
        /// </summary>
        public string SIMCardSN { get; private set; }
        /// <summary>
        /// 固件版本
        /// </summary>
        public string FirmwareVersion { get; private set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; private set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public int DevType { get; private set; }
        /// <summary>
        /// 设备大类
        /// </summary>
        public ushort DevClass { get; private set; }
        /// <summary>
        /// 协议版本
        /// </summary>
        public string ProtocolVersion { get; private set; }
        /// <summary>
        /// SessionKey
        /// </summary>
        public string SessionKey { get; private set; }
        /// <summary>
        /// 硬盘数量
        /// </summary>
        public uint DiskNumber { get; private set; }
        /// <summary>
        /// 起始视频通道号
        /// </summary>
        public uint StartChannel { get; private set; }
        /// <summary>
        /// 模拟通道个数
        /// </summary>
        public uint ChannelNumber { get; private set; }
        /// <summary>
        /// 通道总数（包括模拟通道和数字通道）。
        /// </summary>
        public uint ChannelAmount { get; private set; }

        public DeviceContext(CmsContextOptions options, int iUserID, NET_EHOME_DEV_REG_INFO_V12 struDevInfo)
        {
            this.options = options;
            LoginID = iUserID;
            Id = StringUtils.ByteArray2String(struDevInfo.struRegInfo.byDeviceID, options.Encoding);
            SessionKey = StringUtils.ByteArray2String(struDevInfo.struRegInfo.bySessionKey, options.Encoding);
            Serial = StringUtils.ByteArray2String(struDevInfo.struRegInfo.sDeviceSerial, options.Encoding);
            FirmwareVersion = StringUtils.ByteArray2String(struDevInfo.struRegInfo.byFirmwareVersion, options.Encoding);
            IPAddress = new string(struDevInfo.struRegInfo.struDevAdd.szIP).Trim(char.MinValue);
            Port = struDevInfo.struRegInfo.struDevAdd.wPort;
            DevType = struDevInfo.struRegInfo.dwDevType;
            ProtocolVersion = StringUtils.ByteArray2String(struDevInfo.struRegInfo.byDevProtocolVersion, options.Encoding);
        }

        /// <summary>
        /// 刷新设备信息
        /// </summary>
        public void RefreshDeviceInfo()
        {
            NET_EHOME_DEVICE_INFO struDevInfo = new NET_EHOME_DEVICE_INFO();
            NET_EHOME_CONFIG struCfg = new NET_EHOME_CONFIG();
            struDevInfo.sSerialNumber = new byte[MAX_SERIALNO_LEN];
            struDevInfo.sSIMCardSN = new byte[MAX_SERIALNO_LEN];
            struDevInfo.sSIMCardPhoneNum = new byte[MAX_PHOMENUM_LEN];
            struDevInfo.byRes = new byte[160];
            struDevInfo.dwSize = Marshal.SizeOf(struDevInfo);

            IntPtr ptrDevInfo = Marshal.AllocHGlobal(struDevInfo.dwSize);
            Marshal.StructureToPtr(struDevInfo, ptrDevInfo, false);

            struCfg.pOutBuf = ptrDevInfo;
            struCfg.byRes = new byte[40];
            struCfg.dwOutSize = (uint)struDevInfo.dwSize;
            uint dwConfigSize = (uint)Marshal.SizeOf(struCfg);
            IntPtr ptrCfg = Marshal.AllocHGlobal(Marshal.SizeOf(struCfg));
            try
            {
                Invoke(NET_ECMS_GetDevConfig(LoginID, NET_EHOME_GET_DEVICE_INFO, ref struCfg, dwConfigSize));
                struDevInfo = (NET_EHOME_DEVICE_INFO)Marshal.PtrToStructure(ptrDevInfo, typeof(NET_EHOME_DEVICE_INFO));
                //更新设备信息
                DiskNumber = struDevInfo.dwDiskNumber;
                ChannelAmount = struDevInfo.dwChannelAmount;
                StartChannel = struDevInfo.dwStartChannel;
                ChannelNumber = struDevInfo.dwChannelNumber;
                Serial = StringUtils.ByteArray2String(struDevInfo.sSerialNumber, options.Encoding);
                DevType = (int)struDevInfo.dwDevType;
                DevClass = struDevInfo.wDevClass;
                SIMCardPhoneNum = StringUtils.ByteArray2String(struDevInfo.sSIMCardPhoneNum, options.Encoding);
                SIMCardSN = StringUtils.ByteArray2String(struDevInfo.sSIMCardSN, options.Encoding);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrDevInfo);
                Marshal.FreeHGlobal(ptrCfg);
            }
        }

        /// <summary>
        /// 刷新设备基本信息
        /// </summary>
        public void RefreshDeviceCfg()
        {
            NET_EHOME_DEVICE_CFG struDevCfg = new NET_EHOME_DEVICE_CFG();
            NET_EHOME_CONFIG struCfg = new NET_EHOME_CONFIG();
            struDevCfg.sServerName = new byte[MAX_DEVICE_NAME_LEN];
            struDevCfg.sSerialNumber = new byte[MAX_SERIALNO_LEN];
            struDevCfg.byRes = new byte[292];
            struDevCfg.dwSize = Marshal.SizeOf(struDevCfg);

            IntPtr ptrDevCfg = Marshal.AllocHGlobal(struDevCfg.dwSize);
            Marshal.StructureToPtr(struDevCfg, ptrDevCfg, false);

            struCfg.pOutBuf = ptrDevCfg;
            struCfg.byRes = new byte[40];
            struCfg.dwOutSize = (uint)struDevCfg.dwSize;
            uint dwConfigSize = (uint)Marshal.SizeOf(struCfg);
            IntPtr ptrCfg = Marshal.AllocHGlobal(Marshal.SizeOf(struCfg));
            try
            {
                Invoke(NET_ECMS_GetDevConfig(LoginID, NET_EHOME_GET_DEVICE_CFG, ref struCfg, dwConfigSize));
                struDevCfg = (NET_EHOME_DEVICE_CFG)Marshal.PtrToStructure(ptrDevCfg, typeof(NET_EHOME_DEVICE_CFG));
                //更新设备信息
                Name = StringUtils.ByteArray2String(struDevCfg.sServerName, options.Encoding);
                Serial = StringUtils.ByteArray2String(struDevCfg.sSerialNumber, options.Encoding);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrDevCfg);
                Marshal.FreeHGlobal(ptrCfg);
            }
        }
    }
}
