using System.Collections.Generic;
using Base;
using GameSystem;

namespace AgentSpace
{
    public class Agent
    {
        //多一份引用,在buffList里面,后续可优化
        private EntityBase entity;

        //buff列表
        private List<BuffBase> buffList;

        //初始化
        public void initialize(EntityBase entity)
        {
            if (entity != null) this.entity = entity;
        }

        public void initialize(EntityBase entity, List<BuffBase> buffList)
        {
            initialize(entity);
            if (buffList != null)
            {
                this.buffList = new List<BuffBase>();
                this.buffList.AddRange(buffList);
            }
            else
            {
                this.buffList = new List<BuffBase>();
            }
        }

        #region 获取引用

        public EntityBase getEntity()
        {
            return entity;
        }

        public List<BuffBase> getBuffList()
        {
            return buffList;
        }

        #endregion

        //添加buff
        public void addBuff(BuffBase buff)
        {
            if (buff == null) return;
            if (buffList == null)
            {
                buffList = new List<BuffBase>();
            }

            // 简单添加，冲突处理由BuffMgr负责
            buffList.Add(buff);
            
            // 触发buff列表更新
            GameEventManager.TriggerBuffListUpdate(entity.getEntityId(), buffList);
        }

        //添加bufflist
        public void addBuffList(List<BuffBase> buffList)
        {
            if (buffList == null) return;
            if (this.buffList == null)
            {
                this.buffList = new List<BuffBase>();
            }

            this.buffList.AddRange(buffList);
        }

        //移除buff
        public void removeBuff(BuffBase buff)
        {
            if (buff == null) return;
            if (buffList == null) return;
            buffList.Remove(buff);
        }

        //移除buffById
        public void removeBuffById(BuffBase buff)
        {
            if (buff == null) return;
            if (buffList == null) return;
            for (int i = 0; i < buffList.Count; i++)
            {
                if (buffList[i].getBuffId() == buff.getBuffId())
                {
                    buffList.Remove(buffList[i]);
                    break;
                }
            }
        }

        //移除bufflist ByIds
        public void removeBuffListByIds(List<BuffBase> buffList)
        {
            if (buffList == null) return;
            if (this.buffList == null) return;
            for (int i = 0; i < this.buffList.Count; i++)
            {
                for (int j = 0; j < buffList.Count; j++)
                {
                    if (this.buffList[j].getBuffId() == buffList[i].getBuffId())
                    {
                        this.buffList.Remove(this.buffList[j]);
                        break;
                    }
                }
            }
        }

        //清空buff列表
        public void clearBuffList()
        {
            if (buffList == null) return;
            buffList.Clear();
        }

        // 通知buff列表变化
        public void NotifyBuffListChanged()
        {
            if (entity != null && buffList != null)
            {
                GameEventManager.TriggerBuffListUpdate(entity.getEntityId(), buffList);
            }
        }

        //销毁对象
        public void destroyAgent()
        {
            clearBuffList();
            if (entity != null) entity.destory();
            entity = null;
        }
    }
}