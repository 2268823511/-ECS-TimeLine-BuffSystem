using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XJXSingleton;
using XJXMoudle;

namespace XJXMgr
{
    /// <summary>
    /// Buff管理器
    /// 负责管理所有buff的添加、移除、冲突处理等逻辑
    /// </summary>
    public class BuffMgr : Singleton<BuffMgr>
    {
        private Dictionary<int, List<BuffBase>> agentBuffsMap = new Dictionary<int, List<BuffBase>>();

        /// <summary>
        /// 添加buff到指定代理
        /// </summary>
        /// <param name="caster">施法者</param>
        /// <param name="target">目标</param>
        /// <param name="buffId">buff类型ID</param>
        public void AddBuffToAgent(Agent caster, Agent target, int buffId)
        {
            if (target == null)
            {
                Debug.LogError($"[BuffMgr] 目标代理为空，无法添加buff {buffId}");
                return;
            }
            //TEST

            int agentId = target.GetAgentId();
            BuffBase newBuff = CreateBuff(buffId, caster, target);

            if (newBuff == null)
            {
                Debug.LogError($"[BuffMgr] 无法创建buff {buffId}");
                return;
            }

            // 确保代理有buff列表
            if (!agentBuffsMap.ContainsKey(agentId))
            {
                agentBuffsMap[agentId] = new List<BuffBase>();
            }

            var existingBuffs = agentBuffsMap[agentId];

            // 检查冲突和叠加规则,这里需要返回值吧? 这里这样写不行
            HandleBuffConflicts(existingBuffs, newBuff);

            // 添加新buff
            existingBuffs.Add(newBuff);

            // 应用buff
            newBuff.ApplyBuff();

            Debug.Log($"[BuffMgr] 成功给代理 {agentId} 添加了 {newBuff.buffData.BuffName}(ID:{buffId})");
        }

        /// <summary>
        /// 从代理移除指定类型的buff
        /// </summary>
        /// <param name="target">目标代理</param>
        /// <param name="buffId">要移除的buff类型ID</param>
        public void RemoveBuffFromAgent(Agent target, int buffId)
        {
            if (target == null) return;

            int agentId = target.GetAgentId();
            if (!agentBuffsMap.ContainsKey(agentId)) return;

            var buffs = agentBuffsMap[agentId];
            var buffToRemove = buffs.FirstOrDefault(b => b.buffData.BuffTypeId == buffId);

            if (buffToRemove != null)
            {
                buffToRemove.RemoveBuff();
                buffs.Remove(buffToRemove);
                Debug.Log($"[BuffMgr] 移除了代理 {agentId} 的buff {buffId}");
            }
        }

        /// <summary>
        /// 移除代理的所有buff
        /// </summary>
        /// <param name="target">目标代理</param>
        public void RemoveAllBuffsFromAgent(Agent target)
        {
            if (target == null) return;

            int agentId = target.GetAgentId();
            if (!agentBuffsMap.ContainsKey(agentId)) return;

            var buffs = agentBuffsMap[agentId];
            foreach (var buff in buffs)
            {
                buff.RemoveBuff();
            }

            buffs.Clear();
            Debug.Log($"[BuffMgr] 移除了代理 {agentId} 的所有buff");
        }

        /// <summary>
        /// 获取代理的所有buff
        /// </summary>
        /// <param name="agentId">代理ID</param>
        /// <returns>buff列表</returns>
        public List<BuffBase> GetAgentBuffs(int agentId)
        {
            if (agentBuffsMap.ContainsKey(agentId))
            {
                return new List<BuffBase>(agentBuffsMap[agentId]);
            }

            return new List<BuffBase>();
        }

        /// <summary>
        /// 检查代理是否有指定buff
        /// </summary>
        /// <param name="agentId">代理ID</param>
        /// <param name="buffId">buff类型ID</param>
        /// <returns>是否有该buff</returns>
        public bool HasBuff(int agentId, int buffId)
        {
            if (!agentBuffsMap.ContainsKey(agentId)) return false;

            return agentBuffsMap[agentId].Any(b => b.buffData.BuffTypeId == buffId);
        }

        /// <summary>
        /// 获取所有活跃的buff（用于BuffSystem更新）
        /// </summary>
        /// <returns>所有活跃buff的列表</returns>
        public List<BuffBase> GetAllActiveBuffs()
        {
            var allBuffs = new List<BuffBase>();
            foreach (var buffList in agentBuffsMap.Values)
            {
                allBuffs.AddRange(buffList);
            }

            return allBuffs;
        }

        /// <summary>
        /// 处理buff冲突和叠加规则
        /// </summary>
        /// <param name="existingBuffs">已有buff列表</param>
        /// <param name="newBuff">新buff</param>
        private void HandleBuffConflicts(List<BuffBase> existingBuffs, BuffBase newBuff)
        {
            var newBuffData = newBuff.buffData;

            // 检查是否已有相同类型的buff
            var existingSameBuff = existingBuffs.FirstOrDefault(b => b.buffData.BuffTypeId == newBuffData.BuffTypeId);
            if (existingSameBuff != null)
            {
                HandleSameBuffType(existingBuffs, existingSameBuff, newBuff);
                return;
            }

            // 处理冲突buff
            if (newBuffData.ConflictType == EBuffConflictType.OverrideConflict)
            {
                HandleConflictBuffs(existingBuffs, newBuff);
            }
        }

        /// <summary>
        /// 处理相同类型buff的叠加
        /// </summary>
        private void HandleSameBuffType(List<BuffBase> existingBuffs, BuffBase existingBuff, BuffBase newBuff)
        {
            var buffData = newBuff.buffData;

            switch (buffData.BuffLvType)
            {
                case EBuffLvType.SimLv:
                    // 单层buff，根据刷新规则处理
                    if (buffData.BuffTickType == EBuffTickType.NeedReflash)
                    {
                        // 刷新现有buff的时间
                        existingBuff.RemoveBuff();
                        existingBuffs.Remove(existingBuff);
                        Debug.Log($"[BuffMgr] 刷新了buff {buffData.BuffName}");
                    }
                    else
                    {
                        Debug.Log($"[BuffMgr] buff {buffData.BuffName} 已存在且不刷新，忽略新buff");
                        return; // 不添加新buff
                    }

                    break;

                case EBuffLvType.MulLv:
                    // 多层buff，检查是否超过最大层数
                    var sameTypeCount = existingBuffs.Count(b => b.buffData.BuffTypeId == buffData.BuffTypeId);
                    if (sameTypeCount >= buffData.MaxLv)
                    {
                        Debug.Log($"[BuffMgr] buff {buffData.BuffName} 已达到最大层数 {buffData.MaxLv}");
                        return; // 不添加新buff
                    }

                    Debug.Log($"[BuffMgr] 添加buff {buffData.BuffName} 新层级，当前层数: {sameTypeCount + 1}");
                    break;
            }
        }

        /// <summary>
        /// 处理冲突buff
        /// </summary>
        private void HandleConflictBuffs(List<BuffBase> existingBuffs, BuffBase newBuff)
        {
            var conflictIds = newBuff.buffData.ConflictBuffIds;
            if (conflictIds == null || conflictIds.Count == 0) return;

            var buffsToRemove = new List<BuffBase>();
            foreach (var buff in existingBuffs)
            {
                if (conflictIds.Contains(buff.buffData.BuffTypeId))
                {
                    buffsToRemove.Add(buff);
                }
            }

            // 移除冲突的buff
            foreach (var buffToRemove in buffsToRemove)
            {
                buffToRemove.ExitBuff(Time.time);
                existingBuffs.Remove(buffToRemove);
                Debug.Log($"[BuffMgr] 因冲突移除了buff {buffToRemove.buffData.BuffName}");
            }
        }

        /// <summary>
        /// 创建指定类型的buff实例
        /// </summary>
        /// <param name="buffId">buff类型ID</param>
        /// <param name="caster">施法者</param>
        /// <param name="target">目标</param>
        /// <returns>创建的buff实例</returns>
        private BuffBase CreateBuff(int buffId, Agent caster, Agent target)
        {
            try
            {
                switch (buffId)
                {
                    case 1001: return new Buff1001(caster, target);
                    case 1002: return new Buff1002(caster, target);
                    case 1003: return new Buff1003(caster, target);
                    case 1004: return new Buff1004(caster, target);
                    case 1005: return new Buff1005(caster, target);
                    case 1006: return new Buff1006(caster, target);
                    case 1007: return new Buff1007(caster, target);
                    case 1008: return new Buff1008(caster, target);
                    default:
                        Debug.LogError($"[BuffMgr] 未知的buff ID: {buffId}");
                        return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[BuffMgr] 创建buff {buffId} 时发生异常: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 清理已过期或被标记为移除的buff
        /// </summary>
        public void CleanupExpiredBuffs()
        {
            var agentsToClean = new List<int>();

            foreach (var kvp in agentBuffsMap)
            {
                var agentId = kvp.Key;
                var buffs = kvp.Value;

                // 移除需要删除的buff
                buffs.RemoveAll(buff => buff == null);

                if (buffs.Count == 0)
                {
                    agentsToClean.Add(agentId);
                }
            }

            // 清理空的代理记录
            foreach (var agentId in agentsToClean)
            {
                agentBuffsMap.Remove(agentId);
            }
        }
    }
}