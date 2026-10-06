using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D85 RID: 7557
	public struct OverlapResult
	{
		// Token: 0x0600DF48 RID: 57160 RVA: 0x004FA7B7 File Offset: 0x004F89B7
		public OverlapResult(Vector3 normal, Collider collider)
		{
			this.Normal = normal;
			this.Collider = collider;
		}

		// Token: 0x0400A8D8 RID: 43224
		public Vector3 Normal;

		// Token: 0x0400A8D9 RID: 43225
		public Collider Collider;
	}
}
