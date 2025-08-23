using UnityEngine;
using Entity;

namespace Entity
{
    /// <summary>
    /// 玩家控制器 - 连接Unity组件和EntityPlayer
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private EntityPlayer entityPlayer;

        /// <summary>
        /// 初始化控制器
        /// </summary>
        /// <param name="player">实体玩家</param>
        public void Initialize(EntityPlayer player)
        {
            entityPlayer = player;
        }

        /// <summary>
        /// Unity碰撞检测回调
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (entityPlayer == null) return;
            entityPlayer.OnCollisionEnter(collision);
        }
    }
}