using System;

// Token: 0x02000B4A RID: 2890
public interface IDynamicDecorator
{
	// Token: 0x060057BD RID: 22461
	void DecorateChunk(World _world, Chunk _chunk);

	// Token: 0x060057BE RID: 22462
	void Cleanup();
}
