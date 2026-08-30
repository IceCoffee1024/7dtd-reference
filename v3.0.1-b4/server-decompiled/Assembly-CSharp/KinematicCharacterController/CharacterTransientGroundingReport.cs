using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D2D RID: 7469
	public struct CharacterTransientGroundingReport
	{
		// Token: 0x0600DC2E RID: 56366 RVA: 0x004EF174 File Offset: 0x004ED374
		public void CopyFrom(CharacterGroundingReport groundingReport)
		{
			this.FoundAnyGround = groundingReport.FoundAnyGround;
			this.IsStableOnGround = groundingReport.IsStableOnGround;
			this.SnappingPrevented = groundingReport.SnappingPrevented;
			this.GroundNormal = groundingReport.GroundNormal;
			this.InnerGroundNormal = groundingReport.InnerGroundNormal;
			this.OuterGroundNormal = groundingReport.OuterGroundNormal;
		}

		// Token: 0x0400A6D6 RID: 42710
		public bool FoundAnyGround;

		// Token: 0x0400A6D7 RID: 42711
		public bool IsStableOnGround;

		// Token: 0x0400A6D8 RID: 42712
		public bool SnappingPrevented;

		// Token: 0x0400A6D9 RID: 42713
		public Vector3 GroundNormal;

		// Token: 0x0400A6DA RID: 42714
		public Vector3 InnerGroundNormal;

		// Token: 0x0400A6DB RID: 42715
		public Vector3 OuterGroundNormal;
	}
}
