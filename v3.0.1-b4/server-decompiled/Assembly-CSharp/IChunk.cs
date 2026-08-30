using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000AEC RID: 2796
public interface IChunk : IBlockAccess
{
	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x06005345 RID: 21317
	// (set) Token: 0x06005346 RID: 21318
	int X { get; set; }

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x06005347 RID: 21319
	int Y { get; }

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x06005348 RID: 21320
	// (set) Token: 0x06005349 RID: 21321
	int Z { get; set; }

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x0600534A RID: 21322
	// (set) Token: 0x0600534B RID: 21323
	Vector3i ChunkPos { get; set; }

	// Token: 0x0600534C RID: 21324
	bool GetAvailable();

	// Token: 0x0600534D RID: 21325
	BlockValue GetBlockNoDamage(int _x, int _y, int _z);

	// Token: 0x0600534E RID: 21326
	void GetBlockColumn(int _x, int _y, int _z, BlockValue[] _blocks);

	// Token: 0x0600534F RID: 21327
	bool IsOnlyTerrain(int _y);

	// Token: 0x06005350 RID: 21328
	bool IsOnlyTerrainLayer(int _idx);

	// Token: 0x06005351 RID: 21329
	bool IsEmpty();

	// Token: 0x06005352 RID: 21330
	bool IsEmpty(int _y);

	// Token: 0x06005353 RID: 21331
	bool IsEmptyLayer(int _idx);

	// Token: 0x06005354 RID: 21332
	byte GetStability(int _x, int _y, int _z);

	// Token: 0x06005355 RID: 21333
	void SetStability(int _x, int _y, int _z, byte _v);

	// Token: 0x06005356 RID: 21334
	byte GetLight(int x, int y, int z, Chunk.LIGHT_TYPE type);

	// Token: 0x06005357 RID: 21335
	int GetLightValue(int x, int y, int z, int _darknessV);

	// Token: 0x06005358 RID: 21336
	float GetLightBrightness(int x, int y, int z, int _ss);

	// Token: 0x06005359 RID: 21337
	Vector3i GetWorldPos();

	// Token: 0x0600535A RID: 21338
	byte GetHeight(int _blockOffset);

	// Token: 0x0600535B RID: 21339
	byte GetHeight(int _x, int _z);

	// Token: 0x0600535C RID: 21340
	sbyte GetDensity(int _x, int _y, int _z);

	// Token: 0x0600535D RID: 21341
	bool HasSameDensityValue(int _y);

	// Token: 0x0600535E RID: 21342
	sbyte GetSameDensityValue(int _y);

	// Token: 0x0600535F RID: 21343
	int GetBlockFaceTexture(int _x, int _y, int _z, BlockFace _blockFace, int channel);

	// Token: 0x06005360 RID: 21344
	long GetTextureFull(int _x, int _y, int _z, int channel = 0);

	// Token: 0x06005361 RID: 21345 RVA: 0x001FE1B8 File Offset: 0x001FC3B8
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

	// Token: 0x06005362 RID: 21346
	BlockEntityData GetBlockEntity(Vector3i _blockPos);

	// Token: 0x06005363 RID: 21347
	BlockEntityData GetBlockEntity(Transform _transform);

	// Token: 0x06005364 RID: 21348
	void SetTopSoilBroken(int _x, int _z);

	// Token: 0x06005365 RID: 21349
	bool IsTopSoil(int _x, int _z);

	// Token: 0x06005366 RID: 21350
	byte GetTerrainHeight(int _x, int _z);

	// Token: 0x06005367 RID: 21351
	WaterValue GetWater(int _x, int _y, int _z);

	// Token: 0x06005368 RID: 21352
	bool IsWater(int _x, int _y, int _z);

	// Token: 0x06005369 RID: 21353
	bool IsAir(int _x, int _y, int _z);
}
