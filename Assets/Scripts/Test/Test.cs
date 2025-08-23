
using UnityEngine;
using XJXMgr;
using XJXMoudle;
using System.Collections.Generic;
using XJXSystem;

public class Test : MonoBehaviour
{
    public GameObject testEntityGo;
    public Vector3 targetPos;
    
    private int currentAgentId = 0;
    private List<Agent> testAgents = new List<Agent>();
    
    void Start()
    {
        Debug.Log("测试脚本启动 - 按空格键创建代理，按数字键1-8添加Buff1001-1008");
    }

    void Update()
    {
        // 按空格键创建代理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateTestAgent();
        }
        
        // 按数字键1-8添加相应的buff
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddBuff(1001);
        if (Input.GetKeyDown(KeyCode.Alpha2)) AddBuff(1002);
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddBuff(1003);
        if (Input.GetKeyDown(KeyCode.Alpha4)) AddBuff(1004);
        if (Input.GetKeyDown(KeyCode.Alpha5)) AddBuff(1005);
        if (Input.GetKeyDown(KeyCode.Alpha6)) AddBuff(1006);
        if (Input.GetKeyDown(KeyCode.Alpha7)) AddBuff(1007);
        if (Input.GetKeyDown(KeyCode.Alpha8)) AddBuff(1008);
        
        // 按R键移除最后一个代理的所有buff（用于测试）
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveAllBuffsFromLastAgent();
        }
        
        // 按C键清除所有代理（用于测试）
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearAllAgents();
        }
        
        // 按T键测试buff冲突（用于测试）
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestBuffConflicts();
        }
        
        // 按M键创建怪物代理（用于测试）
        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateMonsterAgent();
        }
        
        // 按P键打印所有代理状态
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintAllAgentsStatus();
        }
    }
    
    private void CreateTestAgent()
    {
        currentAgentId++;
        
        // 使用工厂方法创建带PlayerEntity的代理
        Vector3 spawnPos = targetPos + Vector3.right * (currentAgentId - 1) * 2f;
        Agent newAgent = AgentMgr.Instance.CreatePlayerAgent(currentAgentId, spawnPos, testEntityGo);
        
        if (newAgent != null)
        {
            testAgents.Add(newAgent);
            Debug.Log($"创建了代理 {currentAgentId}，当前总共有 {testAgents.Count} 个代理");
        }
    }
    
    private void AddBuff(int buffId)
    {
        if (testAgents.Count == 0)
        {
            Debug.LogWarning("没有代理可以添加buff！请先按空格键创建代理。");
            return;
        }
        
        // 给最后创建的代理添加buff
        Agent targetAgent = testAgents[testAgents.Count - 1];
        
        // 使用BuffMgr添加buff（需要先实现BuffMgr）
        BuffMgr.Instance.AddBuffToAgent(targetAgent, targetAgent, buffId);
        
        Debug.Log($"给代理 {targetAgent.GetAgentId()} 添加了 Buff{buffId}");
    }
    
    private void RemoveAllBuffsFromLastAgent()
    {
        if (testAgents.Count == 0)
        {
            Debug.LogWarning("没有代理可以移除buff！");
            return;
        }
        
        Agent targetAgent = testAgents[testAgents.Count - 1];
        BuffMgr.Instance.RemoveAllBuffsFromAgent(targetAgent);
        
        Debug.Log($"移除了代理 {targetAgent.GetAgentId()} 的所有buff");
    }
    
    private void ClearAllAgents()
    {
        foreach (Agent agent in testAgents)
        {
            AgentMgr.Instance.RemoveAgent(agent.GetAgentId());
        }
        
        testAgents.Clear();
        currentAgentId = 0;
        
        Debug.Log("清除了所有测试代理");
    }
    
    #region 新增测试方法
    
    private void CreateMonsterAgent()
    {
        int monsterId = currentAgentId + 1000; // 使用1000+的ID表示怪物
        Vector3 spawnPos = targetPos + Vector3.left * 3f; // 在左侧创建怪物
        
        Agent monsterAgent = AgentMgr.Instance.CreateMonsterAgent(monsterId, spawnPos);
        if (monsterAgent != null)
        {
            testAgents.Add(monsterAgent);
            Debug.Log($"创建了怪物代理 {monsterId}");
        }
    }
    
    private void TestBuffConflicts()
    {
        if (testAgents.Count == 0)
        {
            Debug.LogWarning("没有代理可以测试buff冲突！");
            return;
        }
        
        Agent targetAgent = testAgents[testAgents.Count - 1];
        
        Debug.Log("=== 开始测试Buff冲突 ===");
        
        // 测试1: 添加Buff1002（老寒腿）和Buff1005（疾跑），它们应该冲突
        Debug.Log("测试1: 添加Buff1002（老寒腿，减速）");
        targetAgent.AddBuff(1002);
        
        Debug.Log("测试2: 添加Buff1005（疾跑，加速），应该与Buff1002冲突");
        targetAgent.AddBuff(1005);
        
        // 测试2: 添加Buff1003（变大）和Buff1004（变小），它们应该冲突
        System.Threading.Tasks.Task.Delay(2000).ContinueWith(_ => {
            UnityEngine.Debug.Log("测试3: 添加Buff1003（变大2倍）");
            targetAgent.AddBuff(1003);
            
            UnityEngine.Debug.Log("测试4: 添加Buff1004（变小2倍），应该与Buff1003冲突");
            targetAgent.AddBuff(1004);
        });
        
        // 测试3: 多层buff测试
        System.Threading.Tasks.Task.Delay(4000).ContinueWith(_ => {
            UnityEngine.Debug.Log("测试5: 多次添加Buff1001（多层buff）");
            targetAgent.AddBuff(1001);
            targetAgent.AddBuff(1001);
            targetAgent.AddBuff(1001);
        });
    }
    
    private void PrintAllAgentsStatus()
    {
        Debug.Log("=== 所有代理状态 ===");
        AgentMgr.Instance.PrintAllAgentsStatus();
        
        // 打印Buff系统统计
        var buffStats = BuffSystem.Instance.GetStats();
        Debug.Log($"Buff系统状态 - 活跃buff数量: {buffStats.TotalActiveBuffs}");
        
        // 打印每个代理的详细buff信息
        foreach (var agent in testAgents)
        {
            var buffs = agent.GetAllBuffs();
            Debug.Log($"代理 {agent.GetAgentId()} 拥有 {buffs.Count} 个buff:");
            foreach (var buff in buffs)
            {
                Debug.Log($"  - {buff.buffData.BuffName} (ID:{buff.buffData.BuffTypeId}, 状态:{buff.CurrentState})");
            }
        }
    }
    
    #endregion
    
    private void OnGUI()
    {
        // 在屏幕上显示操作提示
        GUILayout.BeginArea(new Rect(10, 10, 400, 300));
        GUILayout.Label("=== Buff系统测试 ===");
        GUILayout.Label("Space: 创建玩家代理");
        GUILayout.Label("M: 创建怪物代理");
        GUILayout.Label("1-8: 添加Buff1001-1008");
        GUILayout.Label("R: 移除最后代理的所有buff");
        GUILayout.Label("T: 测试buff冲突");
        GUILayout.Label("P: 打印所有代理状态");
        GUILayout.Label("C: 清除所有代理");
        GUILayout.Space(10);
        GUILayout.Label($"当前代理数量: {testAgents.Count}");
        
        if (testAgents.Count > 0)
        {
            Agent lastAgent = testAgents[testAgents.Count - 1];
            GUILayout.Label($"最后代理ID: {lastAgent.GetAgentId()}");
            GUILayout.Label($"代理类型: {lastAgent.GetEntity()?.GetType().Name ?? "None"}");
            GUILayout.Label($"Buff数量: {lastAgent.GetBuffCount()}");
            
            var entity = lastAgent.GetEntity();
            if (entity != null)
            {
                GUILayout.Label($"HP: {entity.Hp:F1}");
                GUILayout.Label($"Scale: {entity.Scale:F1}");
                GUILayout.Label($"SpeedX: {entity.SpeedX:F1}");
            }
        }
        
        // 显示系统统计
        GUILayout.Space(10);
        var buffStats = XJXSystem.BuffSystem.Instance.GetStats();
        GUILayout.Label($"活跃Buff数量: {buffStats.TotalActiveBuffs}");
        
        GUILayout.EndArea();
    }
}
