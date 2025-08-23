// 自动生成的Buff类，请勿手动修改
// 如需修改配置，请使用Buff编辑器

using System;
using System.Collections.Generic;
using UnityEngine;
using XJXMgr;
using XJXMoudle;

namespace XJXMoudle
{
    public class Buff1004 : BuffBase
    {
		float minAgentScale = 0.5f;
		float dealutAgentScale = 1.0f;
		float addAgentScale;
		// BuffData在构造时初始化
		public Buff1004(Agent caster, Agent target)
		{
			this.caster = caster;
			this.target = target;
			buffData = new BuffData
			{
				BuffTypeId = 1004,
				MaxLv = 1,
				BuffLvType = EBuffLvType.SimLv,
				BuffTickType = EBuffTickType.NoReflash,
				BaseDuration = 20.0f,
				BaseCDTime = 25.0f,
				BaseInterval = 0.3f,
				ConflictType = EBuffConflictType.OverrideConflict,
				ConflictBuffIds = new List<int> { 1003 },
				BuffPriority = 0,
				ReduceLv = 0,
				BuffName = "变小2倍",
				BuffDesc = "线性变化体型到原来的两倍,持续一段时间后退出",
				BuffIconPath = "",
				BuffEffectPath = ""
			};
			addAgentScale = 0.5f;
		}

        public override void ApplyBuff()
        {
            base.ApplyBuff();
            // TODO: 实现Buff1004的应用逻辑
        }

		public override void EnterBuff(float time)
		{
			base.EnterBuff(time);
			triggerTime = time + buffData.BaseInterval;
            // TODO: 实现Buff1004的进入逻辑
		}

		public override void UpdateBuff(float time)
		{
			base.UpdateBuff(time);
			// TODO: 实现Buff1004的更新逻辑
			if (Math.Abs(time - triggerTime) <= 0.01f && target.entity.EntityGO.GetComponent<Transform>().localScale.x > minAgentScale)
			{
				SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
				{
					AgentId = target.agentId,
					PropertyType = PropertyType.Scale,
					Value = -(addAgentScale * curLv),
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
			SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
			{
				AgentId = target.agentId,
				PropertyType = PropertyType.Scale,
				Value = dealutAgentScale * curLv,
				ModifyType = PropertyModifyType.Set,
				Duration = 0,
				SourceBuffId = buffData.BuffTypeId,
				IsProcessed = false,
				CreateTime = time,
			});
            // TODO: 实现Buff1004的退出逻辑
		}

		public override void RemoveBuff()
		{
			base.RemoveBuff();
            // TODO: 实现Buff1004的移除逻辑
		}
    }
}
