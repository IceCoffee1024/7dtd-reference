using System;
using System.Collections.Generic;
using UnityEngine;

namespace UAI
{
	// Token: 0x0200175F RID: 5983
	public static class UAIUtils
	{
		// Token: 0x0600B908 RID: 47368 RVA: 0x00451408 File Offset: 0x0044F608
		public static float DistanceSqr(Vector3 pointA, Vector3 pointB)
		{
			Vector3 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
		}

		// Token: 0x0600B909 RID: 47369 RVA: 0x00451448 File Offset: 0x0044F648
		public static float DistanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x0600B90A RID: 47370 RVA: 0x00451478 File Offset: 0x0044F678
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

		// Token: 0x02001760 RID: 5984
		public class NearestWaypointSorter : IComparer<Vector3>
		{
			// Token: 0x0600B90B RID: 47371 RVA: 0x004514A4 File Offset: 0x0044F6A4
			public NearestWaypointSorter(Entity _self)
			{
				this.self = _self;
			}

			// Token: 0x0600B90C RID: 47372 RVA: 0x004514B4 File Offset: 0x0044F6B4
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

			// Token: 0x04008AB0 RID: 35504
			[PublicizedFrom(EAccessModifier.Private)]
			public Entity self;
		}

		// Token: 0x02001761 RID: 5985
		public class NearestEntitySorter : IComparer<Entity>
		{
			// Token: 0x0600B90D RID: 47373 RVA: 0x004514E8 File Offset: 0x0044F6E8
			public NearestEntitySorter(Entity _self)
			{
				this.self = _self;
			}

			// Token: 0x0600B90E RID: 47374 RVA: 0x004514F8 File Offset: 0x0044F6F8
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

			// Token: 0x04008AB1 RID: 35505
			[PublicizedFrom(EAccessModifier.Private)]
			public Entity self;
		}
	}
}
