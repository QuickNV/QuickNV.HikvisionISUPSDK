namespace Hikvision.ISUPSDK.Api.Utils
{
    public class ByteUtils
    {
        public static void Reverse(byte[] buffer)
        {
            byte tmp;
            for (var i = 0; i < buffer.Length / 2; i++)
            {
                tmp = buffer[i];
                buffer[i] = buffer[buffer.Length - 1 - i];
                buffer[buffer.Length - 1 - i] = tmp;
            }
        }
    }
}
