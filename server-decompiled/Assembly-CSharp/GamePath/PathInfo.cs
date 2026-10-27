using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x0200193C RID: 6460
	public abstract class PathInfo
	{
		// Token: 0x0600C6E9 RID: 50921 RVA: 0x0048F7D5 File Offset: 0x0048D9D5
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

		// Token: 0x0600C6EA RID: 50922 RVA: 0x0048F80F File Offset: 0x0048DA0F
		public void SetStartPos(Vector3 _startPos)
		{
			this.startPos = _startPos;
			this.hasStart = true;
		}

		// Token: 0x0600C6EB RID: 50923 RVA: 0x0048F81F File Offset: 0x0048DA1F
		public virtual void ResetPathInfo()
		{
			this.path = null;
		}

		// Token: 0x04009612 RID: 38418
		public EntityAlive entity;

		// Token: 0x04009613 RID: 38419
		public PathInfo.State state;

		// Token: 0x04009614 RID: 38420
		public Vector3 startPos;

		// Token: 0x04009615 RID: 38421
		public bool hasStart;

		// Token: 0x04009616 RID: 38422
		public bool canBreakBlocks;

		// Token: 0x04009617 RID: 38423
		public float speed;

		// Token: 0x04009618 RID: 38424
		public EAIBase aiTask;

		// Token: 0x04009619 RID: 38425
		public bool calculatePartial;

		// Token: 0x0400961A RID: 38426
		public ChunkCache chunkcache;

		// Token: 0x0400961B RID: 38427
		public OnPathDelegate OnPathResult;

		// Token: 0x0400961C RID: 38428
		public PathEntity path;

		// Token: 0x0200193D RID: 6461
		public enum State
		{
			// Token: 0x0400961E RID: 38430
			Queued,
			// Token: 0x0400961F RID: 38431
			Pathing,
			// Token: 0x04009620 RID: 38432
			Done
		}
	}
}
