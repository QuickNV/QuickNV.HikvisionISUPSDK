using System;
using System.Net.Sockets;

namespace Hikvision.ISUPSDK.Api.Rtp
{
    public class TcpRtpSender : RtpSender
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] buffer = new byte[2];

        public TcpRtpSender(RtpSenderOptions options) : base(options)
        {
            tcpClient = new TcpClient();
        }

        public override void Connect()
        {
            tcpClient.Connect(Options.Host, Options.Port);
            stream = tcpClient.GetStream();
        }

        protected override void SendRtpPacket(ArraySegment<byte> packet)
        {
            var length = Convert.ToUInt16(packet.Count);
            var lengthSpan = new Span<byte>(buffer);
            BitConverter.TryWriteBytes(lengthSpan, length);
            if (BitConverter.IsLittleEndian)
                lengthSpan.Reverse();
            stream.Write(lengthSpan);
            stream.Write(packet);
        }

        public override void Dispose()
        {
            base.Dispose();
            stream.Dispose();
            tcpClient.Dispose();
        }
    }
}
