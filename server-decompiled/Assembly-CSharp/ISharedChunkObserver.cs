using System;

// Token: 0x02000BBC RID: 3004
public interface ISharedChunkObserver : IDisposable
{
	// Token: 0x06005A71 RID: 23153
	void Reference();

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x06005A72 RID: 23154
	Vector2i ChunkPos { get; }

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x06005A73 RID: 23155
	SharedChunkObserverCache Owner { get; }
}
