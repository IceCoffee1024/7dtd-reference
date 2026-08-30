using System;

// Token: 0x02000C07 RID: 3079
public interface IChunkCallback
{
	// Token: 0x06005DDF RID: 24031
	void OnChunkAdded(Chunk _c);

	// Token: 0x06005DE0 RID: 24032
	void OnChunkBeforeRemove(Chunk _c);

	// Token: 0x06005DE1 RID: 24033
	void OnChunkBeforeSave(Chunk _c);
}
