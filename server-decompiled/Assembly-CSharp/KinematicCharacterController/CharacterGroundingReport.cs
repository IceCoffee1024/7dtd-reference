using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001DB7 RID: 7607
	public struct CharacterGroundingReport
	{
		// Token: 0x0600E05D RID: 57437 RVA: 0x004FE368 File Offset: 0x004FC568
		public void CopyFrom(CharacterTransientGroundingReport transientGroundingReport)
		{
			this.FoundAnyGround = transientGroundingReport.FoundAnyGround;
			this.IsStableOnGround = transientGroundingReport.IsStableOnGround;
			this.SnappingPrevented = transientGroundingReport.SnappingPrevented;
			this.GroundNormal = transientGroundingReport.GroundNormal;
			this.InnerGroundNormal = transientGroundingReport.InnerGroundNormal;
			this.OuterGroundNormal = transientGroundingReport.OuterGroundNormal;
			this.GroundCollider = null;
			this.GroundPoint = Vector3.zero;
		}

		// Token: 0x0400A9C3 RID: 43459
		public bool FoundAnyGround;

		// Token: 0x0400A9C4 RID: 43460
		public bool IsStableOnGround;

		// Token: 0x0400A9C5 RID: 43461
		public bool SnappingPrevented;

		// Token: 0x0400A9C6 RID: 43462
		public Vector3 GroundNormal;

		// Token: 0x0400A9C7 RID: 43463
		public Vector3 InnerGroundNormal;

		// Token: 0x0400A9C8 RID: 43464
		public Vector3 OuterGroundNormal;

		// Token: 0x0400A9C9 RID: 43465
		public Collider GroundCollider;

		// Token: 0x0400A9CA RID: 43466
		public Vector3 GroundPoint;
	}
}
