// 自动生成的Buff类，请勿手动修改
// 如需修改配置，请使用Buff编辑器

using System;
using System.Collections.Generic;
using XJXMgr;
using XJXMoudle;

namespace XJXMoudle
{
    public class Buff1005 : BuffBase
    {

		float maxAgentSpeedX = 10; // 最大代理速度,后续可以换成读配
		float dealutSpeedX = 5; // 默认代理速度,后续可以换成读配置
		float addSpeedX; // 加速速度
						 // BuffData在构造时初始化
		public Buff1005(Agent caster, Agent target)
		{
			this.caster = caster;
			this.target = target;
			buffData = new BuffData
			{
				BuffTypeId = 1005,
				MaxLv = 1,
				BuffLvType = EBuffLvType.SimLv,
				BuffTickType = EBuffTickType.NoReflash,
				BaseDuration = 15.0f,
				BaseCDTime = 18.0f,
				BaseInterval = 3.0f,
				ConflictType = EBuffConflictType.OverrideConflict,
				ConflictBuffIds = new List<int> { 1005 },
				BuffPriority = 1,
				ReduceLv = 0,
				BuffName = "疾跑",
				BuffDesc = "每间隔一定时间增加一定速度,增加固定次数后,固定速度,退出恢复",
				BuffIconPath = "",
				BuffEffectPath = ""
			};

			addSpeedX = 2.5f;
		}

        public override void ApplyBuff()
        {
            base.ApplyBuff();
            // TODO: 实现Buff1005的应用逻辑
        }

		public override void EnterBuff(float time)
		{
			base.EnterBuff(time);
			triggerTime = time + buffData.BaseInterval;
            // TODO: 实现Buff1005的进入逻辑
		}

		public override void UpdateBuff(float time)
		{
			base.UpdateBuff(time);
			if (Math.Abs(time - triggerTime) <= 0.01f&&target.entity.SpeedX < maxAgentSpeedX)
			{
				SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
				{
					AgentId = target.agentId,
					PropertyType = PropertyType.SpeedX,
					Value = addSpeedX * curLv,
					ModifyType = PropertyModifyType.Add,
					Duration = 0,
					SourceBuffId = buffData.BuffTypeId,
					IsProcessed = false,
					CreateTime = time,
				});

				triggerTime = time + buffData.BaseInterval; 
			}
            // TODO: 实现Buff1005的更新逻辑
		}

		public override void ExitBuff(float time)
		{
			base.ExitBuff(time);
			SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
			{
				AgentId = target.agentId,
				PropertyType = PropertyType.SpeedX,
				Value = dealutSpeedX * curLv,
				ModifyType = PropertyModifyType.Set,
				Duration = 0,
				SourceBuffId = buffData.BuffTypeId,
				IsProcessed = false,
				CreateTime = time,
			});
            // TODO: 实现Buff1005的退出逻辑
		}

        public override void RemoveBuff()
        {
            base.RemoveBuff();
            // TODO: 实现Buff1005的移除逻辑
        }
    }
}
