using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018C2 RID: 6338
	public class PathPoint
	{
		// Token: 0x0600C30F RID: 49935 RVA: 0x004819D0 File Offset: 0x0047FBD0
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

		// Token: 0x0600C310 RID: 49936 RVA: 0x00481A64 File Offset: 0x0047FC64
		public static void CompactPool()
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			lock (s_pool)
			{
				MemoryPools.s_pool.Compact();
			}
		}

		// Token: 0x0600C311 RID: 49937 RVA: 0x00481AA8 File Offset: 0x0047FCA8
		public void Release()
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			lock (s_pool)
			{
				MemoryPools.s_pool.Free(this);
			}
		}

		// Token: 0x0600C312 RID: 49938 RVA: 0x00481AEC File Offset: 0x0047FCEC
		public static int makeHash(int _x, int _y, int _z)
		{
			return (_y & 255) | ((_x & 32767) << 8) | ((_z & 32767) << 24) | ((_x >= 0) ? 0 : int.MinValue) | ((_z >= 0) ? 0 : 32768);
		}

		// Token: 0x0600C313 RID: 49939 RVA: 0x00481B24 File Offset: 0x0047FD24
		public override bool Equals(object _obj)
		{
			PathPoint pathPoint = _obj as PathPoint;
			return pathPoint != null && this.hash == pathPoint.hash && this.IsSamePos(pathPoint);
		}

		// Token: 0x0600C314 RID: 49940 RVA: 0x00481B54 File Offset: 0x0047FD54
		public bool IsSamePos(PathPoint _p)
		{
			return _p.x == this.x && _p.y == this.y && _p.z == this.z;
		}

		// Token: 0x0600C315 RID: 49941 RVA: 0x00481B82 File Offset: 0x0047FD82
		public override int GetHashCode()
		{
			return this.hash;
		}

		// Token: 0x0600C316 RID: 49942 RVA: 0x00481B8C File Offset: 0x0047FD8C
		public float GetDistanceSq(int _x, int _y, int _z)
		{
			int num = this.x - _x;
			int num2 = this.y - _y;
			int num3 = this.z - _z;
			return (float)(num * num + num2 * num2 + num3 * num3);
		}

		// Token: 0x0600C317 RID: 49943 RVA: 0x00481BBE File Offset: 0x0047FDBE
		public Vector3 AdjustedPositionForEntity(Entity entity)
		{
			return this.projectedLocation;
		}

		// Token: 0x0600C318 RID: 49944 RVA: 0x00481BBE File Offset: 0x0047FDBE
		public Vector3 ProjectToGround(Entity entity)
		{
			return this.projectedLocation;
		}

		// Token: 0x0600C319 RID: 49945 RVA: 0x00481BC6 File Offset: 0x0047FDC6
		public Vector3i GetBlockPos()
		{
			return World.worldToBlockPos(this.projectedLocation);
		}

		// Token: 0x0600C31A RID: 49946 RVA: 0x00481BD4 File Offset: 0x0047FDD4
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

		// Token: 0x04009364 RID: 37732
		public Vector3 projectedLocation;

		// Token: 0x04009365 RID: 37733
		[PublicizedFrom(EAccessModifier.Private)]
		public int x;

		// Token: 0x04009366 RID: 37734
		[PublicizedFrom(EAccessModifier.Private)]
		public int y;

		// Token: 0x04009367 RID: 37735
		[PublicizedFrom(EAccessModifier.Private)]
		public int z;

		// Token: 0x04009368 RID: 37736
		[PublicizedFrom(EAccessModifier.Private)]
		public int hash;
	}
}
