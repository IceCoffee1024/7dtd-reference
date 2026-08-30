using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D53 RID: 7507
	public struct OverlapResult
	{
		// Token: 0x0600DDCD RID: 56781 RVA: 0x004F5B6F File Offset: 0x004F3D6F
		public OverlapResult(Vector3 normal, Collider collider)
		{
			this.Normal = normal;
			this.Collider = collider;
		}

		// Token: 0x0400A801 RID: 43009
		public Vector3 Normal;

		// Token: 0x0400A802 RID: 43010
		public Collider Collider;
	}
}
