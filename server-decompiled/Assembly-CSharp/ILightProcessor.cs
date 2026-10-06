using System;

// Token: 0x02000B64 RID: 2916
public interface ILightProcessor
{
	// Token: 0x060057F9 RID: 22521
	void LightChunk(Chunk _chunk);

	// Token: 0x060057FA RID: 22522
	void GenerateSunlight(Chunk chunk, bool bSpreadLight);

	// Token: 0x060057FB RID: 22523
	void SpreadLight(Chunk c, int blockX, int blockY, int blockZ, byte lightValue, Chunk.LIGHT_TYPE type, bool bSetAtStarterPos = true);

	// Token: 0x060057FC RID: 22524
	void UnspreadLight(Chunk c, int x, int y, int z, byte lightValue, Chunk.LIGHT_TYPE type);

	// Token: 0x060057FD RID: 22525
	void RefreshSunlightAtLocalPos(Chunk c, int x, int y, bool _isSpread);

	// Token: 0x060057FE RID: 22526
	byte RefreshLightAtLocalPos(Chunk c, int x, int y, int z, Chunk.LIGHT_TYPE type);
}
