using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018B6 RID: 6326
	public class PathEntity
	{
		// Token: 0x0600C2D7 RID: 49879 RVA: 0x00481594 File Offset: 0x0047F794
		public void Destruct()
		{
			PathPoint[] array = this.points;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Release();
			}
			this.points = null;
		}

		// Token: 0x0600C2D8 RID: 49880 RVA: 0x004815C5 File Offset: 0x0047F7C5
		public void SetPoints(PathPoint[] _points)
		{
			this.points = _points;
			this.pathLength = _points.Length;
		}

		// Token: 0x0600C2D9 RID: 49881 RVA: 0x004815D7 File Offset: 0x0047F7D7
		public bool HasPoints()
		{
			return this.points != null;
		}

		// Token: 0x0600C2DA RID: 49882 RVA: 0x004815E2 File Offset: 0x0047F7E2
		public bool isFinished()
		{
			return this.currentPathIndex >= this.pathLength;
		}

		// Token: 0x0600C2DB RID: 49883 RVA: 0x004815F5 File Offset: 0x0047F7F5
		public int NodeCountRemaining()
		{
			return this.pathLength - this.currentPathIndex;
		}

		// Token: 0x0600C2DC RID: 49884 RVA: 0x00481604 File Offset: 0x0047F804
		public Vector3 GetEndPos()
		{
			if (this.pathLength > 0)
			{
				return this.points[this.pathLength - 1].projectedLocation;
			}
			return this.rawEndPos;
		}

		// Token: 0x0600C2DD RID: 49885 RVA: 0x0048162A File Offset: 0x0047F82A
		public PathPoint GetEndPoint()
		{
			if (this.pathLength > 0)
			{
				return this.points[this.pathLength - 1];
			}
			return null;
		}

		// Token: 0x0600C2DE RID: 49886 RVA: 0x00481648 File Offset: 0x0047F848
		public void ShortenEnd(float _distance)
		{
			if (this.pathLength >= 2)
			{
				PathPoint pathPoint = this.points[this.pathLength - 2];
				this.points[this.pathLength - 1].projectedLocation = pathPoint.projectedLocation;
			}
		}

		// Token: 0x0600C2DF RID: 49887 RVA: 0x00481688 File Offset: 0x0047F888
		public PathPoint getPathPointFromIndex(int _idx)
		{
			return this.points[_idx];
		}

		// Token: 0x0600C2E0 RID: 49888 RVA: 0x00481692 File Offset: 0x0047F892
		public int getCurrentPathLength()
		{
			return this.pathLength;
		}

		// Token: 0x0600C2E1 RID: 49889 RVA: 0x0048169A File Offset: 0x0047F89A
		public void setCurrentPathLength(int _length)
		{
			this.pathLength = _length;
		}

		// Token: 0x0600C2E2 RID: 49890 RVA: 0x004816A3 File Offset: 0x0047F8A3
		public int getCurrentPathIndex()
		{
			return this.currentPathIndex;
		}

		// Token: 0x0600C2E3 RID: 49891 RVA: 0x004816AB File Offset: 0x0047F8AB
		public void setCurrentPathIndex(int _idx, Entity entity, Vector3 entityPos)
		{
			this.currentPathIndex = _idx;
		}

		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x0600C2E4 RID: 49892 RVA: 0x004816B4 File Offset: 0x0047F8B4
		public PathPoint CurrentPoint
		{
			get
			{
				return this.points[this.currentPathIndex];
			}
		}

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x0600C2E5 RID: 49893 RVA: 0x004816C4 File Offset: 0x0047F8C4
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

		// Token: 0x0600C2E6 RID: 49894 RVA: 0x004816F0 File Offset: 0x0047F8F0
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

		// Token: 0x0600C2E7 RID: 49895 RVA: 0x00481754 File Offset: 0x0047F954
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

		// Token: 0x04009330 RID: 37680
		public PathPoint[] points;

		// Token: 0x04009331 RID: 37681
		public Vector3 toPos;

		// Token: 0x04009332 RID: 37682
		public Vector3 rawEndPos;

		// Token: 0x04009333 RID: 37683
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentPathIndex;

		// Token: 0x04009334 RID: 37684
		[PublicizedFrom(EAccessModifier.Private)]
		public int pathLength;
	}
}
