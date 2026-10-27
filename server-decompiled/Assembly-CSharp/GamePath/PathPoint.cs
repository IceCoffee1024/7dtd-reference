using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001945 RID: 6469
	public class PathPoint
	{
		// Token: 0x0600C702 RID: 50946 RVA: 0x0048F9F0 File Offset: 0x0048DBF0
		public static PathPoint Allocate(Vector3 _pos)
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			PathPoint pathPoint2;
			lock (s_pool)
			{
				PathPoint pathPoint = MemoryPools.s_pool.Allocate();
				pathPoint.x = (int)_pos.x;
				pathPoint.y = (int)_pos.y;
				pathPoint.z = (int)_pos.z;
				pathPoint.projectedLocation = _pos;
				pathPoint.hash = PathPoint.makeHash(pathPoint.x, pathPoint.y, pathPoint.z);
				pathPoint2 = pathPoint;
			}
			return pathPoint2;
		}

		// Token: 0x0600C703 RID: 50947 RVA: 0x0048FA84 File Offset: 0x0048DC84
		public static void CompactPool()
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			lock (s_pool)
			{
				MemoryPools.s_pool.Compact();
			}
		}

		// Token: 0x0600C704 RID: 50948 RVA: 0x0048FAC8 File Offset: 0x0048DCC8
		public void Release()
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			lock (s_pool)
			{
				MemoryPools.s_pool.Free(this);
			}
		}

		// Token: 0x0600C705 RID: 50949 RVA: 0x0048FB0C File Offset: 0x0048DD0C
		public static int makeHash(int _x, int _y, int _z)
		{
			return (_y & 255) | ((_x & 32767) << 8) | ((_z & 32767) << 24) | ((_x >= 0) ? 0 : int.MinValue) | ((_z >= 0) ? 0 : 32768);
		}

		// Token: 0x0600C706 RID: 50950 RVA: 0x0048FB44 File Offset: 0x0048DD44
		public override bool Equals(object _obj)
		{
			PathPoint pathPoint = _obj as PathPoint;
			return pathPoint != null && this.hash == pathPoint.hash && this.IsSamePos(pathPoint);
		}

		// Token: 0x0600C707 RID: 50951 RVA: 0x0048FB74 File Offset: 0x0048DD74
		public bool IsSamePos(PathPoint _p)
		{
			return _p.x == this.x && _p.y == this.y && _p.z == this.z;
		}

		// Token: 0x0600C708 RID: 50952 RVA: 0x0048FBA2 File Offset: 0x0048DDA2
		public override int GetHashCode()
		{
			return this.hash;
		}

		// Token: 0x0600C709 RID: 50953 RVA: 0x0048FBAC File Offset: 0x0048DDAC
		public float GetDistanceSq(int _x, int _y, int _z)
		{
			int num = this.x - _x;
			int num2 = this.y - _y;
			int num3 = this.z - _z;
			return (float)(num * num + num2 * num2 + num3 * num3);
		}

		// Token: 0x0600C70A RID: 50954 RVA: 0x0048FBDE File Offset: 0x0048DDDE
		public Vector3 AdjustedPositionForEntity(Entity entity)
		{
			return this.projectedLocation;
		}

		// Token: 0x0600C70B RID: 50955 RVA: 0x0048FBDE File Offset: 0x0048DDDE
		public Vector3 ProjectToGround(Entity entity)
		{
			return this.projectedLocation;
		}

		// Token: 0x0600C70C RID: 50956 RVA: 0x0048FBE6 File Offset: 0x0048DDE6
		public Vector3i GetBlockPos()
		{
			return World.worldToBlockPos(this.projectedLocation);
		}

		// Token: 0x0600C70D RID: 50957 RVA: 0x0048FBF4 File Offset: 0x0048DDF4
		public string toString()
		{
			return string.Concat(new string[]
			{
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString()
			});
		}

		// Token: 0x0400963C RID: 38460
		public Vector3 projectedLocation;

		// Token: 0x0400963D RID: 38461
		[PublicizedFrom(EAccessModifier.Private)]
		public int x;

		// Token: 0x0400963E RID: 38462
		[PublicizedFrom(EAccessModifier.Private)]
		public int y;

		// Token: 0x0400963F RID: 38463
		[PublicizedFrom(EAccessModifier.Private)]
		public int z;

		// Token: 0x04009640 RID: 38464
		[PublicizedFrom(EAccessModifier.Private)]
		public int hash;
	}
}
