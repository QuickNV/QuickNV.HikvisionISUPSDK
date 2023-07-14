using Hikvision.ISUPSDK.Api.Rtp;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Hikvision.ISUPSDK.Api
{
    public class UdpRtpSender : RtpSender
    {
        private UdpClient udpClient;
        private IPEndPoint remoteEndPoint;

        public UdpRtpSender(RtpSenderOptions options)
            : base(options)
        {
            var ipAddress = Dns.GetHostAddresses(options.Host).FirstOrDefault();
            remoteEndPoint = new IPEndPoint(ipAddress, options.Port);
        }

        public override void Connect()
        {
            udpClient = new UdpClient();
        }

        public override void SendRtpPacket(ReadOnlySpan<byte> packet)
        {
            udpClient.Send(packet, remoteEndPoint);
        }

        public override void Dispose()
        {
            base.Dispose();
            udpClient.Dispose();
        }
    }
}
