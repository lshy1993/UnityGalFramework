# API

## TextPiece
文字操作块

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

## DialogPiece 消息显示
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
该方法实际调用 SetSprite(-1, spriteName, x, y, alpha)
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
该方法实际调用 SetVideoSprite(-1, spriteName, isLoop, x, y, alpha)
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

<param name="maskName">蒙版图</param>

### Blur()
模糊特效

### Mosaic()
马赛克特效

### Gray(int depth = -1)
灰度化特效

### OldPhoto(int depth = -1)
老照片效果

### Mono(int depth, uint colornum)
颜色叠加

### Wait(float time)
等待特效完成
        /// </summary>
        /// <param name="time">时长s</param>

### ClearEffect(int depth = -1)
清除图层上的特效


## Action 相关
精灵动作（移动 缩放 旋转 改变透明度）相关

### FadeSprite(int depth, float alpha, float time = 0, bool sync = false)

/// 渐变到指定透明度
/// </summary>
/// <param name="depth">操作图层</param>
/// <param name="alpha">目标透明度</param>
/// <param name="time">时间，默认0</param>

### FadeSpriteBy(int depth, float alpha, float time = 0, bool sync = false)

### FadeSpriteFrom(int depth, float alpha, float time = 0, bool sync = false)

### FadeBackground(float alpha, float time = 0, bool sync = false)

### FadeBackgroundBy(float alpha, float time = 0, bool sync = false)

### FadeBackgroundFrom(float alpha, float time = 0, bool sync = false)

### MoveSprite(int depth, string position, float time = 0)
::: warning
该方法将在下个版本被废弃
:::
/// <summary>
/// 移动立绘（从当前位置移动）
/// </summary>
/// <param name="depth">目标所在层级</param>
/// <param name="position">left | middle | right</param>
/// <param name="time">移动时间，默认0.5s</param>

### MoveSprite(int depth, float x, float y, float time = 0, bool sync = false)
/// <summary>
/// 移动立绘（从当前坐标开始）
/// </summary>
/// <param name="depth">目标所在的层级</param>
/// <param name="x">目标位置的x轴坐标</param>
/// <param name="y">目标位置的y轴坐标</param>
/// <param name="time">移动的时间，默认0.5s</param>

### MoveSpriteBy(int depth, float x, float y, float time = 0, bool sync = false)
/// <summary>
/// 相对移动立绘（从当前坐标开始）
/// </summary>
/// <param name="depth">目标所在的层级</param>
/// <param name="x">目标位置的x轴坐标</param>
/// <param name="y">目标位置的y轴坐标</param>
/// <param name="time">移动的时间，默认0.5s</param>
/// <param name="sync">是否等待完成（异步）</param>

### MoveSpriteFrom(int depth, float x_o, float y_o, float x, float y, float time = 0, bool sync = false)
/// <summary>
/// 移动立绘（指定起点终点）
/// </summary>
/// <param name="depth">目标所在的层级</param>
/// <param name="x_o">移动起点的x轴坐标</param>
/// <param name="y_o">移动起点的y轴坐标</param>
/// <param name="x">目标位置的x轴坐标</param>
/// <param name="y">目标位置的y轴坐标</param>
/// <param name="time">移动的时间，默认0.5s</param>
/// <param name="sync">是否等待完成（异步）</param>

### MoveBackground(float x, float y, float time = 0, bool sync = false)
/// <summary>
/// 移动背景（从当前坐标开始）
/// </summary>
/// <param name="x">移动终点的x轴坐标</param>
/// <param name="y">移动终点的y轴坐标</param>
/// <param name="time">移动的时间，默认0.5s</param>
/// <param name="sync">是否等待完成（异步）</param>

### MoveBackgroundBy(float x, float y, float time = 0, bool sync = false)

/// <summary>
/// 相对移动背景（从当前坐标开始）
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
/// <param name="time"></param>
/// <param name="sync"></param>
/// <returns></returns>

### MoveBackgroundFrom(float x_o, float y_o, float x, float y, float time = 0, bool sync = false)

/// <summary>
/// 移动背景（指定起点终点）
/// </summary>
/// <param name="x_o">移动起点的x轴坐标</param>
/// <param name="y_o">移动起点的y轴坐标</param>
/// <param name="x">目标位置的x轴坐标</param>
/// <param name="y">目标位置的y轴坐标</param>
/// <param name="time">移动的时间，默认0.5s</param>
/// <param name="sync">是否等待完成（异步）</param>

### RotateSprite(int depth, float angle, float time = 0, bool sync = false)

### RotateSpriteBy(int depth, float angle, float time = 0, bool sync = false)

### RotateSpriteFrom(int depth, float from, float to, float time = 0, bool sync = false)

### RotateBackground(float angle, float time = 0, bool sync = false)

### RotateBackgroundBy(float angle, float time = 0, bool sync = false)

### RotateBackgroundFrom(float from, float to, float time = 0, bool sync = false)

### ScaleSprite(int depth, int x, int y, float time = 0, bool sync = false)
/// <summary>
/// 缩放精灵
/// </summary>
/// <param name="depth">目标层级</param>
/// <param name="x">x方向缩放</param>
/// <param name="y">y方向缩放</param>
/// <param name="time">时长，默认0</param>
/// <param name="sync">是否同步进行</param>

### ScaleSprite(int depth, float scale, float time = 0, bool sync = false)
缩放精灵
/// <param name="depth">目标层级</param>
/// <param name="scale">缩放倍数</param>
/// <param name="time">时长，默认0</param>
/// <param name="sync">是否同步进行</param>

### ScaleBackground(int x, int y, float time = 0, bool sync = false)
缩放背景精灵
::: tip
该方法实际调用 ScaleSprite(-1, x, y, time, sync)
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| x | `int` | | x方向缩放 |
| y | `int` | | y方向缩放 |
| time | `float` | 0 | 时长，默认0 |
| sync | `bool` | false | 是否同步进行 |

### ScaleBackground(float scale, float time = 0, bool sync = false)
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
立绘摇动

### SpriteVibration(int depth, float v=10f, int freq = 100 , float time=0.5f, bool sync=true)
        /// <summary>
        /// 立绘震动
        /// </summary>
        /// <param name="depth">震动对象</param>
        /// <param name="v">震动量</param>
        /// <param name="speed">每秒震动次数</param>
        /// <param name="time">持续时间</param>

### WindowVibration(float time = 0.5f, float v = 0.01f, int freq = 100, bool sync = true)
        /// <summary>
        /// 窗口抖动（含UI）
        /// </summary>
        /// <param name="time">持续时间s，0则为永久</param>
        /// <param name="v">震动量</param>
        /// <param name="freq">每秒震动次数</param>

### WaitAction(int depth = -1)
等待异步Action完成

## Movie 影片相关

### PlayMovie(string filename)

## SoundPiece

