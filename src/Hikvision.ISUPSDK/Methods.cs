using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using static Hikvision.ISUPSDK.Defines;

namespace Hikvision.ISUPSDK
{
    public partial class Methods
    {
        public static bool NET_ECMS_Init()
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_Init();
            else
                return Methods_Linux.NET_ECMS_Init();
        }

        public static bool NET_ECMS_Fini()
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_Fini();
            else
                return Methods_Linux.NET_ECMS_Fini();
        }
        public static int NET_ECMS_GetLastError()
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_GetLastError();
            else
                return Methods_Linux.NET_ECMS_GetLastError();
        }
        public static long NET_ECMS_GetBuildVersion()
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_GetBuildVersion();
            else
                return Methods_Linux.NET_ECMS_GetBuildVersion();
        }
        public static bool NET_ECMS_SetSDKInitCfg(Int32 enumType, IntPtr lpInBuff)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_SetSDKInitCfg(enumType, lpInBuff);
            else
                return Methods_Linux.NET_ECMS_SetSDKInitCfg(enumType, lpInBuff);
        }
        public static int NET_ECMS_StartListen(ref NET_EHOME_CMS_LISTEN_PARAM lpCMSListenPara)//
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartListen(ref lpCMSListenPara);
            else
                return Methods_Linux.NET_ECMS_StartListen(ref lpCMSListenPara);
        }
        public static bool NET_ECMS_StopListen(Int32 iHandle)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StopListen(iHandle);
            else
                return Methods_Linux.NET_ECMS_StopListen(iHandle);
        }
        public static bool NET_ECMS_ForceLogout(Int32 lUserID)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_ForceLogout(lUserID);
            else
                return Methods_Linux.NET_ECMS_ForceLogout(lUserID);
        }
        public static bool NET_ECMS_SetLogToFile(Int32 iLogLevel, char[] szLogDir, bool bAutoDel)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_SetLogToFile(iLogLevel, szLogDir, bAutoDel);
            else
                return Methods_Linux.NET_ECMS_SetLogToFile(iLogLevel, szLogDir, bAutoDel);
        }
        public static bool NET_ECMS_SetSDKLocalCfg(NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr pInBuff)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_SetSDKLocalCfg(enumType, pInBuff);
            else
                return Methods_Linux.NET_ECMS_SetSDKLocalCfg(enumType, pInBuff);
        }
        public static bool NET_ECMS_GetSDKLocalCfg(NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr POutBuff)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_GetSDKLocalCfg(enumType, POutBuff);
            else
                return Methods_Linux.NET_ECMS_GetSDKLocalCfg(enumType, POutBuff);
        }
        public static bool NET_ECMS_StartGetRealStream(Int32 lUserID, ref NET_EHOME_PREVIEWINFO_IN pPreviewInfoIn, ref NET_EHOME_PREVIEWINFO_OUT pPreviewInfoOut) //lUserID由SDK分配的用户ID，由设备注册回调时fDeviceRegisterCallBack返回
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartGetRealStream(lUserID, ref pPreviewInfoIn, ref pPreviewInfoOut);
            else
                return Methods_Linux.NET_ECMS_StartGetRealStream(lUserID, ref pPreviewInfoIn, ref pPreviewInfoOut);
        }
        public static bool NET_ECMS_StartGetRealStreamV11(Int32 lUserID, ref NET_EHOME_PREVIEWINFO_IN_V11 pPreviewInfoIn, ref NET_EHOME_PREVIEWINFO_OUT pPreviewInfoOut)//iUserID由SDK分配的用户ID，由设备注册回调时fDeviceRegisterCallBack返回
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartGetRealStreamV11(lUserID, ref pPreviewInfoIn, ref pPreviewInfoOut);
            else
                return Methods_Linux.NET_ECMS_StartGetRealStreamV11(lUserID, ref pPreviewInfoIn, ref pPreviewInfoOut);
        }
        public static bool NET_ECMS_StopGetRealStream(Int32 lUserID, Int32 lSessionID)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StopGetRealStream(lUserID, lSessionID);
            else
                return Methods_Linux.NET_ECMS_StopGetRealStream(lUserID, lSessionID);
        }
        public static bool NET_ECMS_StartPushRealStream(Int32 lUserID, ref NET_EHOME_PUSHSTREAM_IN pPushInfoIn, ref NET_EHOME_PUSHSTREAM_OUT pPushInfoOut) 
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartPushRealStream(lUserID, ref pPushInfoIn, ref pPushInfoOut);
            else
                return Methods_Linux.NET_ECMS_StartPushRealStream(lUserID, ref pPushInfoIn, ref pPushInfoOut);
        }
        public static bool NET_ECMS_GetDevConfig(int lUserID, uint dwCommand, ref NET_EHOME_CONFIG lpConfig, uint dwConfigSize)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_GetDevConfig(lUserID, dwCommand, ref lpConfig, dwConfigSize);
            else
                return Methods_Linux.NET_ECMS_GetDevConfig(lUserID, dwCommand, ref lpConfig, dwConfigSize);
        }
        public static bool NET_ECMS_SetDevConfig(Int32 lUserID, Int32 dwCommand, ref NET_EHOME_CONFIG lpConfig, Int32 dwConfigSize)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_SetDevConfig(lUserID, dwCommand, ref lpConfig, dwConfigSize);
            else
                return Methods_Linux.NET_ECMS_SetDevConfig(lUserID, dwCommand, ref lpConfig, dwConfigSize);
        }
        public static int  NET_ECMS_StartVoiceTalk(Int32 lUserID, uint dwVoiceChan, ref NET_EHOME_VOICETALK_PARA pVoiceTalkPara)
        {
            if (IsWindows)
                return Methods_Win. NET_ECMS_StartVoiceTalk(lUserID, dwVoiceChan, ref pVoiceTalkPara);
            else
                return Methods_Linux. NET_ECMS_StartVoiceTalk(lUserID, dwVoiceChan, ref pVoiceTalkPara);
        }
        public static bool  NET_ECMS_StartVoiceWithStmServer(Int32 lUserID, ref NET_EHOME_VOICE_TALK_IN lpVoiceTalkIn,ref NET_EHOME_VOICE_TALK_OUT lpVoiceTalkOut)
        {
            if (IsWindows)
                return Methods_Win. NET_ECMS_StartVoiceWithStmServer(lUserID, ref lpVoiceTalkIn, ref lpVoiceTalkOut);
            else
                return Methods_Linux. NET_ECMS_StartVoiceWithStmServer(lUserID, ref lpVoiceTalkIn, ref lpVoiceTalkOut);
        }
        public static bool  NET_ECMS_StartPushVoiceStream(Int32 lUserID,ref NET_EHOME_PUSHVOICE_IN lpPushParamIn, ref NET_EHOME_PUSHVOICE_OUT lpPushParamOut)
        {
            if (IsWindows)
                return Methods_Win. NET_ECMS_StartPushVoiceStream(lUserID, ref lpPushParamIn, ref lpPushParamOut);
            else
                return Methods_Linux. NET_ECMS_StartPushVoiceStream(lUserID, ref lpPushParamIn, ref lpPushParamOut);
        }
        public static bool  NET_ECMS_StopVoiceTalk(Int32 iVoiceHandle)
        {
            if (IsWindows)
                return Methods_Win. NET_ECMS_StopVoiceTalk(iVoiceHandle);
            else
                return Methods_Linux. NET_ECMS_StopVoiceTalk(iVoiceHandle);
        }
        public static bool   NET_ECMS_StopVoiceTalkWithStmServer(Int32 lUserID, Int32 lSessionID)
        {
            if (IsWindows)
                return Methods_Win.  NET_ECMS_StopVoiceTalkWithStmServer(lUserID, lSessionID);
            else
                return Methods_Linux.  NET_ECMS_StopVoiceTalkWithStmServer(lUserID, lSessionID);
        }
        public static bool  NET_ECMS_SendVoiceTransData(Int32 iVoiceHandle, char[] pSendBuf, uint dwBufSize)
        {
            if (IsWindows)
                return Methods_Win. NET_ECMS_SendVoiceTransData(iVoiceHandle, pSendBuf, dwBufSize);
            else
                return Methods_Linux. NET_ECMS_SendVoiceTransData(iVoiceHandle, pSendBuf, dwBufSize);
        }
        public static bool NET_ECMS_XMLConfig(Int32 lUserID, IntPtr pXmlCfg, Int32 dwConfigSize)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_XMLConfig(lUserID, pXmlCfg, dwConfigSize);
            else
                return Methods_Linux.NET_ECMS_XMLConfig(lUserID, pXmlCfg, dwConfigSize);
        }
        public static bool NET_ECMS_RemoteControl(Int32 lUserID, Int32 dwCommand, ref NET_EHOME_REMOTE_CTRL_PARAM lpCtrlParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_RemoteControl(lUserID, dwCommand, ref lpCtrlParam);
            else
                return Methods_Linux.NET_ECMS_RemoteControl(lUserID, dwCommand, ref lpCtrlParam);
        }
        public static bool NET_ECMS_XMLRemoteControl(Int32 lUserID, IntPtr lpCtrlParam, Int32 dwCtrlSize)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_XMLRemoteControl(lUserID, lpCtrlParam, dwCtrlSize);
            else
                return Methods_Linux.NET_ECMS_XMLRemoteControl(lUserID, lpCtrlParam, dwCtrlSize);
        }
        public static bool NET_ECMS_SetDeviceSessionKey(ref NET_EHOME_DEV_SESSIONKEY pDeviceKey)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_SetDeviceSessionKey(ref pDeviceKey);
            else
                return Methods_Linux.NET_ECMS_SetDeviceSessionKey(ref pDeviceKey);
        }
        public static bool NET_ECMS_GetDeviceSessionKey(ref NET_EHOME_DEV_SESSIONKEY pDeviceKey)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_GetDeviceSessionKey(ref pDeviceKey);
            else
                return Methods_Linux.NET_ECMS_GetDeviceSessionKey(ref pDeviceKey);
        }
        public static int NET_ECMS_StartListenProxy(ref NET_EHOME_PT_PARAM pStruProxyParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartListenProxy(ref pStruProxyParam);
            else
                return Methods_Linux.NET_ECMS_StartListenProxy(ref pStruProxyParam);
        }
        public static bool NET_ECMS_StopListenProxy(Int32 lListenHandle, Int32 dwProxyType)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StopListenProxy(lListenHandle, dwProxyType);
            else
                return Methods_Linux.NET_ECMS_StopListenProxy(lListenHandle, dwProxyType);
        }
        public static Int32 NET_ECMS_StartFindFile_V11(Int32 lUserID, Int32 lSearchType, IntPtr pFindCond, Int32 dwCondSize)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartFindFile_V11(lUserID, lSearchType, pFindCond, dwCondSize);
            else
                return Methods_Linux.NET_ECMS_StartFindFile_V11(lUserID, lSearchType, pFindCond, dwCondSize);
        }
        public static Int32 NET_ECMS_FindNextFile_V11(Int32 lHandle, IntPtr pFindData, Int32 dwDataSize)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_FindNextFile_V11(lHandle, pFindData, dwDataSize);
            else
                return Methods_Linux.NET_ECMS_FindNextFile_V11(lHandle, pFindData, dwDataSize);
        }
        public static Int32 NET_ECMS_StopFindFile(Int32 lHandle)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StopFindFile(lHandle);
            else
                return Methods_Linux.NET_ECMS_StopFindFile(lHandle);
        }
        public static bool NET_ECMS_StartPlayBack(Int32 lUserID, ref NET_EHOME_PLAYBACK_INFO_IN_NAME pPlayBackInfoIn, ref NET_EHOME_PLAYBACK_INFO_OUT pPlayBackInfoOut)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartPlayBack(lUserID, ref pPlayBackInfoIn, ref pPlayBackInfoOut);
            else
                return Methods_Linux.NET_ECMS_StartPlayBack(lUserID, ref pPlayBackInfoIn, ref pPlayBackInfoOut);
        }
        public static bool NET_ECMS_StartPlayBack(Int32 lUserID, ref NET_EHOME_PLAYBACK_INFO_IN_TIME pPlayBackInfoIn, ref NET_EHOME_PLAYBACK_INFO_OUT pPlayBackInfoOut)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartPlayBack(lUserID, ref pPlayBackInfoIn, ref pPlayBackInfoOut);
            else
                return Methods_Linux.NET_ECMS_StartPlayBack(lUserID, ref pPlayBackInfoIn, ref pPlayBackInfoOut);
        }
        public static bool NET_ECMS_StartPushPlayBack(Int32 lUserID, ref NET_EHOME_PUSHPLAYBACK_IN pPushInfoIn, ref NET_EHOME_PUSHPLAYBACK_OUT pPushInfoOut)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StartPushPlayBack(lUserID, ref pPushInfoIn, ref pPushInfoOut);
            else
                return Methods_Linux.NET_ECMS_StartPushPlayBack(lUserID, ref pPushInfoIn, ref pPushInfoOut);
        }
        public static bool NET_ECMS_StopPlayBack(Int32 iUserID, IntPtr pStopParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StopPlayBack(iUserID, pStopParam);
            else
                return Methods_Linux.NET_ECMS_StopPlayBack(iUserID, pStopParam);
        }
        public static bool NET_ECMS_StopPlayBackEx(Int32 iUserID, IntPtr pStopParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_StopPlayBackEx(iUserID, pStopParam);
            else
                return Methods_Linux.NET_ECMS_StopPlayBackEx(iUserID, pStopParam);
        }
        public static bool NET_ECMS_GetPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_GetPTXMLConfig(iUserID, lpPTXMLParam);
            else
                return Methods_Linux.NET_ECMS_GetPTXMLConfig(iUserID, lpPTXMLParam);
        }
        public static bool NET_ECMS_PutPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_PutPTXMLConfig(iUserID, lpPTXMLParam);
            else
                return Methods_Linux.NET_ECMS_PutPTXMLConfig(iUserID, lpPTXMLParam);
        }
        public static bool NET_ECMS_PostPTXMLConfig(Int32 iUserID, IntPtr lpPTXMLParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_PostPTXMLConfig(iUserID, lpPTXMLParam);
            else
                return Methods_Linux.NET_ECMS_PostPTXMLConfig(iUserID, lpPTXMLParam);
        }
        public static bool NET_ECMS_DeletePTXMLConfig(Int32 lUserID, IntPtr lpPTXMLParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_DeletePTXMLConfig(lUserID, lpPTXMLParam);
            else
                return Methods_Linux.NET_ECMS_DeletePTXMLConfig(lUserID, lpPTXMLParam);
        }
        public static bool NET_ECMS_ISAPIPassThrough(Int32 lUserID, IntPtr lpParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ECMS_ISAPIPassThrough(lUserID, lpParam);
            else
                return Methods_Linux.NET_ECMS_ISAPIPassThrough(lUserID, lpParam);
        }
    }
}
