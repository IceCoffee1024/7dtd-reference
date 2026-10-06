using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x0200190D RID: 6413
	public class PathInfoSingleTarget : PathInfo
	{
		// Token: 0x0600C5D8 RID: 50648 RVA: 0x0048BE7C File Offset: 0x0048A07C
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

		// Token: 0x04009538 RID: 38200
		public Vector3 targetPos;

		// Token: 0x04009539 RID: 38201
		public PathEndingCondition endingCondition;
	}
}
