using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x0200193E RID: 6462
	public class PathInfoSingleTarget : PathInfo
	{
		// Token: 0x0600C6EC RID: 50924 RVA: 0x0048F828 File Offset: 0x0048DA28
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

		// Token: 0x04009621 RID: 38433
		public Vector3 targetPos;

		// Token: 0x04009622 RID: 38434
		public PathEndingCondition endingCondition;
	}
}
