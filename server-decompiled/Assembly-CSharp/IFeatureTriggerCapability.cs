using System;
using System.Collections.Generic;

// Token: 0x0200016F RID: 367
public interface IFeatureTriggerCapability
{
	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000A8D RID: 2701
	IFeatureTriggerCapability.ETriggerRole TriggerRole { get; }

	// Token: 0x06000A8E RID: 2702
	void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy);

	// Token: 0x02000170 RID: 368
	[Flags]
	public enum ETriggerRole
	{
		// Token: 0x04000A27 RID: 2599
		None = 0,
		// Token: 0x04000A28 RID: 2600
		TriggeredBy = 1,
		// Token: 0x04000A29 RID: 2601
		Source = 2,
		// Token: 0x04000A2A RID: 2602
		Both = 3
	}
}
