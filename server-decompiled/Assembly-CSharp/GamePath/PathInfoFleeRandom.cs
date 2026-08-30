using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018E5 RID: 6373
	public class PathInfoFleeRandom : PathInfo
	{
		// Token: 0x0600C483 RID: 50307 RVA: 0x004879CA File Offset: 0x00485BCA
		public PathInfoFleeRandom(EntityAlive _entity, int _searchLength, Vector3 _aimBias, float _aimStrength, float _speed, EAIBase _aiTask)
			: base(_entity, false, _speed, _aiTask)
		{
			this.searchLength = _searchLength;
			this.aimBias = _aimBias;
			this.aimStrength = _aimStrength;
		}

		// Token: 0x04009480 RID: 38016
		public int searchLength;

		// Token: 0x04009481 RID: 38017
		public Vector3 aimBias = Vector3.zero;

		// Token: 0x04009482 RID: 38018
		public float aimStrength;
	}
}
