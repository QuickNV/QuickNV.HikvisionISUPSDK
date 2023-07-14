using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hikvision.ISUPSDK.Api
{
    /// <summary>
    /// 码流类型
    /// </summary>
    public enum SmsStreamType : uint
    {
        /// <summary>
        /// 主码流
        /// </summary>
        Main = 0,
        /// <summary>
        /// 子码流
        /// </summary>
        Sub = 1,
        /// <summary>
        /// 第三码流
        /// </summary>
        StreamType3 = 2
    }
}
