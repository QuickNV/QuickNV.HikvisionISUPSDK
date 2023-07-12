using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Hikvision.ISUPSDK.Api
{
    public class RtpSender : IDisposable
    {
        public const int PS_ANY_HEAD_LENGTH = 4;
        public const int PS_ANY_HEAD_LENGTH_LENGTH = 2;
        public const int RTP_HEAD_LENGTH = 12;
        public const int RTP_PAYLOAD_LENGTH = 1400;
        private RtpSenderOptions options;
        private UdpClient udpClient;
        private byte[] buffer;
        private ushort seq = 0;
        private int payloadOffset = 0;
        private int contentLeftLength = 0;
        private ReadState currentReadState = ReadState.ReadHeader;
        private enum ReadState
        {
            ReadHeader,
            ReadLength,
            ReadPayload
        }

        public RtpSender(RtpSenderOptions options)
        {
            this.options = options;
            buffer = new byte[RTP_HEAD_LENGTH + RTP_PAYLOAD_LENGTH];
            buffer[0] = 0x80;
            buffer[1] = 0x60;
            //RTP头
            //2字节开头+2字节包序号+4字节时间戳+4字节SSRC
            //写入SSRC
            var ssrcSpan = new Span<byte>(buffer, 8, 4);
            BitConverter.TryWriteBytes(ssrcSpan, options.SSRC);
            if (BitConverter.IsLittleEndian)
                ssrcSpan.Reverse();
        }
        private IPEndPoint remoteEndPoint;

        public void Connect()
        {
            udpClient = new UdpClient();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(options.Host), options.Port);
        }

        private void sendRtpPackage(Span<byte> data)
        {
            //写入序号
            var seqSpan = data.Slice(2, 2);
            BitConverter.TryWriteBytes(seqSpan, seq);
            if (BitConverter.IsLittleEndian)
                seqSpan.Reverse();
            //发送包
            udpClient.Send(data, remoteEndPoint);

            payloadOffset = 0;
            seq++;
            if (seq == ushort.MaxValue)
                seq = 0;
        }

        public void PushData(Span<byte> data)
        {
            var allSpan = new Span<byte>(buffer);
            while (data.Length > 0)
            {
                //如果缓冲区满了，发送一次数据
                if (payloadOffset >= RTP_PAYLOAD_LENGTH)
                {
                    var sendSpan = allSpan.Slice(0, RTP_HEAD_LENGTH + payloadOffset);
                    sendRtpPackage(sendSpan);
                }
                var paylaodSpan = allSpan.Slice(RTP_HEAD_LENGTH + payloadOffset);
                switch (currentReadState)
                {
                    case ReadState.ReadHeader:
                        {
                            var head = data.Slice(0, PS_ANY_HEAD_LENGTH);
                            data = data.Slice(PS_ANY_HEAD_LENGTH);
                            //如果是PS头
                            if (head.SequenceEqual(PsHeaders.PsStartHeader))
                            {
                                if (payloadOffset > 0)
                                {
                                    sendRtpPackage(allSpan.Slice(0, RTP_HEAD_LENGTH + payloadOffset));
                                }
                                paylaodSpan = allSpan.Slice(RTP_HEAD_LENGTH + payloadOffset);
                                head.CopyTo(paylaodSpan);
                                paylaodSpan = paylaodSpan.Slice(head.Length);
                                payloadOffset += head.Length;
                                //PS头后还有10字节
                                var psHead = data.Slice(0, 10);
                                data = data.Slice(10);
                                psHead.CopyTo(paylaodSpan);
                                payloadOffset += psHead.Length;
                                //后面还有几个扩展字节
                                var extendByteLength = psHead[9] & 0x07;
                                contentLeftLength = extendByteLength;
                                currentReadState = ReadState.ReadPayload;
                            }
                            //否则是其他类型包
                            else if (
                                head.SequenceEqual(PsHeaders.PsSystemHeader)
                                || head.SequenceEqual(PsHeaders.PsSystemMap)
                                || head.SequenceEqual(PsHeaders.PesHeader_Video)
                                || head.SequenceEqual(PsHeaders.PsPrivateData)
                                )
                            {
                                head.CopyTo(paylaodSpan);
                                payloadOffset += head.Length;
                                currentReadState = ReadState.ReadLength;
                            }
                            else
                            {
                                payloadOffset = 0;
                                contentLeftLength = 0;
                                return;
                            }
                            break;
                        }
                    case ReadState.ReadLength:
                        {
                            //读取长度
                            var lengthSpan = data.Slice(0, PS_ANY_HEAD_LENGTH_LENGTH);
                            lengthSpan.CopyTo(paylaodSpan);
                            if (BitConverter.IsLittleEndian)
                                lengthSpan.Reverse();
                            contentLeftLength = BitConverter.ToUInt16(lengthSpan);
                            if (BitConverter.IsLittleEndian)
                                lengthSpan.Reverse();
                            data = data.Slice(lengthSpan.Length);
                            paylaodSpan = paylaodSpan.Slice(lengthSpan.Length);
                            lengthSpan.CopyTo(paylaodSpan);
                            payloadOffset += lengthSpan.Length;
                            currentReadState = ReadState.ReadPayload;
                            break;
                        }
                    case ReadState.ReadPayload:
                        {
                            var copyLength = Math.Min(Math.Min(paylaodSpan.Length, contentLeftLength), data.Length);
                            if (copyLength <= 0)
                            {
                                currentReadState = ReadState.ReadHeader;
                                continue;
                            }
                            data.Slice(0, copyLength).CopyTo(paylaodSpan);
                            data = data.Slice(copyLength);
                            payloadOffset += copyLength;
                            contentLeftLength -= copyLength;
                            if (contentLeftLength <= 0)
                                currentReadState = ReadState.ReadHeader;
                            break;
                        }
                }
            }
        }

        public void Dispose()
        {
            udpClient.Dispose();
        }
    }
}
