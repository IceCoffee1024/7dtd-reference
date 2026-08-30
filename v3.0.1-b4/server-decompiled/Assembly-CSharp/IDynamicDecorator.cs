using System;

// Token: 0x02000B38 RID: 2872
public interface IDynamicDecorator
{
	// Token: 0x06005735 RID: 22325
	void DecorateChunk(World _world, Chunk _chunk);

	// Token: 0x06005736 RID: 22326
	void Cleanup();
}
