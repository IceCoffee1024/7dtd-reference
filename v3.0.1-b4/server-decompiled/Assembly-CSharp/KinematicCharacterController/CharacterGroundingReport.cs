using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D2C RID: 7468
	public struct CharacterGroundingReport
	{
		// Token: 0x0600DC2D RID: 56365 RVA: 0x004EF10C File Offset: 0x004ED30C
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

		// Token: 0x0400A6CE RID: 42702
		public bool FoundAnyGround;

		// Token: 0x0400A6CF RID: 42703
		public bool IsStableOnGround;

		// Token: 0x0400A6D0 RID: 42704
		public bool SnappingPrevented;

		// Token: 0x0400A6D1 RID: 42705
		public Vector3 GroundNormal;

		// Token: 0x0400A6D2 RID: 42706
		public Vector3 InnerGroundNormal;

		// Token: 0x0400A6D3 RID: 42707
		public Vector3 OuterGroundNormal;

		// Token: 0x0400A6D4 RID: 42708
		public Collider GroundCollider;

		// Token: 0x0400A6D5 RID: 42709
		public Vector3 GroundPoint;
	}
}
