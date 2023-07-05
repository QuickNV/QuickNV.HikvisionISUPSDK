using System.ComponentModel;

namespace Hikvision.ISUPSDK
{
    public enum Errors
    {
        [Description("无错误")]
         NET_AUDIOINTERCOM_OK              =     600,
        [Description("不支持")]
         NET_AUDIOINTECOM_ERR_NOTSUPORT    =     601,
        [Description("内存申请错误")]
         NET_AUDIOINTECOM_ERR_ALLOC_MEMERY =     602,
        [Description("参数错误")]
         NET_AUDIOINTECOM_ERR_PARAMETER    =     603,
        [Description("调用次序错误")]
         NET_AUDIOINTECOM_ERR_CALL_ORDER   =     604,
        [Description("未发现设备")]
         NET_AUDIOINTECOM_ERR_FIND_DEVICE  =     605,
        [Description("不能打开设备诶")]
         NET_AUDIOINTECOM_ERR_OPEN_DEVICE  =     606,
        [Description("设备上下文出错")]
         NET_AUDIOINTECOM_ERR_NO_CONTEXT   =     607,
        [Description("WAV文件出错")]
         NET_AUDIOINTECOM_ERR_NO_WAVFILE   =     608,
        [Description("无效的WAV参数类型")]
         NET_AUDIOINTECOM_ERR_INVALID_TYPE =     609,
        [Description("编码失败")]
         NET_AUDIOINTECOM_ERR_ENCODE_FAIL  =     610,
        [Description("解码失败")]
         NET_AUDIOINTECOM_ERR_DECODE_FAIL  =     611,
        [Description("播放失败")]
         NET_AUDIOINTECOM_ERR_NO_PLAYBACK  =     612,
        [Description("降噪失败")]
         NET_AUDIOINTECOM_ERR_DENOISE_FAIL =     613,
        [Description("未知错误")]
         NET_AUDIOINTECOM_ERR_UNKOWN       =     619
    }
}
