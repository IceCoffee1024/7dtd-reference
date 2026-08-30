using System;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018E1 RID: 6369
	public class PathInfoSingleTarget : PathInfo
	{
		// Token: 0x0600C480 RID: 50304 RVA: 0x00487934 File Offset: 0x00485B34
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

		// Token: 0x04009472 RID: 38002
		public Vector3 targetPos;

		// Token: 0x04009473 RID: 38003
		public PathEndingCondition endingCondition;
	}
}
