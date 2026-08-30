using System;

// Token: 0x02000B52 RID: 2898
public interface ILightProcessor
{
	// Token: 0x060057DF RID: 22495
	void LightChunk(Chunk _chunk);

	// Token: 0x060057E0 RID: 22496
	void GenerateSunlight(Chunk chunk, bool bSpreadLight);

	// Token: 0x060057E1 RID: 22497
	void SpreadLight(Chunk c, int blockX, int blockY, int blockZ, byte lightValue, Chunk.LIGHT_TYPE type, bool bSetAtStarterPos = true);

	// Token: 0x060057E2 RID: 22498
	void UnspreadLight(Chunk c, int x, int y, int z, byte lightValue, Chunk.LIGHT_TYPE type);

	// Token: 0x060057E3 RID: 22499
	void RefreshSunlightAtLocalPos(Chunk c, int x, int y, bool _isSpread);

	// Token: 0x060057E4 RID: 22500
	byte RefreshLightAtLocalPos(Chunk c, int x, int y, int z, Chunk.LIGHT_TYPE type);
}
