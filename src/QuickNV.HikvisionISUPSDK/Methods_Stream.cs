using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static QuickNV.HikvisionISUPSDK.Defines;

namespace QuickNV.HikvisionISUPSDK
{
    public partial class Methods
    {

        public static bool NET_ESTREAM_Init()
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_Init();
            else
                return Methods_Linux.NET_ESTREAM_Init();
        }
        public static bool NET_ESTREAM_Fini()
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_Fini();
            else
                return Methods_Linux.NET_ESTREAM_Fini();
        }
        public static bool NET_ESTREAM_GetLastError()
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_GetLastError();
            else
                return Methods_Linux.NET_ESTREAM_GetLastError();
        }
        public static bool NET_ESTREAM_SetExceptionCallBack(Int32 dwMessage, Int32 hWnd, fExceptionCallBack fExCB, IntPtr pUser)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_SetExceptionCallBack(dwMessage, hWnd, fExCB, pUser);
            else
                return Methods_Linux.NET_ESTREAM_SetExceptionCallBack(dwMessage, hWnd, fExCB, pUser);
        }
        public static bool NET_ESTREAM_SetLogToFile(Int32 iLogLevel, String strLogDir, bool bAutoDel )
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_SetLogToFile(iLogLevel, strLogDir, bAutoDel);
            else
                return Methods_Linux.NET_ESTREAM_SetLogToFile(iLogLevel, strLogDir, bAutoDel);
        }
        public static bool NET_ESTREAM_GetBuildVersion()
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_GetBuildVersion();
            else
                return Methods_Linux.NET_ESTREAM_GetBuildVersion();
        }
        public static Int32 NET_ESTREAM_StartListenPreview(ref NET_EHOME_LISTEN_PREVIEW_CFG pListenParam)//ref NET_EHOME_LISTEN_PREVIEW_CFG pListenParam
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StartListenPreview(ref pListenParam);
            else
                return Methods_Linux.NET_ESTREAM_StartListenPreview(ref pListenParam);
        }
        public static bool NET_ESTREAM_StopListenPreview(Int32 iListenHandle)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StopListenPreview(iListenHandle);
            else
                return Methods_Linux.NET_ESTREAM_StopListenPreview(iListenHandle);
        }
        public static bool NET_ESTREAM_StopPreview(Int32 iPreviewHandle)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StopPreview(iPreviewHandle);
            else
                return Methods_Linux.NET_ESTREAM_StopPreview(iPreviewHandle);
        }
        
        public static bool NET_ESTREAM_SetPreviewDataCB(Int32 iHandle, ref NET_EHOME_PREVIEW_DATA_CB_PARAM struCBParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_SetPreviewDataCB(iHandle, ref struCBParam);
            else
                return Methods_Linux.NET_ESTREAM_SetPreviewDataCB(iHandle, ref struCBParam);
        }

        public static bool NET_ESTREAM_SetStandardPreviewDataCB(Int32 iHandle, ref NET_EHOME_PREVIEW_DATA_CB_PARAM struCBParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_SetStandardPreviewDataCB(iHandle, ref struCBParam);
            else
                return Methods_Linux.NET_ESTREAM_SetStandardPreviewDataCB(iHandle, ref struCBParam);
        }

        public static Int32 NET_ESTREAM_StartListenVoiceTalk(ref NET_EHOME_LISTEN_VOICETALK_CFG pListenParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StartListenVoiceTalk(ref pListenParam);
            else
                return Methods_Linux.NET_ESTREAM_StartListenVoiceTalk(ref pListenParam);
        }
        public static bool NET_ESTREAM_StopListenVoiceTalk(Int32 lListenHandle)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StopListenVoiceTalk(lListenHandle);
            else
                return Methods_Linux.NET_ESTREAM_StopListenVoiceTalk(lListenHandle);
        }
        public static bool NET_ESTREAM_SetVoiceTalkDataCB(Int32 lHandle, ref NET_EHOME_VOICETALK_DATA_CB_PARAM pStruCBParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_SetVoiceTalkDataCB(lHandle, ref pStruCBParam);
            else
                return Methods_Linux.NET_ESTREAM_SetVoiceTalkDataCB(lHandle, ref pStruCBParam);
        }
        public static int NET_ESTREAM_SendVoiceTalkData (Int32 lHandle, ref NET_EHOME_VOICETALK_DATA pVoicTalkData)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_SendVoiceTalkData (lHandle, ref pVoicTalkData);
            else
                return Methods_Linux.NET_ESTREAM_SendVoiceTalkData (lHandle, ref pVoicTalkData);
        }
        public static bool NET_ESTREAM_StopVoiceTalk(Int32 lHandle)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StopVoiceTalk(lHandle);
            else
                return Methods_Linux.NET_ESTREAM_StopVoiceTalk(lHandle);
        }
        public static Int32 NET_ESTREAM_StartListenPlayBack(ref NET_EHOME_PLAYBACK_LISTEN_PARAM pListenParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StartListenPlayBack(ref pListenParam);
            else
                return Methods_Linux.NET_ESTREAM_StartListenPlayBack(ref pListenParam);
        }
        public static bool NET_ESTREAM_SetPlayBackDataCB(Int32 iPlayBackLinkHandle, IntPtr ptrDataCBParam)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_SetPlayBackDataCB(iPlayBackLinkHandle, ptrDataCBParam);
            else
                return Methods_Linux.NET_ESTREAM_SetPlayBackDataCB(iPlayBackLinkHandle, ptrDataCBParam);
        }
        public static bool NET_ESTREAM_StopPlayBack(Int32 iPlayBackLinkHandle)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StopPlayBack(iPlayBackLinkHandle);
            else
                return Methods_Linux.NET_ESTREAM_StopPlayBack(iPlayBackLinkHandle);
        }
        public static bool NET_ESTREAM_StopListenPlayBack(Int32 iPlaybackListenHandle)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_StopListenPlayBack(iPlaybackListenHandle);
            else
                return Methods_Linux.NET_ESTREAM_StopListenPlayBack(iPlaybackListenHandle);
        }
        public static bool NET_ESTREAM_SetSDKLocalCfg(NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr lpInBuff)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_SetSDKLocalCfg(enumType, lpInBuff);
            else
                return Methods_Linux.NET_ESTREAM_SetSDKLocalCfg(enumType, lpInBuff);
        }
        public static bool NET_ESTREAM_GetSDKLocalCfg(NET_EHOME_LOCAL_CFG_TYPE enumType, IntPtr lpOutBuff)
        {
            if (IsWindows)
                return Methods_Win.NET_ESTREAM_GetSDKLocalCfg(enumType, lpOutBuff);
            else
                return Methods_Linux.NET_ESTREAM_GetSDKLocalCfg(enumType, lpOutBuff);
        }
    }
}
