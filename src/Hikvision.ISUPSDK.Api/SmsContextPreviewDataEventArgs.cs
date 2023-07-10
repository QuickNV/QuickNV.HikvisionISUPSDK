using System;
using System.Collections.Generic;
using System.Text;

namespace Hikvision.ISUPSDK.Api
{
    public class SmsContextPreviewDataEventArgs
    {
        public int LinkHandle { get; set; }
        public IntPtr DataIntPtr { get; set; }
        public int DataLength { get; set; }
        public SmsContextPreviewDataType DataType { get; set; }

        public Span<byte> GetDataSpan()
        {
            unsafe
            {
                return new Span<byte>(DataIntPtr.ToPointer(), DataLength);
            }
        }
    }
}
