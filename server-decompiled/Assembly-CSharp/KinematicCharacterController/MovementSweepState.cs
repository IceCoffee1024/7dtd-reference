using System;

namespace KinematicCharacterController
{
	// Token: 0x02001D83 RID: 7555
	public enum MovementSweepState
	{
		// Token: 0x0400A8CB RID: 43211
		Initial,
		// Token: 0x0400A8CC RID: 43212
		AfterFirstHit,
		// Token: 0x0400A8CD RID: 43213
		FoundBlockingCrease,
		// Token: 0x0400A8CE RID: 43214
		FoundBlockingCorner
	}
}
