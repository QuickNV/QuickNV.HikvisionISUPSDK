using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <summary>
        /// 发送RTP数据包
        /// </summary>
        /// <param name="packet"></param>
        public abstract void SendRtpPacket(ReadOnlySpan<byte> packet);
        /// <summary>
        /// 发送PS数据包
        /// </summary>
        /// <param name="data"></param>
        public void SendPsPacket(Span<byte> data)
        {
            packer.Write(data);
        }

        public virtual void Dispose()
        {
            packer.RtpPacketPacked -= Packer_RtpPacketPacked;
        }
    }
}
