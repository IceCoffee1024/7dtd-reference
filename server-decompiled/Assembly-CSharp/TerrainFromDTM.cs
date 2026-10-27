using System;

// Token: 0x02000C64 RID: 3172
[PublicizedFrom(EAccessModifier.Internal)]
public class TerrainFromDTM : TerrainGeneratorWithBiomeResource
{
	// Token: 0x06005FEC RID: 24556 RVA: 0x002612A0 File Offset: 0x0025F4A0
	public void Init(ArrayWithOffset<byte> _dtm, IBiomeProvider _biomeProvider, string _levelName, string _worldPath, int _seed)
	{
		base.Init(null, _biomeProvider, _seed);
		this.m_DTM = _dtm;
		if (!HeightMapUtils.TryGetHeightDataFromResources(_levelName, out this.heightData))
		{
			this.heightData = HeightMapUtils.GetHeightDataFromWorld(_worldPath, true);
		}
		this.heightData = HeightMapUtils.SmoothTerrain(5, this.heightData);
	}

	// Token: 0x06005FED RID: 24557 RVA: 0x002612EC File Offset: 0x0025F4EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void fillDensityInBlock(Chunk _chunk, int _x, int _y, int _z, BlockValue _bv)
	{
		int num = (_chunk.X << 4) + _x + this.heightData.GetLength(0) / 2;
		int num2 = (_chunk.Z << 4) + _z + this.heightData.GetLength(1) / 2;
		if (num < 0 || num2 < 0 || num >= this.heightData.GetLength(0) || num2 >= this.heightData.GetLength(1))
		{
			return;
		}
		float num3 = this.heightData[num, num2] + 0.5f;
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
		}
		_chunk.SetDensity(_x, _y, _z, b);
	}

	// Token: 0x06005FEE RID: 24558 RVA: 0x002613BE File Offset: 0x0025F5BE
	public override sbyte GetDensityAt(int _xWorld, int _yWorld, int _zWorld, BiomeDefinition _bd, float _biomeIntensity)
	{
		if ((int)this.m_DTM[_xWorld, _zWorld] > _yWorld)
		{
			return MarchingCubes.DensityAir;
		}
		return MarchingCubes.DensityTerrain;
	}

	// Token: 0x06005FEF RID: 24559 RVA: 0x002613DC File Offset: 0x0025F5DC
	public override byte GetTerrainHeightByteAt(int _x, int _z)
	{
		int num = _x + this.heightData.GetLength(0) / 2;
		int num2 = _z + this.heightData.GetLength(1) / 2;
		if (num < 0 || num2 < 0 || num >= this.heightData.GetLength(0) || num2 >= this.heightData.GetLength(1))
		{
			return 0;
		}
		return (byte)((int)(this.heightData[num, num2] + 0.5f));
	}

	// Token: 0x06005FF0 RID: 24560 RVA: 0x00261448 File Offset: 0x0025F648
	public override float GetTerrainHeightAt(int x, int z)
	{
		float num;
		try
		{
			x += this.heightData.GetLength(0) / 2;
			z += this.heightData.GetLength(1) / 2;
			num = this.heightData[x, z];
		}
		catch (Exception)
		{
			num = 0f;
		}
		return num;
	}

	// Token: 0x04004B5B RID: 19291
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayWithOffset<byte> m_DTM;

	// Token: 0x04004B5C RID: 19292
	[PublicizedFrom(EAccessModifier.Private)]
	public float[,] heightData;
}
