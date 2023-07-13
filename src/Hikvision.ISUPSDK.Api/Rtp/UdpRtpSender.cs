using Hikvision.ISUPSDK.Api.Rtp;
using System;
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
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(options.Host), options.Port);
        }


        public override void Connect()
        {
            udpClient = new UdpClient();
        }

        protected override void SendRtpPacket(ArraySegment<byte> packet)
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
