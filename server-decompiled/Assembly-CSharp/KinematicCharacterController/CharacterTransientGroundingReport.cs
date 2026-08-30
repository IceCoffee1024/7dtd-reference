using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D55 RID: 7509
	public struct CharacterTransientGroundingReport
	{
		// Token: 0x0600DDCF RID: 56783 RVA: 0x004F5BE8 File Offset: 0x004F3DE8
		public void CopyFrom(CharacterGroundingReport groundingReport)
		{
			this.FoundAnyGround = groundingReport.FoundAnyGround;
			this.IsStableOnGround = groundingReport.IsStableOnGround;
			this.SnappingPrevented = groundingReport.SnappingPrevented;
			this.GroundNormal = groundingReport.GroundNormal;
			this.InnerGroundNormal = groundingReport.InnerGroundNormal;
			this.OuterGroundNormal = groundingReport.OuterGroundNormal;
		}

		// Token: 0x0400A80B RID: 43019
		public bool FoundAnyGround;

		// Token: 0x0400A80C RID: 43020
		public bool IsStableOnGround;

		// Token: 0x0400A80D RID: 43021
		public bool SnappingPrevented;

		// Token: 0x0400A80E RID: 43022
		public Vector3 GroundNormal;

		// Token: 0x0400A80F RID: 43023
		public Vector3 InnerGroundNormal;

		// Token: 0x0400A810 RID: 43024
		public Vector3 OuterGroundNormal;
	}
}
