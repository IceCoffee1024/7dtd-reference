using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001911 RID: 6417
	public class PathInfoFleeRandom : PathInfo
	{
		// Token: 0x0600C5DB RID: 50651 RVA: 0x0048BF12 File Offset: 0x0048A112
		public PathInfoFleeRandom(EntityAlive _entity, int _searchLength, Vector3 _aimBias, float _aimStrength, float _speed, EAIBase _aiTask)
			: base(_entity, false, _speed, _aiTask)
		{
			this.searchLength = _searchLength;
			this.aimBias = _aimBias;
			this.aimStrength = _aimStrength;
		}

		// Token: 0x04009546 RID: 38214
		public int searchLength;

		// Token: 0x04009547 RID: 38215
		public Vector3 aimBias = Vector3.zero;

		// Token: 0x04009548 RID: 38216
		public float aimStrength;
	}
}
