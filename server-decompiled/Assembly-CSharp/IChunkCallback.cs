using System;

// Token: 0x02000C24 RID: 3108
public interface IChunkCallback
{
	// Token: 0x06005E1D RID: 24093
	void OnChunkAdded(Chunk _c);

	// Token: 0x06005E1E RID: 24094
	void OnChunkBeforeRemove(Chunk _c);

	// Token: 0x06005E1F RID: 24095
	void OnChunkBeforeSave(Chunk _c);
}
