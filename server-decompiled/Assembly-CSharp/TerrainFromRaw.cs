using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C65 RID: 3173
public class TerrainFromRaw : TerrainGeneratorWithBiomeResource
{
	// Token: 0x06005FF2 RID: 24562 RVA: 0x002614AC File Offset: 0x0025F6AC
	public void Init(HeightMap _heightMap, IBiomeProvider _biomeProvider, int _seed)
	{
		base.Init(null, _biomeProvider, _seed);
		this.heightMap = _heightMap;
		this.terrainWidth = this.heightMap.GetWidth() << this.heightMap.GetScaleShift();
		this.terrainHeight = this.heightMap.GetHeight() << this.heightMap.GetScaleShift();
		this.terrainWidthHalf = this.terrainWidth / 2;
		this.terrainHeightHalf = this.terrainHeight / 2;
	}

	// Token: 0x06005FF3 RID: 24563 RVA: 0x00261525 File Offset: 0x0025F725
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool checkCoordinates(ref int _x, ref int _z)
	{
		_x += this.terrainWidthHalf;
		if ((ulong)_x >= (ulong)((long)this.terrainWidth))
		{
			return false;
		}
		_z += this.terrainHeightHalf;
		return (ulong)_z < (ulong)((long)this.terrainHeight);
	}

	// Token: 0x06005FF4 RID: 24564 RVA: 0x0026155C File Offset: 0x0025F75C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void fillDensityInBlock(Chunk _chunk, int _x, int _y, int _z, BlockValue _bv)
	{
		int num = (_chunk.X << 4) + _x;
		int num2 = (_chunk.Z << 4) + _z;
		if (!this.checkCoordinates(ref num, ref num2))
		{
			return;
		}
		float num3 = this.heightMap.GetAt(num, num2) + 0.5f;
		int num4 = (int)num3;
		sbyte b;
		if (_y < num4)
		{
			b = MarchingCubes.DensityTerrain;
		}
		else if (_y > num4 + 1)
		{
			b = MarchingCubes.DensityAir;
		}
		else
		{
			float num5 = num3 - (float)num4;
			if (num4 == _y)
			{
				b = (sbyte)((float)MarchingCubes.DensityTerrain * num5);
			}
			else
			{
				b = (sbyte)((float)MarchingCubes.DensityAir * (1f - num5));
			}
			if (b == 0)
			{
				if (_bv.Block.shape.IsTerrain())
				{
					b = -1;
				}
				else
				{
					b = 1;
				}
			}
		}
		_chunk.SetDensity(_x, _y, _z, b);
	}

	// Token: 0x06005FF5 RID: 24565 RVA: 0x00261615 File Offset: 0x0025F815
	public override sbyte GetDensityAt(int _xWorld, int _yWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity)
	{
		if (!this.checkCoordinates(ref _xWorld, ref _zWorld))
		{
			return MarchingCubes.DensityAir;
		}
		if (this.heightMap.GetAt(_xWorld, _zWorld) > (float)_yWorld)
		{
			return MarchingCubes.DensityAir;
		}
		return MarchingCubes.DensityTerrain;
	}

	// Token: 0x06005FF6 RID: 24566 RVA: 0x00261645 File Offset: 0x0025F845
	public override byte GetTerrainHeightByteAt(int _x, int _z)
	{
		if (!this.checkCoordinates(ref _x, ref _z))
		{
			return 0;
		}
		return (byte)(this.heightMap.GetAt(_x, _z) + 0.5f);
	}

	// Token: 0x06005FF7 RID: 24567 RVA: 0x00261669 File Offset: 0x0025F869
	public override float GetTerrainHeightAt(int _x, int _z)
	{
		if (!this.checkCoordinates(ref _x, ref _z))
		{
			return 0f;
		}
		return this.heightMap.GetAt(_x, _z);
	}

	// Token: 0x06005FF8 RID: 24568 RVA: 0x0026168C File Offset: 0x0025F88C
	public List<float[,]> ConvertToUnityHeightmap(int _sliceAtWidth)
	{
		int width = this.heightMap.GetWidth();
		int height = this.heightMap.GetHeight();
		int scaleSteps = this.heightMap.GetScaleSteps();
		List<float[,]> list = new List<float[,]>();
		int num = width / _sliceAtWidth;
		int num2 = height / _sliceAtWidth;
		new Terrain[num, num2];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				float[,] array = new float[_sliceAtWidth, _sliceAtWidth];
				for (int k = _sliceAtWidth - 1; k >= 0; k--)
				{
					for (int l = _sliceAtWidth - 1; l >= 0; l--)
					{
						float at = this.heightMap.GetAt(j * _sliceAtWidth + k * scaleSteps, i * _sliceAtWidth + l * scaleSteps);
						array[l, k] = at / 256f;
					}
				}
				list.Add(array);
			}
		}
		return list;
	}

	// Token: 0x04004B5D RID: 19293
	[PublicizedFrom(EAccessModifier.Private)]
	public HeightMap heightMap;

	// Token: 0x04004B5E RID: 19294
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainWidth;

	// Token: 0x04004B5F RID: 19295
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainHeight;

	// Token: 0x04004B60 RID: 19296
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainWidthHalf;

	// Token: 0x04004B61 RID: 19297
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainHeightHalf;
}
