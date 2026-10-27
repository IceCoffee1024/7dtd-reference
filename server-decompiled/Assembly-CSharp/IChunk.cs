using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B1B RID: 2843
public interface IChunk : IBlockAccess
{
	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x0600540B RID: 21515
	// (set) Token: 0x0600540C RID: 21516
	int X { get; set; }

	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x0600540D RID: 21517
	int Y { get; }

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x0600540E RID: 21518
	// (set) Token: 0x0600540F RID: 21519
	int Z { get; set; }

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x06005410 RID: 21520
	// (set) Token: 0x06005411 RID: 21521
	Vector3i ChunkPos { get; set; }

	// Token: 0x06005412 RID: 21522
	bool GetAvailable();

	// Token: 0x06005413 RID: 21523
	BlockValue GetBlockNoDamage(int _x, int _y, int _z);

	// Token: 0x06005414 RID: 21524
	void GetBlockColumn(int _x, int _y, int _z, BlockValue[] _blocks);

	// Token: 0x06005415 RID: 21525
	bool IsOnlyTerrain(int _y);

	// Token: 0x06005416 RID: 21526
	bool IsOnlyTerrainLayer(int _idx);

	// Token: 0x06005417 RID: 21527
	bool IsEmpty();

	// Token: 0x06005418 RID: 21528
	bool IsEmpty(int _y);

	// Token: 0x06005419 RID: 21529
	bool IsEmptyLayer(int _idx);

	// Token: 0x0600541A RID: 21530
	byte GetStability(int _x, int _y, int _z);

	// Token: 0x0600541B RID: 21531
	void SetStability(int _x, int _y, int _z, byte _v);

	// Token: 0x0600541C RID: 21532
	byte GetLight(int x, int y, int z, Chunk.LIGHT_TYPE type);

	// Token: 0x0600541D RID: 21533
	int GetLightValue(int x, int y, int z, int _darknessV);

	// Token: 0x0600541E RID: 21534
	float GetLightBrightness(int x, int y, int z, int _ss);

	// Token: 0x0600541F RID: 21535
	Vector3i GetWorldPos();

	// Token: 0x06005420 RID: 21536
	byte GetHeight(int _blockOffset);

	// Token: 0x06005421 RID: 21537
	byte GetHeight(int _x, int _z);

	// Token: 0x06005422 RID: 21538
	sbyte GetDensity(int _x, int _y, int _z);

	// Token: 0x06005423 RID: 21539
	bool HasSameDensityValue(int _y);

	// Token: 0x06005424 RID: 21540
	sbyte GetSameDensityValue(int _y);

	// Token: 0x06005425 RID: 21541
	int GetBlockFaceTexture(int _x, int _y, int _z, BlockFace _blockFace, int channel);

	// Token: 0x06005426 RID: 21542
	long GetTextureFull(int _x, int _y, int _z, int channel = 0);

	// Token: 0x06005427 RID: 21543 RVA: 0x0020560C File Offset: 0x0020380C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	TextureFullArray GetTextureFullArray(int _x, int _y, int _z)
	{
		TextureFullArray textureFullArray;
		for (int i = 0; i < 1; i++)
		{
			textureFullArray[i] = this.GetTextureFull(_x, _y, _z, i);
		}
		return textureFullArray;
	}

	// Token: 0x06005428 RID: 21544
	BlockEntityData GetBlockEntity(Vector3i _blockPos);

	// Token: 0x06005429 RID: 21545
	BlockEntityData GetBlockEntity(Transform _transform);

	// Token: 0x0600542A RID: 21546
	void SetTopSoilBroken(int _x, int _z);

	// Token: 0x0600542B RID: 21547
	bool IsTopSoil(int _x, int _z);

	// Token: 0x0600542C RID: 21548
	byte GetTerrainHeight(int _x, int _z);

	// Token: 0x0600542D RID: 21549
	WaterValue GetWater(int _x, int _y, int _z);

	// Token: 0x0600542E RID: 21550
	bool IsWater(int _x, int _y, int _z);

	// Token: 0x0600542F RID: 21551
	bool IsAir(int _x, int _y, int _z);
}
