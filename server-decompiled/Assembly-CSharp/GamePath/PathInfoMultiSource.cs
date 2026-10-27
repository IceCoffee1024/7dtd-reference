using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001941 RID: 6465
	public class PathInfoMultiSource : PathInfo
	{
		// Token: 0x0600C6EE RID: 50926 RVA: 0x0048F8A0 File Offset: 0x0048DAA0
		public PathInfoMultiSource(EntityAlive _entity, List<Vector3> _sourcePositions, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.sourcePositions = _sourcePositions ?? new List<Vector3>();
		}

		// Token: 0x0400962C RID: 38444
		public readonly List<Vector3> sourcePositions;

		// Token: 0x0400962D RID: 38445
		public OnPathDelegate[] OnTargetPathFinished;

		// Token: 0x0400962E RID: 38446
		public List<Vector3>[] vectorPaths;
	}
}
