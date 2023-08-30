using System.ComponentModel;

namespace QuickNV.HikvisionISUPSDK
{
    public enum Errors
    {
        [Description("无错误")]
        NET_DVR_NOERROR = 0,
        [Description("用户名或密码不正确")]
        NET_DVR_PASSWORD_ERROR = 1,
        [Description("没有权限")]
        NET_DVR_NOENOUGHPRI = 2,
        [Description("SDK未初始化")]
        NET_DVR_NOINIT = 3,
        [Description("通道号不正确")]
        NET_DVR_CHANNEL_ERROR = 4,
        [Description("无法再添加设备")]
        NET_DVR_OVER_MAXLINK = 5,
        [Description("SDK和设备版本不匹配")]
        NET_DVR_VERSIONNOMATCH = 6,
        [Description("连接到设备失败。设备掉线或网络连接断开。")]
        NET_DVR_NETWORK_FAIL_CONNECT = 7,
        [Description("发送数据失败")]
        NET_DVR_NETWORK_SEND_ERROR = 8,
        [Description("从设备接收数据失败")]
        NET_DVR_NETWORK_RECV_ERROR = 9,
        [Description("从设备接收数据超时")]
        NET_DVR_NETWORK_RECV_TIMEOUT = 10,
        [Description("数据错误")]
        NET_DVR_NETWORK_ERRORDATA = 11,
        [Description("调用顺序错误")]
        NET_DVR_ORDER_ERROR = 12,
        [Description("没有权限")]
        NET_DVR_OPERNOPERMIT = 13,
        [Description("设备执行命令超时")]
        NET_DVR_COMMANDTIMEOUT = 14,
        [Description("参数无效")]
        NET_DVR_PARAMETER_ERROR = 17,
        [Description("设备不支持")]
        NET_DVR_NOSUPPORT = 23,
        [Description("操作失败")]
        NET_DVR_DVROPRATEFAILED = 29,
        [Description("路径不正确")]
        NET_DVR_DIR_ERROR = 40,
        [Description("SDK资源分配错误")]
        NET_DVR_ALLOC_RESOURCE_ERROR = 41,
        [Description("声卡模式不匹配")]
        NET_DVR_AUDIO_MODE_ERROR = 42,
        [Description("数据或图片缓冲区不足")]
        NET_DVR_NOENOUGH_BUF = 43,
        [Description("创建套接字失败")]
        NET_DVR_CREATESOCKET_ERROR = 44,
        [Description("设置套接字失败")]
        NET_DVR_SETSOCKET_ERROR = 45,
        [Description("没有可连接的设备")]
        NET_DVR_MAX_NUM = 46,
        [Description("该用户不存在")]
        NET_DVR_USERNOTEXIST = 47,
        [Description("获取本地IP或Mac地址失败")]
        NET_DVR_GETLOCALIPANDMACFAIL = 53,
        [Description("声卡已被占用")]
        NET_DVR_VOICEMONOPOLIZE = 69,
        [Description("创建日志文件目录失败")]
        NET_DVR_CREATEDIR_ERROR = 71,
        [Description("绑定套接字失败")]
        NET_DVR_BINDSOCKET_ERROR = 72,
        [Description("套接字掉线")]
        NET_DVR_SOCKETCLOSE_ERROR = 73,
        [Description("注销失败。该用户正在进行其他操作。")]
        NET_DVR_USERID_ISUSING = 74,
        [Description("监听失败")]
        NET_DVR_SOCKETLISTEN_ERROR = 75,
        [Description("加在动态转码链接库（systemTransform.dll）失败")]
        NET_DVR_CONVERT_SDK_ERROR = 85,
        [Description("该操作系统不支持该功能")]
        NET_DVR_FUNCTION_NOT_SUPPORT_OS = 98,
        [Description("日志已启用")]
        NET_DVR_USE_LOG_SWITCH_FILE = 103,
        [Description("码流封装格式无效")]
        NET_DVR_PACKET_TYPE_NOT_SUPPORT = 105,
        [Description("网络接入配置时IP地址有误")]
        NET_DVR_IPPARA_IPID_ERROR = 106,
        [Description("码流加密校验失败")]
        NET_DVR_STREAM_ENCRYPT_CHECK_FAIL = 130,
        [Description("支持5.0版本ISUP的设备未上传码流初始信息。")]
        NET_DVR_STREAM_STATUS_NOT_INIT = 131,
        [Description("证书错误")]
        NET_DVR_CERTIFICATE_FILE_ERROR = 147,
        [Description("加载SSL库失败")]
        NET_DVR_LOAD_SSL_LIB_ERROR = 148,
        [Description("加载帧分析库失败")]
        NET_DVR_LOAD_ANALYZE_DATA_LIB_ERROR = 149,
        [Description("加载libeay32.dll库失败")]
        NET_DVR_LOAD_LIBEAY32_DLL_ERROR = 156,
        [Description("加载ssleay32.dll库失败")]
        NET_DVR_LOAD_SSLEAY32_DLL_ERROR = 157,
        [Description("加载libiconv2.dll库失败")]
        NET_ERR_LOAD_LIBICONV = 158,
        [Description("SSL连接失败")]
        NET_ERR_SSL_CONNECT_FAILED = 159,
        [Description("加载zlib.dll库失败")]
        NET_ERR_LOAD_ZLIB = 161,
        [Description("该通道已在发流")]
        NET_PREVIEW_ERR_CHANNEL_BUSY = 165,
        [Description("取流地址重复")]
        NET_PREVIEW_ERR_CLIENT_BYSY = 166,
        [Description("不支持的码流类型")]
        NET_PREVIEW_ERR_STREAM_UNSUPPORT = 167,
        [Description("不支持的传输方式")]
        NET_PREVIEW_ERR_TRANSPORT_UNSUPPORT = 168,
        [Description("连接预览流媒体服务器失败")]
        NET_PREVIEW_ERR_CONNECT_SERVER_FAIL = 169,
        [Description("查询设备公网出口地址失败")]
        NET_PREVIEW_ERR_QUERY_WLAN_INFO_FAIL = 170,
        [Description("无视频源")]
        NET_PREVIEW_ERR_NO_VIDEO_FAIL = 171,
        [Description("设置编码参数失败")]
        NET_PREVIEW_ERR_SET_ENCODE_PARAM_FAIL = 172,
        [Description("设置码流分支类型失败")]
        NET_PREVIEW_ERR_SET_PACK_TYPE_FAIL = 173,
        [Description("设备已经在取流，不再支持预取流。")]
        NET_PREVIEW_ERR_NOW_IN_PREVIEW_FAIL = 174,
        [Description("设备已经在预取流，不再支持预取流。")]
        NET_PREVIEW_ERR_NOW_IN_PRESTREAM_FAIL = 175,
        [Description("设备触发另一路取流，断开前一路的预取流。")]
        NET_PREVIEW_ERR_BREAKOFF_PRESTREAM_FAIL = 176,
        [Description("P2P取流通道不存在")]
        NET_PREVIEW_ERR_P2P_NOT_FOUND = 177,
        [Description("无错误")]
        NET_AUDIOINTERCOM_OK = 600,
        [Description("不支持")]
        NET_AUDIOINTECOM_ERR_NOTSUPORT = 601,
        [Description("内存申请错误")]
        NET_AUDIOINTECOM_ERR_ALLOC_MEMERY = 602,
        [Description("参数错误")]
        NET_AUDIOINTECOM_ERR_PARAMETER = 603,
        [Description("调用次序错误")]
        NET_AUDIOINTECOM_ERR_CALL_ORDER = 604,
        [Description("未发现设备")]
        NET_AUDIOINTECOM_ERR_FIND_DEVICE = 605,
        [Description("不能打开设备诶")]
        NET_AUDIOINTECOM_ERR_OPEN_DEVICE = 606,
        [Description("设备上下文出错")]
        NET_AUDIOINTECOM_ERR_NO_CONTEXT = 607,
        [Description("WAV文件出错")]
        NET_AUDIOINTECOM_ERR_NO_WAVFILE = 608,
        [Description("无效的WAV参数类型")]
        NET_AUDIOINTECOM_ERR_INVALID_TYPE = 609,
        [Description("编码失败")]
        NET_AUDIOINTECOM_ERR_ENCODE_FAIL = 610,
        [Description("解码失败")]
        NET_AUDIOINTECOM_ERR_DECODE_FAIL = 611,
        [Description("播放失败")]
        NET_AUDIOINTECOM_ERR_NO_PLAYBACK = 612,
        [Description("降噪失败")]
        NET_AUDIOINTECOM_ERR_DENOISE_FAIL = 613,
        [Description("未知错误")]
        NET_AUDIOINTECOM_ERR_UNKOWN = 619
    }
}
