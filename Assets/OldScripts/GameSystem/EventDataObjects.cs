using System.Collections.Generic;
using Base;
using Entity;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// Buff UI数据传输对象
    /// </summary>
    public class BuffUIData
    {
        public int BuffTypeId { get; set; }
        public string State { get; set; }
        public int Level { get; set; }
        public int CurrentTick { get; set; }
        public float TotalTime { get; set; }
        public float RemainingTime { get; set; }

        public BuffUIData(BuffBase buff)
        {
            BuffTypeId = buff.buffTypeId;
            State = buff.buffState.ToString();
            Level = buff.nowBuffLv;
            CurrentTick = buff.tick;
            TotalTime = buff.totalTime;

            // 计算剩余时间
            if (buff.totalTime > 0)
            {
                float elapsed = buff.tick * Time.fixedDeltaTime;
                RemainingTime = Mathf.Max(0, buff.totalTime - elapsed);
            }
            else
            {
                RemainingTime = -1; // 永久buff
            }
        }
    }

    /// <summary>
    /// 玩家UI数据传输对象
    /// </summary>
    public class PlayerUIData
    {
        public int Hp { get; set; }
        public int SpeedX { get; set; }
        public int SpeedY { get; set; }
        public int AtkSpecial { get; set; }
        public bool DoubleJump { get; set; }
        public float Scale { get; set; }
        public int EntityId { get; set; }

        public PlayerUIData(EntityPlayer player)
        {
            EntityId = player.getEntityId();
            Hp = player.getHp();

            // 计算实际的水平速度 (基础速度 + buff加成 - buff减成)
            SpeedX = player.getSpeed() + player.speedAddX - player.speedCutX;

            // 垂直速度用跳跃倍数表示
            SpeedY = player.JumPMulFor;

            // 攻击力
            AtkSpecial = player.getAtk();

            // 跳跃次数
            DoubleJump = player.IsOpenDoubleJump;

            // 缩放
            Scale = player.scalePrefab;
        }
    }

    /// <summary>
    /// Buff列表数据传输对象
    /// </summary>
    public class BuffListData
    {
        public List<BuffUIData> ActiveBuffs { get; set; }
        public int EntityId { get; set; }

        public BuffListData(int entityId, List<BuffBase> buffs)
        {
            EntityId = entityId;
            ActiveBuffs = new List<BuffUIData>();

            if (buffs != null)
            {
                foreach (var buff in buffs)
                {
                    if (buff.buffState == BuffStateType.hasEnter) // 只包含激活的buff
                    {
                        ActiveBuffs.Add(new BuffUIData(buff));
                    }
                }
            }
        }
    }
}