# API

## TextPiece
文字操作块

### t
向MessageLayer显示对话文字，可设定角色名，语音与头像
将会自动清除页面

### t(dialog, name, voice, avatar, model)
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

## DialogPiece
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

### OpenDialog(float time = 0.5f)
显示对话框
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| time | `float` | 0.5f | 淡入的时间，默认0.5s |

## EffectPiece

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
该方法即将要被废弃的
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
该方法将调用 SetSprite 默认depth为-1
:::
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` |  | 图片名 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |

### SetMovieBackground(filename, isLoop, x, y, alpha)
设置动态背景
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` |  | 图片名 |
| isLoop | `bool` | `true` | 是否循环播放 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |


### SetCG(spriteName, x, y, alpha)
设置CG（背景）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` |  | 图片名 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |

### SetMovieCG(spriteName, isLoop, x, y, alpha)
设置动态CG（背景）
| Name        | Type    | Default  | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` |  | 图片名 |
| isLoop | `bool` | `true` | 是否循环播放 |
| x | `int` | 0 | x轴坐标 |
| y | `int` | 0 | y轴坐标 |
| alpha | `float` | 1 | 初始透明度 |


## SoundPiece

