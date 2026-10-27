using System;

// Token: 0x02000B6F RID: 2927
public interface ILightProcessor
{
	// Token: 0x0600581E RID: 22558
	void LightChunk(Chunk _chunk);

	// Token: 0x0600581F RID: 22559
	void GenerateSunlight(Chunk chunk, bool bSpreadLight);

	// Token: 0x06005820 RID: 22560
	void SpreadLight(Chunk c, int blockX, int blockY, int blockZ, byte lightValue, Chunk.LIGHT_TYPE type, bool bSetAtStarterPos = true);

	// Token: 0x06005821 RID: 22561
	void UnspreadLight(Chunk c, int x, int y, int z, byte lightValue, Chunk.LIGHT_TYPE type);

	// Token: 0x06005822 RID: 22562
	void RefreshSunlightAtLocalPos(Chunk c, int x, int y, bool _isSpread);

	// Token: 0x06005823 RID: 22563
	byte RefreshLightAtLocalPos(Chunk c, int x, int y, int z, Chunk.LIGHT_TYPE type);
}
