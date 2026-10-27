using System;

// Token: 0x02000BC7 RID: 3015
public interface ISharedChunkObserver : IDisposable
{
	// Token: 0x06005A96 RID: 23190
	void Reference();

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x06005A97 RID: 23191
	Vector2i ChunkPos { get; }

	// Token: 0x1700099C RID: 2460
	// (get) Token: 0x06005A98 RID: 23192
	SharedChunkObserverCache Owner { get; }
}
