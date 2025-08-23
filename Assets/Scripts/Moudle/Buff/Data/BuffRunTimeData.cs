
using UnityEngine;

namespace XJXMoudle
{
    /// <summary>
    /// Buff运行时数据
    /// 管理Buff的冷却时间和状态
    /// </summary>
    public class BuffRunTimeData
    {
        public int BuffTypeId { get; private set; } //buff配置id
        public int BuffInsId { get; private set; } //buff实例id
        public CoolDownState State { get; private set; } //buffCD状态
        public float LastCasterTime { get; private set; } //上次施放时间
        public float BaseCDTime { get; private set; } //基础冷却时间
        public float StartCDTime { get; private set; } //冷却开始时间
        public float CDMulValue { get; private set; } = 1.0f; //冷却倍率
        public float CDReduceValue { get; private set; } = 0f; //冷却减少值(固定值)

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="buffTypeId">buff类型ID</param>
        /// <param name="buffInsId">buff实例ID</param>
        /// <param name="baseCDTime">基础冷却时间</param>
        public BuffRunTimeData(int buffTypeId, int buffInsId, float baseCDTime)
        {
            BuffTypeId = buffTypeId;
            BuffInsId = buffInsId;
            BaseCDTime = baseCDTime;
            State = CoolDownState.Ready;
            LastCasterTime = 0f;
            StartCDTime = 0f;
            CDMulValue = 1.0f;
            CDReduceValue = 0f;
        }

        /// <summary>
        /// 检查是否可以使用（冷却是否结束）
        /// </summary>
        /// <returns>是否准备就绪</returns>
        public bool IsReady()
        {
            return State == CoolDownState.Ready;
        }

        /// <summary>
        /// 开始冷却
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        public void StartCooldown(float currentTime)
        {
            LastCasterTime = currentTime;
            StartCDTime = currentTime;
            State = CoolDownState.NotReady;
            
            Debug.Log($"[BuffRunTimeData] Buff {BuffTypeId} 开始冷却，时长: {GetFinalCDTime()}秒");
        }

        /// <summary>
        /// 强制完成冷却
        /// </summary>
        public void FinishCooldown()
        {
            State = CoolDownState.Ready;
            Debug.Log($"[BuffRunTimeData] Buff {BuffTypeId} 冷却完成");
        }

        /// <summary>
        /// 更新冷却状态
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <returns>是否状态有变化</returns>
        public bool UpdateCooldown(float currentTime)
        {
            if (State == CoolDownState.NotReady)
            {
                var remainingTime = GetRemainingCooldownTime(currentTime);
                if (remainingTime <= 0)
                {
                    State = CoolDownState.Ready;
                    Debug.Log($"[BuffRunTimeData] Buff {BuffTypeId} 冷却结束");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取最终冷却时间（考虑倍率和减少值）
        /// </summary>
        /// <returns>最终冷却时间</returns>
        public float GetFinalCDTime()
        {
            return Mathf.Max(0f, (BaseCDTime * CDMulValue) - CDReduceValue);
        }

        /// <summary>
        /// 获取剩余冷却时间
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <returns>剩余冷却时间（秒）</returns>
        public float GetRemainingCooldownTime(float currentTime)
        {
            if (State == CoolDownState.Ready) return 0f;
            
            var finalCDTime = GetFinalCDTime();
            var elapsedTime = currentTime - StartCDTime;
            return Mathf.Max(0f, finalCDTime - elapsedTime);
        }

        /// <summary>
        /// 获取冷却进度百分比
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <returns>进度百分比 (0-1)</returns>
        public float GetCooldownProgress(float currentTime)
        {
            if (State == CoolDownState.Ready) return 1f;
            
            var finalCDTime = GetFinalCDTime();
            if (finalCDTime <= 0) return 1f;
            
            var elapsedTime = currentTime - StartCDTime;
            return Mathf.Clamp01(elapsedTime / finalCDTime);
        }

        /// <summary>
        /// 设置冷却倍率
        /// </summary>
        /// <param name="multiplier">冷却倍率</param>
        public void SetCooldownMultiplier(float multiplier)
        {
            CDMulValue = Mathf.Max(0f, multiplier);
            Debug.Log($"[BuffRunTimeData] Buff {BuffTypeId} 冷却倍率设置为: {CDMulValue}");
        }

        /// <summary>
        /// 设置冷却减少值
        /// </summary>
        /// <param name="reduction">减少的冷却时间（秒）</param>
        public void SetCooldownReduction(float reduction)
        {
            CDReduceValue = Mathf.Max(0f, reduction);
            Debug.Log($"[BuffRunTimeData] Buff {BuffTypeId} 冷却减少设置为: {CDReduceValue}秒");
        }

        /// <summary>
        /// 修改冷却倍率（累加）
        /// </summary>
        /// <param name="delta">倍率变化值</param>
        public void ModifyCooldownMultiplier(float delta)
        {
            SetCooldownMultiplier(CDMulValue + delta);
        }

        /// <summary>
        /// 修改冷却减少值（累加）
        /// </summary>
        /// <param name="delta">减少值变化</param>
        public void ModifyCooldownReduction(float delta)
        {
            SetCooldownReduction(CDReduceValue + delta);
        }

        /// <summary>
        /// 重置所有CD修正值
        /// </summary>
        public void ResetCooldownModifiers()
        {
            CDMulValue = 1.0f;
            CDReduceValue = 0f;
            Debug.Log($"[BuffRunTimeData] Buff {BuffTypeId} CD修正值已重置");
        }

        /// <summary>
        /// 获取调试信息
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <returns>调试信息字符串</returns>
        public string GetDebugInfo(float currentTime)
        {
            return $"Buff{BuffTypeId} - 状态:{State}, " +
                   $"剩余CD:{GetRemainingCooldownTime(currentTime):F1}s, " +
                   $"进度:{GetCooldownProgress(currentTime) * 100:F0}%";
        }

        /// <summary>
        /// 打印当前状态到控制台
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        public void PrintStatus(float currentTime)
        {
            Debug.Log($"[BuffRunTimeData] {GetDebugInfo(currentTime)}");
        }
    }
}
