using Base;
using Entity;
using UnityEngine;

namespace Buff
{
    public class Buff1008 : BuffBase
    {
        public int buffSpeedCutValue; //buff减速值

        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1008;
            maxBufflv = 1;
            buffType = BuffType.single; //不刷新时间
            conflictType = BuffConflictType.priorityOverride; //有优先级覆盖
            conflictBuffIds = new int[1] { 1007 }; //与1007互斥
            cdTime = 5f;
            buffSpeedCutValue = 0; //绝对值
        }

        public override void applyBuff()
        {
            if (nowBuffLv >= maxBufflv || Time.time - lastUseTime < cdTime) return;
            base.applyBuff(); //默认刷新了时间了,不刷新时间得重新处理
            nowBuffLv = 1;
            var entityPlayer = target as EntityPlayer;
            entityPlayer.speedCutX = buffSpeedCutValue; // 立即恢复速度
            Debug.Log("恢复速度buff生效");
        }

        public override void updateBuff()
        {
            buffState = BuffStateType.hasEnd;
            base.updateBuff();
        }

        public override void exitBuff()
        {
            base.exitBuff();
            lastUseTime = Time.time; // buff退出后开始计算CD
        }
    }
}