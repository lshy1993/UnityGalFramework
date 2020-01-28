# 快速上手
UGF还处于开发阶段，对可能出现的bug，原作者决定跑路，概不负责。

## 介绍
::: warning 辣眼睛注意  
本框架是作者非常业余的个人作品，最早用于个人学习unity编写。  
源码包含大量的垃圾代码和冗余结构，请大佬们高抬贵手。
:::

## 安装
本框架提供2种安装方式，请任意选择

### Unity Assets Store
下载unitypackage直接导入所有内容

### Github
Git Clone后，置于unity标准工程的相同位置

## 导入Unity
导入完成后，会在Assets文件夹内生成对应的文件夹

其中Scripts为代码存放位置
| Folder | Description  |
| ------------- |:------------- |
| Framework | 框架主要核心代码，也可以自行修改 |
| Scenario | 游戏脚本文件 |

## 开始运行
打开Scenes文件夹下的MainScene，按下运行按钮或者F5启动即可

## 修改脚本
打开默认的入口文件demo0_0.cs文件  
List内即为执行命令  
编写对应脚本即可在运行后看到效果

## 文字

### 全屏框
在脚本文件内写入如下代码，运行游戏，点击NewGame

``` csharp
// 显示默认的对话框
f.OpenDialog(),
// 显示一段文字
f.l("这是一段文字"),
f.l("这是第二段文字"),
// 换行
f.r(),
f.l("这是第三段文字"),
// 清空当前文字
f.p(),
f.l("这是新的一页文字"),
// 关闭对话框
f.CloseDialog(),
f.l("这个文字是看不见的"),
```

可以看到，文字顺利的显示在了屏幕上。  
这里你会看到几种指令的用法。

- [OpenDialog 显示消息层](../api/#opendialog-time)  
`OpenDialog`是将消息层显示的函数，可以带一个参数淡入时长，默认为0.5秒。  

- [l 显示文字](../api/#l-dialog-voice-model)  
`l`是显示文字的函数，将一整句文字显示在屏幕上。  
你也许发现了，l函数是不会对文字进行换行的，当然你可以在字符串内加入`\n`实现，但这并不在讨论范围内。  

- [r 换行](../api/#r)  
`r`会将文字进行换行，接下来的文字将会从下一行开头开始显示。  

- [p 清空文字](../api/#p)  
`p`将会消除当前屏幕上所有文字。  

- [CloseDialog 关闭消息层](../api/#closedialog-time)  
`CloseDialog`的作用是关闭消息层，可以带一个参数淡出时长，默认为0.5秒。  

::: warning 注意  
所有的文字，只有在消息层打开的时候才可以看见，否则是看不见的。
:::

::: details 详细原理  
暂停Unity，查看检视器，你会看到所有的文字都显示在了消息层的Text组件上。  
其中，`OpenDialog`和`CloseDialog`将会控制MessagePanel父物体的显示与否。  
因此，必须确认消息层处于打开的情况下，才能执行显示文字。  
:::

但是如果想显示人物的姓名该怎么办呢？

### 单独对话框
在脚本文件内写入如下代码，运行游戏，点击NewGame

``` csharp
// 设置对话框样式
f.SetDialogWindow(new Vector2(1920,280), Color.White, new Vector2(0,1000)),
// 显示默认的对话框
f.OpenDialog(),
// 显示文字
f.t("这是一句话","a"),
f.t("这是第二句话","b"),
// 关闭对话框
f.CloseDialog()
```

可以看到，文字和人物的姓名顺利的显示在了屏幕上。  
这里你又会看到新的指令的用法。

- [SetDialogWindow 设置对话框的样式](../api/#setdialogwindow)  
`SetDialogWindow`是一个多重载的函数，详情请参考API相关页面。  
这里创建了一个大小为 1920*280，白色为背景色，位于(0,1000)的对话框。  

- [t 显示文字和姓名](../api/#t-dialog-name-voice-avatar-model)  
`t`模式与`l`模式不同，t方法显示的文字将会带有姓名框  
而且会在鼠标点击后，自动清空，不再需要`p`指令

::: details 详细原理  
单独对话框和全屏对话框使用的是同一个GameObject。  
当检测到使用的是`t`指令后，将会自动开启姓名框并填入姓名。  
当然，如果传入一个空的姓名，则不会显示姓名框。  
:::

## 图像

### 显示精灵
精灵是所有显示图像的基础形式，无论是背景还是人物。  
在脚本文件内写入如下代码  

``` csharp
// 设置背景图片
f.SetBackground("classroom"),
// 等待2秒
f.Wait(2),
// 显示0层立绘
f.SetSprite(0,"girl",200,300),
f.Wait(2),
// 删除0层立绘
f.RemoveSprite(0),
f.Wait(2),
// 删除背景图片
f.RemoveBackground()
```

可以看到在顺利显示背景图片2秒后，又显示了可爱的立绘，再过2秒立绘间消失，最后背景图片消失。  

- [SetBackground 设置背景图片](../api/#setbackground-spritename-x-y-alpha)  
`SetBackground`指令可以将图片文件显示在舞台上。  
该指令的参数只需带有**文件名**而不需要**后缀名**，默认会寻找*Resources/Background*文件夹下的图片文件。  
另外，这里隐藏了另外2个参数，x和y，默认值为(0,0)。其含义为图片的坐标，由于是背景图片，这里使用了默认值。  

::: danger 限制  
由于Unity限制，请勿使用相同的文件名，即使文件的后缀名不同。
::: 

::: tip 关于坐标系  
x轴为水平轴，0则代表距离**屏幕左侧**为0。  
y轴为垂直轴，0则代表距离**屏幕上端**为0。  
计算坐标时，图片的锚点均为左上角。  
:::

- [SetSprite 设置精灵](../api/#setsprite-depth-spritename-x-y-alpha)  
`SetSprite`指令同样可以将图片文件显示在舞台上。  
与`SetBackground`不同的是，`SetSprite`需要先指定一个**唯一**的整数作为**层级数**。  
同时，这里的文件名**需要带有路径**，默认寻找*Resources*文件夹下的图片。  

::: tip 层级数  
层级意味着距离屏幕的远近，**数字越大，越靠近屏幕**。  
你可能突然疑惑，如果设置层级数为负数，会发生什么？  
在这里，我们约定**背景图片的层级数为负数，默认的单一背景的层级数为-1**。  
因此，`SetBackground`等同于`SetSprite(-1)`。  
:::

::: warning 注意  
对于层级数为**负数**的`SetSprite`指令，与`SetBackground`相同，默认寻找*Resources/Background*文件夹下的图片文件。
::: 

- [Wait 等待](../api/#wait-time)  
`Wait`指令可以让脚本执行暂停一段时间，之后再继续后面的指令。  
**另外，在等待期间，任何交互均是锁定的。**

- [RemoveSprite 删除指定精灵](../api/#removesprite-depth-delete)  
与`SetSprite`指令相反，`RemoveSprite`可以让精灵图片消失。  
这里需要指定Set时候指定的层级数，如果带入了一个错误的层级数，那么这个操作将会跳过，并在控制台显示报错信息。

- [RemoveBackground 删除背景](../api/#removebackground-delete)  
同样与`SetBackground`指令相反，`RemoveBackground`可以让背景图片消失。  
背景图片的层级数为负数，因此你也可以使用`RemoveSprite(-1)`来删除背景。  

在这里，所有的图片都是**瞬间**消失的，这是正常的。~~并不是什么Bug~~  
因为`Set`和`Remove`都是即时的操作。  
如果想要让图片不是那么快的显示/消失，只需要再加入几行命令。  

### 渐变
修改脚本文件为以下代码

``` csharp
f.PretransAll(),
f.SetBackground("classroom"),
f.TransAll(),
f.Wait(2),
f.PretransAll(),
f.SetBackground("bedroom"),
f.TransAll(),
f.Wait(2),
f.PretransAll(),
f.RemoveBackground(),
f.TransAll()
```

你可看到，与之前不同的是，这次的图片是以渐变的形式显示在屏幕上的。  

- [PretransAll](../api/#pretransall)  
将当前所有的图片进行预渐变。 

- [TransAll](../api/#transall)  
渐变到新的图片。 

### 动作

### 特效

## 声音

## 高阶