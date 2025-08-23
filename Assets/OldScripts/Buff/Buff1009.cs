using Base;
using Entity;
using UnityEngine;

namespace Buff
{
    public class Buff1009 : BuffBase
    {
        public int buffSpeedAddValue; //buff加速值
        public int buffMaxCount;
        public int buffNowCount;
        private float firstTriggerTime; //第一次触发时间
        private bool firstTriggered = false; //是否已经第一次触发

        public override void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            base.initBuff(buffId, owner, target);
            buffTypeId = 1009;
            maxBufflv = 1;
            buffType = BuffType.single;
            buffMaxCount = 2;
            conflictType = BuffConflictType.none;
            cdTime = 10f;
            buffSpeedAddValue = 2; //绝对值
            totalTime = -1f; // 不依赖持续时间，通过触发次数控制
            firstTriggered = false;
            buffNowCount = 0;
        }

        public override void applyBuff()
        {
            if (nowBuffLv >= maxBufflv || Time.time - lastUseTime < cdTime) return;
            base.applyBuff(); //默认刷新了时间了,不刷新时间得重新处理
            nowBuffLv = 1;
            
            // 第一次触发加速
            var entityPlayer = target as EntityPlayer;
            entityPlayer.speedAddX += buffSpeedAddValue; //第一次加速
            buffNowCount = 1;
            firstTriggered = true;
            firstTriggerTime = Time.time;
            Debug.Log("加速buff第一次触发");
        }


        public override void updateBuff()
        {
            if (buffState != BuffStateType.hasEnter) return;
            tick++;
            
            // 检查是否需要结束
            if (buffNowCount >= buffMaxCount)
            {
                buffState = BuffStateType.hasEnd;
                lastUseTime = Time.time; // 第一次触发加速后开始计算CD
                return;
            }

            // 检查第二次触发（3秒后）
            if (firstTriggered && buffNowCount == 1 && Time.time - firstTriggerTime >= 3f)
            {
                var entityPlayer = target as EntityPlayer;
                entityPlayer.speedAddX += buffSpeedAddValue; //第二次加速
                buffNowCount++;
                Debug.Log("加速buff第二次触发");
            }
        }
    }
}