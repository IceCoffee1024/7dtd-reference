using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x020018BF RID: 6335
	public class PathInfoFleeRandom : PathInfo
	{
		// Token: 0x0600C2FC RID: 49916 RVA: 0x0048189E File Offset: 0x0047FA9E
		public PathInfoFleeRandom(EntityAlive _entity, int _searchLength, Vector3 _aimBias, float _aimStrength, float _speed, EAIBase _aiTask)
			: base(_entity, false, _speed, _aiTask)
		{
			this.searchLength = _searchLength;
			this.aimBias = _aimBias;
			this.aimStrength = _aimStrength;
		}

		// Token: 0x04009357 RID: 37719
		public int searchLength;

		// Token: 0x04009358 RID: 37720
		public Vector3 aimBias = Vector3.zero;

		// Token: 0x04009359 RID: 37721
		public float aimStrength;
	}
}
