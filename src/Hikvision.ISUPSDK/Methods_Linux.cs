using System;
using System.Runtime.InteropServices;
using static Hikvision.ISUPSDK.Defines;

namespace Hikvision.ISUPSDK
{
    internal class Methods_Linux
    {
        public const string DllPath = "HCISUPCMS.so";

        [DllImport(DllPath)]
        public static extern bool NET_ECMS_Init();
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_Fini();
        [DllImport(DllPath)]
        public static extern int NET_ECMS_GetLastError();
        [DllImport(DllPath)]
        public static extern long NET_ECMS_GetBuildVersion();
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_SetSDKInitCfg(Int32 enumType, IntPtr lpInBuff);
        [DllImport(DllPath)]
        public static extern int NET_ECMS_StartListen(ref NET_EHOME_CMS_LISTEN_PARAM lpCMSListenPara);//
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StopListen(Int32 iHandle);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_ForceLogout(Int32 lUserID);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_SetLogToFile(Int32 iLogLevel, char[] szLogDir, bool bAutoDel);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_SetSDKLocalCfg(NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr pInBuff);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_GetSDKLocalCfg(NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr POutBuff);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StartGetRealStream(Int32 lUserID, ref NET_EHOME_PREVIEWINFO_IN pPreviewInfoIn, ref NET_EHOME_PREVIEWINFO_OUT pPreviewInfoOut); //lUserID由SDK分配的用户ID，由设备注册回调时fDeviceRegisterCallBack返回
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StartGetRealStreamV11(Int32 lUserID, ref NET_EHOME_PREVIEWINFO_IN_V11 pPreviewInfoIn, ref NET_EHOME_PREVIEWINFO_OUT pPreviewInfoOut);//iUserID由SDK分配的用户ID，由设备注册回调时fDeviceRegisterCallBack返回
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StopGetRealStream(Int32 lUserID, Int32 lSessionID);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StartPushRealStream(Int32 lUserID, ref NET_EHOME_PUSHSTREAM_IN pPushInfoIn, ref NET_EHOME_PUSHSTREAM_OUT pPushInfoOut); 
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_GetDevConfig(int lUserID, uint dwCommand, ref NET_EHOME_CONFIG lpConfig, int dwConfigSize);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_SetDevConfig(Int32 lUserID, Int32 dwCommand, ref NET_EHOME_CONFIG lpConfig, Int32 dwConfigSize);
        [DllImport(DllPath)]
        public static extern int  NET_ECMS_StartVoiceTalk(Int32 lUserID, uint dwVoiceChan, ref NET_EHOME_VOICETALK_PARA pVoiceTalkPara);
        [DllImport(DllPath)]
        public static extern bool  NET_ECMS_StartVoiceWithStmServer(Int32 lUserID, ref NET_EHOME_VOICE_TALK_IN lpVoiceTalkIn,ref NET_EHOME_VOICE_TALK_OUT lpVoiceTalkOut);
        [DllImport(DllPath)]
        public static extern bool  NET_ECMS_StartPushVoiceStream(Int32 lUserID,ref NET_EHOME_PUSHVOICE_IN lpPushParamIn, ref NET_EHOME_PUSHVOICE_OUT lpPushParamOut);
        [DllImport(DllPath)]
        public static extern bool  NET_ECMS_StopVoiceTalk(Int32 iVoiceHandle);
        [DllImport(DllPath)]
        public static extern bool   NET_ECMS_StopVoiceTalkWithStmServer(Int32 lUserID, Int32 lSessionID);
        [DllImport(DllPath)]
        public static extern bool  NET_ECMS_SendVoiceTransData(Int32 iVoiceHandle, char[] pSendBuf, uint dwBufSize);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_XMLConfig(Int32 lUserID, IntPtr pXmlCfg, Int32 dwConfigSize);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_RemoteControl(Int32 lUserID, Int32 dwCommand, ref NET_EHOME_REMOTE_CTRL_PARAM lpCtrlParam);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_XMLRemoteControl(Int32 lUserID, IntPtr lpCtrlParam, Int32 dwCtrlSize);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_SetDeviceSessionKey(ref NET_EHOME_DEV_SESSIONKEY pDeviceKey);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_GetDeviceSessionKey(ref NET_EHOME_DEV_SESSIONKEY pDeviceKey);
        [DllImport(DllPath)]
        public static extern int NET_ECMS_StartListenProxy(ref NET_EHOME_PT_PARAM pStruProxyParam);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StopListenProxy(Int32 lListenHandle, Int32 dwProxyType);
        [DllImport(DllPath)]
        public static extern Int32 NET_ECMS_StartFindFile_V11(Int32 lUserID, Int32 lSearchType, IntPtr pFindCond, Int32 dwCondSize);
        [DllImport(DllPath)]
        public static extern Int32 NET_ECMS_FindNextFile_V11(Int32 lHandle, IntPtr pFindData, Int32 dwDataSize);
        [DllImport(DllPath)]
        public static extern Int32 NET_ECMS_StopFindFile(Int32 lHandle);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StartPlayBack(Int32 lUserID, ref NET_EHOME_PLAYBACK_INFO_IN_NAME pPlayBackInfoIn, ref NET_EHOME_PLAYBACK_INFO_OUT pPlayBackInfoOut);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StartPlayBack(Int32 lUserID, ref NET_EHOME_PLAYBACK_INFO_IN_TIME pPlayBackInfoIn, ref NET_EHOME_PLAYBACK_INFO_OUT pPlayBackInfoOut);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StartPushPlayBack(Int32 lUserID, ref NET_EHOME_PUSHPLAYBACK_IN pPushInfoIn, ref NET_EHOME_PUSHPLAYBACK_OUT pPushInfoOut);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StopPlayBack(Int32 iUserID, IntPtr pStopParam);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_StopPlayBackEx(Int32 iUserID, IntPtr pStopParam);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_GetPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_PutPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_PostPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_DeletePTXMLConfig(Int32 lUserID, IntPtr lpPTXMLParam);
        [DllImport(DllPath)]
        public static extern bool NET_ECMS_ISAPIPassThrough(Int32 lUserID, IntPtr lpParam);
    }
}
