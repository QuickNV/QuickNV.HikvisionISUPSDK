using System;
using System.Collections.Generic;
using System.Text;

namespace Hikvision.ISUPSDK.Api.Utils
{
    public class StringUtils
    {
        public static string ByteArray2String(byte[] buffer, Encoding encoding)
        {
            return encoding.GetString(buffer).Trim(char.MinValue);
        }
    }
}
