using System.Collections.Generic;
using UnityEngine;
using XJXMoudle;
using XJXSingleton;

namespace XJXMgr
{
    /// <summary>
    /// Agent管理器
    /// 负责管理所有Agent的创建、销毁和查找
    /// </summary>
    public class AgentMgr : Singleton<AgentMgr>
    {
        List<Agent> AgentList = new List<Agent>();

        public void InitMgr()
        {
            foreach (var agent in AgentList)
            {
                agent.Destory();
            }

            AgentList.Clear();
            Debug.Log("[AgentMgr] 管理器已初始化，清空所有代理");
        }

        #region 基础代理创建方法

        /// <summary>
        /// 通过ID创建一个基础代理（无Entity）
        /// </summary>
        /// <param name="agentId">代理ID</param>
        public void CreateAgent(int agentId)
        {
            if (GetAgent(agentId) != null)
            {
                Debug.LogWarning($"[AgentMgr] 代理 {agentId} 已存在，跳过创建");
                return;
            }

            Agent agent = new Agent(agentId);
            AgentList.Add(agent);
            Debug.Log($"[AgentMgr] 创建了基础代理 {agentId}");
        }

        #endregion

        #region 工厂方法 - 创建带Entity的代理

        /// <summary>
        /// 创建带PlayerEntity的代理
        /// </summary>
        /// <param name="agentId">代理ID</param>
        /// <param name="position">初始位置</param>
        /// <param name="prefab">预制体（可选）</param>
        /// <returns>创建的代理</returns>
        public Agent CreatePlayerAgent(int agentId, Vector3 position, GameObject prefab = null)
        {
            if (GetAgent(agentId) != null)
            {
                Debug.LogWarning($"[AgentMgr] 代理 {agentId} 已存在，跳过创建");
                return GetAgent(agentId);
            }

            // 创建代理
            Agent agent = new Agent(agentId);

            // 创建游戏对象
            GameObject agentGO = CreateGameObject(prefab, position, $"PlayerAgent_{agentId}");

            // 创建PlayerEntity
            PlayerEntity playerEntity = new PlayerEntity
            {
                EntityGO = agentGO,
                Hp = 100f,
                Mp = 50f,
                Attack = 20f,
                Defense = 10f,
                SpeedX = 5f,
                SpeedY = 5f,
                Scale = 1f,
                JumpMaxTimes = 2
            };

            agent.entity = playerEntity;
            AgentList.Add(agent);

            Debug.Log($"[AgentMgr] 创建了玩家代理 {agentId}");
            return agent;
        }

        /// <summary>
        /// 创建带MonsterEntity的代理
        /// </summary>
        /// <param name="agentId">代理ID</param>
        /// <param name="position">初始位置</param>
        /// <param name="prefab">预制体（可选）</param>
        /// <returns>创建的代理</returns>
        public Agent CreateMonsterAgent(int agentId, Vector3 position, GameObject prefab = null)
        {
            if (GetAgent(agentId) != null)
            {
                Debug.LogWarning($"[AgentMgr] 代理 {agentId} 已存在，跳过创建");
                return GetAgent(agentId);
            }

            // 创建代理
            Agent agent = new Agent(agentId);

            // 创建游戏对象
            GameObject agentGO = CreateGameObject(prefab, position, $"MonsterAgent_{agentId}");

            // 创建MonsterEntity
            MonsterEntity monsterEntity = new MonsterEntity
            {
                EntityGO = agentGO,
                Hp = 80f,
                Mp = 30f,
                Attack = 25f,
                Defense = 15f,
                SpeedX = 3f,
                SpeedY = 3f,
                Scale = 1.2f
            };

            agent.entity = monsterEntity;
            AgentList.Add(agent);

            Debug.Log($"[AgentMgr] 创建了怪物代理 {agentId}");
            return agent;
        }

        /// <summary>
        /// 创建自定义Entity的代理
        /// </summary>
        /// <param name="agentId">代理ID</param>
        /// <param name="entity">自定义Entity</param>
        /// <param name="position">初始位置</param>
        /// <param name="prefab">预制体（可选）</param>
        /// <returns>创建的代理</returns>
        public Agent CreateCustomAgent(int agentId, EntityBase entity, Vector3 position, GameObject prefab = null)
        {
            if (GetAgent(agentId) != null)
            {
                Debug.LogWarning($"[AgentMgr] 代理 {agentId} 已存在，跳过创建");
                return GetAgent(agentId);
            }

            // 创建代理
            Agent agent = new Agent(agentId);

            // 创建或设置游戏对象
            if (entity.EntityGO == null)
            {
                entity.EntityGO = CreateGameObject(prefab, position, $"CustomAgent_{agentId}");
            }

            agent.entity = entity;
            AgentList.Add(agent);

            Debug.Log($"[AgentMgr] 创建了自定义代理 {agentId}，Entity类型: {entity.GetType().Name}");
            return agent;
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 创建游戏对象
        /// </summary>
        /// <param name="prefab">预制体（可选）</param>
        /// <param name="position">位置</param>
        /// <param name="name">对象名称</param>
        /// <returns>创建的游戏对象</returns>
        private GameObject CreateGameObject(GameObject prefab, Vector3 position, string name)
        {
            GameObject go;
            
            if (prefab != null)
            {
                go = Object.Instantiate(prefab, position, Quaternion.identity);
            }
            else
            {
                // 创建默认的cube
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = position;
            }
            
            go.name = name;
            return go;
        }

        #endregion

        #region 代理管理

        //通过id移除
        public void RemoveAgent(int agentId)
        {
            Agent agent = GetAgent(agentId);
            if (agent != null)
            {
                AgentList.Remove(agent);
                agent.Destory();
                Debug.Log($"[AgentMgr] 移除了代理 {agentId}");
            }
        }

        //获取所有代理
        public List<Agent> GetAllAgents()
        {
            return new List<Agent>(AgentList);
        }

        //获取指定的Agent
        public Agent GetAgent(int agentId)
        {
            foreach (var agent in AgentList)
            {
                if (agent.GetAgentId() == agentId)
                {
                    return agent;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取代理数量
        /// </summary>
        /// <returns>当前代理数量</returns>
        public int GetAgentCount()
        {
            return AgentList.Count;
        }

        /// <summary>
        /// 检查代理是否存在
        /// </summary>
        /// <param name="agentId">代理ID</param>
        /// <returns>是否存在</returns>
        public bool HasAgent(int agentId)
        {
            return GetAgent(agentId) != null;
        }

        /// <summary>
        /// 获取所有玩家代理
        /// </summary>
        /// <returns>玩家代理列表</returns>
        public List<Agent> GetPlayerAgents()
        {
            List<Agent> playerAgents = new List<Agent>();
            foreach (var agent in AgentList)
            {
                if (agent.entity is PlayerEntity)
                {
                    playerAgents.Add(agent);
                }
            }
            return playerAgents;
        }

        /// <summary>
        /// 获取所有怪物代理
        /// </summary>
        /// <returns>怪物代理列表</returns>
        public List<Agent> GetMonsterAgents()
        {
            List<Agent> monsterAgents = new List<Agent>();
            foreach (var agent in AgentList)
            {
                if (agent.entity is MonsterEntity)
                {
                    monsterAgents.Add(agent);
                }
            }
            return monsterAgents;
        }

        #endregion

        #region 调试方法

        /// <summary>
        /// 打印所有代理的状态信息
        /// </summary>
        public void PrintAllAgentsStatus()
        {
            Debug.Log($"=== AgentMgr状态 - 总共{AgentList.Count}个代理 ===");
            foreach (var agent in AgentList)
            {
                Debug.Log(agent.GetDebugInfo());
            }
        }

        #endregion
    }
}