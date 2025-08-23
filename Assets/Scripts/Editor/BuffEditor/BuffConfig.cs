using UnityEngine;
using XJXMoudle;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BuffConfig", menuName = "Buff System/Buff Config")]
public class BuffConfig : ScriptableObject
{
    [Header("Buff基本信息")]
    [Tooltip("Buff的唯一标识符，用于区分不同的Buff类型")]
    public int buffId = 1000;
    
    [Tooltip("生成的Buff类名，格式为Buff+ID")]
    public string buffClassName = "Buff1000";
    
    [Header("Buff数据配置")]
    [Tooltip("包含所有Buff相关的配置数据")]
    public BuffDataConfig buffDataConfig = new BuffDataConfig();
}

[System.Serializable]
public class BuffDataConfig
{
    [Header("基础配置")]
    [Tooltip("Buff类型ID，与buffId相同，用于运行时识别")]
    public int buffTypeId = 1000;
    
    [Tooltip("Buff最大叠加层数\n• 1: 不可叠加\n• >1: 可叠加到指定层数")]
    [Range(1, 99)]
    public int maxLv = 1;
    
    [Tooltip("Buff层级类型\n• simLv: 单层Buff，不可叠加\n• mulLv: 多层Buff，可以叠加")]
    public EBuffLvType buffLvType = EBuffLvType.SimLv;
    
    [Tooltip("Buff刷新类型\n• noReflash: 不刷新持续时间\n• needReflash: 重新添加时刷新持续时间")]
    public EBuffTickType buffTickType = EBuffTickType.NoReflash;

    [Space(10)]
    [Header("时间配置")]
    [Tooltip("Buff基础持续时间（秒）\n• 0: 永久Buff\n• >0: 限时Buff")]
    [Min(0)]
    public float baseDuration = 10f;
    
    [Tooltip("Buff基础冷却时间（秒）\n通常用于控制相同Buff的施加频率")]
    [Min(0)]
    public float baseCDTime = 0f;
    
    [Tooltip("Buff基础间隔时间（秒）\n用于周期性触发的Buff效果,小于0无效")]
    
    public float baseInterval = 1f;

    [Space(10)]
    [Header("冲突配置")]
    [Tooltip("Buff冲突处理方式\n• noConflict: 无冲突，可共存\n• overrideConflict: 覆盖冲突，新Buff替换旧Buff")]
    public EBuffConflictType conflictType = EBuffConflictType.NoConflict;
    
    [Tooltip("冲突Buff ID列表\n当添加此Buff时，会根据冲突类型处理列表中的Buff\n点击右下角'+'号可以添加更多冲突BuffID")]
    public List<int> conflictBuffIds = new List<int>(); // 保持小写名称
    
    [Tooltip("Buff优先级\n数值越大优先级越高，用于冲突处理和显示排序")]
    [Range(0, 100)]
    public int buffPriority = 0;
    
    [Tooltip("减少层数\n当Buff结束时减少的层数，通常为1")]
    [Min(0)]
    public int reduceLv = 0;

    [Space(10)]
    [Header("表现配置")]
    [Tooltip("Buff显示名称\n在游戏界面中显示给玩家看的名字")]
    public string buffName = "新Buff";
    
    [Tooltip("Buff详细描述\n支持换行，详细说明Buff的效果和机制")]
    [TextArea(3, 5)]
    public string buffDesc = "Buff描述";
    
    [Tooltip("Buff图标资源路径\n用于在UI中显示的图标文件路径")]
    public string buffIconPath = "";
    
    [Tooltip("Buff特效资源路径\n用于在角色身上显示的特效文件路径")]
    public string buffEffectPath = "";
}
