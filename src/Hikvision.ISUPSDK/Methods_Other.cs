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

        public static void INIT_NATIVE_FILES()
        {
            if (IsWindows)
                return;

            var programDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var nativeDir = GET_NATIVE_DIR_PATH();
            nativeDir = Path.Combine(programDir, nativeDir);

            foreach (var path in GET_LINUX_NATIVE_FILES())
            {
                var srcFi = new FileInfo(Path.Combine(nativeDir, path));
                if (!srcFi.Exists)
                    continue;
                var desFi = new FileInfo(Path.Combine(programDir, path));
                //如果文件存在，则大小相等，修改时间相同
                if (desFi.Exists
                    && desFi.Length == srcFi.Length
                    && desFi.LastWriteTime == srcFi.LastWriteTime)
                    continue;
                var desDi = desFi.Directory;
                if (!desDi.Exists)
                    desDi.Create();
                srcFi.CopyTo(desFi.FullName, true);
            }
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
