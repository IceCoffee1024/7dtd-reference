using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018BB RID: 6331
	public class PathInfoSingleTarget : PathInfo
	{
		// Token: 0x0600C2F9 RID: 49913 RVA: 0x00481808 File Offset: 0x0047FA08
		public PathInfoSingleTarget(EntityAlive _entity, Vector3 _targetPos, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.entity = _entity;
			this.hasStart = false;
			this.targetPos = _targetPos;
			this.canBreakBlocks = _canBreakBlocks;
			this.speed = _speed;
			this.aiTask = _aiTask;
			this.path = null;
			this.calculatePartial = true;
		}

		// Token: 0x04009349 RID: 37705
		public Vector3 targetPos;

		// Token: 0x0400934A RID: 37706
		public PathEndingCondition endingCondition;
	}
}
