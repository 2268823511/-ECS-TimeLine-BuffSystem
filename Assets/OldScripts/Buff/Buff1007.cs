using Base;
using Entity;
using UnityEngine;

namespace Buff
{
    /// <summary>
    /// 间隔减速buff 10秒内减速2次的buff
    /// </summary>
    public class Buff1007 : BuffBase
    {
        public int bufspeedCutValue;
        private float lastIntervalTime; // 上次间隔触发时间

        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1007;
            maxBufflv = 2;
            buffType = BuffType.multiplyNoRefreshTime; //不刷新时间
            conflictType = BuffConflictType.priorityOverride; //有优先级覆盖
            conflictBuffIds = new int[1] { 1008 }; //与1008互斥
            cdTime = 15f; //cd
            totalTime = 10f; //总共时长
            bufspeedCutValue = 2; //绝对值
            intervalTime = 3f; //间隔时间，按需求改为3秒
        }

        public override void applyBuff()
        {
            if (Time.time - lastUseTime < cdTime) return;
            base.applyBuff();
            lastUseTime = Time.time;
            lastIntervalTime = Time.time; // 重置间隔时间
        }

        public override void updateBuff()
        {
            base.updateBuff();
            
            // 检查3秒间隔，每3秒减速一次
            if (Time.time - lastIntervalTime >= intervalTime && nowBuffLv < maxBufflv)
            {
                lastIntervalTime = Time.time;
                var entityPlayer = target as EntityPlayer;
                entityPlayer.speedCutX -= bufspeedCutValue;
                nowBuffLv++;
                Debug.Log($"减速buff生效，当前层数: {nowBuffLv}");
            }
        }


        public override void exitBuff()
        {
            base.exitBuff();
            var entityPlayer = target as EntityPlayer;
            entityPlayer.speedCutX = 0;
        }
    }
}