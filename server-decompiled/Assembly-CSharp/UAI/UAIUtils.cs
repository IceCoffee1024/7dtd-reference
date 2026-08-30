using System;
using System.Collections.Generic;
using UnityEngine;

namespace UAI
{
	// Token: 0x02001785 RID: 6021
	public static class UAIUtils
	{
		// Token: 0x0600BA8B RID: 47755 RVA: 0x00457180 File Offset: 0x00455380
		public static float DistanceSqr(Vector3 pointA, Vector3 pointB)
		{
			Vector3 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
		}

		// Token: 0x0600BA8C RID: 47756 RVA: 0x004571C0 File Offset: 0x004553C0
		public static float DistanceSqr(Vector2 pointA, Vector2 pointB)
		{
			Vector2 vector = pointA - pointB;
			return vector.x * vector.x + vector.y * vector.y;
		}

		// Token: 0x0600BA8D RID: 47757 RVA: 0x004571F0 File Offset: 0x004553F0
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

		// Token: 0x02001786 RID: 6022
		public class NearestWaypointSorter : IComparer<Vector3>
		{
			// Token: 0x0600BA8E RID: 47758 RVA: 0x0045721C File Offset: 0x0045541C
			public NearestWaypointSorter(Entity _self)
			{
				this.self = _self;
			}

			// Token: 0x0600BA8F RID: 47759 RVA: 0x0045722C File Offset: 0x0045542C
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

			// Token: 0x04008BCC RID: 35788
			[PublicizedFrom(EAccessModifier.Private)]
			public Entity self;
		}

		// Token: 0x02001787 RID: 6023
		public class NearestEntitySorter : IComparer<Entity>
		{
			// Token: 0x0600BA90 RID: 47760 RVA: 0x00457260 File Offset: 0x00455460
			public NearestEntitySorter(Entity _self)
			{
				this.self = _self;
			}

			// Token: 0x0600BA91 RID: 47761 RVA: 0x00457270 File Offset: 0x00455470
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

			// Token: 0x04008BCD RID: 35789
			[PublicizedFrom(EAccessModifier.Private)]
			public Entity self;
		}
	}
}
