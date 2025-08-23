using System;
using System.Collections.Generic;
using SingleTonSpace;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 通用事件管理器 - 基于字符串和泛型的事件系统
    /// </summary>
    public class GameEventManager : SingleTon<GameEventManager>
    {
        private static Dictionary<string, Delegate> eventDictionary = new Dictionary<string, Delegate>();

        #region 泛型事件订阅和取消订阅

        public static void Subscribe<T>(string eventName, Action<T> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] = Delegate.Combine(eventDictionary[eventName], listener);
            }
            else
            {
                eventDictionary[eventName] = listener;
            }
        }

        public static void Subscribe(string eventName, Action listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] = Delegate.Combine(eventDictionary[eventName], listener);
            }
            else
            {
                eventDictionary[eventName] = listener;
            }
        }

        public static void Unsubscribe<T>(string eventName, Action<T> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] = Delegate.Remove(eventDictionary[eventName], listener);
                if (eventDictionary[eventName] == null)
                {
                    eventDictionary.Remove(eventName);
                }
            }
        }

        public static void Unsubscribe(string eventName, Action listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] = Delegate.Remove(eventDictionary[eventName], listener);
                if (eventDictionary[eventName] == null)
                {
                    eventDictionary.Remove(eventName);
                }
            }
        }

        #endregion

        #region 事件触发

        public static void Trigger<T>(string eventName, T data)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                var action = eventDictionary[eventName] as Action<T>;
                action?.Invoke(data);

#if UNITY_EDITOR
                //Debug.Log($"[Event] Triggered: {eventName} with data type: {typeof(T).Name}");
#endif
            }
        }

        public static void Trigger(string eventName)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                var action = eventDictionary[eventName] as Action;
                action?.Invoke();

#if UNITY_EDITOR
                Debug.Log($"[Event] Triggered: {eventName}");
#endif
            }
        }

        #endregion

        #region 便捷方法 - 专门为你的项目设计

        /// <summary>
        /// 触发Buff UI数据更新
        /// </summary>
        public static void TriggerBuffUIUpdate(Base.BuffBase buff)
        {
            var uiData = new BuffUIData(buff);
            Trigger(GameEventNames.BuffUI_DataUpdated, uiData);
        }

        /// <summary>
        /// 触发玩家UI数据更新
        /// </summary>
        public static void TriggerPlayerUIUpdate(Entity.EntityPlayer player)
        {
            var uiData = new PlayerUIData(player);
            Trigger(GameEventNames.PlayerUI_DataUpdated, uiData);
        }

        /// <summary>
        /// 触发Buff列表更新
        /// </summary>
        public static void TriggerBuffListUpdate(int entityId, List<Base.BuffBase> buffs)
        {
            var listData = new BuffListData(entityId, buffs);
            Trigger(GameEventNames.BuffUI_ListUpdated, listData);
        }

        #endregion

        public static void ClearAllEvents()
        {
            eventDictionary.Clear();
        }
    }
}




