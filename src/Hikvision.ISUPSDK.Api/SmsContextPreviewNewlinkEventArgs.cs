using System;

namespace Hikvision.ISUPSDK.Api
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
        /// <summary>
        /// 0-主码流，1-子码流
        /// </summary>
        public byte StreamType { get; set; }
        /// <summary>
        /// //封装格式：0- PS,1-标准流(入参)
        /// </summary>
        public byte StreamFormat { get; set; }
    }
}
