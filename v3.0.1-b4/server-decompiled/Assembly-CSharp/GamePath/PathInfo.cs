using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018B9 RID: 6329
	public abstract class PathInfo
	{
		// Token: 0x0600C2F6 RID: 49910 RVA: 0x004817B5 File Offset: 0x0047F9B5
		public PathInfo(EntityAlive _entity, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
		{
			this.entity = _entity;
			this.hasStart = false;
			this.canBreakBlocks = _canBreakBlocks;
			this.speed = _speed;
			this.aiTask = _aiTask;
			this.calculatePartial = false;
			this.path = null;
		}

		// Token: 0x0600C2F7 RID: 49911 RVA: 0x004817EF File Offset: 0x0047F9EF
		public void SetStartPos(Vector3 _startPos)
		{
			this.startPos = _startPos;
			this.hasStart = true;
		}

		// Token: 0x0600C2F8 RID: 49912 RVA: 0x004817FF File Offset: 0x0047F9FF
		public virtual void ResetPathInfo()
		{
			this.path = null;
		}

		// Token: 0x0400933A RID: 37690
		public EntityAlive entity;

		// Token: 0x0400933B RID: 37691
		public PathInfo.State state;

		// Token: 0x0400933C RID: 37692
		public Vector3 startPos;

		// Token: 0x0400933D RID: 37693
		public bool hasStart;

		// Token: 0x0400933E RID: 37694
		public bool canBreakBlocks;

		// Token: 0x0400933F RID: 37695
		public float speed;

		// Token: 0x04009340 RID: 37696
		public EAIBase aiTask;

		// Token: 0x04009341 RID: 37697
		public bool calculatePartial;

		// Token: 0x04009342 RID: 37698
		public ChunkCache chunkcache;

		// Token: 0x04009343 RID: 37699
		public OnPathDelegate OnPathResult;

		// Token: 0x04009344 RID: 37700
		public PathEntity path;

		// Token: 0x020018BA RID: 6330
		public enum State
		{
			// Token: 0x04009346 RID: 37702
			Queued,
			// Token: 0x04009347 RID: 37703
			Pathing,
			// Token: 0x04009348 RID: 37704
			Done
		}
	}
}
