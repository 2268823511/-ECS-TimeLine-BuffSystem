using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XJXSingleton;
using XJXMoudle;
using XJXInterface;

namespace XJXMgr
{
    /// <summary>
    /// 系统数据管理器 或者叫控制器 contoller比较好
    /// 负责管理各个System之间的数据传递，确保System解耦
    /// 采用单例模式，全局唯一
    /// </summary>
    public class SystemDataMgr : Singleton<SystemDataMgr>,ILateUpdate
    {
        #region 属性修改相关

        /// <summary>
        /// 待处理的属性修改请求列表
        /// BuffSystem向此列表添加修改请求，AgentSystem从此列表消费请求
        /// </summary>
        private List<PropertyModifyComponent> pendingPropertyModifies = new List<PropertyModifyComponent>();

        /// <summary>
        /// 已处理的属性修改历史记录（用于调试）
        /// 可以通过Inspector查看最近的修改历史
        /// </summary>
        [SerializeField]
        private List<PropertyModifyComponent> processedHistory = new List<PropertyModifyComponent>();

        /// <summary>
        /// 历史记录最大保存数量
        /// </summary>
        private const int MAX_HISTORY_COUNT = 100;

        #endregion

        #region 公共接口

        /// <summary>
        /// 请求属性修改
        /// 由BuffSystem调用，添加属性修改请求到待处理队列
        /// </summary>
        /// <param name="modifyData">属性修改数据</param>
        public void RequestPropertyModify(PropertyModifyComponent modifyData)
        {
            pendingPropertyModifies.Add(modifyData);
            
            // 调试日志
            Debug.Log($"[SystemDataMgr] 收到属性修改请求: Agent{modifyData.AgentId} " +
                     $"{modifyData.PropertyType} {modifyData.ModifyType} {modifyData.Value}");
        }

        /// <summary>
        /// 批量请求属性修改
        /// </summary>
        /// <param name="modifyDataList">属性修改数据列表</param>
        public void RequestPropertyModifies(List<PropertyModifyComponent> modifyDataList)
        {
            pendingPropertyModifies.AddRange(modifyDataList);
            Debug.Log($"[SystemDataMgr] 收到批量属性修改请求: {modifyDataList.Count}个");
        }

        /// <summary>
        /// 获取所有待处理的属性修改请求
        /// 由AgentSystem调用
        /// </summary>
        /// <returns>待处理的属性修改请求列表（只读）</returns>
        public List<PropertyModifyComponent> GetPendingPropertyModifies()
        {
            return pendingPropertyModifies.Where(data => !data.IsProcessed).ToList();
        }

        /// <summary>
        /// 标记属性修改为已处理
        /// 由AgentSystem调用，标记指定的修改请求为已处理
        /// </summary>
        /// <param name="index">要标记的修改请求索引</param>
        public void MarkAsProcessed(int index)
        {
            if (index >= 0 && index < pendingPropertyModifies.Count)
            {
                var data = pendingPropertyModifies[index];
                data.IsProcessed = true;
                pendingPropertyModifies[index] = data;

                // 添加到历史记录
                AddToHistory(data);
            }
        }

        /// <summary>
        /// 批量标记为已处理
        /// </summary>
        /// <param name="indices">索引列表</param>
        public void MarkMultipleAsProcessed(List<int> indices)
        {
            foreach (int index in indices)
            {
                MarkAsProcessed(index);
            }
        }

        /// <summary>
        /// 清理已处理的数据
        /// 移除所有已标记为处理的修改请求，释放内存
        /// </summary>
        public void CleanProcessedData()
        {
            int removedCount = pendingPropertyModifies.RemoveAll(data => data.IsProcessed);
            
            if (removedCount > 0)
            {
                Debug.Log($"[SystemDataMgr] 清理了 {removedCount} 个已处理的属性修改请求");
            }
        }

        /// <summary>
        /// 清理过期的持续性修改
        /// 移除所有已过期的持续性属性修改
        /// </summary>
        public void CleanExpiredData(float time)
        {
            int removedCount = pendingPropertyModifies.RemoveAll(data => data.IsExpired(time));
            
            if (removedCount > 0)
            {
                Debug.Log($"[SystemDataMgr] 清理了 {removedCount} 个过期的属性修改请求");
            }
        }

        #endregion

        #region 帧管理

        /// <summary>
        /// 帧结束时的清理工作
        /// 由GameRoot在LateUpdate中调用
        /// </summary>
        public void EndFrame(float time)
        {
            // 清理已处理的数据
            CleanProcessedData();
            
            // 清理过期的数据
            CleanExpiredData(time);
            
            // 检查是否有长时间未处理的请求（可能是bug）
            CheckUnprocessedRequests();
        }

        /// <summary>
        /// 检查未处理的请求
        /// 用于调试，发现可能的系统问题
        /// </summary>
        private void CheckUnprocessedRequests()
        {
            var unprocessedCount = pendingPropertyModifies.Count(data => !data.IsProcessed);
            
            if (unprocessedCount > 50) // 阈值可调整
            {
                Debug.LogWarning($"[SystemDataMgr] 发现大量未处理的属性修改请求: {unprocessedCount}个，" +
                               "可能AgentSystem没有正常处理或处理速度过慢");
            }
        }

        #endregion

        #region 调试和统计

        /// <summary>
        /// 添加到历史记录
        /// </summary>
        private void AddToHistory(PropertyModifyComponent data)
        {
            processedHistory.Add(data);
            
            // 限制历史记录数量
            if (processedHistory.Count > MAX_HISTORY_COUNT)
            {
                processedHistory.RemoveAt(0);
            }
        }

        /// <summary>
        /// 获取统计信息
        /// </summary>
        public SystemDataStats GetStats()
        {
            return new SystemDataStats
            {
                PendingCount = pendingPropertyModifies.Count(data => !data.IsProcessed),
                ProcessedCount = processedHistory.Count,
                TotalRequestsThisFrame = pendingPropertyModifies.Count
            };
        }

        /// <summary>
        /// 清空所有数据（用于场景切换等）
        /// </summary>
        public void ClearAllData()
        {
            pendingPropertyModifies.Clear();
            processedHistory.Clear();
            Debug.Log("[SystemDataMgr] 清空所有数据");
        }

        #endregion

        #region Unity生命周期

    
        /// <summary>
        /// 销毁时清理
        /// </summary>
        private void OnDestroy()
        {
            ClearAllData();
        }

		public void LateUpdate(float time)
		{
			EndFrame(time);
		}



		#endregion
	}

    /// <summary>
    /// 系统数据统计信息
    /// 用于监控和调试
    /// </summary>
    [System.Serializable]
    public struct SystemDataStats
    {
        public int PendingCount;        // 待处理请求数量
        public int ProcessedCount;      // 已处理请求数量
        public int TotalRequestsThisFrame; // 本帧总请求数量
    }
}