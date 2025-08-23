// 自动生成的Buff类，请勿手动修改
// 如需修改配置，请使用Buff编辑器

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using XJXMgr;
using XJXMoudle;

namespace XJXMoudle
{
	public class Buff1001 : BuffBase
	{
		float delHp;

		public Buff1001(Agent caster, Agent target)
		{
			this.caster = caster;
			this.target = target;
			buffData = new BuffData
			{
				BuffTypeId = 1001,
				MaxLv = 5,
				BuffLvType = EBuffLvType.MulLv,
				BuffTickType = EBuffTickType.NoReflash,
				BaseDuration = 10.0f,
				BaseCDTime = 30.0f,
				BaseInterval = 1f,
				ConflictType = EBuffConflictType.NoConflict,
				ConflictBuffIds = new List<int> { },
				BuffPriority = 0,
				ReduceLv = 0,
				BuffName = "炼金の毒液",
				BuffDesc = "让玩家持续中毒,扣除Hp",
				BuffIconPath = "",
				BuffEffectPath = ""
			};

			delHp = 5;
		}

		public override void ApplyBuff()
		{
			base.ApplyBuff();
			// TODO: 实现Buff1001的应用逻辑
		}

		public override void EnterBuff(float time)
		{
			base.EnterBuff(time);
			triggerTime = time + buffData.BaseInterval;
			Debug.Log("Buff1001: 进入中毒状态"+triggerTime);
			// TODO: 实现Buff1001的进入逻辑
		}

		public override void UpdateBuff(float time)
		{

			base.UpdateBuff(time);
			if (Math.Abs(time - triggerTime) <= 0.01f)
			{
				SystemDataMgr.Instance.RequestPropertyModify(new PropertyModifyComponent
				{
					AgentId = target.agentId,
					PropertyType = PropertyType.Hp,
					Value = - (delHp * curLv),
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
			// TODO: 实现Buff1001的退出逻辑
		}


		public override void RemoveBuff()
		{
			base.RemoveBuff();
			// TODO: 实现Buff1001的移除逻辑
		}
				
    }
}
