namespace Hikvision.ISUPSDK.Api.Utils
{
    public static class MediaStreamUtils
    {
        public static string GetSSRC(int mediaId)
        {
            return GetSSRC(0, mediaId);
        }

        public static string GetSSRC(int firstDigital, int mediaId)
        {
            return GetSSRC(firstDigital, "00000", mediaId);
        }

        public static string GetSSRC(int firstDigital, string mediaRealm, int mediaId)
        {
            return $"{firstDigital}{mediaRealm}{mediaId.ToString().PadLeft(4, '0')}";
        }

        public static string GetStreamId(string ssrc)
        {
            return string.Format("{0:X8}", uint.Parse(ssrc));
        }

        public static int GetMediaIdFromSSRC(string ssrc)
        {
            var ret = ssrc.PadLeft(10, '0');
            return int.Parse(ret.Substring(6, 4));
        }

        public static int GetMediaIdFromStreamId(string streamId)
        {
            var i = int.Parse(streamId, System.Globalization.NumberStyles.HexNumber);
            return GetMediaIdFromSSRC(i.ToString());
        }
    }
}
