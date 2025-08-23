using Base;
using Entity;
using UnityEngine;

namespace Buff
{
    public class Buff1006 : BuffBase
    {
        public int buffGain;

        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1006;
            maxBufflv = 1;
            buffType = BuffType.single;
            conflictType = BuffConflictType.none;
            cdTime = 4f; // 按需求改为4秒CD
            buffGain = 50;
        }

        public override void applyBuff()
        {
            if (nowBuffLv > maxBufflv || Time.time - lastUseTime < cdTime) return;
            base.applyBuff(); //默认刷新了时间了
            nowBuffLv = 1;
            lastUseTime = Time.time; //刷新了上次添加buff的时间,下次添加的时候判断cd
            var entityPlayer = target as EntityPlayer;
            int currentHp = entityPlayer.getHp();
            entityPlayer.setHp(currentHp + buffGain);
            Debug.Log($"加血buff恢复 {buffGain} 点生命值");
        }

        public override void updateBuff()
        {
            buffState = BuffStateType.hasEnd;
            base.updateBuff();
        }

    }
}