using Base;
using Entity;
using UnityEngine;

namespace Buff
{
    public class Buff1005 : BuffBase
    {
        public int buffDamage;
        private float lastIntervalTime; // 上次间隔触发时间

        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1005;
            maxBufflv = 3;
            totalTime = 10f;
            buffType = BuffType.multiplyRefreshTime;
            conflictType = BuffConflictType.none;
            cdTime = 3f;
            buffDamage = 10;
            intervalTime = 1f;
        }

        public override void applyBuff()
        {
            if (Time.time - lastUseTime < cdTime) return;
            
            if (nowBuffLv >= maxBufflv)
            {
                // 已达到最大层数，只刷新时间
                base.applyBuff(); // 刷新持续时间
                lastUseTime = Time.time;
                return;
            }
            
            base.applyBuff(); //默认刷新了时间了
            nowBuffLv++;
            lastUseTime = Time.time; //刷新了上次添加buff的时间,下次添加的时候判断cd
            lastIntervalTime = Time.time; // 重置间隔时间
        }

        public override void updateBuff()
        {
            base.updateBuff();
            
            // 检查1秒间隔，每秒掉血
            if (Time.time - lastIntervalTime >= intervalTime)
            {
                lastIntervalTime = Time.time;
                var entityPlayer = target as EntityPlayer;
                int totalDamage = buffDamage * nowBuffLv; // 每层造成10点伤害
                int currentHp = entityPlayer.getHp();
                entityPlayer.setHp(currentHp - totalDamage);
                Debug.Log($"中毒buff造成 {totalDamage} 点伤害 (层数: {nowBuffLv})");
                //死亡不在这里做,外层处理
            }
        }
    }
}