using Hikvision.ISUPSDK.Api;
using Newtonsoft.Json;

CmsContext.Init();
var options = new CmsContextOptions()
{
    ListenIPAddress = "0.0.0.0",
    ListenPort = 7660
};
var context = new CmsContext(options);
context.DeviceOnline += Context_DeviceOnline;

void Context_DeviceOnline(object? sender, DeviceInfo e)
{
    Console.WriteLine("设备上线！" + JsonConvert.SerializeObject(e, Formatting.Indented));
}
Console.WriteLine($"正在启动CMS...");
context.Start();
Console.WriteLine($"CMS启动完成。监听端点：{options.ListenIPAddress}:{options.ListenPort}");
Console.ReadLine();
Console.WriteLine($"正在停止CMS...");
context.Stop();
Console.WriteLine($"CMS已停止.");