using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D54 RID: 7508
	public struct CharacterGroundingReport
	{
		// Token: 0x0600DDCE RID: 56782 RVA: 0x004F5B80 File Offset: 0x004F3D80
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

		// Token: 0x0400A803 RID: 43011
		public bool FoundAnyGround;

		// Token: 0x0400A804 RID: 43012
		public bool IsStableOnGround;

		// Token: 0x0400A805 RID: 43013
		public bool SnappingPrevented;

		// Token: 0x0400A806 RID: 43014
		public Vector3 GroundNormal;

		// Token: 0x0400A807 RID: 43015
		public Vector3 InnerGroundNormal;

		// Token: 0x0400A808 RID: 43016
		public Vector3 OuterGroundNormal;

		// Token: 0x0400A809 RID: 43017
		public Collider GroundCollider;

		// Token: 0x0400A80A RID: 43018
		public Vector3 GroundPoint;
	}
}
