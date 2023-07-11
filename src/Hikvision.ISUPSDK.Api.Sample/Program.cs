using Hikvision.ISUPSDK.Api;
using Hikvision.ISUPSDK.Api.Sample;
using Hikvision.ISUPSDK.Api.Utils;
using Newtonsoft.Json;
using System.Net.Sockets;

var zlmServerIpAddress = "127.0.0.1";
var zlmServerRtpPort = 10000;
var smsServerIpAddrss = "127.0.0.1";

UdpClient zlmClient = new UdpClient(zlmServerIpAddress, zlmServerRtpPort);

CmsContext.Init();
SmsContext.Init();

var cmsOptions = new CmsContextOptions()
{
    ListenIPAddress = "0.0.0.0",
    ListenPort = 7660
};
var smsOptions = new SmsContextOptions()
{
    ListenIPAddress = "0.0.0.0",
    ListenPort = 7760,
    LinkMode = SmsLinkMode.TCP
};

var cmsContext = new CmsContext(cmsOptions);
cmsContext.DeviceOnline += Context_DeviceOnline;
cmsContext.DeviceOffline += Context_DeviceOffline;

var smsContext = new SmsContext(smsOptions);
smsContext.PreviewNewlink += SmsContext_PreviewNewlink;
smsContext.PreviewData += SmsContext_PreviewData;

void SmsContext_PreviewNewlink(object? sender, SmsContextPreviewNewlinkEventArgs e)
{
    Console.WriteLine($"[SMS]新预览连接：" + JsonConvert.SerializeObject(e, Formatting.Indented));
    var mediaId = (int)e.SessionId;
    var ssrc = MediaStreamUtils.GetSSRC(mediaId);
    var streamId = MediaStreamUtils.GetStreamId(ssrc);
    Console.WriteLine($"[SMS]MediaId:{mediaId},SSRC:{ssrc},StreamId:{streamId}");
}



void SmsContext_PreviewData(object? sender, SmsContextPreviewDataEventArgs e)
{
    if (e.DataType == SmsContextPreviewDataType.NET_DVR_SYSHEAD)
        return;

    var buffer = new byte[1400];
    var dataSpan = e.GetDataSpan();

    
    while (dataSpan.Length > 0)
    {
        var ms = new MemoryStream(buffer);
        ms.Write(new byte[] { 0x80, 0x60 });

        //SequenceNumber
        var sequenceNumberBuffer = BitConverter.GetBytes(Global.SequenceNumber);
        Global.SequenceNumber++;
        if (BitConverter.IsLittleEndian)
            Array.Reverse(sequenceNumberBuffer);
        ms.Write(sequenceNumberBuffer);
        //Time
        byte[] timeSpanBuffer = BitConverter.GetBytes(Convert.ToUInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds));
        if (BitConverter.IsLittleEndian)
            Array.Reverse(timeSpanBuffer);
        ms.Write(timeSpanBuffer);
        //SSRC
        ms.Write(new byte[] { 0x12, 0x34, 0x56, 0x78 });
        //Payload
        var takeCount = buffer.Length - Convert.ToInt32(ms.Position);
        takeCount = Math.Min(takeCount, dataSpan.Length);
        var currentDataSpan = dataSpan.Slice(0, takeCount);
        dataSpan = dataSpan.Slice(takeCount);
        ms.Write(currentDataSpan);

        var spanLength = Convert.ToInt32(ms.Length);
        ms.Close();
        ms.Dispose();
        var span = new ReadOnlySpan<byte>(buffer, 0, spanLength);
        zlmClient.Send(span);
        Console.WriteLine("[SMS]新数据：" + span.ToString());
        //Console.Write($"[SMS]转发数据[{span.Length}字节]：");
        //foreach (var b in span)
        //{
        //    Console.Write(b.ToString("X2"));
        //}
        //Console.WriteLine();
        Global.SequenceNumber++;
    }
    //smsContext.PreviewData -= SmsContext_PreviewData;
}

void Context_DeviceOffline(object? sender, DeviceContext e)
{
    Console.WriteLine("[CMS]设备下线！" + JsonConvert.SerializeObject(e, Formatting.Indented));
}

void Context_DeviceOnline(object? sender, DeviceContext e)
{
    Console.WriteLine("[CMS]设备上线！" + JsonConvert.SerializeObject(e, Formatting.Indented));
    e.StartPushStream(1, smsServerIpAddrss, smsOptions.ListenPort);
}

Console.WriteLine($"正在启动SMS...");
smsContext.Start();
Console.WriteLine($"SMS启动完成。监听端点：{smsOptions.ListenIPAddress}:{smsOptions.ListenPort}");
Console.WriteLine($"正在启动CMS...");
cmsContext.Start();
Console.WriteLine($"CMS启动完成。监听端点：{cmsOptions.ListenIPAddress}:{cmsOptions.ListenPort}");
Console.ReadLine();
Console.WriteLine($"正在停止CMS...");
cmsContext.Stop();
Console.WriteLine($"CMS已停止.");
Console.WriteLine($"正在停止SMS...");
smsContext.Stop();
Console.WriteLine($"SMS已停止.");