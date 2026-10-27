using System;
using System.Collections.Generic;
using UnityEngine;

namespace UAI
{
	// Token: 0x020017E2 RID: 6114
	public static class UAIUtils
	{
		// Token: 0x0600BCF6 RID: 48374 RVA: 0x0045F04C File Offset: 0x0045D24C
		public static float DistanceSqr(Vector3 pointA, Vector3 pointB)
		{
			Vector3 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
		}

		// Token: 0x0600BCF7 RID: 48375 RVA: 0x0045F08C File Offset: 0x0045D28C
		public static float DistanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x0600BCF8 RID: 48376 RVA: 0x0045F0BC File Offset: 0x0045D2BC
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

		// Token: 0x020017E3 RID: 6115
		public class NearestWaypointSorter : IComparer<Vector3>
		{
			// Token: 0x0600BCF9 RID: 48377 RVA: 0x0045F0E8 File Offset: 0x0045D2E8
			public NearestWaypointSorter(Entity _self)
			{
				this.self = _self;
			}

			// Token: 0x0600BCFA RID: 48378 RVA: 0x0045F0F8 File Offset: 0x0045D2F8
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

			// Token: 0x04008D7A RID: 36218
			[PublicizedFrom(EAccessModifier.Private)]
			public Entity self;
		}

		// Token: 0x020017E4 RID: 6116
		public class NearestEntitySorter : IComparer<Entity>
		{
			// Token: 0x0600BCFB RID: 48379 RVA: 0x0045F12C File Offset: 0x0045D32C
			public NearestEntitySorter(Entity _self)
			{
				this.self = _self;
			}

			// Token: 0x0600BCFC RID: 48380 RVA: 0x0045F13C File Offset: 0x0045D33C
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

			// Token: 0x04008D7B RID: 36219
			[PublicizedFrom(EAccessModifier.Private)]
			public Entity self;
		}
	}
}
