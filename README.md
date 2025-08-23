==============================================================================================
采用时间轴+ECS思想的设计模式设计的buff系统,思路来源于此前工作中采用DOTS开发的buff系统,
其中的SystemBase类似于Dots中各种系统,而SystemDataMgr相当于指令中心collter,有一点点类似于DOTS中的Job
各种Mgr则是为Entity和System之间的交互,避免直接饮用。该系统实现了一整套的buff流水线(其实不止是buff如果有其他系统也可以这样编写)
使的各种buff业务不需要直接知道修改的代理或者说实体对象的数据,只需要产生数据指令即可。大致流程如下:
===============================================================================================
游戏系统执行流程
├── 🚀 启动阶段
│   ├── GameRoot.Awake()
│   ├── 注册系统 (AgentSystem, BuffSystem)
│   ├── 初始化所有系统
│   ├── AgentSystem.Start() → 初始化AgentMgr
│   ├── BuffSystem.Start() → 开始管理buff生命周期
│   └── 收集系统接口 (IStart, IUpdate, IFixedUpdate, ILateUpdate)
│
├── 🔄 主循环阶段
│   ├── Update() - 每帧执行
│   │   ├── 🎯 AgentSystem.Update()
│   │   │   ├── updateAgentProperty(time)
│   │   │   │   ├── 从SystemDataMgr获取待处理指令
│   │   │   │   ├── 遍历每个PropertyModifyComponent
│   │   │   │   ├── 根据PropertyType执行修改
│   │   │   │   │   ├── PropertyType.Hp → entity.Hp += value
│   │   │   │   │   ├── PropertyType.Mp → entity.Mp += value
│   │   │   │   │   ├── PropertyType.Attack → entity.Attack += value
│   │   │   │   │   ├── PropertyType.Defense → entity.Defense += value
│   │   │   │   │   ├── PropertyType.SpeedX → entity.SpeedX += value
│   │   │   │   │   ├── PropertyType.SpeedY → entity.SpeedY += value
│   │   │   │   │   ├── PropertyType.Scale → entity.Scale += value
│   │   │   │   │   └── PropertyType.JumpMaxTimes → playerEntity.JumpMaxTimes += value
│   │   │   │   ├── 根据ModifyType执行操作游戏系统执行流程
├── 🚀 启动阶段
│   ├── GameRoot.Awake()
│   ├── 注册系统 (AgentSystem, BuffSystem)
│   ├── 初始化所有系统
│   └── 系统启动 (AgentMgr初始化, BuffSystem开始工作)
│
├── 🔄 主循环阶段 (每帧执行)
│   ├── Update()
│   │   ├── AgentSystem.Update()
│   │   │   └── 处理属性修改指令 → 修改Entity属性
│   │   │
│   │   └── BuffSystem.Update()
│   │       └── 更新所有Buff状态 → 产生属性修改指令
│   │
│   ├── FixedUpdate()
│   │   └── AgentSystem.FixedUpdate()
│   │       └── 更新物理相关 (Transform, 动画, 特效)
│   │
│   └── LateUpdate()
│       └── SystemDataMgr.LateUpdate()
│           └── 清理已处理和过期的指令
│
└── 🎯 外部交互
    ├── 添加Buff: Agent.AddBuff() → BuffMgr → 创建Buff → 进入循环
    └── 数据流: Buff → 指令 → SystemDataMgr → AgentSystem → Entity


===========================================================================================================================
后续会更新和优化,修复buff之类的,目前还有一些小问题
