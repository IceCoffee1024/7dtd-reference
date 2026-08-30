using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018C0 RID: 6336
	public class PathInfoFleeTarget : PathInfoFleeRandom
	{
		// Token: 0x0600C2FD RID: 49917 RVA: 0x004818CD File Offset: 0x0047FACD
		public PathInfoFleeTarget(EntityAlive _entity, Vector3 _fleeTarget, int _searchLength, Vector3 _aimBias, float _aimStrength, float _speed, EAIBase _aiTask)
			: base(_entity, _searchLength, _aimBias, _aimStrength, _speed, _aiTask)
		{
			this.fleeTarget = _fleeTarget;
		}

		// Token: 0x0400935A RID: 37722
		public Vector3 fleeTarget;
	}
}
