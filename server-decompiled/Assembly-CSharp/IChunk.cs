using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000AFE RID: 2814
public interface IChunk : IBlockAccess
{
	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x060053CD RID: 21453
	// (set) Token: 0x060053CE RID: 21454
	int X { get; set; }

	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x060053CF RID: 21455
	int Y { get; }

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x060053D0 RID: 21456
	// (set) Token: 0x060053D1 RID: 21457
	int Z { get; set; }

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x060053D2 RID: 21458
	// (set) Token: 0x060053D3 RID: 21459
	Vector3i ChunkPos { get; set; }

	// Token: 0x060053D4 RID: 21460
	bool GetAvailable();

	// Token: 0x060053D5 RID: 21461
	BlockValue GetBlockNoDamage(int _x, int _y, int _z);

	// Token: 0x060053D6 RID: 21462
	void GetBlockColumn(int _x, int _y, int _z, BlockValue[] _blocks);

	// Token: 0x060053D7 RID: 21463
	bool IsOnlyTerrain(int _y);

	// Token: 0x060053D8 RID: 21464
	bool IsOnlyTerrainLayer(int _idx);

	// Token: 0x060053D9 RID: 21465
	bool IsEmpty();

	// Token: 0x060053DA RID: 21466
	bool IsEmpty(int _y);

	// Token: 0x060053DB RID: 21467
	bool IsEmptyLayer(int _idx);

	// Token: 0x060053DC RID: 21468
	byte GetStability(int _x, int _y, int _z);

	// Token: 0x060053DD RID: 21469
	void SetStability(int _x, int _y, int _z, byte _v);

	// Token: 0x060053DE RID: 21470
	byte GetLight(int x, int y, int z, Chunk.LIGHT_TYPE type);

	// Token: 0x060053DF RID: 21471
	int GetLightValue(int x, int y, int z, int _darknessV);

	// Token: 0x060053E0 RID: 21472
	float GetLightBrightness(int x, int y, int z, int _ss);

	// Token: 0x060053E1 RID: 21473
	Vector3i GetWorldPos();

	// Token: 0x060053E2 RID: 21474
	byte GetHeight(int _blockOffset);

	// Token: 0x060053E3 RID: 21475
	byte GetHeight(int _x, int _z);

	// Token: 0x060053E4 RID: 21476
	sbyte GetDensity(int _x, int _y, int _z);

	// Token: 0x060053E5 RID: 21477
	bool HasSameDensityValue(int _y);

	// Token: 0x060053E6 RID: 21478
	sbyte GetSameDensityValue(int _y);

	// Token: 0x060053E7 RID: 21479
	int GetBlockFaceTexture(int _x, int _y, int _z, BlockFace _blockFace, int channel);

	// Token: 0x060053E8 RID: 21480
	long GetTextureFull(int _x, int _y, int _z, int channel = 0);

	// Token: 0x060053E9 RID: 21481 RVA: 0x00200E28 File Offset: 0x001FF028
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

	// Token: 0x060053EA RID: 21482
	BlockEntityData GetBlockEntity(Vector3i _blockPos);

	// Token: 0x060053EB RID: 21483
	BlockEntityData GetBlockEntity(Transform _transform);

	// Token: 0x060053EC RID: 21484
	void SetTopSoilBroken(int _x, int _z);

	// Token: 0x060053ED RID: 21485
	bool IsTopSoil(int _x, int _z);

	// Token: 0x060053EE RID: 21486
	byte GetTerrainHeight(int _x, int _z);

	// Token: 0x060053EF RID: 21487
	WaterValue GetWater(int _x, int _y, int _z);

	// Token: 0x060053F0 RID: 21488
	bool IsWater(int _x, int _y, int _z);

	// Token: 0x060053F1 RID: 21489
	bool IsAir(int _x, int _y, int _z);
}
