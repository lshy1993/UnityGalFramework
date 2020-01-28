# API

## 文字显示 TextPiece
文字操作相关

### t(dialog, name, voice, avatar, model)
向MessageLayer显示对话文字，可设定角色名，语音与头像
将会自动清除页面
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| dialog | `string` | | 正文 |
| name   | `string` | "" | 角色名字 |
| voice | `string` | "" | 语音文件 |
| avatar | `string` | "" | 头像 |
| model | `int` |    -1| l2d模型编号 |

### t(dialog, name, voice, avatar, model, simpleLogic)
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| dialog | `string` | | 正文 |
| name   | `string` | "" | 角色名字 |
| voice | `string` | "" | 语音文件 |
| avatar | `string` | "" | 头像 |
| model | `int` |    -1| l2d模型编号 | 
| simpleLogic | `Func<int>` | | 简单的逻辑段 | 

### l(dialog, voice, model)
向MessageLayer添加页面文字，可设语音和嘴型
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| dialog | `string` | | 正文 |
| voice | `string` | "" | 语音文件 |
| model | `int` |    -1| l2d模型编号 |

### r()
换行，即添加"\n"

### p()
清空当前显示的所有文字

## 消息层显示 DialogPiece
消息层相关方法

### SetDialogWindow()
设置全屏透明消息层

### SetDialogWindow(size)
设置一定大小的透明消息层
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| size | `Vector2` | | 消息框大小 |

### SetDialogWindow(size, color)
设置一定大小、颜色的消息层
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| size | `Vector2` | | 消息框大小 |
| color | `Color` | | 消息框颜色rgba |

### SetDialogWindow(size, color, pos)
设置一定大小、颜色、位置的消息层
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| size | `Vector2` | | 消息框大小 |
| color | `Color` | | 消息框颜色rgba |
| pos | `Vector2` | | 消息框的位置（锚点：左上） |

### SetDialogWindow(size, color, pos, rect, xint, yint)
设置一定大小、颜色、位置、边距的消息层
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| size | `Vector2` | | 消息框大小 |
| color | `Color` | | 消息框颜色rgba |
| pos | `Vector2` | | 消息框的位置（锚点：左上） |
| rect | `Vector2` | | 文字边距（左上右下） |
| xint | `float` | 0 | 文字间距 |
| yint | `float` | 0 | 文字间距 |

### SetDialogWindow(size, filename)
设置一定大小、颜色的消息层
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| size | `Vector2` | | 消息框大小 |
| filename | `string` | | 消息框图片名 |

### SetDialogWindow(size, filename, pos)
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| size | `Vector2` | | 消息框大小 |
| filename | `string` | | 消息框图片名 |
| pos | `Vector2` | | 消息框的位置（锚点：左上） |

### SetDialogWindow(size, filename, pos, rect, xint, yint)
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| size | `Vector2` | | 消息框大小 |
| filename | `string` | | 消息框图片名 |
| pos | `Vector2` | | 消息框的位置（锚点：左上） |
| rect | `Vector2` | | 文字边距（左上右下） |
| xint | `float` | 0 | 文字间距 |
| yint | `float` | 0 | 文字间距 |

### CloseDialog(time)
隐去对话框
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| time | `float` | 0.5f | 淡出的时间，默认0.5s |

### OpenDialog(time)
显示对话框
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| time | `float` | 0.5f | 淡入的时间，默认0.5s |

## 精灵创建移除相关

### SetSprite(depth, spriteName, x, y, alpha)
设置立绘（带坐标）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` |  | 目标所在的层级 |
| spriteName | `string` |  | 图片名 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |

### SetSprite(depth, spriteName, position, alpha)
::: warning
该方法将在下个版本被废弃
:::
设置立绘（预设位置：左 中 右）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` |  | 目标所在的层级 |
| spriteName | `string` |  | 图片名 |
| position | `string` |  | left,middle,right 左中右 |
| alpha | `float` | 1 | 初始透明度 |

### SetVideoSprite(depth, filename, isLoop, x, y, alpha)
设置动态立绘（带坐标）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` |  | 目标所在的层级 |
| filename | `string` |  | 图片名 |
| isLoop | `bool` | `true` | 是否循环播放 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |

### SetBackground(spriteName, x, y, alpha)
设置背景
::: tip
该方法实际调用 SetSprite （默认depth为-1）
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` |  | 图片名 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |

### SetVideoBackground(filename, isLoop, x, y, alpha)
设置动态背景
::: tip
该方法实际调用 SetVideoSprite （默认depth为-1）
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` |  | 图片名 |
| isLoop | `bool` | `true` | 是否循环播放 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |

### SetCG(spriteName, x, y, alpha)
设置CG（背景层）并解锁CG列表
::: tip
该方法与 SetSprite(-1, spriteName, x, y, alpha) 相同
并向数据写入画廊文件解锁
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` |  | 图片名 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |

### SetVideoCG(spriteName, isLoop, x, y, alpha)
设置动态CG（背景层）并解锁CG列表
::: tip
该方法与 SetVideoSprite(-1, spriteName, isLoop, x, y, alpha) 相同
并向数据写入画廊文件解锁
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` |  | 图片名 |
| isLoop | `bool` | `true` | 是否循环播放 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |

### AddTo(depth, target)
将精灵添加到特定的层
::: warning
delete为false时，相同层级的精灵将会继承原先的位置缩放等信息
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` |  | 选择目标层级 |
| target | `int` |  | 被添加的层级 |

### RemoveSprite(depth, delete)
将精灵删除
::: tip
delete为false时，相同层级的精灵将会继承原先的位置缩放等信息
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` |  | 目标层级 |
| delete | `bool` | `true` | 是否删除图片位置缩放旋转等信息 |

### RemoveBackground(delete)
将背景精灵删除
::: tip
该方法实际调用 RemoveSprite(-1,delete)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` |  | 目标层级 |
| delete | `bool` | `true` | 是否删除图片位置缩放旋转等信息 |

## 渐变相关

### PreTransBackground()
预渐变背景
::: tip
该方法实际调用 PreTransSprite(-1)
:::

### TransBackground(transtime)
渐变背景
::: tip
该方法实际调用 TransSprite(-1)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| transtime | `float` | 0.5f | 渐变时间，默认0.5s |

### TransBackgroundToImage(spriteName, transtime)
快速渐变到新的背景
::: tip
该方法实际调用 TransSpriteToImage(-1, spriteName, transtime)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| spriteName | `string` | | 新图片名 |
| transtime | `float` | 0.5f | 渐变时间，默认0.5s |

### TransBackgroundToMovie(spriteName, transtime, isLoop)
快速渐变到新的动态背景
::: tip
该方法实际调用 TransSpriteToMovie(-1, spriteName, transtime)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| spriteName | `string` | | 新图片名 |
| transtime | `float` | 0.5f | 渐变时间，默认0.5s |
| isLoop | `bool` | `true` | 是否循环影片 |

### TransSpriteToImage(depth, spriteName, x, y, transtime)
快速渐变立绘（xy坐标）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` |  | 目标所在的层级 |
| spriteName | `string` | | 新显示的图片名 |
| x | `float` | | x轴坐标 |
| y | `float` | | y轴坐标 |
| transtime | `float` | 0.5f | 渐变时间，默认0.5s |

### ChangeBackground(spriteName, fadeout, fadein)
变更背景（淡出旧背景+淡入新背景）
::: tip
该方法实际调用 ChangeSprite(-1, spriteName, transtime)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| spriteName | `string` | | 新图片名 |
| fadeout | `float` | 0.5f | 原图淡出的时间，默认0.5s |
| fadein | `float` | 0.5f | 新图淡入的时间，默认0.5s |

### ChangeSprite(depth, spriteName, fadeout, fadein)
变更立绘（淡出旧+淡入新）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标所在的层级 |
| spriteName | `string` | | 新显示的图片名 |
| fadeout | `float` | 0.5f | 原图淡出的时间，默认0.5s |
| fadein | `float` | 0.5f | 新图淡入的时间，默认0.5s |

### PreTransSprite(depth)
预渐变立绘
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 所在层级 |

### TransSprite(depth, transtime)
直接渐变立绘（预设坐标）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 所在层级 |
| transtime | `float` | 0.5f | 渐变时间，默认0.5s |

## Live2d 相关

### SetLive2dSprite(int depth, string modelName, int x = 0,int y = 0, float alpha = 1f)

### FadeinLive2dSprite(int depth, string modelName, int x = 0, int y = 0, float fadein = 0.5f)

### SetLive2dExpression(int depth, int expression)

### SetLive2dMotion(int depth, int mostion)

## Effect 特效相关
精灵图层的特效

### Mask(maskName)
蒙版
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| maskName | `string` | | 蒙版图片名 |

### Blur()
模糊特效

### Mosaic()
马赛克特效

### Gray(depth)
灰度化特效
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级，默认-1 |

### OldPhoto(depth)
老照片效果
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | -1 | 目标层级，默认-1 |

### Mono(int depth, uint colornum)
颜色叠加
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | -1 | 目标层级，默认-1 |
| color | `uint` |  | 色彩代码0x000000形式 |

### Wait(time)
等待特效完成
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| time | `float` | | 时长(s) |

### ClearEffect(depth)
清除图层上的特效
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | -1 | 目标层级，默认-1 |

## Action 相关
精灵动作（移动 缩放 旋转 改变透明度）相关

### FadeSprite(depth, alpha, time, sync)
渐变到指定透明度
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| alpha | `float` | | 透明度（0-1） |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### FadeSpriteBy(depth, alpha, time, sync)
渐变到相对透明度
::: error
该方法还在测试中
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| alpha | `float` | | 透明度（0-1） |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### FadeSpriteFrom(depth, alpha, time, sync)
从指定透明度渐变到当前
::: error
该方法还在测试中
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| alpha | `float` | | 透明度（0-1） |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### FadeBackground(alpha, time, sync)
渐变背景到指定透明度
::: tip
该方法实际调用FadeSprite(-1,alpha,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| alpha | `float` | | 透明度（0-1） |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### FadeBackgroundBy(alpha, time, sync)
渐变背景到相对透明度
::: error
该方法还在测试中
实际调用FadeSpriteBy(-1,alpha,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| alpha | `float` | | 透明度（0-1） |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### FadeBackgroundFrom(alpha, time, sync)
从指定透明度渐变到当前
::: error
该方法还在测试中
实际调用FadeSpriteFrom(-1,alpha,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| alpha | `float` | | 透明度（0-1） |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### MoveSprite(depth, position, time, sync)
移动精灵（从当前位置移动）
::: warning
该方法将在下个版本被废弃
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| position | `string` | | 预设位置 left/middle/right |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### MoveSprite(depth, x, y, time, sync)
移动精灵（从当前坐标开始）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| x | `float` | | 移动终点的x轴坐标 |
| y | `float` | | 移动终点的y轴坐标 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### MoveSpriteBy(depth, x, y, time, sync)
移动精灵（相对当前坐标开始）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| x | `float` | | 移动终点相对x轴坐标 |
| y | `float` | | 移动终点相对y轴坐标 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### MoveSpriteFrom(depth, x_o, y_o, time, sync)
移动精灵（从起点到当前）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| x_o | `float` | | 移动起点x轴坐标 |
| y_o | `float` | | 移动起点y轴坐标 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### MoveSpriteFromTo(depth, x_o, y_o, x, y, time, sync)
移动精灵（指定起点终点）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| x_o | `float` | | 移动终点相对x轴坐标 |
| y_o | `float` | | 移动终点相对y轴坐标 |
| x | `float` | | 移动终点x轴坐标 |
| y | `float` | | 移动终点y轴坐标 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### MoveBackground(x, y, time, sync)
移动背景（从当前位置移动）
::: tip
该方法实际调用 MoveSprite(-1,x,y,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| x | `float` | | 移动终点的x轴坐标 |
| y | `float` | | 移动终点的y轴坐标 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### MoveBackgroundBy(x, y, time, sync)
相对移动背景（从当前坐标开始）
::: tip
该方法实际调用 MoveSpriteBy(-1,x,y,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| x | `float` | | 移动终点相对x轴坐标 |
| y | `float` | | 移动终点相对y轴坐标 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### MoveBackgroundFromTo(x_o, y_o, x, y, time, sync)
移动背景（指定起点终点）
::: tip
该方法实际调用 MoveSpriteFromTo(-1,x_o,y_o,x,y,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| x_o | `float` | | 移动终点相对x轴坐标 |
| y_o | `float` | | 移动终点相对y轴坐标 |
| x | `float` | | 移动终点x轴坐标 |
| y | `float` | | 移动终点y轴坐标 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### RotateSprite(depth, angle, time, sync)
旋转精灵（指定角度）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| angle | `float` | | 旋转角度 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### RotateSpriteBy(depth, angle, time, sync)
旋转精灵（指定相对角度）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| angle | `float` | | 相对旋转角度 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### RotateSpriteFrom(depth, from, to, time, sync)
旋转精灵（从指定角度到）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| from | `float` | | 起始角度 |
| to | `float` | | 结束角度 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### RotateBackground(angle, time, sync)
旋转背景（指定角度）
::: tip
该方法实际调用 RotateSprite(-1,angle,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| angle | `float` | | 旋转角度 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### RotateBackgroundBy(angle, time, sync)
旋转背景（指定相对角度）
::: tip
该方法实际调用 RotateSpriteBy(-1,angle,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| angle | `float` | | 相对旋转角度 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### RotateBackgroundFrom(from, to, time, sync)
旋转背景（从指定角度到）
::: tip
该方法实际调用 RotateSpriteFrom(-1,from,to,time,sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| from | `float` | | 起始角度 |
| to | `float` | | 结束角度 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### ScaleSprite(depth, x, y, time, sync)
缩放精灵（指定xy缩放比率）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| x | `int` | | x方向缩放 |
| y | `int` | | y方向缩放 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### ScaleSprite(depth, scale, time, sync)
缩放精灵（指定缩放倍率）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| scale | `float` | | 缩放倍数 |
| time | `float` | 0 | 时长(s)，默认0 |
| sync | `bool` | false | 是否异步进行 |

### ScaleBackground(x, y, time, sync)
缩放背景（指定xy缩放比率）
::: tip
该方法实际调用 ScaleSprite(-1, x, y, time, sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| x | `int` | | x方向缩放 |
| y | `int` | | y方向缩放 |
| time | `float` | 0 | 时长，默认0 |
| sync | `bool` | false | 是否同步进行 |

### ScaleBackground(scale, time, sync)
缩放背景精灵
::: tip
该方法实际调用 ScaleSprite(-1, scale, time, sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| scale | `float` | | 缩放倍数 |
| time | `float` | 0 | 时长，默认0 |
| sync | `bool` | false | 是否同步进行 |

### SpriteShake()
摇动精灵

### SpriteVibration(depth, v, freq, time, sync)
震动精灵
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | | 目标层级 |
| v | `float` | 10f | 震动量 |
| speed | `int` | 100 | 每秒震动次数 |
| time | `float` | 0.5 | 持续时间(s) |
| sync | `bool` | false | 是否同步进行 |

### WindowVibration(v, freq, time, sync)
窗口抖动（含UI）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| v | `float` | 0.01f | 震动量 |
| speed | `int` | 100 | 每秒震动次数 |
| time | `float` | 0.5 | 持续时间(s) -1则为永久 |
| sync | `bool` | false | 是否同步进行 |

### WaitAction(int depth = -1)
等待异步Action完成

## Movie 影片相关

### PlayMovie(string filename)

## 音频控制 SoundPiece

### PlayBGM(name, vol, fadein, loop)
播放背景音乐
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| name | `string` |  | 音频文件名 |
| vol | `int` | 100 | 音频限制音量 |
| fadein | `float` | 0.5 | 淡入时间(s)，默认0.5 |
| loop | `bool` | true | 是否循环 |

### StopBGM(fadeout)
停止背景音乐
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| fadeout | `float` | 0.5 | 淡出时间(s)，默认0.5 |

### PauseBGM(fadeout)
暂停背景音乐
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| fadeout | `float` | 0 | 淡出时间(s)，默认0 |

### UnpauseBGM(fadein)
继续背景音乐
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| fadeout | `float` | 0 | 淡入时间(s)，默认0 |

### ChangeVolumeBGM(vol, time)
改变背景音乐音量
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| vol | `float` |  | 目标最终音量 |
| time | `float` | 0.5 | 变动时间(s)，默认0.5 |

### PlaySE(name, fadein, loop)
播放音效
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| name | `string` |  | 音频文件名 |
| vol | `int` | 100 | 音频限制音量 |
| fadein | `float` | 0 | 淡入时间(s)，默认0 |
| loop | `bool` | false | 是否循环 |

### StopSE(fadeout)
停止当前正在播放的音效
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| fadeout | `float` | 0 | 淡入时间(s)，默认0 |

### PlayVoice(name, fadein, loop)
播放语音
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| name | `string` |  | 音频文件名 |
| vol | `int` | 100 | 音频限制音量 |
| fadein | `float` | 0 | 淡入时间(s)，默认0 |
| loop | `bool` | false | 是否循环 |

### WaitBGM()
等待BGM播放完毕（loop则跳过）

### WaitSE()
等待SE播放完毕（loop则跳过）

### WaitVoice()
等待语音播放完毕
