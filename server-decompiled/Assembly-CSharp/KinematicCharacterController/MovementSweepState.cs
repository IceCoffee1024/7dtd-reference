using System;

namespace KinematicCharacterController
{
	// Token: 0x02001DB4 RID: 7604
	public enum MovementSweepState
	{
		// Token: 0x0400A9B4 RID: 43444
		Initial,
		// Token: 0x0400A9B5 RID: 43445
		AfterFirstHit,
		// Token: 0x0400A9B6 RID: 43446
		FoundBlockingCrease,
		// Token: 0x0400A9B7 RID: 43447
		FoundBlockingCorner
	}
}
