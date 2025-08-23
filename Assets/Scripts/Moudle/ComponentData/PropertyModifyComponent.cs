using System;
using UnityEngine;

namespace XJXMoudle
{
    /// <summary>
    /// 属性修改组件
    /// 这是一个纯数据结构，用于在BuffSystem和AgentSystem之间传递属性修改信息
    /// 遵循ECS架构中的数据驱动原则
    /// </summary>
    [Serializable]
    public struct PropertyModifyComponent
    {
        /// <summary>
        /// 目标Agent的ID
        /// 指定要修改属性的Agent
        /// </summary>
        public int AgentId;

        /// <summary>
        /// 要修改的属性类型
        /// 如：Hp, Mp, Attack等
        /// </summary>
        public PropertyType PropertyType;

        /// <summary>
        /// 修改的数值
        /// 具体含义取决于ModifyType:
        /// - Add: 增加的数值
        /// - Multiply: 乘法系数
        /// - Set: 设置的目标值
        /// </summary>
        public float Value;

        /// <summary>
        /// 修改类型
        /// 决定Value如何应用到目标属性上
        /// </summary>
        public PropertyModifyType ModifyType;

        /// <summary>
        /// 持续时间（秒）
        /// 0表示立即生效的一次性修改
        /// >0表示持续性修改，需要在时间到期后恢复
        /// </summary>
        public float Duration;   //这个字段是干什么的?
 
        /// <summary>
        /// 来源Buff的实例ID
        /// 用于追踪修改来源，便于调试和管理
        /// </summary>
        public int SourceBuffId;

        /// <summary>
        /// 处理标记
        /// true: 已被AgentSystem处理
        /// false: 等待处理
        /// </summary>
        public bool IsProcessed;

        /// <summary>
        /// 创建时间戳
        /// 用于计算持续时间和调试
        /// </summary>
        public float CreateTime;

        /// <summary>
        /// 构造函数
        /// </summary>
        // public PropertyModifyComponent(int agentId, PropertyType propertyType, float value, 
        //     PropertyModifyType modifyType, float duration = 0f, int sourceBuffId = 0,float time = 0)
        // {
        //     AgentId = agentId;
        //     PropertyType = propertyType;
        //     Value = value;
        //     ModifyType = modifyType;
        //     Duration = duration;
        //     SourceBuffId = sourceBuffId;
        //     IsProcessed = false;
        //     CreateTime = time;
        // }

        /// <summary>
        /// 检查是否已过期（仅对持续性修改有效）
        /// </summary>
        public bool IsExpired(float time) //外部传入当前帧时间
        {
            if (Duration <= 0) return false; // 一次性修改不会过期
            return time >= CreateTime + Duration;
        }

        /// <summary>
        /// 获取剩余时间
        /// </summary>
        public float GetRemainingTime(float time) //外部传入当前帧时间
        {
            if (Duration <= 0) return 0f;
            return Mathf.Max(0f, CreateTime + Duration - time);
        }
    }
}