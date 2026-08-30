using System;

namespace KinematicCharacterController
{
	// Token: 0x02001D51 RID: 7505
	public enum MovementSweepState
	{
		// Token: 0x0400A7F4 RID: 42996
		Initial,
		// Token: 0x0400A7F5 RID: 42997
		AfterFirstHit,
		// Token: 0x0400A7F6 RID: 42998
		FoundBlockingCrease,
		// Token: 0x0400A7F7 RID: 42999
		FoundBlockingCorner
	}
}
