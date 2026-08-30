using System;

// Token: 0x02000C46 RID: 3142
public interface IWorldDecorator
{
	// Token: 0x06005FAD RID: 24493
	void DecorateChunkOverlapping(World _world, Chunk chunk, Chunk cXp1Z, Chunk cXZp1, Chunk cXp1Zp1, int seed);
}
