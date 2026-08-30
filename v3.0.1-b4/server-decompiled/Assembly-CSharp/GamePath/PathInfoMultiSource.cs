using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018BE RID: 6334
	public class PathInfoMultiSource : PathInfo
	{
		// Token: 0x0600C2FB RID: 49915 RVA: 0x00481880 File Offset: 0x0047FA80
		public PathInfoMultiSource(EntityAlive _entity, List<Vector3> _sourcePositions, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.sourcePositions = _sourcePositions ?? new List<Vector3>();
		}

		// Token: 0x04009354 RID: 37716
		public readonly List<Vector3> sourcePositions;

		// Token: 0x04009355 RID: 37717
		public OnPathDelegate[] OnTargetPathFinished;

		// Token: 0x04009356 RID: 37718
		public List<Vector3>[] vectorPaths;
	}
}
