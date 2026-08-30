using System;

namespace KinematicCharacterController
{
	// Token: 0x02001D29 RID: 7465
	public enum MovementSweepState
	{
		// Token: 0x0400A6BF RID: 42687
		Initial,
		// Token: 0x0400A6C0 RID: 42688
		AfterFirstHit,
		// Token: 0x0400A6C1 RID: 42689
		FoundBlockingCrease,
		// Token: 0x0400A6C2 RID: 42690
		FoundBlockingCorner
	}
}
