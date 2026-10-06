using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B10 RID: 2832
public interface IChunk : IBlockAccess
{
	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x060053E6 RID: 21478
	// (set) Token: 0x060053E7 RID: 21479
	int X { get; set; }

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x060053E8 RID: 21480
	int Y { get; }

	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x060053E9 RID: 21481
	// (set) Token: 0x060053EA RID: 21482
	int Z { get; set; }

	// Token: 0x17000934 RID: 2356
	// (get) Token: 0x060053EB RID: 21483
	// (set) Token: 0x060053EC RID: 21484
	Vector3i ChunkPos { get; set; }

	// Token: 0x060053ED RID: 21485
	bool GetAvailable();

	// Token: 0x060053EE RID: 21486
	BlockValue GetBlockNoDamage(int _x, int _y, int _z);

	// Token: 0x060053EF RID: 21487
	void GetBlockColumn(int _x, int _y, int _z, BlockValue[] _blocks);

	// Token: 0x060053F0 RID: 21488
	bool IsOnlyTerrain(int _y);

	// Token: 0x060053F1 RID: 21489
	bool IsOnlyTerrainLayer(int _idx);

	// Token: 0x060053F2 RID: 21490
	bool IsEmpty();

	// Token: 0x060053F3 RID: 21491
	bool IsEmpty(int _y);

	// Token: 0x060053F4 RID: 21492
	bool IsEmptyLayer(int _idx);

	// Token: 0x060053F5 RID: 21493
	byte GetStability(int _x, int _y, int _z);

	// Token: 0x060053F6 RID: 21494
	void SetStability(int _x, int _y, int _z, byte _v);

	// Token: 0x060053F7 RID: 21495
	byte GetLight(int x, int y, int z, Chunk.LIGHT_TYPE type);

	// Token: 0x060053F8 RID: 21496
	int GetLightValue(int x, int y, int z, int _darknessV);

	// Token: 0x060053F9 RID: 21497
	float GetLightBrightness(int x, int y, int z, int _ss);

	// Token: 0x060053FA RID: 21498
	Vector3i GetWorldPos();

	// Token: 0x060053FB RID: 21499
	byte GetHeight(int _blockOffset);

	// Token: 0x060053FC RID: 21500
	byte GetHeight(int _x, int _z);

	// Token: 0x060053FD RID: 21501
	sbyte GetDensity(int _x, int _y, int _z);

	// Token: 0x060053FE RID: 21502
	bool HasSameDensityValue(int _y);

	// Token: 0x060053FF RID: 21503
	sbyte GetSameDensityValue(int _y);

	// Token: 0x06005400 RID: 21504
	int GetBlockFaceTexture(int _x, int _y, int _z, BlockFace _blockFace, int channel);

	// Token: 0x06005401 RID: 21505
	long GetTextureFull(int _x, int _y, int _z, int channel = 0);

	// Token: 0x06005402 RID: 21506 RVA: 0x00204088 File Offset: 0x00202288
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

	// Token: 0x06005403 RID: 21507
	BlockEntityData GetBlockEntity(Vector3i _blockPos);

	// Token: 0x06005404 RID: 21508
	BlockEntityData GetBlockEntity(Transform _transform);

	// Token: 0x06005405 RID: 21509
	void SetTopSoilBroken(int _x, int _z);

	// Token: 0x06005406 RID: 21510
	bool IsTopSoil(int _x, int _z);

	// Token: 0x06005407 RID: 21511
	byte GetTerrainHeight(int _x, int _z);

	// Token: 0x06005408 RID: 21512
	WaterValue GetWater(int _x, int _y, int _z);

	// Token: 0x06005409 RID: 21513
	bool IsWater(int _x, int _y, int _z);

	// Token: 0x0600540A RID: 21514
	bool IsAir(int _x, int _y, int _z);
}
