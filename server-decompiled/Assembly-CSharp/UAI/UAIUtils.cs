using System;
using System.Collections.Generic;
using UnityEngine;

namespace UAI
{
	// Token: 0x020017B1 RID: 6065
	public static class UAIUtils
	{
		// Token: 0x0600BBE2 RID: 48098 RVA: 0x0045B6A0 File Offset: 0x004598A0
		public static float DistanceSqr(Vector3 pointA, Vector3 pointB)
		{
			Vector3 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
		}

		// Token: 0x0600BBE3 RID: 48099 RVA: 0x0045B6E0 File Offset: 0x004598E0
		public static float DistanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x0600BBE4 RID: 48100 RVA: 0x0045B710 File Offset: 0x00459910
		public static EntityAlive ConvertToEntityAlive(object obj)
		{
			EntityAlive entityAlive = null;
			try
			{
				entityAlive = (EntityAlive)obj;
			}
			catch
			{
			}
			return entityAlive;
		}

		// Token: 0x020017B2 RID: 6066
		public class NearestWaypointSorter : IComparer<Vector3>
		{
			// Token: 0x0600BBE5 RID: 48101 RVA: 0x0045B73C File Offset: 0x0045993C
			public NearestWaypointSorter(Entity _self)
			{
				this.self = _self;
			}

			// Token: 0x0600BBE6 RID: 48102 RVA: 0x0045B74C File Offset: 0x0045994C
			public int Compare(Vector3 _obj1, Vector3 _obj2)
			{
				float distanceSq = this.self.GetDistanceSq(_obj1);
				float distanceSq2 = this.self.GetDistanceSq(_obj2);
				if (distanceSq < distanceSq2)
				{
					return -1;
				}
				if (distanceSq > distanceSq2)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04008C91 RID: 35985
			[PublicizedFrom(EAccessModifier.Private)]
			public Entity self;
		}

		// Token: 0x020017B3 RID: 6067
		public class NearestEntitySorter : IComparer<Entity>
		{
			// Token: 0x0600BBE7 RID: 48103 RVA: 0x0045B780 File Offset: 0x00459980
			public NearestEntitySorter(Entity _self)
			{
				this.self = _self;
			}

			// Token: 0x0600BBE8 RID: 48104 RVA: 0x0045B790 File Offset: 0x00459990
			public int Compare(Entity _obj1, Entity _obj2)
			{
				float distanceSq = this.self.GetDistanceSq(_obj1);
				float distanceSq2 = this.self.GetDistanceSq(_obj2);
				if (distanceSq < distanceSq2)
				{
					return -1;
				}
				if (distanceSq > distanceSq2)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04008C92 RID: 35986
			[PublicizedFrom(EAccessModifier.Private)]
			public Entity self;
		}
	}
}
