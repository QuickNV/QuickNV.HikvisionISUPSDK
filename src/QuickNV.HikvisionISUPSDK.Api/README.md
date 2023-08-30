# QuickNV.HikvisionISUPSDK
* Avaliable as [nuget](https://www.nuget.org/packages/QuickNV.HikvisionISUPSDK/) 

* [![NuGet Downloads](https://img.shields.io/nuget/dt/QuickNV.HikvisionISUPSDK.svg)](https://www.nuget.org/packages/QuickNV.HikvisionISUPSDK/)

* QuickNV.HikvisionISUPSDK.

说明
```
SDK定义类：QuickNV.HikvisionISUPSDK.Defines
SDK方法类：QuickNV.HikvisionISUPSDK.Methods
SDK错误枚举：QuickNV.HikvisionISUPSDK.Errors
```

示例
```
using QuickNV.HikvisionISUPSDK.Api;
using QuickNV.HikvisionISUPSDK.Api.Rtp;
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
    LinkMode = SmsLinkMode.UDP
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
    Console.WriteLine($"[SMS]DeviceSerial:{e.DeviceSerial},Channel:{e.ChannelId},MediaId:{mediaId},StreamFormat:{e.StreamFormat},StreamType:{e.StreamType}");
}

void SmsContext_PreviewData(object? sender, SmsContextPreviewDataEventArgs e)
{
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
    try
    {
        var streamId = e.StartGetRealStreamV11(smsServerIpAddrss, smsOptions.ListenPort, 1, SmsLinkMode.UDP, SmsStreamType.Sub);
        Console.WriteLine("[CMS]StreamId:" + streamId);
        rtpSender.Connect();
        e.StartPushRealStream(streamId);
    }
    catch (Exception ex)
    {
        Console.WriteLine("发起推流失败。" + ex.ToString());
    }
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
```