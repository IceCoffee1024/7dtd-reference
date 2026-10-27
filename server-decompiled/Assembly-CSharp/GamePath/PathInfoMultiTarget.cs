using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001940 RID: 6464
	public class PathInfoMultiTarget : PathInfo
	{
		// Token: 0x0600C6ED RID: 50925 RVA: 0x0048F87B File Offset: 0x0048DA7B
		public PathInfoMultiTarget(EntityAlive _entity, List<Vector3> _targetPositions, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.targetPositions = _targetPositions ?? new List<Vector3>();
		}

		// Token: 0x04009627 RID: 38439
		public readonly List<Vector3> targetPositions;

		// Token: 0x04009628 RID: 38440
		public bool pathsForAll;

		// Token: 0x04009629 RID: 38441
		public OnPathDelegate[] OnTargetPathFinished;

		// Token: 0x0400962A RID: 38442
		public MultiTargetPathSelection pathSelection = MultiTargetPathSelection.Closest;

		// Token: 0x0400962B RID: 38443
		public List<Vector3>[] vectorPaths;
	}
}
