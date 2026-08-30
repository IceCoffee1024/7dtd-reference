using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018DF RID: 6367
	public abstract class PathInfo
	{
		// Token: 0x0600C47D RID: 50301 RVA: 0x004878E1 File Offset: 0x00485AE1
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

		// Token: 0x0600C47E RID: 50302 RVA: 0x0048791B File Offset: 0x00485B1B
		public void SetStartPos(Vector3 _startPos)
		{
			this.startPos = _startPos;
			this.hasStart = true;
		}

		// Token: 0x0600C47F RID: 50303 RVA: 0x0048792B File Offset: 0x00485B2B
		public virtual void ResetPathInfo()
		{
			this.path = null;
		}

		// Token: 0x04009463 RID: 37987
		public EntityAlive entity;

		// Token: 0x04009464 RID: 37988
		public PathInfo.State state;

		// Token: 0x04009465 RID: 37989
		public Vector3 startPos;

		// Token: 0x04009466 RID: 37990
		public bool hasStart;

		// Token: 0x04009467 RID: 37991
		public bool canBreakBlocks;

		// Token: 0x04009468 RID: 37992
		public float speed;

		// Token: 0x04009469 RID: 37993
		public EAIBase aiTask;

		// Token: 0x0400946A RID: 37994
		public bool calculatePartial;

		// Token: 0x0400946B RID: 37995
		public ChunkCache chunkcache;

		// Token: 0x0400946C RID: 37996
		public OnPathDelegate OnPathResult;

		// Token: 0x0400946D RID: 37997
		public PathEntity path;

		// Token: 0x020018E0 RID: 6368
		public enum State
		{
			// Token: 0x0400946F RID: 37999
			Queued,
			// Token: 0x04009470 RID: 38000
			Pathing,
			// Token: 0x04009471 RID: 38001
			Done
		}
	}
}
