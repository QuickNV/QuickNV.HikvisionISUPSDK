using Hikvision.ISUPSDK.Api;
using Hikvision.ISUPSDK.Api.Rtp;
using Hikvision.ISUPSDK.Api.Utils;
using Newtonsoft.Json;

var zlmServerIpAddress = "127.0.0.1";
var zlmServerRtpPort = 10000;
var smsServerIpAddrss = "127.0.0.1";

var rtpSender = new TcpRtpSender(new RtpSenderOptions()
{
    Host = zlmServerIpAddress,
    Port = zlmServerRtpPort,
    SSRC = 0x12345678
});
rtpSender.Connect();

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

    var dataSpan = e.GetDataSpan();
    rtpSender.Write(dataSpan);
}

void Context_DeviceOffline(object? sender, DeviceContext e)
{
    Console.WriteLine("[CMS]设备下线！" + JsonConvert.SerializeObject(e, Formatting.Indented));
}

void Context_DeviceOnline(object? sender, DeviceContext e)
{
    Console.WriteLine("[CMS]设备上线！" + JsonConvert.SerializeObject(e, Formatting.Indented));
    var streamId = e.StartGetRealStreamV11(smsServerIpAddrss, smsOptions.ListenPort, 1, SmsLinkMode.TCP, SmsStreamType.Main);
    e.StartPushRealStream(streamId);
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