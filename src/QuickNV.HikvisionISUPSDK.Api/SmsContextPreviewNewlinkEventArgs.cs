using System;

namespace QuickNV.HikvisionISUPSDK.Api
{
    public class SmsContextPreviewNewlinkEventArgs : EventArgs
    {
        /// <summary>
        /// 是否允许连接
        /// </summary>
        public bool Allowed { get; set; } = true;
        public int SessionId { get; set; }        
        public int LinkHandle { get; set; }
        public string DeviceId { get; set; }
        public string DeviceSerial { get; set; }
        public int ChannelId { get; set; }
        public SmsStreamType StreamType { get; set; }
        public SmsStreamFormat StreamFormat { get; set; }
    }
}
