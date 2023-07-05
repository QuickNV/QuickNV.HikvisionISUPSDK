using System;
using System.Collections.Generic;
using System.Text;

namespace Hikvision.ISUPSDK.Api
{
    public class DeviceInfo
    {
        public int LoginID { get; set; }
        public string Id { get; set; }
        public string Serial { get; set; }
        public string FirmwareVersion { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public int Type { get; set; }
        public string ProtocolVersion { get; set; }
        public string SessionKey { get; set; }
    }
}
