using System.Collections.Generic;
using UnityEngine;
using XJXInterface;
using XJXSingleton;
using XJXMgr;
using XJXMoudle;

namespace XJXSystem{
    /// <summary>
    /// Buff系统
    /// 负责管理所有buff的生命周期，驱动buff的状态更新
    /// </summary>
    public class BuffSystem : SystemBase, IStart, IUpdate
    {
        public static BuffSystem Instance => Singleton<BuffSystem>.Instance;

        protected override void OnInitialize()
        {
            Debug.Log("BuffSystem Initialized");
        }
        
        public void Start()
        {
            Debug.Log("BuffSystem Started - 开始管理buff生命周期");
        }

        /// <summary>
        /// 每帧更新所有活跃的buff
        /// </summary>
        /// <param name="deltaTime">当前游戏时间</param>
        public void Update(float deltaTime)
        {
            UpdateAllBuffs(deltaTime);
            CleanupExpiredBuffs();
        }
        
        /// <summary>
        /// 更新所有活跃buff的状态
        /// </summary>
        /// <param name="time">当前游戏时间</param>
        private void UpdateAllBuffs(float time)
        {
            var allBuffs = BuffMgr.Instance.GetAllActiveBuffs();
            var buffsToRemove = new List<BuffBase>();
            
            foreach (var buff in allBuffs)
            {
                if (buff == null) continue;
                
                try
                {
                    // 根据buff当前状态执行相应逻辑
                    UpdateBuffState(buff, time);
                    
                    // 检查buff是否应该被移除
                    if (ShouldRemoveBuff(buff, time))
                    {
                        buffsToRemove.Add(buff);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[BuffSystem] 更新buff时发生异常: {buff.buffData?.BuffName} - {e.Message}");
                    buffsToRemove.Add(buff); // 异常的buff也应该被移除
                }
            }
            
            // 移除需要清理的buff
            foreach (var buffToRemove in buffsToRemove)
            {
                RemoveBuffFromTarget(buffToRemove);
            }
        }
        
        /// <summary>
        /// 更新单个buff的状态
        /// </summary>
        /// <param name="buff">要更新的buff</param>
        /// <param name="time">当前游戏时间</param>
        private void UpdateBuffState(BuffBase buff, float time)
        {
            // 获取buff的当前状态（这里需要通过反射或者添加公开属性来获取）
            var buffState = GetBuffState(buff);
            
            switch (buffState)
            {
                case BuffState.ApplyBuff:
                    // buff刚被添加，需要进入
                    buff.EnterBuff(time);
                    break;
                    
                case BuffState.EnterBuff:
                    // buff正在进入，可能需要转换到更新状态
                    buff.EnterBuff(time);
                    break;
                    
                case BuffState.UpdateBuff:
                    // buff正在持续生效
                    buff.UpdateBuff(time);
                    break;
                    
                case BuffState.ExitBuff:
                    // buff准备退出
                    buff.ExitBuff(time);
                    break;
                    
                case BuffState.RemoveBuff:
					// buff应该被移除（这种情况在ShouldRemoveBuff中处理）
					buff.RemoveBuff();
                    break;
            }
        }
        
        /// <summary>
        /// 获取buff的当前状态
        /// </summary>
        /// <param name="buff">buff实例</param>
        /// <returns>buff状态</returns>
        private BuffState GetBuffState(BuffBase buff)
        {
            return buff.CurrentState;
        }
        
        /// <summary>
        /// 检查buff是否应该被移除
        /// </summary>
        /// <param name="buff">要检查的buff</param>
        /// <param name="time">当前游戏时间</param>
        /// <returns>是否应该移除</returns>
        private bool ShouldRemoveBuff(BuffBase buff, float time)
        {
            if (buff?.target == null) return true;
            
            var buffState = GetBuffState(buff);
            return buffState == BuffState.RemoveBuff;
        }
        
        /// <summary>
        /// 从目标代理移除buff
        /// </summary>
        /// <param name="buff">要移除的buff</param>
        private void RemoveBuffFromTarget(BuffBase buff)
        {
            if (buff?.target == null) return;
            
            try
            {
                BuffMgr.Instance.RemoveBuffFromAgent(buff.target, buff.buffData.BuffTypeId);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[BuffSystem] 移除buff时发生异常: {e.Message}");
            }
        }
        
        /// <summary>
        /// 清理过期的buff
        /// </summary>
        private void CleanupExpiredBuffs()
        {
            BuffMgr.Instance.CleanupExpiredBuffs();
        }
        
        /// <summary>
        /// 获取系统统计信息（用于调试）
        /// </summary>
        /// <returns>统计信息</returns>
        public BuffSystemStats GetStats()
        {
            var allBuffs = BuffMgr.Instance.GetAllActiveBuffs();
            var stateCounts = new System.Collections.Generic.Dictionary<BuffState, int>();
            
            foreach (var buff in allBuffs)
            {
                if (buff == null) continue;
                
                var state = GetBuffState(buff);
                if (stateCounts.ContainsKey(state))
                {
                    stateCounts[state]++;
                }
                else
                {
                    stateCounts[state] = 1;
                }
            }
            
            return new BuffSystemStats
            {
                TotalActiveBuffs = allBuffs.Count,
                StateDistribution = stateCounts
            };
        }
    }
    
    /// <summary>
    /// Buff系统统计信息
    /// </summary>
    [System.Serializable]
    public struct BuffSystemStats
    {
        public int TotalActiveBuffs;
        public System.Collections.Generic.Dictionary<BuffState, int> StateDistribution;
    }
}
