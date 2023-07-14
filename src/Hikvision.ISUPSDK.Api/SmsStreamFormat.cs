using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hikvision.ISUPSDK.Api
{
    public enum SmsStreamFormat : byte
    {
        /// <summary>
        /// PS流
        /// </summary>
        PS = 0,
        /// <summary>
        /// 标准流
        /// </summary>
        Standard = 1
    }
}
