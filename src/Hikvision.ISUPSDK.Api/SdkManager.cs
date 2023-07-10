using static Hikvision.ISUPSDK.Methods;

namespace Hikvision.ISUPSDK.Api
{
    internal class SdkManager
    {
        private static bool _Inited = false;
        public static void Init()
        {
            if (_Inited)
                return;

            INIT_NATIVE_FILES();
            _Inited = true;
        }
    }
}
