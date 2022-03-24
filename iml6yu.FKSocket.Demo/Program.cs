// See https://aka.ms/new-console-template for more information
using iml6yu.FKSocket;

Console.WriteLine("Hello, World!");

var client = FKSocketManager.CreateClient(new SocketOption()
{
    Host = "127.0.0.1",
    Port = 30000
}); 

//连接状态
client.ConnectStateChanged += (flag, msg) =>
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"连接状态：{flag},描述信息：{msg}");
    Console.ForegroundColor = ConsoleColor.White;
};

//收到数据
client.Received += (msg) =>
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"接收到信息：{msg}");
    Console.ForegroundColor = ConsoleColor.White;
};

client.Connect().OpenHeartCheck();

string input;
Console.WriteLine("请输入您需要发送的内容！");
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
while ((input = Console.ReadLine()) != "exit")
{
    client.Send(input);
    Console.WriteLine("请输入您需要发送的内容！");
}
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。

client.Dispose();

Console.WriteLine("一切到此结束了！");
