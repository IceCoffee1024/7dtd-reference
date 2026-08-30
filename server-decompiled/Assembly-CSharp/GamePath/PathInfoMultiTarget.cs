using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018BD RID: 6333
	public class PathInfoMultiTarget : PathInfo
	{
		// Token: 0x0600C2FA RID: 49914 RVA: 0x0048185B File Offset: 0x0047FA5B
		public PathInfoMultiTarget(EntityAlive _entity, List<Vector3> _targetPositions, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.targetPositions = _targetPositions ?? new List<Vector3>();
		}

		// Token: 0x0400934F RID: 37711
		public readonly List<Vector3> targetPositions;

		// Token: 0x04009350 RID: 37712
		public bool pathsForAll;

		// Token: 0x04009351 RID: 37713
		public OnPathDelegate[] OnTargetPathFinished;

		// Token: 0x04009352 RID: 37714
		public MultiTargetPathSelection pathSelection = MultiTargetPathSelection.Closest;

		// Token: 0x04009353 RID: 37715
		public List<Vector3>[] vectorPaths;
	}
}
