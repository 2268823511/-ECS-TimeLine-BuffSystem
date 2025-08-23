using Base;
using Entity;

namespace Buff
{
    /// <summary>
    /// 跳高次数buff
    /// </summary>
    public class Buff1002 : BuffBase
    {
        public int maxJumpCount;

        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1002;
            maxBufflv = 1;
            buffType = BuffType.single;
            conflictType = BuffConflictType.priorityOverride;
            conflictBuffIds = new int[1] { 1003 };
            buffPriority = (int)buffPrioity.lowPrioity;
            maxJumpCount = 3;
        }

        public override void applyBuff()
        {
            var entityPlayer = target as EntityPlayer;
            if (entityPlayer.NowJumpCount > 0 || nowBuffLv >= maxBufflv) return; //添加失败

            base.applyBuff();
            nowBuffLv = 1;
            entityPlayer.NowJumpCount = maxJumpCount;
            entityPlayer.JumPMulFor = 2; //跳高倍数为2
        }


        public override void exitBuff()
        {
            base.exitBuff();
            var entityPlayer = target as EntityPlayer;
            entityPlayer.NowJumpCount = 0; // 或者根据其他buff的影响来设置
            entityPlayer.JumPMulFor = 1; // 还原到默认倍数
        }
    }
}