# 玩家系统设置说明

## 🎯 架构概述

现在你的系统架构如下：
- **GameManager**: 在GameRoot中管理玩家创建和系统更新
- **EntityPlayer**: 集成了PlayerMove的移动逻辑和buff系统
- **PlayerController**: Unity组件，处理碰撞检测
- **PlayerInfo**: UI界面，连接到GameManager获取玩家引用

## 🔧 场景设置步骤

### 1. 创建GameManager
- 在场景中创建空的GameObject，命名为"GameManager"
- 添加`GameManager`组件
- 设置`autoUpdateBuffs = true`

### 2. 设置PlayerRoot（可选）
有两种方式：

**方式A：使用预制体**
- 创建玩家预制体，包含：
  - Rigidbody2D
  - BoxCollider2D
  - 子物体包含SpriteRenderer和Animator
- 拖拽预制体到GameManager的`playerRootPrefab`字段

**方式B：场景中已有PlayerRoot**
- 确保场景中有名为"PlayerRoot"的GameObject
- GameManager会自动找到并使用它

**方式C：自动创建**
- 什么都不设置，GameManager会自动创建默认的PlayerRoot

### 3. 设置生成点（可选）
- 创建空的GameObject作为生成点
- 拖拽到GameManager的`playerSpawnPoint`字段

### 4. 设置UI
- 确保场景中有PlayerInfo组件
- 它会自动找到GameManager并连接

## 🎮 运行效果

启动游戏后：
1. GameManager自动创建玩家实体
2. 玩家可以用WASD/方向键移动，空格键跳跃
3. 点击UI按钮添加buff到玩家
4. 玩家属性受buff影响（速度、跳跃、缩放等）
5. UI实时显示玩家状态和激活的buff

## 🧪 测试功能

在GameManager组件上右键菜单：
- "手动更新Buff" - 手动触发buff更新
- "测试添加Buff1001" - 添加光圈buff
- "测试减血" - 减少玩家血量
- "测试加速" - 增加玩家移动速度

在PlayerInfo组件上右键菜单：
- "刷新玩家数据" - 手动刷新UI显示

## 🎯 buff功能说明

- **Buff1001**: 光圈buff，30秒持续，0.5秒间隔触发
- **Buff1002**: 跳高buff，Y轴速度×2，3次跳跃
- **Buff1003**: 二连跳buff，永久生效
- **Buff1004**: 变小buff，缩放为0.5，30秒
- **Buff1005**: 中毒buff，多层叠加，每秒掉血
- **Buff1006**: 加血buff，立即回复50血
- **Buff1007**: 减速buff，2层减速，间隔3秒
- **Buff1008**: 恢复速度buff，立即恢复正常速度
- **Buff1009**: 加速buff，两次触发，间隔3秒

所有buff都会在UI中实时显示状态、层级和tick信息！







