using System;

// Token: 0x02000B40 RID: 2880
public interface ILightProcessor
{
	// Token: 0x06005757 RID: 22359
	void LightChunk(Chunk _chunk);

	// Token: 0x06005758 RID: 22360
	void GenerateSunlight(Chunk chunk, bool bSpreadLight);

	// Token: 0x06005759 RID: 22361
	void SpreadLight(Chunk c, int blockX, int blockY, int blockZ, byte lightValue, Chunk.LIGHT_TYPE type, bool bSetAtStarterPos = true);

	// Token: 0x0600575A RID: 22362
	void UnspreadLight(Chunk c, int x, int y, int z, byte lightValue, Chunk.LIGHT_TYPE type);

	// Token: 0x0600575B RID: 22363
	void RefreshSunlightAtLocalPos(Chunk c, int x, int y, bool _isSpread);

	// Token: 0x0600575C RID: 22364
	byte RefreshLightAtLocalPos(Chunk c, int x, int y, int z, Chunk.LIGHT_TYPE type);
}
