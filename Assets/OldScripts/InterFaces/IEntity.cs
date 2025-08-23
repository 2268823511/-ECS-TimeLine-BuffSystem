namespace InterFaces
{
    public interface IEntity
    {
        public abstract void setEntityId(int entityId);
        public abstract int getEntityId();

        public abstract void setHp(int hp);
        public abstract int getHp();

        public abstract void setAtk(int Atk);
        public abstract int getAtk();

        public abstract void setDef(int Def);
        public abstract int getDef();

        public abstract void setSpeed(int speed);
        public abstract int getSpeed();
    }
}