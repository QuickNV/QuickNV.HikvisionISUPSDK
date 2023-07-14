using System;
using System.Collections.Generic;
using System.Text;

namespace Hikvision.ISUPSDK.Api
{
    public class SmsContextPreviewDataEventArgs
    {
        public int LinkHandle { get; private set; }
        private IntPtr DataIntPtr;
        private int DataLength;

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
