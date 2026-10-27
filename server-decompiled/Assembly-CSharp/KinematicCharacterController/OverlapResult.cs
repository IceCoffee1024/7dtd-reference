using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DB6 RID: 7606
	public struct OverlapResult
	{
		// Token: 0x0600E05C RID: 57436 RVA: 0x004FE357 File Offset: 0x004FC557
		public OverlapResult(Vector3 normal, Collider collider)
		{
			this.Normal = normal;
			this.Collider = collider;
		}

		// Token: 0x0400A9C1 RID: 43457
		public Vector3 Normal;

		// Token: 0x0400A9C2 RID: 43458
		public Collider Collider;
	}
}
