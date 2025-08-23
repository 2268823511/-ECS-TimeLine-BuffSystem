using System.Collections.Generic;

namespace XJXMoudle
{
	[System.Serializable]
	public class BuffData
	{
		// 改为属性，支持对象初始化器语法
		public int BuffTypeId { get; set; } //Buff id,不是实例id
		public int MaxLv { get; set; } = 1; //最大层数
		public EBuffLvType BuffLvType { get; set; } = EBuffLvType.SimLv; //Buff等级类型
		public EBuffTickType BuffTickType { get; set; } = EBuffTickType.NoReflash; //Buff触发类型

		public float BaseDuration { get; set; } = 10f; //基础持续时间
		public float BaseCDTime { get; set; } = 0f; //基础冷却时间
		public float BaseInterval { get; set; } = 1f; //基础间隔时间

		public EBuffConflictType ConflictType { get; set; } = EBuffConflictType.NoConflict; //Buff冲突类型
		public List<int> ConflictBuffIds { get; set; } = new List<int>(); //Buff冲突列表
		public int BuffPriority { get; set; } = 0; //Buff优先级
		public int ReduceLv { get; set; } = 0; //减少层数

		#region 表现配置
		public string BuffName { get; set; } = ""; //Buff名称
		public string BuffDesc { get; set; } = ""; //Buff描述
		public string BuffIconPath { get; set; } = ""; //Buff图标路径
		public string BuffEffectPath { get; set; } = ""; //Buff特效路径
		#endregion

		// 无参构造函数
		public BuffData() { }

		// 带参构造函数（可选）
		public BuffData(int buffTypeId, string buffName = "", string buffDesc = "")
		{
			BuffTypeId = buffTypeId;
			BuffName = buffName;
			BuffDesc = buffDesc;
		}
	}

	// public enum EBuffConflictType
	// {
	// 	noConflict,//无冲突
	// 	overrideConflict,//覆盖冲突
	// }

	// public enum EBuffTickType
	// {
	// 	noReflash,//不刷新tick
	// 	needReflash,//需要重新刷新tick
	// }

	// public enum EBuffLvType
	// {
	// 	simLv,//单层
	// 	mulLv,//多层
	// }
}
