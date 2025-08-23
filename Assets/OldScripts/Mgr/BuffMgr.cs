using System.Collections.Generic;
using AgentSpace;
using Base;
using SingleTonSpace;
using GameSystem;
using UnityEngine;

namespace Mgr
{
    public class BuffMgr : SingleTon<BuffMgr>
    {
        private List<Agent> agbuffList = new List<Agent>();


        //增加一个代理
        public void addAgent(Agent agent)
        {
            foreach (var t in agbuffList)
            {
                if (t.getEntity().getEntityId() == agent.getEntity().getEntityId())
                {
                    return;
                }
            }

            agbuffList.Add(agent);
        }

        //通过entity id唯一标识去添加buff
        // public void addBufftoAgent(int entityId, BuffBase buff)
        // {
        //     foreach (var t in agbuffList)
        //     {
        //         if (t.getEntity().getEntityId() == entityId)
        //         {
        //             t.addBuff(buff);
        //             return;
        //         }
        //     }
        // }

        //通过entity本身去添加buff
        public void addBufftoAgent(int entityId, BuffBase buff)
        {
            foreach (var t in agbuffList)
            {
                if (t.getEntity().getEntityId() == entityId)
                {
                    // 处理buff冲突（包含优先级检查和覆盖逻辑）
                    if (HandleBuffConflicts(t, buff))
                    {
                        t.addBuff(buff); // 只有通过冲突检测才添加
                    }
                    else
                    {
                        Debug.Log($"无法添加Buff {buff.buffTypeId}：存在更高优先级的冲突buff");
                    }
                    return;
                }
            }
        }


        //通过entity本身去添加buff列表
        public void addBuffstoAgent(EntityBase entity, List<BuffBase> buffList)
        {
            foreach (var t in agbuffList)
            {
                if (t.getEntity().getEntityId() == entity.getEntityId())
                {
                    t.getBuffList().AddRange(buffList);
                    return;
                }
            }
        }


        //到期移除指定buff,entity本身
        public void removeBuff(EntityBase entity, BuffBase buff)
        {
            foreach (var t in agbuffList)
            {
                if (t.getEntity().getEntityId() == entity.getEntityId())
                {
                    t.removeBuff(buff);
                    return;
                }
            }
        }


        //到期移除指定buff,entity id
        public void removeBuff(int entityId, BuffBase buff)
        {
            foreach (var t in agbuffList)
            {
                if (t.getEntity().getEntityId() == entityId)
                {
                    t.removeBuff(buff);
                    return;
                }
            }
        }


        public void UpdateAllAgentBuffs()
        {
            foreach (var agent in agbuffList)
            {
                var buffList = agent.getBuffList();
                if (buffList == null || buffList.Count == 0) return;


                for (int i = buffList.Count - 1; i >= 0; i--)
                {
                    var buff = buffList[i];
                    switch (buff.getBuffState())
                    {
                        case BuffStateType.hasNoInit:
                            buff.applyBuff();
                            break;
                        case BuffStateType.hasInit:
                            buff.enterBuff();
                            break;
                        case BuffStateType.hasEnter:
                            buff.updateBuff();
                            break;
                        case BuffStateType.hasEnd:
                            buff.exitBuff();
                            buff.removeBuff();
                            buffList.RemoveAt(i);

                            // 当buff被移除时通知UI更新
                            agent.NotifyBuffListChanged();
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// 根据BuffTypeId获取buff数据
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <param name="buffTypeId">BuffTypeId</param>
        /// <returns></returns>
        public BuffBase GetBuffByTypeId(int entityId, int buffTypeId)
        {
            foreach (var agent in agbuffList)
            {
                if (agent.getEntity().getEntityId() == entityId)
                {
                    var buffList = agent.getBuffList();
                    foreach (var buff in buffList)
                    {
                        if (buff.buffTypeId == buffTypeId && buff.buffState == BuffStateType.hasEnter)
                        {
                            return buff;
                        }
                    }
                }
            }

            return null;
        }

        // 处理buff冲突（统一的冲突检测和覆盖逻辑）
        private bool HandleBuffConflicts(Agent agent, BuffBase newBuff)
        {
            if (newBuff.conflictType != BuffConflictType.priorityOverride) 
            {
                return true; // 无冲突类型，直接允许添加
            }

            var buffList = agent.getBuffList();
            if (buffList == null || buffList.Count == 0) return true;

            for (int i = buffList.Count - 1; i >= 0; i--)
            {
                var existingBuff = buffList[i];

                // 检查是否在新buff的冲突列表中
                if (IsInConflictList(newBuff, existingBuff))
                {
                    if (newBuff.buffPriority >= existingBuff.buffPriority)
                    {
                        // 优先级够高，移除冲突的buff
                        existingBuff.exitBuff();
                        existingBuff.removeBuff();
                        buffList.RemoveAt(i);
                    }
                    else
                    {
                        // 优先级不够，拒绝添加
                        return false;
                    }
                }
            }
            return true; // 通过冲突检测，可以添加
        }

        private bool IsInConflictList(BuffBase newBuff, BuffBase existingBuff)
        {
            if (newBuff.conflictBuffIds == null) return false;

            foreach (int conflictId in newBuff.conflictBuffIds)
            {
                if (conflictId == existingBuff.buffTypeId) return true;
            }

            return false;
        }
    }
}