# Development

## 介绍
最基础的【指令】列表  
UGF是基于这些指令对舞台上的精灵进行操作
::: warning
API中的所有操作，均是对基础指令的排列组合  
脚本语言解析器（如Krkr Parser）会根据脚本直接生成这些指令
:::

## 精灵图层相关操作

### PreTransAll()

### TransAll(float time)

### SetSpriteByDepth(int depth, string fname, bool cg = false)
加载一个精灵，并生成GameObject
| Name        | Type    | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | 唯一层级 |
| fname | `string` | 目标文件名 |
| cg | `bool` | 是否从CG文件夹中读取 |
::: details
如果depth小于0，则默认从Background文件夹中寻找
如果cg=true，则从CG文件夹中寻找
否则根据 fname 全路径
:::

### SetMovieSprite(int depth, string fname, bool isLoop = false)
加载一个动态精灵，并生成GameObject
| Name        | Type    | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | 唯一层级 |
| fname | `string` | 目标文件名 |
| isLoop | `bool` | 是否循环播放 |
::: details
动态精灵采用RawImage配合VideoPlayer和一个RenderedTexture
:::

### SetPostionByDepth(int depth, Vector3 position)
设定精灵的位置
| Name        | Type    | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | 唯一层级 |
| position | `Vector3` | 坐标 |

### SetDefaultPostionByDepth(int depth, string pstr)
自动设定精灵的位置
::: warning
该方法将在下一个版本中废弃
:::

### SetAlphaByDepth(int depth, float alpha)
设定精灵的透明度
| Name        | Type    | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | 唯一层级 |
| alpha | `float` | 透明度 |
::: details
修改Image的Color属性
:::

### RemoveSpriteByDepth(int depth, bool delete)
移除一个精灵
::: details
delete为true时才Destory该GameObject
否则只将精灵隐藏
:::

### SetLive2dSpriteByDepth(int depth, string modelName)
加载一个live2d精灵，并生成GameObject
| Name        | Type    | Description  |
| ------------- |:-------------|:-----|:-----|
| depth | `int` | 唯一层级 |
| fname | `string` | 目标文件名 |
| isLoop | `bool` | 是否循环播放 |
::: details
live2d精灵采用RawImage配合一个Camera与RenderedTexture
:::

### ChangeLive2dExpression(int depth, int expre)
改变Live2d的表情


## Trans类

### PreTransByDepth(int depth)

### PreTransByDepth(int depth, string sprite, Vector3 postision)

### PreTransByDepth(int depth, string sprite, string postision)
::: warning
该方法将在下一个版本中废弃
:::

### TransByDepth(int depth, float time)

### RotateFade(string sprite, bool inverse, float time)

### SideFade(string sprite, string direction, float time)

### Circle(string sprite, bool inverse, float time)

### Scroll(string sprite, string direction, float time)

### ScrollBoth(string sprite, string direction, float time)

### Universial(string sprite, string rule, float time)

### Shutter(string sprite, string direction, float time)

## Effect类
Effect类指令，均会生成一个Shader应用于Image/RawImage上

### Masking(string mask)

### Blur()

### Mosaic()

### Gray()

### OldPhoto()

## Action类
Action类指令，均会生成一个SpriteTweener挂载于Image/RawImage上

### FadeToByDepth(int depth, float finalalpha, float time, bool sync)

### ScaleToByDepth(int depth, Vector3 scale, float time, bool sync)

### RotateToByDepth(int depth, Vector3 angle, float time, bool sync)

### MoveByDepth(int depth, Vector3 final, float time, bool sync)

### MoveDefaultByDepth(int depth, string pstr, float time)
::: warning
该方法将在下一个版本中废弃
:::

### SpriteShakeByDepth(int depth, float v, int freq, float time, bool sync)

### WindowShake(float v, int freq, float time, bool sync)

## 其他类

### FadeOutAll(float time)

### FadeOutAllChara(float time)

### FadeOutAllPic(float time)

### AddSpriteTo(int depth, int target)

### Remove(int depth, bool delete)

### RemoveAll()

### RemoveAllChara()

### RemoveAllPic()

### Wait(time)
一个等待块
| Name        | Type    | Description  |
| ------------- |:-------------|:-----|:-----|
| time | `float` | 等待的时间 |
::: details
产生一个WaitForSeconds(time)的Couroutine
:::

### UnlockGallery(filename)
解锁画廊
| Name        | Type    | Description  |
| ------------- |:-------------|:-----|:-----|
| filename | `string` | 唯一文件名 |
::: details
将filename加入CG Table中
:::