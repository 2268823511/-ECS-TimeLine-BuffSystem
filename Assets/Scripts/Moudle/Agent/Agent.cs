using System.Collections.Generic;
using System.Linq;
using XJXInterface;
using XJXMgr;

namespace XJXMoudle
{
	/// <summary>
	/// Agent代理类
	/// 代表游戏中的一个实体，可以拥有buff和属性
	/// </summary>
	public class Agent : IDestory
	{
		public int agentId;
		public EntityBase entity;
		List<BuffBase> buffList; // 本地buff列表（用于快速访问）
		List<BuffRunTimeData> buffRunTimeList;

		public Agent(int agentId)
		{
			this.agentId = agentId;
			buffList = new List<BuffBase>();
			buffRunTimeList = new List<BuffRunTimeData>();
		}

		#region 基础方法

		public int GetAgentId()
		{
			return agentId;
		}

		public EntityBase GetEntity()
		{
			return entity;
		}

		#endregion

		#region Buff相关方法

		/// <summary>
		/// 添加buff到当前代理（自己给自己添加）
		/// </summary>
		/// <param name="buffId">buff类型ID</param>
		public void AddBuff(int buffId)
		{
			BuffMgr.Instance.AddBuffToAgent(this, this, buffId);
		}

		/// <summary>
		/// 给目标代理添加buff（自己给别人添加）
		/// </summary>
		/// <param name="target">目标代理</param>
		/// <param name="buffId">buff类型ID</param>
		public void AddBuffToTarget(Agent target, int buffId)
		{
			BuffMgr.Instance.AddBuffToAgent(this, target, buffId);
		}

		/// <summary>
		/// 移除指定类型的buff
		/// </summary>
		/// <param name="buffId">要移除的buff类型ID</param>
		public void RemoveBuff(int buffId)
		{
			BuffMgr.Instance.RemoveBuffFromAgent(this, buffId);
		}

		/// <summary>
		/// 移除所有buff
		/// </summary>
		public void RemoveAllBuffs()
		{
			BuffMgr.Instance.RemoveAllBuffsFromAgent(this);
		}

		/// <summary>
		/// 检查是否有指定buff
		/// </summary>
		/// <param name="buffId">buff类型ID</param>
		/// <returns>是否有该buff</returns>
		public bool HasBuff(int buffId)
		{
			return BuffMgr.Instance.HasBuff(agentId, buffId);
		}

		/// <summary>
		/// 获取所有buff
		/// </summary>
		/// <returns>buff列表</returns>
		public List<BuffBase> GetAllBuffs()
		{
			return BuffMgr.Instance.GetAgentBuffs(agentId);
		}

		/// <summary>
		/// 获取指定类型的buff
		/// </summary>
		/// <param name="buffId">buff类型ID</param>
		/// <returns>buff实例，如果没有则返回null</returns>
		public BuffBase GetBuff(int buffId)
		{
			var buffs = GetAllBuffs();
			return buffs.FirstOrDefault(b => b.buffData.BuffTypeId == buffId);
		}

		/// <summary>
		/// 获取buff数量
		/// </summary>
		/// <returns>当前buff数量</returns>
		public int GetBuffCount()
		{
			return GetAllBuffs().Count;
		}

		/// <summary>
		/// 获取指定类型buff的层数
		/// </summary>
		/// <param name="buffId">buff类型ID</param>
		/// <returns>该类型buff的层数</returns>
		public int GetBuffLevel(int buffId)
		{
			var buffs = GetAllBuffs();
			return buffs.Count(b => b.buffData.BuffTypeId == buffId);
		}

		#endregion

		#region 生命周期

		public void Destory()
		{
			// 移除所有buff
			RemoveAllBuffs();
			
			// 清理entity资源
			if (entity?.EntityGO != null)
			{
				UnityEngine.Object.Destroy(entity.EntityGO);
			}
			
			// 清空列表
			buffList?.Clear();
			buffRunTimeList?.Clear();
			
			UnityEngine.Debug.Log($"[Agent] 代理 {agentId} 已销毁");
		}

		#endregion

		#region 调试和状态信息

		/// <summary>
		/// 获取代理的详细状态信息（用于调试）
		/// </summary>
		/// <returns>状态信息字符串</returns>
		public string GetDebugInfo()
		{
			var buffs = GetAllBuffs();
			var buffNames = buffs.Select(b => b.buffData.BuffName);
			
			return $"Agent {agentId}: " +
				   $"Entity: {entity?.GetType().Name ?? "None"}, " +
				   $"Buffs: [{string.Join(", ", buffNames)}]";
		}

		/// <summary>
		/// 打印当前状态到控制台
		/// </summary>
		public void PrintStatus()
		{
			UnityEngine.Debug.Log(GetDebugInfo());
		}

		#endregion
	}
}