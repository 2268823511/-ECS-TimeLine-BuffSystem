// 自动生成的Buff类，请勿手动修改
// 如需修改配置，请使用Buff编辑器

using System.Collections.Generic;
using XJXMgr;
using XJXMoudle;

namespace XJXMoudle
{
    public class Buff1008 : BuffBase
    {
		int defalutJumpCount = 1;
		int maxJumpCount;
		// BuffData在构造时初始化
		public Buff1008(Agent caster, Agent target)
		{
			this.caster = caster;
			this.target = target;
			buffData = new BuffData
			{
				BuffTypeId = 1008,
				MaxLv = 1,
				BuffLvType = EBuffLvType.SimLv,
				BuffTickType = EBuffTickType.NoReflash,
				BaseDuration = 15.0f,
				BaseCDTime = 20.0f,
				BaseInterval = 0.0f,
				ConflictType = EBuffConflictType.NoConflict,
				ConflictBuffIds = new List<int> { 1007 },
				BuffPriority = 1,
				ReduceLv = 0,
				BuffName = "三段跳",
				BuffDesc = "一段时间内,拥有三段跳能力",
				BuffIconPath = "",
				BuffEffectPath = ""
			};
			maxJumpCount = 3;
		}

        public override void ApplyBuff()
        {
            base.ApplyBuff();
            // TODO: 实现Buff1008的应用逻辑
        }

		public override void EnterBuff(float time)
		{
			base.EnterBuff(time);
			SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
			{
				AgentId = target.agentId,
				PropertyType = PropertyType.JumpMaxTimes,
				Value = maxJumpCount * curLv,
				ModifyType = PropertyModifyType.Set,
				Duration = 0,
				SourceBuffId = buffData.BuffTypeId,
				IsProcessed = false,
				CreateTime = time,
			});
            // TODO: 实现Buff1008的进入逻辑
		}

        public override void UpdateBuff(float time)
        {
            base.UpdateBuff(time);
            // TODO: 实现Buff1008的更新逻辑
        }

		public override void ExitBuff(float time)
		{
			base.ExitBuff(time);
			SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
			{
				AgentId = target.agentId,
				PropertyType = PropertyType.JumpMaxTimes,
				Value = defalutJumpCount,
				ModifyType = PropertyModifyType.Set,
				Duration = 0,
				SourceBuffId = buffData.BuffTypeId,
				IsProcessed = false,
				CreateTime = time,
			});
            // TODO: 实现Buff1008的退出逻辑
		}

        public override void RemoveBuff()
        {
            base.RemoveBuff();
            // TODO: 实现Buff1008的移除逻辑
        }
    }
}
