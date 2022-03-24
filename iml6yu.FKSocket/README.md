@[TOC]

# FKSocket
一个超级轻量的socket类库，一切才刚刚开始，还有很多工作要做。

## Clients
客户端，包含客户端的连接，心跳等方法
 
### FKSocketManage
客户端管理对象
用法
```csharp
//创建一个客户端对象
var client = FKSocketManager.CreateClient(new SocketOption()
{
    Host = "127.0.0.1",
    Port = 30000
});
```

### FKSocketClient
客户端对象，创建后需要关注几个事件

```csharp
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

```

*连接客户端*

```charp
client.Connect();
```

*启用心跳管理*

```csharp
client.OpenHeartCheck();
```

简写

```charp
client.Connect().OpenHeartCheck();
```

## 完整demo代码
```charp
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

```

