using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001939 RID: 6457
	public class PathEntity
	{
		// Token: 0x0600C6CA RID: 50890 RVA: 0x0048F5B4 File Offset: 0x0048D7B4
		public void Destruct()
		{
			PathPoint[] array = this.points;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Release();
			}
			this.points = null;
		}

		// Token: 0x0600C6CB RID: 50891 RVA: 0x0048F5E5 File Offset: 0x0048D7E5
		public void SetPoints(PathPoint[] _points)
		{
			this.points = _points;
			this.pathLength = _points.Length;
		}

		// Token: 0x0600C6CC RID: 50892 RVA: 0x0048F5F7 File Offset: 0x0048D7F7
		public bool HasPoints()
		{
			return this.points != null;
		}

		// Token: 0x0600C6CD RID: 50893 RVA: 0x0048F602 File Offset: 0x0048D802
		public bool isFinished()
		{
			return this.currentPathIndex >= this.pathLength;
		}

		// Token: 0x0600C6CE RID: 50894 RVA: 0x0048F615 File Offset: 0x0048D815
		public int NodeCountRemaining()
		{
			return this.pathLength - this.currentPathIndex;
		}

		// Token: 0x0600C6CF RID: 50895 RVA: 0x0048F624 File Offset: 0x0048D824
		public Vector3 GetEndPos()
		{
			if (this.pathLength > 0)
			{
				return this.points[this.pathLength - 1].projectedLocation;
			}
			return this.rawEndPos;
		}

		// Token: 0x0600C6D0 RID: 50896 RVA: 0x0048F64A File Offset: 0x0048D84A
		public PathPoint GetEndPoint()
		{
			if (this.pathLength > 0)
			{
				return this.points[this.pathLength - 1];
			}
			return null;
		}

		// Token: 0x0600C6D1 RID: 50897 RVA: 0x0048F668 File Offset: 0x0048D868
		public void ShortenEnd(float _distance)
		{
			if (this.pathLength >= 2)
			{
				PathPoint pathPoint = this.points[this.pathLength - 2];
				this.points[this.pathLength - 1].projectedLocation = pathPoint.projectedLocation;
			}
		}

		// Token: 0x0600C6D2 RID: 50898 RVA: 0x0048F6A8 File Offset: 0x0048D8A8
		public PathPoint getPathPointFromIndex(int _idx)
		{
			return this.points[_idx];
		}

		// Token: 0x0600C6D3 RID: 50899 RVA: 0x0048F6B2 File Offset: 0x0048D8B2
		public int getCurrentPathLength()
		{
			return this.pathLength;
		}

		// Token: 0x0600C6D4 RID: 50900 RVA: 0x0048F6BA File Offset: 0x0048D8BA
		public void setCurrentPathLength(int _length)
		{
			this.pathLength = _length;
		}

		// Token: 0x0600C6D5 RID: 50901 RVA: 0x0048F6C3 File Offset: 0x0048D8C3
		public int getCurrentPathIndex()
		{
			return this.currentPathIndex;
		}

		// Token: 0x0600C6D6 RID: 50902 RVA: 0x0048F6CB File Offset: 0x0048D8CB
		public void setCurrentPathIndex(int _idx, Entity entity, Vector3 entityPos)
		{
			this.currentPathIndex = _idx;
		}

		// Token: 0x17001929 RID: 6441
		// (get) Token: 0x0600C6D7 RID: 50903 RVA: 0x0048F6D4 File Offset: 0x0048D8D4
		public PathPoint CurrentPoint
		{
			get
			{
				return this.points[this.currentPathIndex];
			}
		}

		// Token: 0x1700192A RID: 6442
		// (get) Token: 0x0600C6D8 RID: 50904 RVA: 0x0048F6E4 File Offset: 0x0048D8E4
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

		// Token: 0x0600C6D9 RID: 50905 RVA: 0x0048F710 File Offset: 0x0048D910
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

		// Token: 0x0600C6DA RID: 50906 RVA: 0x0048F774 File Offset: 0x0048D974
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

		// Token: 0x04009608 RID: 38408
		public PathPoint[] points;

		// Token: 0x04009609 RID: 38409
		public Vector3 toPos;

		// Token: 0x0400960A RID: 38410
		public Vector3 rawEndPos;

		// Token: 0x0400960B RID: 38411
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentPathIndex;

		// Token: 0x0400960C RID: 38412
		[PublicizedFrom(EAccessModifier.Private)]
		public int pathLength;
	}
}
