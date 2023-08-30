using System;
using System.Collections.Generic;
using System.Text;

namespace QuickNV.HikvisionISUPSDK.Api
{
    public class CmsContextOptions
    {
        /// <summary>
        /// 监听地址
        /// </summary>
        public string ListenIPAddress { get; set; } = "0.0.0.0";
        /// <summary>
        /// 监听端口
        /// </summary>
        public int ListenPort { get; set; } = 7660;
        /// <summary>
        /// 访问安全
        /// </summary>
        public CmsAccessSecurity AccessSecurity { get; set; } = CmsAccessSecurity.CompatibleMode;
        /// <summary>
        /// 字符编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// 服务器主机，仅限ISUP 5.0及以上版本使用，填写SMS服务器的主机地址
        /// </summary>
        public string ServerHost { get; set; } = "127.0.0.1";
        /// <summary>
        /// 服务器端口，仅限ISUP 5.0及以上版本使用，填写SMS服务器的端口信息。一般情况下与监听端口相同
        /// </summary>
        public int? ServerPort { get; set; }
        /// <summary>
        /// 密码，仅限ISUP 5.0及以上版本使用
        /// </summary>
        public string Password { get; set; }
    }
}
