using System;

// Token: 0x02000C58 RID: 3160
public interface IWorldDecorator
{
	// Token: 0x06005FC6 RID: 24518
	void DecorateChunkOverlapping(World _world, Chunk chunk, Chunk cXp1Z, Chunk cXZp1, Chunk cXp1Zp1, int seed);
}
