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
//默认10s检测时间
client.OpenHeartCheck();
```
*启用断线重连*

```csharp
client.Connect().OpenHeartCheck()
    //打开断线重连
    .OpenAutoReConnection(3);
```
断线重连效果
![在这里插入图片描述](https://img-blog.csdnimg.cn/f92cb91a832645278e35175343df760a.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBAaW1sNnl1,size_20,color_FFFFFF,t_70,g_se,x_16)


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

## 效果
连接
![在这里插入图片描述](https://img-blog.csdnimg.cn/d31a8cab682d4864a413470674bbf038.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBAaW1sNnl1,size_20,color_FFFFFF,t_70,g_se,x_16)


![在这里插入图片描述](https://img-blog.csdnimg.cn/0197ef2dbbcd41bc9a89cbebcdc26667.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBAaW1sNnl1,size_20,color_FFFFFF,t_70,g_se,x_16)
![在这里插入图片描述](https://img-blog.csdnimg.cn/46559d188e424db8a17ebcd29aecfeb7.png?x-oss-process=image/watermark,type_d3F5LXplbmhlaQ,shadow_50,text_Q1NETiBAaW1sNnl1,size_20,color_FFFFFF,t_70,g_se,x_16)
