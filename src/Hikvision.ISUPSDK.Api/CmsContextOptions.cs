using System;
using System.Collections.Generic;
using System.Text;

namespace Hikvision.ISUPSDK.Api
{
    public class CmsContextOptions
    {
        public string ListenIPAddress { get; set; } = "0.0.0.0";
        public int ListenPort { get; set; } = 7660;
    }
}
