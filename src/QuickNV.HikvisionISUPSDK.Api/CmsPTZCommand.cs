using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNV.HikvisionISUPSDK.Api
{
    public enum CmsPTZCommand : byte
    {
        /// <summary>
        /// 向上
        /// </summary>
        PTZ_UP = 0,
        /// <summary>
        /// 向下
        /// </summary>
        PTZ_DOWN = 1,
        /// <summary>
        /// 向左
        /// </summary>
        PTZ_LEFT = 2,
        /// <summary>
        /// 向右
        /// </summary>
        PTZ_RIGHT = 3,
        /// <summary>
        /// 向左上
        /// </summary>
        PTZ_UPLEFT = 4,
        /// <summary>
        /// 向左下
        /// </summary>
        PTZ_DOWNLEFT = 5,
        /// <summary>
        /// 向右上
        /// </summary>
        PTZ_UPRIGHT = 6,
        /// <summary>
        /// 向右下
        /// </summary>
        PTZ_DOWNRIGHT = 7,
        /// <summary>
        /// 变焦（缩小）
        /// </summary>
        PTZ_ZOOMIN = 8,
        /// <summary>
        /// 变焦（放大）
        /// </summary>
        PTZ_ZOOMOUT = 9,
        /// <summary>
        /// 聚焦-
        /// </summary>
        PTZ_FOCUSNEAR = 10,
        /// <summary>
        /// 聚焦+
        /// </summary>
        PTZ_FOCUSFAR = 11,
        /// <summary>
        /// 光圈变大
        /// </summary>
        PTZ_IRISSTARTUP = 12,
        /// <summary>
        /// 光圈变小
        /// </summary>
        PTZ_IRISSTOPDOWN = 13,
        /// <summary>
        /// 补光灯
        /// </summary>
        PTZ_LIGHT = 14,
        /// <summary>
        /// 雨刷
        /// </summary>
        PTZ_WIPER = 15,
        /// <summary>
        /// 自动
        /// </summary>
        PTZ_AUTO = 16
    }
}
