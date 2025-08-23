using Base;
using Entity;

namespace Buff
{
    /// <summary>
    /// 二连跳buff
    /// </summary>
    public class Buff1003 : BuffBase
    {
        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1003;
            buffType = BuffType.single;
            conflictType = BuffConflictType.priorityOverride;
            conflictBuffIds = new int[1] { 1002 };
            buffPriority = (int)buffPrioity.highPrioity;
            totalTime = -1f; // 永久buff，设置为-1表示不会自动结束
            cdTime = 0f;
        }

        public override void applyBuff()
        {
            base.applyBuff();
            var entityPlayer = owner as EntityPlayer;
            entityPlayer.IsOpenDoubleJump = true;
        }

        public override void updateBuff()
        {
            if (buffState != BuffStateType.hasEnter) return;
            tick += 1;
        }


        public override void exitBuff()
        {
            base.exitBuff();
            var entityPlayer = owner as EntityPlayer;
            entityPlayer.IsOpenDoubleJump = false;
        }
    }
}