using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x0200190B RID: 6411
	public abstract class PathInfo
	{
		// Token: 0x0600C5D5 RID: 50645 RVA: 0x0048BE29 File Offset: 0x0048A029
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

		// Token: 0x0600C5D6 RID: 50646 RVA: 0x0048BE63 File Offset: 0x0048A063
		public void SetStartPos(Vector3 _startPos)
		{
			this.startPos = _startPos;
			this.hasStart = true;
		}

		// Token: 0x0600C5D7 RID: 50647 RVA: 0x0048BE73 File Offset: 0x0048A073
		public virtual void ResetPathInfo()
		{
			this.path = null;
		}

		// Token: 0x04009529 RID: 38185
		public EntityAlive entity;

		// Token: 0x0400952A RID: 38186
		public PathInfo.State state;

		// Token: 0x0400952B RID: 38187
		public Vector3 startPos;

		// Token: 0x0400952C RID: 38188
		public bool hasStart;

		// Token: 0x0400952D RID: 38189
		public bool canBreakBlocks;

		// Token: 0x0400952E RID: 38190
		public float speed;

		// Token: 0x0400952F RID: 38191
		public EAIBase aiTask;

		// Token: 0x04009530 RID: 38192
		public bool calculatePartial;

		// Token: 0x04009531 RID: 38193
		public ChunkCache chunkcache;

		// Token: 0x04009532 RID: 38194
		public OnPathDelegate OnPathResult;

		// Token: 0x04009533 RID: 38195
		public PathEntity path;

		// Token: 0x0200190C RID: 6412
		public enum State
		{
			// Token: 0x04009535 RID: 38197
			Queued,
			// Token: 0x04009536 RID: 38198
			Pathing,
			// Token: 0x04009537 RID: 38199
			Done
		}
	}
}
