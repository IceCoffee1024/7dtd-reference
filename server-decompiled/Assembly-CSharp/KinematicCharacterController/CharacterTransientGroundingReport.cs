using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DB8 RID: 7608
	public struct CharacterTransientGroundingReport
	{
		// Token: 0x0600E05E RID: 57438 RVA: 0x004FE3D0 File Offset: 0x004FC5D0
		public void CopyFrom(CharacterGroundingReport groundingReport)
		{
			this.FoundAnyGround = groundingReport.FoundAnyGround;
			this.IsStableOnGround = groundingReport.IsStableOnGround;
			this.SnappingPrevented = groundingReport.SnappingPrevented;
			this.GroundNormal = groundingReport.GroundNormal;
			this.InnerGroundNormal = groundingReport.InnerGroundNormal;
			this.OuterGroundNormal = groundingReport.OuterGroundNormal;
		}

		// Token: 0x0400A9CB RID: 43467
		public bool FoundAnyGround;

		// Token: 0x0400A9CC RID: 43468
		public bool IsStableOnGround;

		// Token: 0x0400A9CD RID: 43469
		public bool SnappingPrevented;

		// Token: 0x0400A9CE RID: 43470
		public Vector3 GroundNormal;

		// Token: 0x0400A9CF RID: 43471
		public Vector3 InnerGroundNormal;

		// Token: 0x0400A9D0 RID: 43472
		public Vector3 OuterGroundNormal;
	}
}
