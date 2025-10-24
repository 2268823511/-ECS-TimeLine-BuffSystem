// 自动生成的Buff类，请勿手动修改
// 如需修改配置，请使用Buff编辑器

using System.Collections.Generic;
using XJXMoudle;

namespace XJXMoudle
{
    public class Buff1009 : BuffBase
    {
        // BuffData在构造时初始化
        public Buff1009(Agent caster, Agent target)
        {
			this.caster = caster;
			this.target = target;
            buffData = new BuffData
            {
                BuffTypeId = 1009,
                MaxLv = 1,
                BuffLvType = EBuffLvType.SimLv,
                BuffTickType = EBuffTickType.NoReflash,
                BaseDuration = 10.0f,
                BaseCDTime = 0.0f,
                BaseInterval = 1.0f,
                ConflictType = EBuffConflictType.NoConflict,
                ConflictBuffIds = new List<int> {  },
                BuffPriority = 0,
                ReduceLv = 0,
                BuffName = "Buff1009",
                BuffDesc = "Buff描述",
                BuffIconPath = "",
                BuffEffectPath = ""
            };
        }

        public override void ApplyBuff()
        {
            base.ApplyBuff();
            // TODO: 实现Buff1009的应用逻辑
        }

        public override void EnterBuff(float time)
        {
            base.EnterBuff(time);
            // TODO: 实现Buff1009的进入逻辑
        }

        public override void UpdateBuff(float time)
        {
            base.UpdateBuff(time);
            // TODO: 实现Buff1009的更新逻辑
        }

        public override void ExitBuff(float time)
        {
            base.ExitBuff(time);
            // TODO: 实现Buff1009的退出逻辑
        }

        public override void RemoveBuff()
        {
            base.RemoveBuff();
            // TODO: 实现Buff1009的移除逻辑
        }
    }
}
