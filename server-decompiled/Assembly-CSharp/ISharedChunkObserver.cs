using System;

// Token: 0x02000BAA RID: 2986
public interface ISharedChunkObserver : IDisposable
{
	// Token: 0x06005A58 RID: 23128
	void Reference();

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06005A59 RID: 23129
	Vector2i ChunkPos { get; }

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06005A5A RID: 23130
	SharedChunkObserverCache Owner { get; }
}
