using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018E4 RID: 6372
	public class PathInfoMultiSource : PathInfo
	{
		// Token: 0x0600C482 RID: 50306 RVA: 0x004879AC File Offset: 0x00485BAC
		public PathInfoMultiSource(EntityAlive _entity, List<Vector3> _sourcePositions, bool _canBreakBlocks, float _speed, EAIBase _aiTask)
			: base(_entity, _canBreakBlocks, _speed, _aiTask)
		{
			this.sourcePositions = _sourcePositions ?? new List<Vector3>();
		}

		// Token: 0x0400947D RID: 38013
		public readonly List<Vector3> sourcePositions;

		// Token: 0x0400947E RID: 38014
		public OnPathDelegate[] OnTargetPathFinished;

		// Token: 0x0400947F RID: 38015
		public List<Vector3>[] vectorPaths;
	}
}
