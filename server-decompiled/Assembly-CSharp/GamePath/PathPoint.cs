using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018E8 RID: 6376
	public class PathPoint
	{
		// Token: 0x0600C496 RID: 50326 RVA: 0x00487AFC File Offset: 0x00485CFC
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

		// Token: 0x0600C497 RID: 50327 RVA: 0x00487B90 File Offset: 0x00485D90
		public static void CompactPool()
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			lock (s_pool)
			{
				MemoryPools.s_pool.Compact();
			}
		}

		// Token: 0x0600C498 RID: 50328 RVA: 0x00487BD4 File Offset: 0x00485DD4
		public void Release()
		{
			DynamicObjectPool<PathPoint> s_pool = MemoryPools.s_pool;
			lock (s_pool)
			{
				MemoryPools.s_pool.Free(this);
			}
		}

		// Token: 0x0600C499 RID: 50329 RVA: 0x00487C18 File Offset: 0x00485E18
		public static int makeHash(int _x, int _y, int _z)
		{
			return (_y & 255) | ((_x & 32767) << 8) | ((_z & 32767) << 24) | ((_x >= 0) ? 0 : int.MinValue) | ((_z >= 0) ? 0 : 32768);
		}

		// Token: 0x0600C49A RID: 50330 RVA: 0x00487C50 File Offset: 0x00485E50
		public override bool Equals(object _obj)
		{
			PathPoint pathPoint = _obj as PathPoint;
			return pathPoint != null && this.hash == pathPoint.hash && this.IsSamePos(pathPoint);
		}

		// Token: 0x0600C49B RID: 50331 RVA: 0x00487C80 File Offset: 0x00485E80
		public bool IsSamePos(PathPoint _p)
		{
			return _p.x == this.x && _p.y == this.y && _p.z == this.z;
		}

		// Token: 0x0600C49C RID: 50332 RVA: 0x00487CAE File Offset: 0x00485EAE
		public override int GetHashCode()
		{
			return this.hash;
		}

		// Token: 0x0600C49D RID: 50333 RVA: 0x00487CB8 File Offset: 0x00485EB8
		public float GetDistanceSq(int _x, int _y, int _z)
		{
			int num = this.x - _x;
			int num2 = this.y - _y;
			int num3 = this.z - _z;
			return (float)(num * num + num2 * num2 + num3 * num3);
		}

		// Token: 0x0600C49E RID: 50334 RVA: 0x00487CEA File Offset: 0x00485EEA
		public Vector3 AdjustedPositionForEntity(Entity entity)
		{
			return this.projectedLocation;
		}

		// Token: 0x0600C49F RID: 50335 RVA: 0x00487CEA File Offset: 0x00485EEA
		public Vector3 ProjectToGround(Entity entity)
		{
			return this.projectedLocation;
		}

		// Token: 0x0600C4A0 RID: 50336 RVA: 0x00487CF2 File Offset: 0x00485EF2
		public Vector3i GetBlockPos()
		{
			return World.worldToBlockPos(this.projectedLocation);
		}

		// Token: 0x0600C4A1 RID: 50337 RVA: 0x00487D00 File Offset: 0x00485F00
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

		// Token: 0x0400948D RID: 38029
		public Vector3 projectedLocation;

		// Token: 0x0400948E RID: 38030
		[PublicizedFrom(EAccessModifier.Private)]
		public int x;

		// Token: 0x0400948F RID: 38031
		[PublicizedFrom(EAccessModifier.Private)]
		public int y;

		// Token: 0x04009490 RID: 38032
		[PublicizedFrom(EAccessModifier.Private)]
		public int z;

		// Token: 0x04009491 RID: 38033
		[PublicizedFrom(EAccessModifier.Private)]
		public int hash;
	}
}
