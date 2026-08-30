using System;
using System.Collections;

// Token: 0x02000C4A RID: 3146
public class WorldBiomeProviderFromHeight : IBiomeProvider
{
	// Token: 0x06005FC5 RID: 24517 RVA: 0x0025D3AF File Offset: 0x0025B5AF
	public WorldBiomeProviderFromHeight(string _levelName, ITerrainGenerator _terrainGenerator, int _waterLevel, BiomeDefinition _waterBiome, BiomeDefinition _otherBiome)
	{
		this.worldName = _levelName;
		this.waterBiome = _waterBiome;
		this.otherBiome = _otherBiome;
		this.waterLevel = _waterLevel;
		this.terrainGenerator = _terrainGenerator;
	}

	// Token: 0x06005FC6 RID: 24518 RVA: 0x0025D3DC File Offset: 0x0025B5DC
	public string GetWorldName()
	{
		return this.worldName;
	}

	// Token: 0x06005FC7 RID: 24519 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetBiomeSizeFactor(float _factor)
	{
	}

	// Token: 0x06005FC8 RID: 24520 RVA: 0x0025D3E4 File Offset: 0x0025B5E4
	public IEnumerator InitData()
	{
		yield break;
	}

	// Token: 0x06005FC9 RID: 24521 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(int _seed, string _worldName, WorldBiomes _biomes, string _params1, string _params2)
	{
	}

	// Token: 0x06005FCA RID: 24522 RVA: 0x000FFF9D File Offset: 0x000FE19D
	public int GetSubBiomeIdxAt(BiomeDefinition bd, int _x, int _y, int _z)
	{
		return -1;
	}

	// Token: 0x06005FCB RID: 24523 RVA: 0x0025D3EC File Offset: 0x0025B5EC
	public BiomeDefinition GetBiomeAt(int x, int z, out float _intensity)
	{
		_intensity = 1f;
		return this.GetBiomeAt(x, z);
	}

	// Token: 0x06005FCC RID: 24524 RVA: 0x0025D3FD File Offset: 0x0025B5FD
	public BiomeDefinition GetBiomeAt(int x, int z)
	{
		if (this.terrainGenerator.GetTerrainHeightAt(x, z) <= (float)this.waterLevel)
		{
			return this.waterBiome;
		}
		return this.otherBiome;
	}

	// Token: 0x06005FCD RID: 24525 RVA: 0x0025D422 File Offset: 0x0025B622
	public Vector2i GetSize()
	{
		return new Vector2i(1, 1);
	}

	// Token: 0x06005FCE RID: 24526 RVA: 0x0025D42B File Offset: 0x0025B62B
	public BiomeIntensity GetBiomeIntensityAt(int _x, int _z)
	{
		return BiomeIntensity.Default;
	}

	// Token: 0x06005FCF RID: 24527 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public float GetHumidityAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005FD0 RID: 24528 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public float GetTemperatureAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005FD1 RID: 24529 RVA: 0x0003D2C6 File Offset: 0x0003B4C6
	public float GetRadiationAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005FD2 RID: 24530 RVA: 0x001AA0C5 File Offset: 0x001A82C5
	public BlockValue GetTopmostBlockValue(int xWorld, int zWorld)
	{
		return BlockValue.Air;
	}

	// Token: 0x04004B00 RID: 19200
	[PublicizedFrom(EAccessModifier.Private)]
	public string worldName;

	// Token: 0x04004B01 RID: 19201
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition waterBiome;

	// Token: 0x04004B02 RID: 19202
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition otherBiome;

	// Token: 0x04004B03 RID: 19203
	[PublicizedFrom(EAccessModifier.Private)]
	public ITerrainGenerator terrainGenerator;

	// Token: 0x04004B04 RID: 19204
	[PublicizedFrom(EAccessModifier.Private)]
	public int waterLevel;
}
