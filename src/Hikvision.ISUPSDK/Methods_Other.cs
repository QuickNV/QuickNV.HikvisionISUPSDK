using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Hikvision.ISUPSDK
{
    public partial class Methods
    {
        private static bool IsWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
        private static string preWorkingDir;
        public static void SetWorkingDirToNativeDir()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;
            var nativeDir = $"runtimes/linux-{RuntimeInformation.ProcessArchitecture.ToString().ToLower()}/native";
            preWorkingDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Path.GetFullPath(nativeDir);
        }

        public static void RestoreWorkingDir()
        {
            if (string.IsNullOrEmpty(preWorkingDir))
                return;
            Thread.Sleep(5000);
            Environment.CurrentDirectory = preWorkingDir;
        }

        public static int Invoke(int result)
        {
            if (result < 0)
            {
                int lastErrorCode = NET_ECMS_GetLastError();
                throw new HikvisionException(lastErrorCode);
            }
            return result;
        }

        public static bool Invoke(bool result)
        {
            if (!result)
            {
                int lastErrorCode = NET_ECMS_GetLastError();
                throw new HikvisionException(lastErrorCode);
            }
            return result;
        }
    }
}
