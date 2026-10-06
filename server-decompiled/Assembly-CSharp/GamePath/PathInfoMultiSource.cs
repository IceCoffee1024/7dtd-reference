using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001910 RID: 6416
	public class PathInfoMultiSource : PathInfo
	{
		// Token: 0x0600C5DA RID: 50650 RVA: 0x0048BEF4 File Offset: 0x0048A0F4
		public PathInfoMultiSource(EntityAlive _entity, List<Vector3> _sourcePositions, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.sourcePositions = _sourcePositions ?? new List<Vector3>();
		}

		// Token: 0x04009543 RID: 38211
		public readonly List<Vector3> sourcePositions;

		// Token: 0x04009544 RID: 38212
		public OnPathDelegate[] OnTargetPathFinished;

		// Token: 0x04009545 RID: 38213
		public List<Vector3>[] vectorPaths;
	}
}
