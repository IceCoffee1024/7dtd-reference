using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018E6 RID: 6374
	public class PathInfoFleeTarget : PathInfoFleeRandom
	{
		// Token: 0x0600C484 RID: 50308 RVA: 0x004879F9 File Offset: 0x00485BF9
		public PathInfoFleeTarget(EntityAlive _entity, Vector3 _fleeTarget, int _searchLength, Vector3 _aimBias, float _aimStrength, float _speed, EAIBase _aiTask)
			: base(_entity, _searchLength, _aimBias, _aimStrength, _speed, _aiTask)
		{
			this.fleeTarget = _fleeTarget;
		}

		// Token: 0x04009483 RID: 38019
		public Vector3 fleeTarget;
	}
}
