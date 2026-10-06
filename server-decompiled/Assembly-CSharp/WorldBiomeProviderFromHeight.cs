using System;
using System.Collections;

// Token: 0x02000C5C RID: 3164
public class WorldBiomeProviderFromHeight : IBiomeProvider
{
	// Token: 0x06005FDE RID: 24542 RVA: 0x002606AB File Offset: 0x0025E8AB
	public WorldBiomeProviderFromHeight(string _levelName, ITerrainGenerator _terrainGenerator, int _waterLevel, BiomeDefinition _waterBiome, BiomeDefinition _otherBiome)
	{
		this.worldName = _levelName;
		this.waterBiome = _waterBiome;
		this.otherBiome = _otherBiome;
		this.waterLevel = _waterLevel;
		this.terrainGenerator = _terrainGenerator;
	}

	// Token: 0x06005FDF RID: 24543 RVA: 0x002606D8 File Offset: 0x0025E8D8
	public string GetWorldName()
	{
		return this.worldName;
	}

	// Token: 0x06005FE0 RID: 24544 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetBiomeSizeFactor(float _factor)
	{
	}

	// Token: 0x06005FE1 RID: 24545 RVA: 0x002606E0 File Offset: 0x0025E8E0
	public IEnumerator InitData()
	{
		yield break;
	}

	// Token: 0x06005FE2 RID: 24546 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(int _seed, string _worldName, WorldBiomes _biomes, string _params1, string _params2)
	{
	}

	// Token: 0x06005FE3 RID: 24547 RVA: 0x0010087D File Offset: 0x000FEA7D
	public int GetSubBiomeIdxAt(BiomeDefinition bd, int _x, int _y, int _z)
	{
		return -1;
	}

	// Token: 0x06005FE4 RID: 24548 RVA: 0x002606E8 File Offset: 0x0025E8E8
	public BiomeDefinition GetBiomeAt(int x, int z, out float _intensity)
	{
		_intensity = 1f;
		return this.GetBiomeAt(x, z);
	}

	// Token: 0x06005FE5 RID: 24549 RVA: 0x002606F9 File Offset: 0x0025E8F9
	public BiomeDefinition GetBiomeAt(int x, int z)
	{
		if (this.terrainGenerator.GetTerrainHeightAt(x, z) <= (float)this.waterLevel)
		{
			return this.waterBiome;
		}
		return this.otherBiome;
	}

	// Token: 0x06005FE6 RID: 24550 RVA: 0x0026071E File Offset: 0x0025E91E
	public Vector2i GetSize()
	{
		return new Vector2i(1, 1);
	}

	// Token: 0x06005FE7 RID: 24551 RVA: 0x00260727 File Offset: 0x0025E927
	public BiomeIntensity GetBiomeIntensityAt(int _x, int _z)
	{
		return BiomeIntensity.Default;
	}

	// Token: 0x06005FE8 RID: 24552 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public float GetHumidityAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005FE9 RID: 24553 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public float GetTemperatureAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005FEA RID: 24554 RVA: 0x0003D3B2 File Offset: 0x0003B5B2
	public float GetRadiationAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005FEB RID: 24555 RVA: 0x001AA175 File Offset: 0x001A8375
	public BlockValue GetTopmostBlockValue(int xWorld, int zWorld)
	{
		return BlockValue.Air;
	}

	// Token: 0x04004B3F RID: 19263
	[PublicizedFrom(EAccessModifier.Private)]
	public string worldName;

	// Token: 0x04004B40 RID: 19264
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition waterBiome;

	// Token: 0x04004B41 RID: 19265
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition otherBiome;

	// Token: 0x04004B42 RID: 19266
	[PublicizedFrom(EAccessModifier.Private)]
	public ITerrainGenerator terrainGenerator;

	// Token: 0x04004B43 RID: 19267
	[PublicizedFrom(EAccessModifier.Private)]
	public int waterLevel;
}
