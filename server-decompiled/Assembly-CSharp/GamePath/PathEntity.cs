using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018DC RID: 6364
	public class PathEntity
	{
		// Token: 0x0600C45E RID: 50270 RVA: 0x004876C0 File Offset: 0x004858C0
		public void Destruct()
		{
			PathPoint[] array = this.points;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Release();
			}
			this.points = null;
		}

		// Token: 0x0600C45F RID: 50271 RVA: 0x004876F1 File Offset: 0x004858F1
		public void SetPoints(PathPoint[] _points)
		{
			this.points = _points;
			this.pathLength = _points.Length;
		}

		// Token: 0x0600C460 RID: 50272 RVA: 0x00487703 File Offset: 0x00485903
		public bool HasPoints()
		{
			return this.points != null;
		}

		// Token: 0x0600C461 RID: 50273 RVA: 0x0048770E File Offset: 0x0048590E
		public bool isFinished()
		{
			return this.currentPathIndex >= this.pathLength;
		}

		// Token: 0x0600C462 RID: 50274 RVA: 0x00487721 File Offset: 0x00485921
		public int NodeCountRemaining()
		{
			return this.pathLength - this.currentPathIndex;
		}

		// Token: 0x0600C463 RID: 50275 RVA: 0x00487730 File Offset: 0x00485930
		public Vector3 GetEndPos()
		{
			if (this.pathLength > 0)
			{
				return this.points[this.pathLength - 1].projectedLocation;
			}
			return this.rawEndPos;
		}

		// Token: 0x0600C464 RID: 50276 RVA: 0x00487756 File Offset: 0x00485956
		public PathPoint GetEndPoint()
		{
			if (this.pathLength > 0)
			{
				return this.points[this.pathLength - 1];
			}
			return null;
		}

		// Token: 0x0600C465 RID: 50277 RVA: 0x00487774 File Offset: 0x00485974
		public void ShortenEnd(float _distance)
		{
			if (this.pathLength >= 2)
			{
				PathPoint pathPoint = this.points[this.pathLength - 2];
				this.points[this.pathLength - 1].projectedLocation = pathPoint.projectedLocation;
			}
		}

		// Token: 0x0600C466 RID: 50278 RVA: 0x004877B4 File Offset: 0x004859B4
		public PathPoint getPathPointFromIndex(int _idx)
		{
			return this.points[_idx];
		}

		// Token: 0x0600C467 RID: 50279 RVA: 0x004877BE File Offset: 0x004859BE
		public int getCurrentPathLength()
		{
			return this.pathLength;
		}

		// Token: 0x0600C468 RID: 50280 RVA: 0x004877C6 File Offset: 0x004859C6
		public void setCurrentPathLength(int _length)
		{
			this.pathLength = _length;
		}

		// Token: 0x0600C469 RID: 50281 RVA: 0x004877CF File Offset: 0x004859CF
		public int getCurrentPathIndex()
		{
			return this.currentPathIndex;
		}

		// Token: 0x0600C46A RID: 50282 RVA: 0x004877D7 File Offset: 0x004859D7
		public void setCurrentPathIndex(int _idx, Entity entity, Vector3 entityPos)
		{
			this.currentPathIndex = _idx;
		}

		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x0600C46B RID: 50283 RVA: 0x004877E0 File Offset: 0x004859E0
		public PathPoint CurrentPoint
		{
			get
			{
				return this.points[this.currentPathIndex];
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x0600C46C RID: 50284 RVA: 0x004877F0 File Offset: 0x004859F0
		public PathPoint NextPoint
		{
			get
			{
				int num = this.currentPathIndex + 1;
				if (num >= this.points.Length)
				{
					return null;
				}
				return this.points[num];
			}
		}

		// Token: 0x0600C46D RID: 50285 RVA: 0x0048781C File Offset: 0x00485A1C
		public override bool Equals(object _other)
		{
			if (!(_other is PathEntity) || _other == null)
			{
				return false;
			}
			PathEntity pathEntity = (PathEntity)_other;
			if (pathEntity.points.Length != this.points.Length)
			{
				return false;
			}
			for (int i = 0; i < this.points.Length; i++)
			{
				if (!this.points[i].IsSamePos(pathEntity.points[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C46E RID: 50286 RVA: 0x00487880 File Offset: 0x00485A80
		public override int GetHashCode()
		{
			if (this.points == null)
			{
				return 0;
			}
			int num = 0;
			foreach (PathPoint pathPoint in this.points)
			{
				num += pathPoint.GetHashCode();
			}
			return num;
		}

		// Token: 0x04009459 RID: 37977
		public PathPoint[] points;

		// Token: 0x0400945A RID: 37978
		public Vector3 toPos;

		// Token: 0x0400945B RID: 37979
		public Vector3 rawEndPos;

		// Token: 0x0400945C RID: 37980
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentPathIndex;

		// Token: 0x0400945D RID: 37981
		[PublicizedFrom(EAccessModifier.Private)]
		public int pathLength;
	}
}
