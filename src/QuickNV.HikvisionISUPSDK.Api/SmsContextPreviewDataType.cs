using System;
using System.Collections.Generic;
using System.Text;

namespace QuickNV.HikvisionISUPSDK.Api
{
    public enum SmsContextPreviewDataType : byte
    {
        /// <summary>
        /// 码流头部
        /// </summary>
        NET_DVR_SYSHEAD = 1,
        /// <summary>
        /// 码流数据
        /// </summary>
        NET_DVR_STREAMDATA = 2
    }
}
