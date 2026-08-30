using System;
using System.Collections.Generic;

// Token: 0x0200016D RID: 365
public interface IFeatureTriggerCapability
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000A87 RID: 2695
	IFeatureTriggerCapability.ETriggerRole TriggerRole { get; }

	// Token: 0x06000A88 RID: 2696
	void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy);

	// Token: 0x0200016E RID: 366
	[Flags]
	public enum ETriggerRole
	{
		// Token: 0x04000A24 RID: 2596
		None = 0,
		// Token: 0x04000A25 RID: 2597
		TriggeredBy = 1,
		// Token: 0x04000A26 RID: 2598
		Source = 2,
		// Token: 0x04000A27 RID: 2599
		Both = 3
	}
}
