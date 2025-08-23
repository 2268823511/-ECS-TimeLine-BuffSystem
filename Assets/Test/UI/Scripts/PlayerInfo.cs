using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;
using Mgr;
using AgentSpace;
using Base;
using gameRoot;

public class PlayerInfo : MonoBehaviour
{
    public GameObject PlayerPro;
    public GameObject BuffDetailInfo;
    public ScrollRect scrollRect;

    // 引用GameManager
    private GameManager gameManager;
    private EntityPlayer currentPlayer;
    private Agent currentAgent;

    private TextMeshProUGUI Hp;
    private TextMeshProUGUI SpeedX;
    private TextMeshProUGUI SpeedY;
    private TextMeshProUGUI AtkSpecial;
    private TextMeshProUGUI DoubleJump;
    private TextMeshProUGUI Scale;

    void Start()
    {
        InitializeUI();
        FindGameManager();
        SubscribeToEvents();
        SetupBuffButtons();
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitializeUI()
    {
        if (PlayerPro != null)
        {
            Hp = PlayerPro.transform.Find("Hp").GetComponent<TextMeshProUGUI>();
            SpeedX = PlayerPro.transform.Find("SpeedX").GetComponent<TextMeshProUGUI>();
            SpeedY = PlayerPro.transform.Find("SpeedY").GetComponent<TextMeshProUGUI>();
            AtkSpecial = PlayerPro.transform.Find("AtkSpecial").GetComponent<TextMeshProUGUI>();
            DoubleJump = PlayerPro.transform.Find("DoubleJump").GetComponent<TextMeshProUGUI>();
            Scale = PlayerPro.transform.Find("Scale").GetComponent<TextMeshProUGUI>();
        }
    }

    private void FindGameManager()
    {
        // 找到GameManager并获取玩家引用
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            // 延迟获取玩家引用，因为GameManager可能还在初始化
            StartCoroutine(WaitForPlayerInitialization());
        }
        else
        {
            Debug.LogError("未找到GameManager!");
        }
    }

    private IEnumerator WaitForPlayerInitialization()
    {
        // 等待GameManager完成玩家初始化
        while (gameManager.GetCurrentPlayer() == null)
        {
            yield return null;
        }

        currentPlayer = gameManager.GetCurrentPlayer();
        currentAgent = gameManager.GetCurrentAgent();
        Debug.Log("PlayerInfo连接到GameManager成功");

        // 立即更新一次UI
        if (currentPlayer != null)
        {
            GameEventManager.TriggerPlayerUIUpdate(currentPlayer);
        }
    }

    private void SubscribeToEvents()
    {
        GameEventManager.Subscribe<PlayerUIData>(GameEventNames.PlayerUI_DataUpdated, UpdatePlayerUI);
        GameEventManager.Subscribe<BuffListData>(GameEventNames.BuffUI_ListUpdated, UpdateBuffList);
    }

    private void UnsubscribeFromEvents()
    {
        GameEventManager.Unsubscribe<PlayerUIData>(GameEventNames.PlayerUI_DataUpdated, UpdatePlayerUI);
        GameEventManager.Unsubscribe<BuffListData>(GameEventNames.BuffUI_ListUpdated, UpdateBuffList);
    }

    private void SetupBuffButtons()
    {
        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            Button button = scrollRect.content.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                int buffTypeId = 1001 + i;
                button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
                    string.Format($"BuffId:{buffTypeId}");
                button.onClick.AddListener(() => { AddBuffToPlayer(buffTypeId); });
            }
        }
    }

    /// <summary>
    /// 添加buff到玩家
    /// </summary>
    /// <param name="buffTypeId">buff类型ID</param>
    private void AddBuffToPlayer(int buffTypeId)
    {
        if (currentPlayer == null)
        {
            Debug.LogError("当前玩家为空！请确保GameManager已正确初始化。");
            return;
        }

        BuffBase newBuff = CreateBuffByTypeId(buffTypeId);
        if (newBuff != null)
        {
            newBuff.initBuff(UnityEngine.Random.Range(1000, 9999), currentPlayer, currentPlayer);
            BuffMgr.getInstance().addBufftoAgent(currentPlayer.getEntityId(), newBuff);

            Debug.Log($"添加buff {buffTypeId} 到玩家 {currentPlayer.getEntityId()}");
            openBuffDetailInfo(buffTypeId);
        }
        else
        {
            Debug.LogWarning($"未找到buff类型: {buffTypeId}");
        }
    }

    /// <summary>
    /// 根据BuffTypeId创建对应的buff实例
    /// </summary>
    /// <param name="buffTypeId">buff类型ID</param>
    /// <returns>buff实例</returns>
    private BuffBase CreateBuffByTypeId(int buffTypeId)
    {
        switch (buffTypeId)
        {
            case 1001: return new Buff.Buff1001();
            case 1002: return new Buff.Buff1002();
            case 1003: return new Buff.Buff1003();
            case 1004: return new Buff.Buff1004();
            case 1005: return new Buff.Buff1005();
            case 1006: return new Buff.Buff1006();
            case 1007: return new Buff.Buff1007();
            case 1008: return new Buff.Buff1008();
            case 1009: return new Buff.Buff1009();
            default: return null;
        }
    }

    /// <summary>
    /// 打开buff详情界面
    /// </summary>
    /// <param name="buffTypeId">buff类型ID</param>
    private void openBuffDetailInfo(int buffTypeId)
    {
        BuffDetailInfo.SetActive(true);
        var com = BuffDetailInfo.GetComponent<BuffItemDetailInfo>();
        com.setData(buffTypeId);

        // 设置当前玩家ID给详情界面
        if (currentPlayer != null)
        {
            com.SetCurrentEntityId(currentPlayer.getEntityId());
        }
    }

    /// <summary>
    /// 更新玩家UI信息 - 通过事件触发
    /// </summary>
    /// <param name="data">玩家UI数据</param>
    private void UpdatePlayerUI(PlayerUIData data)
    {
        // 只更新当前玩家的数据
        if (currentPlayer == null || data.EntityId != currentPlayer.getEntityId()) return;

        if (Hp != null) Hp.text = $"HP: {data.Hp}";
        if (SpeedX != null) SpeedX.text = $"SpeedX: {data.SpeedX}";
        if (SpeedY != null) SpeedY.text = $"SpeedY: {data.SpeedY}";
        if (AtkSpecial != null) AtkSpecial.text = $"Atk: {data.AtkSpecial}";
        if (DoubleJump != null) DoubleJump.text = $"Can Double Jump : {data.DoubleJump}";
        if (Scale != null) Scale.text = $"Scale: {data.Scale:F2}";
    }

    /// <summary>
    /// 更新buff列表 - 通过事件触发
    /// </summary>
    /// <param name="data">buff列表数据</param>
    private void UpdateBuffList(BuffListData data)
    {
        // 只处理当前玩家的buff列表
        if (currentPlayer == null || data.EntityId != currentPlayer.getEntityId()) return;

        Debug.Log($"玩家 {data.EntityId} 当前有 {data.ActiveBuffs.Count} 个激活的buff");

        // 更新buff按钮状态
        UpdateBuffButtonStates(data.ActiveBuffs);
    }

    /// <summary>
    /// 更新buff按钮状态
    /// </summary>
    /// <param name="activeBuffs">当前激活的buff列表</param>
    private void UpdateBuffButtonStates(List<BuffUIData> activeBuffs)
    {
        // 重置所有按钮状态
        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            Button button = scrollRect.content.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                // 默认颜色
                button.GetComponentInChildren<Image>().color = Color.white;
            }
        }

        // 高亮激活的buff按钮
        foreach (var buffData in activeBuffs)
        {
            int buttonIndex = buffData.BuffTypeId - 1001; // 计算按钮索引
            if (buttonIndex >= 0 && buttonIndex < scrollRect.content.childCount)
            {
                Button button = scrollRect.content.GetChild(buttonIndex).GetComponent<Button>();
                if (button != null)
                {
                    // 激活状态颜色（绿色）
                    button.GetComponentInChildren<Image>().color = Color.green;
                }
            }
        }
    }

    /// <summary>
    /// 获取当前玩家（供外部调用）
    /// </summary>
    /// <returns></returns>
    public EntityPlayer GetCurrentPlayer()
    {
        return currentPlayer;
    }

    /// <summary>
    /// 手动刷新玩家数据（用于测试）
    /// </summary>
    [ContextMenu("刷新玩家数据")]
    public void RefreshPlayerData()
    {
        if (currentPlayer != null)
        {
            GameEventManager.TriggerPlayerUIUpdate(currentPlayer);
        }
    }
}