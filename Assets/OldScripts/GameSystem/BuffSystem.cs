using System.Collections.Generic;
using AgentSpace;
using Base;
using Mgr;
using SingleTonSpace;

namespace GameSystem
{
    public class BuffSystem : SingleTon<BuffSystem>
    {
        //注册一个代理,不含buff
        public void registerAgent(Agent agent, EntityBase entity)
        {
            agent.initialize(entity);
            BuffMgr.getInstance().addAgent(agent);
        }

        //注册一个代理,含初始bufflist
        public void registerAgentBuffs(Agent agent, EntityBase entity, List<BuffBase> buffList)
        {
            agent.initialize(entity, buffList);
            BuffMgr.getInstance().addAgent(agent);
            BuffMgr.getInstance().addBuffstoAgent(agent.getEntity(), buffList);
        }

        //更新Buff
        public void FixedUpdateBuffs()
        {
            //BuffMgr.getInstance().startAllAgentBuffs();
            BuffMgr.getInstance().UpdateAllAgentBuffs();
            // BuffMgr.getInstance().exitAllAgentBuffs();
        }

        //移除Buff
        private void removeAgentBuff()
        {
        }

        //移除Bufflist
        public void removeAgentBuffs()
        {
        }

        //移除所有bufflists
        public void removeAgentAllBuff()
        {
        }
    }
}