using System;

// Token: 0x02000C63 RID: 3171
public interface IWorldDecorator
{
	// Token: 0x06005FEB RID: 24555
	void DecorateChunkOverlapping(World _world, Chunk chunk, Chunk cXp1Z, Chunk cXZp1, Chunk cXp1Zp1, int seed);
}
