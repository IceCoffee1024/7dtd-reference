using System;

// Token: 0x02000C19 RID: 3097
public interface IChunkCallback
{
	// Token: 0x06005DF8 RID: 24056
	void OnChunkAdded(Chunk _c);

	// Token: 0x06005DF9 RID: 24057
	void OnChunkBeforeRemove(Chunk _c);

	// Token: 0x06005DFA RID: 24058
	void OnChunkBeforeSave(Chunk _c);
}
