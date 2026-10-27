using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001943 RID: 6467
	public class PathInfoFleeTarget : PathInfoFleeRandom
	{
		// Token: 0x0600C6F0 RID: 50928 RVA: 0x0048F8ED File Offset: 0x0048DAED
		public PathInfoFleeTarget(EntityAlive _entity, Vector3 _fleeTarget, int _searchLength, Vector3 _aimBias, float _aimStrength, float _speed, EAIBase _aiTask)
			: base(_entity, _searchLength, _aimBias, _aimStrength, _speed, _aiTask)
		{
			this.fleeTarget = _fleeTarget;
		}

		// Token: 0x04009632 RID: 38450
		public Vector3 fleeTarget;
	}
}
