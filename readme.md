# Puzzle Sisters:Foer 谜题姐妹：菲迩

http://store.steampowered.com/app/710190/Puzzle_SistersFoer/

## 目录

```
PuzzleSister  // Unity项目
Steam         // Steam上线环境
SteamCmd      // SteamCmd安装目录
tools         // 题目生成工具
xlsx          // 题目
```

## Unity项目

主要介绍**Assets/PuzzleSister**下的目录

```
Asset         // 资源目录：场景，UI，Prefab
BugFixes      // 兼容Unity自己的bug
Common        // 共用程序
Controller    // 业务逻辑
Data          // 题库数据模块：读取，解析
Editor        // UnityEditor扩展
QEditor       // 题库编辑器代码
Resources     // 动态加载的资源：题库，音频等
Service       // 数据封装层
Settings      // 游戏设置相关
Storage       // 存储相关
View          // 视图层
Voice         // 语音相关
```

## Main场景

![Main Scene](./images/hierarchy.png)

* **CanvasXXX** 是不同的UI模块的Canvas层
* **Controller** 对象放置所有的Controller
* **SoundEffect** 播放音效时使用此对象
* **VoicePlayer** 播放配音时使用此对象
* **IllustrationSettings** 图鉴配置

## UI构建说明

TODO ...

## 题库、DLC与创意工坊

TODO ...

## QEditor

TODO ...

## The End