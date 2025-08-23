
namespace XJXMoudle
{
	/// <summary>
	/// 属性类型枚举
	/// 定义了游戏中可以被修改的各种属性类型
	/// </summary>
	public enum PropertyType
	{
		Hp,         // 生命值
		Mp,         // 魔法值/能量值
		Attack,     // 攻击力
		Defense,    // 防御力
		SpeedX,     // X轴移动速度
		SpeedY,     // Y轴移动速度
		Scale,      // 缩放
		JumpMaxTimes,// 跳跃次数上限
	}

    /// <summary>
    /// 属性修改类型枚举
    /// 定义了属性修改的计算方式
    /// </summary>
    public enum PropertyModifyType
    {
        Add,        // 加法修改: 最终值 = 原值 + 修改值
        Multiply,   // 乘法修改: 最终值 = 原值 * 修改值
        Set         // 直接设置: 最终值 = 修改值 (慎用，会覆盖原值)
    }

    /// <summary>
    /// Buff状态枚举
    /// 定义了Buff在其生命周期中的各个状态
    /// </summary>
    public enum BuffState
    {
        ApplyBuff,  // 添加Buff状态 - Buff刚被添加，还未开始生效
        EnterBuff,  // 进入Buff状态 - Buff开始生效，执行进入逻辑
        UpdateBuff, // 更新Buff状态 - Buff正在持续生效中
        ExitBuff,   // 退出Buff状态 - Buff即将结束，执行退出逻辑
        RemoveBuff  // 移除Buff状态 - Buff被强制移除或自然结束
    }

    /// <summary>
    /// 冷却状态枚举
    /// 用于管理Buff的冷却时间
    /// </summary>
    public enum CoolDownState
    {
        Ready,      // 冷却结束，可以使用
        NotReady    // 冷却中，不能使用
    }

    /// <summary>
    /// Buff冲突类型枚举
    /// 定义了当添加新Buff时如何处理与现有Buff的冲突
    /// </summary>
    public enum EBuffConflictType
    {
        NoConflict,         // 无冲突 - 可以与其他Buff共存
        OverrideConflict    // 覆盖冲突 - 新Buff会移除冲突列表中的旧Buff
    }

    /// <summary>
    /// Buff刷新类型枚举
    /// 定义了重复添加相同Buff时的处理方式
    /// </summary>
    public enum EBuffTickType
    {
        NoReflash,      // 不刷新 - 重复添加时不重置持续时间
        NeedReflash     // 需要刷新 - 重复添加时重置持续时间
    }

    /// <summary>
    /// Buff层级类型枚举
    /// 定义了Buff的叠加规则
    /// </summary>
    public enum EBuffLvType
    {
        SimLv,  // 单层Buff - 不可叠加，同类型只能存在一个
        MulLv   // 多层Buff - 可以叠加，同类型可以存在多个
    }
}