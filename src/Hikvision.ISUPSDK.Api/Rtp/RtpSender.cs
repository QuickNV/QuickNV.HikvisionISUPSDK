using System;

namespace Hikvision.ISUPSDK.Api.Rtp
{
    public abstract class RtpSender : IDisposable
    {
        protected RtpSenderOptions Options { get; private set; }
        private PsToRtpPacker packer;

        public RtpSender(RtpSenderOptions options)
        {
            Options = options;
            packer = new PsToRtpPacker(options.SSRC);
            packer.RtpPacketPacked += Packer_RtpPacketPacked;
        }

        private void Packer_RtpPacketPacked(object sender, ArraySegment<byte> e)
        {
            SendRtpPacket(e);
        }

        public abstract void Connect();
        protected abstract void SendRtpPacket(ArraySegment<byte> packet);

        public void Write(Span<byte> data)
        {
            packer.Write(data);
        }

        public virtual void Dispose()
        {
            packer.RtpPacketPacked -= Packer_RtpPacketPacked;
        }
    }
}
