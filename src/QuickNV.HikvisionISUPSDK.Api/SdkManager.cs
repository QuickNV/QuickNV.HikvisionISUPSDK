﻿using static QuickNV.HikvisionISUPSDK.Methods;

namespace QuickNV.HikvisionISUPSDK.Api
{
    internal class SdkManager
    {
        private static bool _Inited = false;
        public static void Init()
        {
            if (_Inited)
                return;

            INIT_NATIVE_DIR();
            _Inited = true;
        }
    }
}
