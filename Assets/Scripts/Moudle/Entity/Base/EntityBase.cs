using UnityEngine;

namespace XJXMoudle
{
	public class EntityBase
	{
		private int EntityId { get; set; }

		public GameObject EntityGO { get; set; }

		#region base property
		public float Hp { get; set; }

		public float Mp { get; set; }

		public float Attack { get; set; }

		public float Defense { get; set; }

		public float SpeedX { get; set; }
		public float SpeedY { get; set; }
		
		//引用到组件上一些属性
		public float Scale { get; set; }

		#endregion

	}
}
