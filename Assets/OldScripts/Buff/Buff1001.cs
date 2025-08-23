using Base;
using Entity;
using UnityEngine;

namespace Buff
{
    /// <summary>
    /// 光圈buff,可以对指定地形进行伤害监测,销毁部分指定地形
    /// </summary>
    public class Buff1001 : BuffBase
    {
        private bool canDetect;
        private float lastIntervalTime; // 上次间隔触发时间


        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1001;
            maxBufflv = 1;
            totalTime = 3f;
            intervalTime = 0.5f;
            buffType = BuffType.single;
            conflictType = BuffConflictType.none;
            canDetect = false;
        }

        public override void applyBuff()
        {
            // 去掉CD检查，只保留层数检查
            if (nowBuffLv >= maxBufflv) return;
            base.applyBuff();
            nowBuffLv = 1;
            lastIntervalTime = Time.time; // 重置间隔时间
        }

        public override void updateBuff()
        {
            base.updateBuff();

            // 每0.5秒给一次检测机会
            if (Time.time - lastIntervalTime >= intervalTime)
            {
                lastIntervalTime = Time.time;
                canDetect = true;
                Debug.Log("光圈buff触发检测 - 获得一次检测机会");
            }
            // 注意：这里不要设置canDetect = false，让EntityPlayer的碰撞检测来消费这个机会

            var player = owner as EntityPlayer;
            player.canDetect = canDetect;
        }


        public override void exitBuff()
        {
            base.exitBuff();
            Debug.Log("光圈buff结束");
            // 移除lastUseTime设置，因为buff会被删除
            canDetect = false;
            var player = owner as EntityPlayer;
            player.canDetect = canDetect;
        }
    }
}