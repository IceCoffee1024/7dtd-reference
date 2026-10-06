using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C5A RID: 3162
public class TerrainFromRaw : TerrainGeneratorWithBiomeResource
{
	// Token: 0x06005FCD RID: 24525 RVA: 0x0025FF84 File Offset: 0x0025E184
	public void Init(HeightMap _heightMap, IBiomeProvider _biomeProvider, int _seed)
	{
		base.Init(null, _biomeProvider, _seed);
		this.heightMap = _heightMap;
		this.terrainWidth = this.heightMap.GetWidth() << this.heightMap.GetScaleShift();
		this.terrainHeight = this.heightMap.GetHeight() << this.heightMap.GetScaleShift();
		this.terrainWidthHalf = this.terrainWidth / 2;
		this.terrainHeightHalf = this.terrainHeight / 2;
	}

	// Token: 0x06005FCE RID: 24526 RVA: 0x0025FFFD File Offset: 0x0025E1FD
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

	// Token: 0x06005FCF RID: 24527 RVA: 0x00260034 File Offset: 0x0025E234
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

	// Token: 0x06005FD0 RID: 24528 RVA: 0x002600ED File Offset: 0x0025E2ED
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

	// Token: 0x06005FD1 RID: 24529 RVA: 0x0026011D File Offset: 0x0025E31D
	public override byte GetTerrainHeightByteAt(int _x, int _z)
	{
		if (!this.checkCoordinates(ref _x, ref _z))
		{
			return 0;
		}
		return (byte)(this.heightMap.GetAt(_x, _z) + 0.5f);
	}

	// Token: 0x06005FD2 RID: 24530 RVA: 0x00260141 File Offset: 0x0025E341
	public override float GetTerrainHeightAt(int _x, int _z)
	{
		if (!this.checkCoordinates(ref _x, ref _z))
		{
			return 0f;
		}
		return this.heightMap.GetAt(_x, _z);
	}

	// Token: 0x06005FD3 RID: 24531 RVA: 0x00260164 File Offset: 0x0025E364
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

	// Token: 0x04004B38 RID: 19256
	[PublicizedFrom(EAccessModifier.Private)]
	public HeightMap heightMap;

	// Token: 0x04004B39 RID: 19257
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainWidth;

	// Token: 0x04004B3A RID: 19258
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainHeight;

	// Token: 0x04004B3B RID: 19259
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainWidthHalf;

	// Token: 0x04004B3C RID: 19260
	[PublicizedFrom(EAccessModifier.Private)]
	public int terrainHeightHalf;
}
