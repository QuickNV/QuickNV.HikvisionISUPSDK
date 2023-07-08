using Hikvision.ISUPSDK.Api.Utils;
using System;
using System.Collections.Generic;
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
        public short DevClass { get; private set; }
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
        public int DiskNumber { get; private set; }
        /// <summary>
        /// 起始视频通道号
        /// </summary>
        public int StartChannel { get; private set; }
        /// <summary>
        /// 模拟通道个数
        /// </summary>
        public int ChannelNumber { get; private set; }
        /// <summary>
        /// 通道总数（包括模拟通道和数字通道）。
        /// </summary>
        public int ChannelAmount { get; private set; }
        /// <summary>
        /// 通道信息
        /// </summary>
        public ChannelInfo[] Channels { get; private set; }

        public DeviceContext(CmsContextOptions options, int iUserID, NET_EHOME_DEV_REG_INFO_V12 struDevInfo)
        {
            this.options = options;
            LoginID = iUserID;
            Id = StringUtils.ByteArray2String(struDevInfo.struRegInfo.byDeviceID, options.Encoding);
            SessionKey = StringUtils.ByteArray2String(struDevInfo.struRegInfo.bySessionKey, options.Encoding);
            Serial = StringUtils.ByteArray2String(struDevInfo.struRegInfo.sDeviceSerial, options.Encoding);
            FirmwareVersion = StringUtils.ByteArray2String(struDevInfo.struRegInfo.byFirmwareVersion, options.Encoding);
            IPAddress = StringUtils.ByteArray2String(struDevInfo.struRegInfo.struDevAdd.szIP, options.Encoding);
            Port = struDevInfo.struRegInfo.struDevAdd.wPort;
            DevType = struDevInfo.struRegInfo.dwDevType;
            ProtocolVersion = StringUtils.ByteArray2String(struDevInfo.struRegInfo.byDevProtocolVersion, options.Encoding);
        }

        /// <summary>
        /// 刷新设备信息
        /// </summary>
        public void RefreshDeviceInfo()
        {
            NET_EHOME_DEVICE_INFO struDevInfo = NET_EHOME_DEVICE_INFO.NewInstance();
            IntPtr ptrDevInfo = Marshal.AllocHGlobal(struDevInfo.dwSize);
            Marshal.StructureToPtr(struDevInfo, ptrDevInfo, false);

            NET_EHOME_CONFIG struCfg = NET_EHOME_CONFIG.NewInstance();
            struCfg.pOutBuf = ptrDevInfo;
            struCfg.dwOutSize = struDevInfo.dwSize;
            var dwConfigSize = Marshal.SizeOf(struCfg);
            try
            {
                Invoke(NET_ECMS_GetDevConfig(LoginID, NET_EHOME_GET_DEVICE_INFO, ref struCfg, dwConfigSize));
                struDevInfo = Marshal.PtrToStructure<NET_EHOME_DEVICE_INFO>(ptrDevInfo);
                //更新设备信息
                DiskNumber = struDevInfo.dwDiskNumber;
                ChannelAmount = struDevInfo.dwChannelAmount;
                StartChannel = struDevInfo.dwStartChannel;
                ChannelNumber = struDevInfo.dwChannelNumber;
                Serial = StringUtils.ByteArray2String(struDevInfo.sSerialNumber, options.Encoding);
                DevType = struDevInfo.dwDevType;
                DevClass = struDevInfo.wDevClass;
                SIMCardPhoneNum = StringUtils.ByteArray2String(struDevInfo.sSIMCardPhoneNum, options.Encoding);
                SIMCardSN = StringUtils.ByteArray2String(struDevInfo.sSIMCardSN, options.Encoding);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrDevInfo);
            }
        }

        /// <summary>
        /// 刷新设备基本信息
        /// </summary>
        public void RefreshDeviceCfg()
        {
            NET_EHOME_DEVICE_CFG struDevCfg = NET_EHOME_DEVICE_CFG.NewInstance();
            IntPtr ptrDevCfg = Marshal.AllocHGlobal(struDevCfg.dwSize);
            Marshal.StructureToPtr(struDevCfg, ptrDevCfg, false);

            NET_EHOME_CONFIG struCfg = NET_EHOME_CONFIG.NewInstance();
            struCfg.pOutBuf = ptrDevCfg;
            struCfg.dwOutSize = struDevCfg.dwSize;
            var dwConfigSize = Marshal.SizeOf(struCfg);
            try
            {
                Invoke(NET_ECMS_GetDevConfig(LoginID, NET_EHOME_GET_DEVICE_CFG, ref struCfg, dwConfigSize));
                struDevCfg = Marshal.PtrToStructure<NET_EHOME_DEVICE_CFG>(ptrDevCfg);
                //更新设备信息
                Name = StringUtils.ByteArray2String(struDevCfg.sServerName, options.Encoding);
                Serial = StringUtils.ByteArray2String(struDevCfg.sSerialNumber, options.Encoding);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrDevCfg);
            }
        }

        private NET_EHOME_PIC_CFG getChannelInfo(int channelId)
        {
            var channelIdBuffer = BitConverter.GetBytes(channelId);
            IntPtr ptrChannel = Marshal.UnsafeAddrOfPinnedArrayElement(channelIdBuffer, 0);

            NET_EHOME_PIC_CFG struPicCfg = NET_EHOME_PIC_CFG.NewInstance();
            IntPtr ptrPicCfg = Marshal.AllocHGlobal(struPicCfg.dwSize);
            Marshal.StructureToPtr(struPicCfg, ptrPicCfg, false);

            NET_EHOME_CONFIG struCfg = NET_EHOME_CONFIG.NewInstance();
            struCfg.dwCondSize = channelIdBuffer.Length;
            struCfg.pCondBuf = ptrChannel;
            struCfg.dwOutSize = struPicCfg.dwSize;
            struCfg.pOutBuf = ptrPicCfg;
            var dwConfigSize = Marshal.SizeOf(struCfg);

            try
            {
                Invoke(NET_ECMS_GetDevConfig(LoginID, NET_EHOME_GET_PIC_CFG, ref struCfg, dwConfigSize));
                struPicCfg = Marshal.PtrToStructure<NET_EHOME_PIC_CFG>(ptrPicCfg);
                return struPicCfg;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrPicCfg);
            }
        }

        /// <summary>
        /// 刷新通道信息
        /// </summary>
        public void RefreshChannels()
        {
            var list = new List<ChannelInfo>();
            for (int i = 0; i < ChannelAmount; i++)
            {
                var channelId = StartChannel + i;
                var ci = getChannelInfo(channelId);
                list.Add(new ChannelInfo()
                {
                    Id = i,
                    Name = StringUtils.ByteArray2String(ci.byChannelName, options.Encoding),
                    IsShowChanName = ci.bIsShowChanName,
                    ChanNameXPos = ci.wChanNameXPos,
                    ChanNameYPos = ci.wChanNameYPos,
                    IsShowOSD = ci.bIsShowOSD,
                    OSDXPos = ci.wOSDXPos,
                    OSDYPos = ci.wOSDYPos,
                    OSDType = ci.byOSDType,
                    OSDAtrib = ci.byOSDAtrib,
                    IsShowWeek = ci.bIsShowWeek
                });
            }
            Channels = list.ToArray();
        }

        public void Load()
        {
            RefreshDeviceInfo();
            RefreshDeviceCfg();
            RefreshChannels();
        }
    }
}
