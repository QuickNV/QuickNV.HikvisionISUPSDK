using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hikvision.ISUPSDK.Api
{
    public class PsHeaders
    {
        public const int HEADER_LENGTH = 4;
        public static byte[] PsPayloadP_1 = new byte[] { 0x00, 0x00, 0x01, 0x41 };
        public static byte[] PsPayloadP_2 = new byte[] { 0x00, 0x00, 0x01, 0x61 };
        public static byte[] PsPayloadI = new byte[] { 0x00, 0x00, 0x01, 0x65 };
        public static byte[] PsPayloadSPS = new byte[] { 0x00, 0x00, 0x01, 0x67 };
        public static byte[] PsPayloadPPS = new byte[] { 0x00, 0x00, 0x01, 0x68 };
        /// <summary>
        /// PS流开始头
        /// </summary>
        public static byte[] PsStartHeader = new byte[] { 0x00, 0x00, 0x01, 0xBA };
        /// <summary>
        /// PS流结束头
        /// </summary>
        public static byte[] PsEndHeader = new byte[] { 0x00, 0x00, 0x01, 0xB9 };
        public static byte[] PsSystemHeader = new byte[] { 0x00, 0x00, 0x01, 0xBB };
        public static byte[] PsSystemMap = new byte[] { 0x00, 0x00, 0x01, 0xBC };
        public static byte[] PsPrivateData = new byte[] { 0x00, 0x00, 0x01, 0xBD };
        public static byte[] PesHeader_Video = new byte[] { 0x00, 0x00, 0x01, 0xE0 };
        public static byte[] PesHeader_Audio = new byte[] { 0x00, 0x00, 0x01, 0xC0 };
    }
}
