using System;
using System.Collections.Generic;
using System.Text;

namespace Hikvision.ISUPSDK.Api
{
    public class ChannelInfo
    {
        /// <summary>
        /// 通道编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 通道名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// OSD是否显示通道名称
        /// </summary>
        public int IsShowChanName { get; set; }
        public short ChanNameXPos { get; set; }
        public short ChanNameYPos { get; set; }
        /// <summary>
        /// 是否显示OSD
        /// </summary>
        public int IsShowOSD { get; set; }
        public ushort OSDXPos { get; set; }
        public ushort OSDYPos { get; set; }
        public ushort OSDType { get; set; }
        public byte OSDAtrib { get; set; }
        public string Res1 { get; set; }
        /// <summary>
        /// OSD是否显示星期
        /// </summary>
        public byte IsShowWeek { get; set; }
    }
}
