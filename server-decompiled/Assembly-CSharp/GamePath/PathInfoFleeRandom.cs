using System;
using UnityEngine;

namespace GamePath
{
	// Token: 0x02001942 RID: 6466
	public class PathInfoFleeRandom : PathInfo
	{
		// Token: 0x0600C6EF RID: 50927 RVA: 0x0048F8BE File Offset: 0x0048DABE
		public PathInfoFleeRandom(EntityAlive _entity, int _searchLength, Vector3 _aimBias, float _aimStrength, float _speed, EAIBase _aiTask)
			: base(_entity, false, _speed, _aiTask)
		{
			this.searchLength = _searchLength;
			this.aimBias = _aimBias;
			this.aimStrength = _aimStrength;
		}

		// Token: 0x0400962F RID: 38447
		public int searchLength;

		// Token: 0x04009630 RID: 38448
		public Vector3 aimBias = Vector3.zero;

		// Token: 0x04009631 RID: 38449
		public float aimStrength;
	}
}
