using UnityEngine;
using GameSystem;
using Entity;
using AgentSpace;
using Mgr;

namespace gameRoot
{
    /// <summary>
    /// 游戏管理器 - 负责驱动buff系统更新和管理玩家
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("系统设置")] 
        public bool autoUpdateBuffs = true;
        
        [Header("玩家设置")]
        public GameObject playerRootPrefab; // 玩家预制体
        public Transform playerSpawnPoint; // 玩家生成点
        
        // 玩家引用
        private EntityPlayer currentPlayer;
        private Agent currentAgent;
        private GameObject playerRootInstance;

        void Start()
        {
            Debug.Log("游戏管理器启动");
            InitializePlayer();
        }

        void Update()
        {
            // 更新玩家移动
            if (currentPlayer != null)
            {
                currentPlayer.UpdateMovement();
            }
        }

        void FixedUpdate()
        {
            if (autoUpdateBuffs)
            {
                // 驱动buff系统更新
                BuffSystem.getInstance().FixedUpdateBuffs();
            }
        }

        void OnDestroy()
        {
            // 清理所有事件
            GameEventManager.ClearAllEvents();
        }

        /// <summary>
        /// 初始化玩家
        /// </summary>
        private void InitializePlayer()
        {
            // 1. 创建或找到PlayerRoot GameObject
            if (playerRootPrefab != null)
            {
                Vector3 spawnPos = playerSpawnPoint != null ? playerSpawnPoint.position : Vector3.zero;
                playerRootInstance = Instantiate(playerRootPrefab, spawnPos, Quaternion.identity);
                playerRootInstance.name = "PlayerRoot";
            }
            else
            {
                // 如果没有预制体，尝试找到场景中的PlayerRoot
                playerRootInstance = GameObject.Find("PlayerRoot");
                if (playerRootInstance == null)
                {
                    // 创建一个默认的PlayerRoot
                    playerRootInstance = new GameObject("PlayerRoot");
                    playerRootInstance.transform.position = playerSpawnPoint != null ? playerSpawnPoint.position : Vector3.zero;
                    
                    // 添加必要的组件
                    playerRootInstance.AddComponent<Rigidbody2D>();
                    playerRootInstance.AddComponent<BoxCollider2D>();
                    
                    // 创建子物体用于显示
                    GameObject visualChild = new GameObject("Visual");
                    visualChild.transform.SetParent(playerRootInstance.transform);
                    visualChild.AddComponent<SpriteRenderer>();
                    
                    Debug.Log("创建了默认的PlayerRoot GameObject");
                }
            }

            // 2. 创建EntityPlayer并绑定GameObject
            currentPlayer = new EntityPlayer();
            currentPlayer.setEntityId(1);
            currentPlayer.BindGameObject(playerRootInstance);
            currentPlayer.initEntityProperty();

            // 3. 创建Agent并注册到BuffMgr
            currentAgent = new Agent();
            currentAgent.initialize(currentPlayer);
            BuffMgr.getInstance().addAgent(currentAgent);

            // 4. 添加PlayerController组件来处理碰撞
            var controller = playerRootInstance.GetComponent<PlayerController>();
            if (controller == null)
            {
                controller = playerRootInstance.AddComponent<PlayerController>();
            }
            controller.Initialize(currentPlayer);

            Debug.Log($"玩家初始化完成: EntityId={currentPlayer.getEntityId()}, GameObject={playerRootInstance.name}");
        }

        /// <summary>
        /// 获取当前玩家实体
        /// </summary>
        /// <returns></returns>
        public EntityPlayer GetCurrentPlayer()
        {
            return currentPlayer;
        }

        /// <summary>
        /// 获取当前玩家的Agent
        /// </summary>
        /// <returns></returns>
        public Agent GetCurrentAgent()
        {
            return currentAgent;
        }

        /// <summary>
        /// 手动触发buff更新（用于测试）
        /// </summary>
        [ContextMenu("手动更新Buff")]
        public void ManualUpdateBuffs()
        {
            BuffSystem.getInstance().FixedUpdateBuffs();
        }

        /// <summary>
        /// 测试添加buff
        /// </summary>
        [ContextMenu("测试添加Buff1001")]
        public void TestAddBuff()
        {
            if (currentPlayer != null)
            {
                var buff = new Buff.Buff1001();
                buff.initBuff(Random.Range(1000, 9999), currentPlayer, currentPlayer);
                BuffMgr.getInstance().addBufftoAgent(currentPlayer.getEntityId(), buff);
                Debug.Log("测试添加Buff1001");
            }
        }
        
        /// <summary>
        /// 测试玩家属性变化
        /// </summary>
        [ContextMenu("测试减血")]
        public void TestDamage()
        {
            if (currentPlayer != null)
            {
                currentPlayer.setHp(currentPlayer.getHp() - 10);
                Debug.Log($"玩家血量: {currentPlayer.getHp()}");
            }
        }
        
        /// <summary>
        /// 测试加速buff
        /// </summary>
        [ContextMenu("测试加速")]
        public void TestSpeedBuff()
        {
            if (currentPlayer != null)
            {
                currentPlayer.speedAddX += 2;
                Debug.Log($"玩家速度加成: {currentPlayer.speedAddX}");
            }
        }
    }
}