using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D87 RID: 7559
	public struct CharacterTransientGroundingReport
	{
		// Token: 0x0600DF4A RID: 57162 RVA: 0x004FA830 File Offset: 0x004F8A30
		public void CopyFrom(CharacterGroundingReport groundingReport)
		{
			this.FoundAnyGround = groundingReport.FoundAnyGround;
			this.IsStableOnGround = groundingReport.IsStableOnGround;
			this.SnappingPrevented = groundingReport.SnappingPrevented;
			this.GroundNormal = groundingReport.GroundNormal;
			this.InnerGroundNormal = groundingReport.InnerGroundNormal;
			this.OuterGroundNormal = groundingReport.OuterGroundNormal;
		}

		// Token: 0x0400A8E2 RID: 43234
		public bool FoundAnyGround;

		// Token: 0x0400A8E3 RID: 43235
		public bool IsStableOnGround;

		// Token: 0x0400A8E4 RID: 43236
		public bool SnappingPrevented;

		// Token: 0x0400A8E5 RID: 43237
		public Vector3 GroundNormal;

		// Token: 0x0400A8E6 RID: 43238
		public Vector3 InnerGroundNormal;

		// Token: 0x0400A8E7 RID: 43239
		public Vector3 OuterGroundNormal;
	}
}
