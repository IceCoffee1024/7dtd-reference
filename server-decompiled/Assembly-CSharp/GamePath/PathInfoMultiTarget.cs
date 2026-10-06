using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x0200190F RID: 6415
	public class PathInfoMultiTarget : PathInfo
	{
		// Token: 0x0600C5D9 RID: 50649 RVA: 0x0048BECF File Offset: 0x0048A0CF
		public PathInfoMultiTarget(EntityAlive _entity, List<Vector3> _targetPositions, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.targetPositions = _targetPositions ?? new List<Vector3>();
		}

		// Token: 0x0400953E RID: 38206
		public readonly List<Vector3> targetPositions;

		// Token: 0x0400953F RID: 38207
		public bool pathsForAll;

		// Token: 0x04009540 RID: 38208
		public OnPathDelegate[] OnTargetPathFinished;

		// Token: 0x04009541 RID: 38209
		public MultiTargetPathSelection pathSelection = MultiTargetPathSelection.Closest;

		// Token: 0x04009542 RID: 38210
		public List<Vector3>[] vectorPaths;
	}
}
