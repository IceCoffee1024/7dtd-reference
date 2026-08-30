using System;

// Token: 0x02000B98 RID: 2968
public interface ISharedChunkObserver : IDisposable
{
	// Token: 0x060059D0 RID: 22992
	void Reference();

	// Token: 0x17000961 RID: 2401
	// (get) Token: 0x060059D1 RID: 22993
	Vector2i ChunkPos { get; }

	// Token: 0x17000962 RID: 2402
	// (get) Token: 0x060059D2 RID: 22994
	SharedChunkObserverCache Owner { get; }
}
