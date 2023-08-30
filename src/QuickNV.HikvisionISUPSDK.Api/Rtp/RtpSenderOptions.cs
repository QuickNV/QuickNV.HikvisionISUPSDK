using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNV.HikvisionISUPSDK.Api.Rtp
{
    public class RtpSenderOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public uint SSRC { get; set; }
    }
}
