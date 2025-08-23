using InterFaces;
using UnityEngine;
using GameSystem;

namespace Base
{
    //buff类型(是否可叠层)
    public enum BuffType
    {
        single,
        multiplyRefreshTime, //可叠层刷新时间,重新计时
        multiplyNoRefreshTime, //可叠层不刷新时间,不重新计时
    }

    public enum BuffConflictType
    {
        none, //无冲突,也就是可共存
        priorityOverride, //写法上,如果是优先级覆盖类型的,可以在添加前检查代理上的所有buffList,然后检查冲突列表,如果冲突列表里面有,则说明需要进入优先级判断!,大的覆盖小的 
        absolute, //绝对,在处于这个buff情况下不允许额外添加任何buff
    }

    public enum BuffStateType
    {
        hasNoInit, //没有初始化
        hasInit, //已经初始化
        hasEnter, //已经进入
        hasEnd, //已经完成
        hasRemove, //已经移除
    }

    //buff优先级
    public enum buffPrioity
    {
        lowPrioity = 1,
        normalPrioity = 2,
        highPrioity = 3,
        topPrioity = 4,
    }

    public abstract class BuffBase : IBuff
    {
        //唯一标识
        public int buffId;

        //唯一buff标识
        public int buffTypeId;

        //buff总层数
        public int maxBufflv;
        public int nowBuffLv;

        //buff时间(s)
        public float totalTime;

        //buff已经持续的时间(s)
        public float curTime;

        //buff剩下时间
        public float leftTime;

        //timeLine 时间轴,由帧数计算实际过了多少时间。
        public int tick;

        public float intervalTime;

        //冷却
        public float cdTime;

        //上一次使用buff的时间
        public float lastUseTime;

        //携带者
        public EntityBase owner;

        //目标
        public EntityBase target;

        //buff类型
        public BuffType buffType;

        //buff冲突类型
        public BuffConflictType conflictType;

        //buff冲突列表
        public int[] conflictBuffIds;

        //buff优先级 (越大越优先) 
        public int buffPriority;

        public BuffStateType buffState = BuffStateType.hasNoInit;

        //初始化buff,demo用代码配置(或者默认读表),实际可以重载方法,提供外部配置
        public virtual void initBuff(int buffId, EntityBase owner, EntityBase target)
        {
            this.buffId = buffId;
            this.owner = owner;
            this.target = target;
            cdTime = 0;
        }

        //func buff回调点
        public virtual void applyBuff()
        {
            tick = 0; //默认刷新时间
            if (buffState != BuffStateType.hasNoInit) return;
            buffState = BuffStateType.hasInit;
            
            // 触发UI数据更新
            GameEventManager.TriggerBuffUIUpdate(this);
        }

        public virtual void enterBuff()
        {
            tick = 1;
            nowBuffLv = 0;
            if (buffState != BuffStateType.hasInit) return;
            buffState = BuffStateType.hasEnter;
            
            // 触发UI数据更新
            GameEventManager.TriggerBuffUIUpdate(this);
        }

        public virtual void updateBuff()
        {
            if (buffState != BuffStateType.hasEnter) return;
            tick += 1;
            
            // 触发UI数据更新 - 每次tick都更新UI
            GameEventManager.TriggerBuffUIUpdate(this);
            
            // 如果totalTime为-1，表示永久buff，不检查时间
            if (totalTime > 0 && tick * Time.fixedDeltaTime * 1000 >= totalTime * 1000) //最小可以到0.001秒
            {
                buffState = BuffStateType.hasEnd;
                // 状态变化时也触发更新
                GameEventManager.TriggerBuffUIUpdate(this);
            }
            //业务实现
        }

        //这里写得有问题 --todo
        public virtual void updateBuffByInterval()
        {
            if (buffState != BuffStateType.hasEnter) return;
            if (intervalTime <= 0) return;
            if ((int)(intervalTime * 1000) % ((int)(tick * Time.fixedDeltaTime) * 1000) != 0)
            {
                return;
            }

            if (tick * Time.fixedDeltaTime * 1000 >= totalTime * 1000)
            {
                buffState = BuffStateType.hasEnd;
            }

            //业务实现
        }

        public virtual void exitBuff()
        {
            tick = 0;
            nowBuffLv = 0;
            buffState = BuffStateType.hasEnd;
            
            // 触发UI数据更新
            GameEventManager.TriggerBuffUIUpdate(this);
        }

        public virtual void removeBuff()
        {
            tick = 0;
            buffState = BuffStateType.hasRemove;
            
            // 触发UI数据更新
            GameEventManager.TriggerBuffUIUpdate(this);
        }

        #region 非回调点方法

        public virtual BuffStateType getBuffState()
        {
            return buffState;
        }

        public virtual int getBuffId()
        {
            return buffId;
        }



        #endregion
    }
}