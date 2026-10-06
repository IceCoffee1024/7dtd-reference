using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001912 RID: 6418
	public class PathInfoFleeTarget : PathInfoFleeRandom
	{
		// Token: 0x0600C5DC RID: 50652 RVA: 0x0048BF41 File Offset: 0x0048A141
		public PathInfoFleeTarget(EntityAlive _entity, Vector3 _fleeTarget, int _searchLength, Vector3 _aimBias, float _aimStrength, float _speed, EAIBase _aiTask)
			: base(_entity, _searchLength, _aimBias, _aimStrength, _speed, _aiTask)
		{
			this.fleeTarget = _fleeTarget;
		}

		// Token: 0x04009549 RID: 38217
		public Vector3 fleeTarget;
	}
}
