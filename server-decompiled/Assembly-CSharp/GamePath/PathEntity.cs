using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001908 RID: 6408
	public class PathEntity
	{
		// Token: 0x0600C5B6 RID: 50614 RVA: 0x0048BC08 File Offset: 0x00489E08
		public void Destruct()
		{
			PathPoint[] array = this.points;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Release();
			}
			this.points = null;
		}

		// Token: 0x0600C5B7 RID: 50615 RVA: 0x0048BC39 File Offset: 0x00489E39
		public void SetPoints(PathPoint[] _points)
		{
			this.points = _points;
			this.pathLength = _points.Length;
		}

		// Token: 0x0600C5B8 RID: 50616 RVA: 0x0048BC4B File Offset: 0x00489E4B
		public bool HasPoints()
		{
			return this.points != null;
		}

		// Token: 0x0600C5B9 RID: 50617 RVA: 0x0048BC56 File Offset: 0x00489E56
		public bool isFinished()
		{
			return this.currentPathIndex >= this.pathLength;
		}

		// Token: 0x0600C5BA RID: 50618 RVA: 0x0048BC69 File Offset: 0x00489E69
		public int NodeCountRemaining()
		{
			return this.pathLength - this.currentPathIndex;
		}

		// Token: 0x0600C5BB RID: 50619 RVA: 0x0048BC78 File Offset: 0x00489E78
		public Vector3 GetEndPos()
		{
			if (this.pathLength > 0)
			{
				return this.points[this.pathLength - 1].projectedLocation;
			}
			return this.rawEndPos;
		}

		// Token: 0x0600C5BC RID: 50620 RVA: 0x0048BC9E File Offset: 0x00489E9E
		public PathPoint GetEndPoint()
		{
			if (this.pathLength > 0)
			{
				return this.points[this.pathLength - 1];
			}
			return null;
		}

		// Token: 0x0600C5BD RID: 50621 RVA: 0x0048BCBC File Offset: 0x00489EBC
		public void ShortenEnd(float _distance)
		{
			if (this.pathLength >= 2)
			{
				PathPoint pathPoint = this.points[this.pathLength - 2];
				this.points[this.pathLength - 1].projectedLocation = pathPoint.projectedLocation;
			}
		}

		// Token: 0x0600C5BE RID: 50622 RVA: 0x0048BCFC File Offset: 0x00489EFC
		public PathPoint getPathPointFromIndex(int _idx)
		{
			return this.points[_idx];
		}

		// Token: 0x0600C5BF RID: 50623 RVA: 0x0048BD06 File Offset: 0x00489F06
		public int getCurrentPathLength()
		{
			return this.pathLength;
		}

		// Token: 0x0600C5C0 RID: 50624 RVA: 0x0048BD0E File Offset: 0x00489F0E
		public void setCurrentPathLength(int _length)
		{
			this.pathLength = _length;
		}

		// Token: 0x0600C5C1 RID: 50625 RVA: 0x0048BD17 File Offset: 0x00489F17
		public int getCurrentPathIndex()
		{
			return this.currentPathIndex;
		}

		// Token: 0x0600C5C2 RID: 50626 RVA: 0x0048BD1F File Offset: 0x00489F1F
		public void setCurrentPathIndex(int _idx, Entity entity, Vector3 entityPos)
		{
			this.currentPathIndex = _idx;
		}

		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x0600C5C3 RID: 50627 RVA: 0x0048BD28 File Offset: 0x00489F28
		public PathPoint CurrentPoint
		{
			get
			{
				return this.points[this.currentPathIndex];
			}
		}

		// Token: 0x170018E2 RID: 6370
		// (get) Token: 0x0600C5C4 RID: 50628 RVA: 0x0048BD38 File Offset: 0x00489F38
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

		// Token: 0x0600C5C5 RID: 50629 RVA: 0x0048BD64 File Offset: 0x00489F64
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

		// Token: 0x0600C5C6 RID: 50630 RVA: 0x0048BDC8 File Offset: 0x00489FC8
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

		// Token: 0x0400951F RID: 38175
		public PathPoint[] points;

		// Token: 0x04009520 RID: 38176
		public Vector3 toPos;

		// Token: 0x04009521 RID: 38177
		public Vector3 rawEndPos;

		// Token: 0x04009522 RID: 38178
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentPathIndex;

		// Token: 0x04009523 RID: 38179
		[PublicizedFrom(EAccessModifier.Private)]
		public int pathLength;
	}
}
