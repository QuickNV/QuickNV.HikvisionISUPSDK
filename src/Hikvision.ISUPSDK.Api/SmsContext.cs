using static Hikvision.ISUPSDK.Defines;
using static Hikvision.ISUPSDK.Methods;

namespace Hikvision.ISUPSDK.Api
{
    public class SmsContext
    {
        private SmsContextOptions options;

        public SmsContext(SmsContextOptions options)
        {
            this.options = options;
        }

        public static void Init()
        {
            SdkManager.Init();
            Invoke(NET_ESTREAM_Init());
        }
    }
}
