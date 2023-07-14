using System;
using System.Collections.Generic;
using System.Text;

namespace Hikvision.ISUPSDK.Api
{
    public class SmsContextPreviewDataEventArgs
    {
        private IntPtr DataIntPtr { get; set; }
        private int DataLength { get; set; }

        public int LinkHandle { get; private set; }

        public Span<byte> GetDataSpan()
        {
            unsafe
            {
                return new Span<byte>(DataIntPtr.ToPointer(), DataLength);
            }
        }

        public SmsContextPreviewDataEventArgs(int linkHandle, IntPtr dataIntPtr, int dataLength)
        {
            LinkHandle = linkHandle;
            DataIntPtr = dataIntPtr;
            DataLength = dataLength;
        }
    }
}
