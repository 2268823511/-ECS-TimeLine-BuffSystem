using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;
using Mgr;

public class BuffItemDetailInfo : MonoBehaviour
{
    public Button BG;

    //buff状态,有初始化,进入,完成,移除
    public TextMeshProUGUI State;

    //层级
    public TextMeshProUGUI Lv;

    //当前tick
    public TextMeshProUGUI CurrTick;

    private int currentBuffTypeId = -1;
    private int currentEntityId = 1; // 默认玩家ID

    // Start is called before the first frame update
    void Start()
    {
        BG?.onClick.AddListener(() => { gameObject.SetActive(false); });

        // 订阅buff数据更新事件
        GameEventManager.Subscribe<BuffUIData>(GameEventNames.BuffUI_DataUpdated, OnBuffDataUpdated);
    }

    void OnDestroy()
    {
        // 取消订阅防止内存泄漏
        GameEventManager.Unsubscribe<BuffUIData>(GameEventNames.BuffUI_DataUpdated, OnBuffDataUpdated);
    }

    /// <summary>
    /// 设置要显示的buff数据
    /// </summary>
    /// <param name="BuffTypeId">buff类型ID</param>
    public void setData(int BuffTypeId)
    {
        currentBuffTypeId = BuffTypeId;

        // 立即从BuffMgr获取当前buff数据并显示
        var buff = BuffMgr.getInstance().GetBuffByTypeId(currentEntityId, BuffTypeId);
        if (buff != null)
        {
            var uiData = new BuffUIData(buff);
            UpdateUI(uiData);
        }
        else
        {
            // 如果buff不存在，显示默认信息
            ShowDefaultInfo();
        }
    }

    /// <summary>
    /// 设置当前玩家ID
    /// </summary>
    /// <param name="entityId">玩家实体ID</param>
    public void SetCurrentEntityId(int entityId)
    {
        currentEntityId = entityId;
    }

    /// <summary>
    /// buff数据更新回调
    /// </summary>
    /// <param name="data">buff UI数据</param>
    private void OnBuffDataUpdated(BuffUIData data)
    {
        // 只更新当前显示的buff
        if (data.BuffTypeId == currentBuffTypeId)
        {
            UpdateUI(data);
        }
    }

    /// <summary>
    /// 更新UI显示
    /// </summary>
    /// <param name="data">buff UI数据</param>
    private void UpdateUI(BuffUIData data)
    {
        if (State != null)
        {
            State.text = $"State : {data.State}";
        }

        if (Lv != null)
        {
            Lv.text = $"level: {data.Level}";
        }

        if (CurrTick != null)
        {
            CurrTick.text = $"Tick: {data.CurrentTick}";
        }

        // 可以添加更多信息显示
        //Debug.Log($"Buff {data.BuffTypeId} UI更新: 状态={data.State}, 层级={data.Level}, Tick={data.CurrentTick}");
    }

    /// <summary>
    /// 显示默认信息（当buff不存在时）
    /// </summary>
    private void ShowDefaultInfo()
    {
        if (State != null)
        {
            State.text = $"状态: 未激活";
        }

        if (Lv != null)
        {
            Lv.text = $"层级: 0";
        }

        if (CurrTick != null)
        {
            CurrTick.text = $"Tick: 0";
        }

        Debug.Log($"Buff {currentBuffTypeId} 当前未激活");
    }
}