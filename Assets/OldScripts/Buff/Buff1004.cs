using Base;
using Entity;
using UnityEngine;

namespace Buff
{
    public class Buff1004 : BuffBase
    {
        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1004;
            maxBufflv = 1;
            totalTime = 30f;
            buffType = BuffType.single;
            conflictType = BuffConflictType.none;
            cdTime = 0f; // 无CD
        }

        public override void applyBuff()
        {
            if (nowBuffLv >= maxBufflv) return;
            base.applyBuff();
            maxBufflv = 1;
            var entityPlayer = target as EntityPlayer;
            //这样写就不是配置写法了
            entityPlayer.scalePrefab = 2.5f;
            entityPlayer.getEntityObj().transform.localScale = new Vector3(entityPlayer.scalePrefab,
                entityPlayer.scalePrefab, entityPlayer.scalePrefab);
        }

        public override void exitBuff()
        {
            base.exitBuff();
            var entityPlayer = target as EntityPlayer;
            entityPlayer.scalePrefab = 5f;
            entityPlayer.getEntityObj().transform.localScale = new Vector3(entityPlayer.scalePrefab,
                entityPlayer.scalePrefab, entityPlayer.scalePrefab);
        }
    }
}