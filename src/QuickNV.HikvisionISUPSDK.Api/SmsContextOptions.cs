using System;
using System.Collections.Generic;
using System.Text;

namespace QuickNV.HikvisionISUPSDK.Api
{
    public class SmsContextOptions
    {
        public string ListenIPAddress { get; set; } = "0.0.0.0";
        public int ListenPort { get; set; } = 7661;
        public SmsLinkMode LinkMode { get; set; } = SmsLinkMode.TCP;
    }
}
