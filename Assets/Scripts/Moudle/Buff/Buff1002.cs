// 自动生成的Buff类，请勿手动修改
// 如需修改配置，请使用Buff编辑器

using System;
using System.Collections.Generic;
using UnityEngine;
using XJXMgr;
using XJXMoudle;

namespace XJXMoudle
{
    public class Buff1002 : BuffBase
    {
		float mixAgentSpeed = 2; // 最小代理速度,后续可以换成读配
		float dealutSpeed = 5; // 默认代理速度,后续可以换成读配置
		float delSpeed; // 减速速度

		// BuffData在构造时初始化
		public Buff1002(Agent caster, Agent target)
		{
			this.caster = caster;
			this.target = target;
			buffData = new BuffData
			{
				BuffTypeId = 1002,
				MaxLv = 1,
				BuffLvType = EBuffLvType.SimLv,
				BuffTickType = EBuffTickType.NoReflash,
				BaseDuration = 10.0f,
				BaseCDTime = 15.0f,
				BaseInterval = 0.5f,
				ConflictType = EBuffConflictType.OverrideConflict,
				ConflictBuffIds = new List<int> { 1005 },
				BuffPriority = 0,
				ReduceLv = 0,
				BuffName = "老寒腿",
				BuffDesc = "持续减速玩家,直某个固定速度为止",
				BuffIconPath = "",
				BuffEffectPath = ""
			};

			delSpeed = 0.5f;
		}

        public override void ApplyBuff()
        {
            base.ApplyBuff();
            // TODO: 实现Buff1002的应用逻辑
        }

		public override void EnterBuff(float time)
		{
			base.EnterBuff(time);
			triggerTime = time + buffData.BaseInterval;
            // TODO: 实现Buff1002的进入逻辑
		}

		public override void UpdateBuff(float time)
		{
			base.UpdateBuff(time);
			// TODO: 实现Buff1002的更新逻辑
			if (Math.Abs(time - triggerTime) <= 0.01f && target.entity.SpeedX > mixAgentSpeed)
			{
				SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
				{
					AgentId = target.agentId,
					PropertyType = PropertyType.SpeedX,
					Value = - (delSpeed * curLv),
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
			SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
			{
				AgentId = target.agentId,
				PropertyType = PropertyType.SpeedX,
				Value = dealutSpeed,
				ModifyType = PropertyModifyType.Set,
				Duration = 0,
				SourceBuffId = buffData.BuffTypeId,
				IsProcessed = false,
				CreateTime = time,
			});
            base.ExitBuff(time);
            // TODO: 实现Buff1002的退出逻辑
        }

		public override void RemoveBuff()
		{
			base.RemoveBuff();
            // TODO: 实现Buff1002的移除逻辑
		}
    }
}
