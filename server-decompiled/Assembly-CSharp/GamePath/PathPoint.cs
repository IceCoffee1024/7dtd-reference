using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001914 RID: 6420
	public class PathPoint
	{
		// Token: 0x0600C5EE RID: 50670 RVA: 0x0048C044 File Offset: 0x0048A244
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

		// Token: 0x0600C5EF RID: 50671 RVA: 0x0048C0D8 File Offset: 0x0048A2D8
		public static void CompactPool()
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			lock (s_pool)
			{
				MemoryPools.s_pool.Compact();
			}
		}

		// Token: 0x0600C5F0 RID: 50672 RVA: 0x0048C11C File Offset: 0x0048A31C
		public void Release()
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			lock (s_pool)
			{
				MemoryPools.s_pool.Free(this);
			}
		}

		// Token: 0x0600C5F1 RID: 50673 RVA: 0x0048C160 File Offset: 0x0048A360
		public static int makeHash(int _x, int _y, int _z)
		{
			return (_y & 255) | ((_x & 32767) << 8) | ((_z & 32767) << 24) | ((_x >= 0) ? 0 : int.MinValue) | ((_z >= 0) ? 0 : 32768);
		}

		// Token: 0x0600C5F2 RID: 50674 RVA: 0x0048C198 File Offset: 0x0048A398
		public override bool Equals(object _obj)
		{
			PathPoint pathPoint = _obj as PathPoint;
			return pathPoint != null && this.hash == pathPoint.hash && this.IsSamePos(pathPoint);
		}

		// Token: 0x0600C5F3 RID: 50675 RVA: 0x0048C1C8 File Offset: 0x0048A3C8
		public bool IsSamePos(PathPoint _p)
		{
			return _p.x == this.x && _p.y == this.y && _p.z == this.z;
		}

		// Token: 0x0600C5F4 RID: 50676 RVA: 0x0048C1F6 File Offset: 0x0048A3F6
		public override int GetHashCode()
		{
			return this.hash;
		}

		// Token: 0x0600C5F5 RID: 50677 RVA: 0x0048C200 File Offset: 0x0048A400
		public float GetDistanceSq(int _x, int _y, int _z)
		{
			int num = this.x - _x;
			int num2 = this.y - _y;
			int num3 = this.z - _z;
			return (float)(num * num + num2 * num2 + num3 * num3);
		}

		// Token: 0x0600C5F6 RID: 50678 RVA: 0x0048C232 File Offset: 0x0048A432
		public Vector3 AdjustedPositionForEntity(Entity entity)
		{
			return this.projectedLocation;
		}

		// Token: 0x0600C5F7 RID: 50679 RVA: 0x0048C232 File Offset: 0x0048A432
		public Vector3 ProjectToGround(Entity entity)
		{
			return this.projectedLocation;
		}

		// Token: 0x0600C5F8 RID: 50680 RVA: 0x0048C23A File Offset: 0x0048A43A
		public Vector3i GetBlockPos()
		{
			return World.worldToBlockPos(this.projectedLocation);
		}

		// Token: 0x0600C5F9 RID: 50681 RVA: 0x0048C248 File Offset: 0x0048A448
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

		// Token: 0x04009553 RID: 38227
		public Vector3 projectedLocation;

		// Token: 0x04009554 RID: 38228
		[PublicizedFrom(EAccessModifier.Private)]
		public int x;

		// Token: 0x04009555 RID: 38229
		[PublicizedFrom(EAccessModifier.Private)]
		public int y;

		// Token: 0x04009556 RID: 38230
		[PublicizedFrom(EAccessModifier.Private)]
		public int z;

		// Token: 0x04009557 RID: 38231
		[PublicizedFrom(EAccessModifier.Private)]
		public int hash;
	}
}
