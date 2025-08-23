using UnityEngine;
using XJXInterface;

namespace XJXMoudle
{
    // public enum BuffState
    // {
    // 	ApplyBuff, //添加buff,帧数还没开始计算
    // 	EnterBuff, //进入buff,帧数开始计算
    // 	updateBuff, //更新buff
    // 	ExitBuff, //退出buff,结束帧数计算
    // 	RemoveBuff, //移除buff
    // }

    public class BuffBase : IBuff
    {
        int insId; //buff实例id
        public Agent caster; //施放者
        public Agent target; //目标
        public BuffData buffData; //buff数据

        protected int curLv = 0; //当前buff等级

        float remainDurTime; //buff剩余持续时间
        protected float maxDurTime; //buff最大持续时间

        protected float buffStartTimer; //buff开始时间
        protected float triggerTime; //buff有效触发时间
        int tick; //帧,只做计算

        BuffState buffState = BuffState.ApplyBuff; //buff状态

        /// <summary>
        /// 获取buff当前状态（提供给BuffSystem使用）
        /// </summary>
        public BuffState CurrentState => buffState;

        public BuffBase()
        {
            insId = System.Guid.NewGuid().GetHashCode();
        }

        public virtual void ApplyBuff()
        {
            if (curLv <= buffData.MaxLv)
            {
                curLv++;
            }
            else return;

            if (buffState != BuffState.ApplyBuff) return;
            buffState = BuffState.EnterBuff;

            tick = 0;
        }


        public virtual void EnterBuff(float time)
        {
            if (buffState != BuffState.EnterBuff) return;
            buffState = BuffState.UpdateBuff;
            if (buffData != null)
            {
                maxDurTime = buffData.BaseDuration;
                buffStartTimer = Time.time;
            }

            tick = 1;
        }

        public virtual void ExitBuff(float time)
        {
            if (buffState != BuffState.ExitBuff) return;
            buffState = BuffState.RemoveBuff;
        }

        public virtual void RemoveBuff()
        {
        }

        public virtual void UpdateBuff(float time)
        {
            if (buffState != BuffState.UpdateBuff) return;
            // 更新剩余时间
            if (maxDurTime > 0) // 0表示永久Buff
            {
                var exitTime = buffStartTimer + maxDurTime;
                if (time > exitTime)
                {
                    buffState = BuffState.ExitBuff;
                    return;
                }
            }

            // 更新帧计数
            tick++;
        }


        /// <summary>
        /// 强制buff进入退出状态（用于提前移除buff）
        /// </summary>
        public void ForceExit()
        {
            if (buffState == BuffState.UpdateBuff || buffState == BuffState.EnterBuff)
            {
                buffState = BuffState.ExitBuff;
            }
        }

        /// <summary>
        /// 重新进入buff（用于多层buff）
        /// </summary>
        /// <param name="time"></param>
        public void ReEnterBuff(float time)
        {
            if (buffData.BuffLvType != EBuffLvType.MulLv) return;
            buffState = BuffState.EnterBuff;
            //curLv = 
        }
    }
}