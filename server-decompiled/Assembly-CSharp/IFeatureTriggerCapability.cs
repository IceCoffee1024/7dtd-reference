using System;
using System.Collections.Generic;

// Token: 0x0200016C RID: 364
public interface IFeatureTriggerCapability
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000A7D RID: 2685
	IFeatureTriggerCapability.ETriggerRole TriggerRole { get; }

	// Token: 0x06000A7E RID: 2686
	void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy);

	// Token: 0x0200016D RID: 365
	[Flags]
	public enum ETriggerRole
	{
		// Token: 0x04000A22 RID: 2594
		None = 0,
		// Token: 0x04000A23 RID: 2595
		TriggeredBy = 1,
		// Token: 0x04000A24 RID: 2596
		Source = 2,
		// Token: 0x04000A25 RID: 2597
		Both = 3
	}
}
