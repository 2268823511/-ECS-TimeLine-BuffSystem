using InterFaces;
using UnityEngine;

namespace Base
{
    public class EntityBase : IEntity
    {
        //实体唯一标志id
        private int entityId;

        //引用,后续可能会更改
        private GameObject entityObj;
        private int hp;
        private int atk;

        private int def;

        //水平移动速度
        private int moveSpeed;

        //是否需要?
        private bool isDead;

        public void setEntityId(int entityId)
        {
            this.entityId = entityId;
        }

        public int getEntityId()
        {
            return entityId;
        }

        public virtual void setHp(int hp)
        {
            this.hp = hp;
        }

        public virtual int getHp()
        {
            return hp;
        }

        public virtual void setAtk(int Atk)
        {
            this.atk = Atk;
        }

        public virtual int getAtk()
        {
            return atk;
        }

        public virtual void setDef(int Def)
        {
            this.def = Def;
        }

        public virtual int getDef()
        {
            return def;
        }

        public virtual void setSpeed(int speed)
        {
            this.moveSpeed = speed;
        }

        public virtual int getSpeed()
        {
            return moveSpeed;
        }

        public virtual void setDead(bool isDead)
        {
            this.isDead = isDead;
        }

        public virtual bool getIsDead()
        {
            return isDead;
        }

        //todo 后续调整
        public virtual void destory()
        {
            if (entityObj != null) GameObject.Destroy(entityObj);
            entityObj = null;
            isDead = true;
            hp = 0;
            atk = 0;
            def = 0;
            moveSpeed = 0;
        }

        public virtual GameObject getEntityObj()
        {
            return entityObj;
        }
    }
}