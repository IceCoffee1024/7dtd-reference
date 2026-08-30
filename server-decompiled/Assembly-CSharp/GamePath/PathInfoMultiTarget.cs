using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018E3 RID: 6371
	public class PathInfoMultiTarget : PathInfo
	{
		// Token: 0x0600C481 RID: 50305 RVA: 0x00487987 File Offset: 0x00485B87
		public PathInfoMultiTarget(EntityAlive _entity, List<Vector3> _targetPositions, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.targetPositions = _targetPositions ?? new List<Vector3>();
		}

		// Token: 0x04009478 RID: 38008
		public readonly List<Vector3> targetPositions;

		// Token: 0x04009479 RID: 38009
		public bool pathsForAll;

		// Token: 0x0400947A RID: 38010
		public OnPathDelegate[] OnTargetPathFinished;

		// Token: 0x0400947B RID: 38011
		public MultiTargetPathSelection pathSelection = MultiTargetPathSelection.Closest;

		// Token: 0x0400947C RID: 38012
		public List<Vector3>[] vectorPaths;
	}
}
