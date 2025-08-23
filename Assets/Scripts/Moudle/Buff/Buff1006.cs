// 自动生成的Buff类，请勿手动修改
// 如需修改配置，请使用Buff编辑器

using System;
using System.Collections.Generic;
using XJXMgr;
using XJXMoudle;

namespace XJXMoudle
{
    public class Buff1006 : BuffBase
    {
		float addHp;
		// BuffData在构造时初始化
		public Buff1006(Agent caster, Agent target)
		{
			this.caster = caster;
			this.target = target;
			buffData = new BuffData
			{
				BuffTypeId = 1006,
				MaxLv = 1,
				BuffLvType = EBuffLvType.SimLv,
				BuffTickType = EBuffTickType.NoReflash,
				BaseDuration = 10.0f,
				BaseCDTime = 30.0f,
				BaseInterval = 2.0f,
				ConflictType = EBuffConflictType.NoConflict,
				ConflictBuffIds = new List<int> { },
				BuffPriority = 0,
				ReduceLv = 0,
				BuffName = "恢复血量",
				BuffDesc = "持续一段时间内回血",
				BuffIconPath = "",
				BuffEffectPath = ""
			};
			addHp = 5;
		}

        public override void ApplyBuff()
        {
            base.ApplyBuff();
            // TODO: 实现Buff1006的应用逻辑
        }

		public override void EnterBuff(float time)
		{
			base.EnterBuff(time);
			// TODO: 实现Buff1006的进入逻辑
			triggerTime = time + buffData.BaseInterval; 
        }

		public override void UpdateBuff(float time)
		{
			base.UpdateBuff(time);
			// TODO: 实现Buff1006的更新逻辑
			if (Math.Abs(time - triggerTime) <= 0.01f)
			{
				SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
				{
					AgentId = target.agentId,
					PropertyType = PropertyType.Hp,
					Value = addHp * curLv,
					ModifyType = PropertyModifyType.Add,
					Duration = 0,
					SourceBuffId = buffData.BuffTypeId,
					IsProcessed = false,
					CreateTime = time,
				});

				triggerTime = time + buffData.BaseInterval; 
			}
        }

        public override void ExitBuff(float time)
        {
            base.ExitBuff(time);
            // TODO: 实现Buff1006的退出逻辑
        }

        public override void RemoveBuff()
        {
            base.RemoveBuff();
            // TODO: 实现Buff1006的移除逻辑
        }
    }
}
