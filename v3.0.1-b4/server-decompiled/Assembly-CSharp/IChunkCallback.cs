using System;

// Token: 0x02000BF5 RID: 3061
public interface IChunkCallback
{
	// Token: 0x06005D55 RID: 23893
	void OnChunkAdded(Chunk _c);

	// Token: 0x06005D56 RID: 23894
	void OnChunkBeforeRemove(Chunk _c);

	// Token: 0x06005D57 RID: 23895
	void OnChunkBeforeSave(Chunk _c);
}
