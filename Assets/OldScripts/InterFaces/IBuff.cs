using Base;

namespace InterFaces
{
    public interface IBuff
    {
        public abstract void applyBuff(); //添加buff,不触发tick

        public abstract void enterBuff(); //tick==1

        public abstract void updateBuff(); //和下文的间隔,两者只需要实现一个即可,实际上只有一个方法也可以,只是为了方便

        public abstract void updateBuffByInterval();

        public abstract void exitBuff(); //tick == max

        public abstract void removeBuff(); //移除buff

        public abstract BuffStateType getBuffState(); //获取buff状态

        public abstract int getBuffId(); //获取buffId 唯一标识id
    }
}