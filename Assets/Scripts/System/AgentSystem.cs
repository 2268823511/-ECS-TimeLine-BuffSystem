using UnityEngine;
using XJXInterface;
using XJXMgr;
using XJXMoudle;
using XJXSingleton;

namespace XJXSystem
{
    public class AgentSystem : SystemBase, IStart, IUpdate, IFixedUpdate
    {
        public static AgentSystem Instance => Singleton<AgentSystem>.Instance;


        protected override void OnInitialize()
        {
            Debug.Log("AgentSystem Initialized");
        }


        //todo 有需要就用, 暂时没用到,可以删除接口
        public void Start()
        {
            AgentMgr.Instance.InitMgr();
        }

        public void Update(float time)
        {
            updateAgentProPery(time);
        }


        public void FixedUpdate(float time)
        {
            updateAgentTransform(time);
            updateAgentAnimation(time);
            updateAgentEffect(time);
        }


		#region private Func

		private void updateAgentProPery(float time)
		{
			var instructList = SystemDataMgr.Instance.GetPendingPropertyModifies();
			for (int i = 0; i < instructList.Count; i++)
			{
				var instructInfo = instructList[i];
				if (instructInfo.IsProcessed)
				{
					continue;
				}
				var agent = AgentMgr.Instance.GetAgent(instructInfo.AgentId);
				if (agent == null)
				{
					continue;
				}
				var entity = agent.GetEntity();
				
				switch (instructInfo.PropertyType)
				{
					case PropertyType.Hp:
						if (instructInfo.ModifyType == PropertyModifyType.Add) entity.Hp += instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Multiply) entity.Hp *= instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Set) entity.Hp = instructInfo.Value;
						break;
					case PropertyType.Mp:
						if (instructInfo.ModifyType == PropertyModifyType.Add) entity.Mp += instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Multiply) entity.Mp *= instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Set) entity.Mp = instructInfo.Value;
						break;
					case PropertyType.Attack:
						if (instructInfo.ModifyType == PropertyModifyType.Add) entity.Attack += instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Multiply) entity.Attack *= instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Set) entity.Attack = instructInfo.Value;
						break;
					case PropertyType.Defense:
						if (instructInfo.ModifyType == PropertyModifyType.Add) entity.Defense += instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Multiply) entity.Defense *= instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Set) entity.Defense = instructInfo.Value;
						break;
					case PropertyType.SpeedX:
						if (instructInfo.ModifyType == PropertyModifyType.Add) entity.SpeedX += instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Multiply) entity.SpeedX *= instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Set) entity.SpeedX = instructInfo.Value;
						break;
					case PropertyType.SpeedY:
						if (instructInfo.ModifyType == PropertyModifyType.Add) entity.SpeedY += instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Multiply) entity.SpeedY *= instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Set) entity.SpeedY = instructInfo.Value;
						break;
					case PropertyType.Scale:
						if (instructInfo.ModifyType == PropertyModifyType.Add) entity.Scale += instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Multiply) entity.Scale *= instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Set) entity.Scale = instructInfo.Value;
						break;
					case PropertyType.JumpMaxTimes:
						var playerEntity = entity as PlayerEntity;
						if (instructInfo.ModifyType == PropertyModifyType.Add) playerEntity.JumpMaxTimes += (int)instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Multiply) playerEntity.JumpMaxTimes *= (int)instructInfo.Value;
						if (instructInfo.ModifyType == PropertyModifyType.Set) playerEntity.JumpMaxTimes = (int)instructInfo.Value;
						break;
				}
				//标记为已处理的
				SystemDataMgr.Instance.MarkAsProcessed(i);
				Debug.Log($"AgentSystem updateAgentProPery: {agent.agentId}, {entity.Hp}, {entity.Mp}, {entity.SpeedX}" );
				
			}
		}

		//physics update
		private void updateAgentTransform(float time)
		{
			//try to update scale
			AgentMgr.Instance.GetAllAgents().ForEach(agent =>
			{
				var entity = agent.GetEntity();
				if (entity == null)
				{
					return;
				}
				var transform = entity.EntityGO.GetComponent<Transform>();
				if (transform.localScale.x == entity.Scale) return;
				transform.localScale = new Vector3(entity.Scale, entity.Scale, entity.Scale);
			});
		}

        private void updateAgentAnimation(float time)
		{
		}

        private void updateAgentEffect(float time)
        {
        }

        #endregion
    }
}