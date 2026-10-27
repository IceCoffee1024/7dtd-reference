using System;

// Token: 0x02000B67 RID: 2919
public interface IDynamicDecorator
{
	// Token: 0x060057FC RID: 22524
	void DecorateChunk(World _world, Chunk _chunk);

	// Token: 0x060057FD RID: 22525
	void Cleanup();
}
