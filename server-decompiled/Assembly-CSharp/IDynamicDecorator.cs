using System;

// Token: 0x02000B5C RID: 2908
public interface IDynamicDecorator
{
	// Token: 0x060057D7 RID: 22487
	void DecorateChunk(World _world, Chunk _chunk);

	// Token: 0x060057D8 RID: 22488
	void Cleanup();
}
