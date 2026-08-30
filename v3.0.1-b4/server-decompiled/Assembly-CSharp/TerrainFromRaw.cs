using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C36 RID: 3126
public class TerrainFromRaw : TerrainGeneratorWithBiomeResource
{
	// Token: 0x06005F2A RID: 24362 RVA: 0x00259DD8 File Offset: 0x00257FD8
	public void Init(HeightMap _heightMap, IBiomeProvider _biomeProvider, int _seed)
	{
		base.Init(null, _biomeProvider, _seed);
		this.heightMap = _heightMap;
		this.terrainWidth = this.heightMap.GetWidth() << this.heightMap.GetScaleShift();
		this.terrainHeight = this.heightMap.GetHeight() << this.heightMap.GetScaleShift();
		this.terrainWidthHalf = this.terrainWidth / 2;
		this.terrainHeightHalf = this.terrainHeight / 2;
	}

	// Token: 0x06005F2B RID: 24363 RVA: 0x00259E51 File Offset: 0x00258051
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

	// Token: 0x06005F2C RID: 24364 RVA: 0x00259E88 File Offset: 0x00258088
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

	// Token: 0x06005F2D RID: 24365 RVA: 0x00259F41 File Offset: 0x00258141
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

	// Token: 0x06005F2E RID: 24366 RVA: 0x00259F71 File Offset: 0x00258171
	public override byte GetTerrainHeightByteAt(int _x, int _z)
	{
		if (!this.checkCoordinates(ref _x, ref _z))
		{
			return 0;
		}
		return (byte)(this.heightMap.GetAt(_x, _z) + 0.5f);
	}

	// Token: 0x06005F2F RID: 24367 RVA: 0x00259F95 File Offset: 0x00258195
	public override float GetTerrainHeightAt(int _x, int _z)
	{
		if (!this.checkCoordinates(ref _x, ref _z))
		{
			return 0f;
		}
		return this.heightMap.GetAt(_x, _z);
	}

	// Token: 0x06005F30 RID: 24368 RVA: 0x00259FB8 File Offset: 0x002581B8
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

	// Token: 0x04004A5C RID: 19036
	[PublicizedFrom(EAccessModifier.Private)]
	public HeightMap heightMap;

	// Token: 0x04004A5D RID: 19037
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainWidth;

	// Token: 0x04004A5E RID: 19038
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainHeight;

	// Token: 0x04004A5F RID: 19039
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainWidthHalf;

	// Token: 0x04004A60 RID: 19040
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainHeightHalf;
}
