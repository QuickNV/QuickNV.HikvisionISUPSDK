using System;
using System.Linq;
using System.Xml.Linq;

namespace Hikvision.ISUPSDK.Api.Rtp
{
    /// <summary>
    /// PS流转RTP流打包器
    /// </summary>
    public class PsToRtpPacker
    {
        public const int PS_ANY_HEAD_LENGTH = 4;
        public const int PS_ANY_HEAD_LENGTH_LENGTH = 2;
        /// <summary>
        /// PS流开始头
        /// </summary>
        public static byte[] PsStartHeader = new byte[] { 0x00, 0x00, 0x01, 0xBA };

        public const int RTP_HEAD_LENGTH = 12;
        public const int RTP_PAYLOAD_LENGTH = 1400;
        public const int RTP_PACKET_LENGTH = RTP_HEAD_LENGTH + RTP_PAYLOAD_LENGTH;

        private byte[] buffer;
        private int readOffset = RTP_HEAD_LENGTH;
        private int writeOffset = RTP_HEAD_LENGTH;

        private ushort seq = 0;
        private int contentLeftLength = 0;
        private ReadState currentReadState = ReadState.ReadHeader;
        private enum ReadState
        {
            ReadHeader,
            ReadLength,
            ReadPayload,
            ReadPsStartHeaderContent
        }
        /// <summary>
        /// SSRC
        /// </summary>
        public uint SSRC { get; private set; }
        /// <summary>
        /// RTP数据包已打包好事件
        /// </summary>
        public event EventHandler<ArraySegment<byte>> RtpPacketPacked;

        public PsToRtpPacker(uint ssrc)
        {
            SSRC = ssrc;

            buffer = new byte[RTP_PACKET_LENGTH];
            buffer[0] = 0x80;
            buffer[1] = 0x60;
            //RTP头
            //2字节开头+2字节包序号+4字节时间戳+4字节SSRC
            //写入SSRC
            var ssrcSpan = new Span<byte>(buffer, 8, 4);
            BitConverter.TryWriteBytes(ssrcSpan, ssrc);
            if (BitConverter.IsLittleEndian)
                ssrcSpan.Reverse();
        }

        //打包RTP数据包
        private void PackRtpPackage()
        {
            //写入序号
            var seqSpan = new Span<byte>(buffer, 2, 2);
            BitConverter.TryWriteBytes(seqSpan, seq);
            if (BitConverter.IsLittleEndian)
                seqSpan.Reverse();

            //触发RTP数据包打包好事件 
            RtpPacketPacked?.Invoke(this, new ArraySegment<byte>(buffer, 0, readOffset));
            //剩余未解析数据
            var leftDataLength = writeOffset - readOffset;
            //如果还有数据未解析，则移动数据
            if (leftDataLength > 0)
            {
                var leftDataSpan = new Span<byte>(buffer, readOffset, leftDataLength);
                var payloadSpan = new Span<byte>(buffer, RTP_HEAD_LENGTH, buffer.Length - RTP_HEAD_LENGTH);
                leftDataSpan.CopyTo(payloadSpan);
            }
            readOffset = RTP_HEAD_LENGTH;
            writeOffset = readOffset + leftDataLength;

            seq++;
            if (seq == ushort.MaxValue)
                seq = 0;
        }

        public void Write(Span<byte> data)
        {
            _Write(data);
        }

        private bool ReadFromBuffer(int length, out Span<byte> outSpan)
        {
            var readSpan = new Span<byte>(buffer, readOffset, writeOffset - readOffset);
            if (readSpan.Length < length)
            {
                outSpan = Span<byte>.Empty;
                return false;
            }
            outSpan = readSpan.Slice(0, length);
            readOffset += length;
            return true;
        }

        private ReadOnlySpan<byte> WriteToBuffer(ReadOnlySpan<byte> inData)
        {
            var writeSpan = new Span<byte>(buffer, writeOffset, buffer.Length - writeOffset);
            var copyLength = Math.Min(writeSpan.Length, inData.Length);
            inData.Slice(0, copyLength).CopyTo(writeSpan);
            writeOffset += copyLength;
            return inData.Slice(copyLength);
        }

        private void _Write(ReadOnlySpan<byte> inData)
        {
            //剩余数据
            var leftInData = inData;

            while (leftInData.Length > 0)
            {
                //如果缓冲区满了，发送一次数据
                if (readOffset >= RTP_PAYLOAD_LENGTH)
                {
                    PackRtpPackage();
                }
                //将输入数据写入到缓存
                leftInData = WriteToBuffer(leftInData);

                //开始读取解析                
                switch (currentReadState)
                {
                    case ReadState.ReadHeader:
                        {
                            if (!ReadFromBuffer(PS_ANY_HEAD_LENGTH, out var head))
                                continue;

                            //如果是PS头
                            if (head.SequenceEqual(PsStartHeader))
                            {
                                //如果发现了已经有PS包头，则发送一次数据
                                if (readOffset > RTP_HEAD_LENGTH + head.Length)
                                {
                                    PackRtpPackage();
                                }
                                currentReadState = ReadState.ReadPsStartHeaderContent;
                            }
                            //否则是其他类型包
                            else
                            {
                                currentReadState = ReadState.ReadLength;
                            }
                            break;
                        }
                    case ReadState.ReadPsStartHeaderContent:
                        {
                            //PS头后还有10字节
                            if (!ReadFromBuffer(10, out var psHead))
                                continue;
                            //后面还有几个扩展字节
                            var extendByteLength = psHead[9] & 0x07;
                            contentLeftLength = extendByteLength;
                            currentReadState = ReadState.ReadPayload;
                            break;
                        }
                    case ReadState.ReadLength:
                        {
                            //读取长度
                            if (!ReadFromBuffer(PS_ANY_HEAD_LENGTH_LENGTH, out var lengthSpan))
                                continue;
                            if (BitConverter.IsLittleEndian)
                                lengthSpan.Reverse();
                            contentLeftLength = BitConverter.ToUInt16(lengthSpan);
                            if (BitConverter.IsLittleEndian)
                                lengthSpan.Reverse();
                            currentReadState = ReadState.ReadPayload;
                            break;
                        }
                    case ReadState.ReadPayload:
                        {
                            var copyLength = Math.Min(writeOffset - readOffset, contentLeftLength);
                            if (copyLength <= 0)
                            {
                                currentReadState = ReadState.ReadHeader;
                                continue;
                            }
                            //读取内容
                            if (!ReadFromBuffer(copyLength, out _))
                                continue;
                            contentLeftLength -= copyLength;
                            if (contentLeftLength <= 0)
                                currentReadState = ReadState.ReadHeader;
                            break;
                        }
                }
            }
        }
    }
}
