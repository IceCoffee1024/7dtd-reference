using System;

// Token: 0x02000C34 RID: 3124
public interface IWorldDecorator
{
	// Token: 0x06005F23 RID: 24355
	void DecorateChunkOverlapping(World _world, Chunk chunk, Chunk cXp1Z, Chunk cXZp1, Chunk cXp1Zp1, int seed);
}
