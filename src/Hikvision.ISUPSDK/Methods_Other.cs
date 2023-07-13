using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Hikvision.ISUPSDK
{
    public partial class Methods
    {
        private static bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static string[] GET_LINUX_NATIVE_FILES()
        {
            return new[]
            {
                "HCAapSDKCom/libiconv2.so",
                "HCAapSDKCom/libSystemTransform.so",
                "libcrypto.so",
                "libcrypto.so.1.0.0",
                "libHCISUPAlarm.so",
                "libHCISUPCMS.so",
                "libHCISUPSS.so",
                "libHCISUPStream.so",
                "libHCNetUtils.so",
                "libhpr.so",
                "libNPQos.so",
                "libsqlite3.so",
                "libssl.so",
                "libssl.so.1.0.0",
                "libz.so"
            };
        }

        public static string GET_NATIVE_DIR_PATH()
        {
            string os;
            var arch = RuntimeInformation.ProcessArchitecture.ToString().ToLower();
            if (IsWindows)
                os = "win7";
            else
                os = "linux";
            var nativeDir = $"runtimes/{os}-{arch}/native";
            return nativeDir;
        }

        public static void INIT_NATIVE_DIR()
        {
            if (IsWindows)
                return;

            var programDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var nativeDir = GET_NATIVE_DIR_PATH();
            nativeDir = Path.Combine(programDir, nativeDir);
            Environment.CurrentDirectory = nativeDir;
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
