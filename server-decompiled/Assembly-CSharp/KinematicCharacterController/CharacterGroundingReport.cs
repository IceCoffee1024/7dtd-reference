using System;
using UnityEngine;

namespace KinematicCharacterController
{
	// Token: 0x02001D86 RID: 7558
	public struct CharacterGroundingReport
	{
		// Token: 0x0600DF49 RID: 57161 RVA: 0x004FA7C8 File Offset: 0x004F89C8
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

		// Token: 0x0400A8DA RID: 43226
		public bool FoundAnyGround;

		// Token: 0x0400A8DB RID: 43227
		public bool IsStableOnGround;

		// Token: 0x0400A8DC RID: 43228
		public bool SnappingPrevented;

		// Token: 0x0400A8DD RID: 43229
		public Vector3 GroundNormal;

		// Token: 0x0400A8DE RID: 43230
		public Vector3 InnerGroundNormal;

		// Token: 0x0400A8DF RID: 43231
		public Vector3 OuterGroundNormal;

		// Token: 0x0400A8E0 RID: 43232
		public Collider GroundCollider;

		// Token: 0x0400A8E1 RID: 43233
		public Vector3 GroundPoint;
	}
}
