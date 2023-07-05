using System;
using System.Collections.Generic;
using System.Text;

namespace Hikvision.ISUPSDK.Api
{
    public enum CmsAccessSecurity : byte
    {
        /// <summary>
        /// 0-兼容模式（允许任意版本的协议接入）
        /// </summary>
        CompatibleMode = 0,
        /// <summary>
        /// 1-普通模式（只支持4.0以下版本，不支持协议安全的版本接入）
        /// </summary>
        NormalMode = 1,
        /// <summary>
        /// 2-安全模式（只允许4.0以上版本，支持协议安全的版本接入）
        /// </summary>
        SecurityMode = 2
    }
}
