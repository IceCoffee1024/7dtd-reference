using System;
using System.Collections;

// Token: 0x02000C38 RID: 3128
public class WorldBiomeProviderFromHeight : IBiomeProvider
{
	// Token: 0x06005F3B RID: 24379 RVA: 0x0025A4FF File Offset: 0x002586FF
	public WorldBiomeProviderFromHeight(string _levelName, ITerrainGenerator _terrainGenerator, int _waterLevel, BiomeDefinition _waterBiome, BiomeDefinition _otherBiome)
	{
		this.worldName = _levelName;
		this.waterBiome = _waterBiome;
		this.otherBiome = _otherBiome;
		this.waterLevel = _waterLevel;
		this.terrainGenerator = _terrainGenerator;
	}

	// Token: 0x06005F3C RID: 24380 RVA: 0x0025A52C File Offset: 0x0025872C
	public string GetWorldName()
	{
		return this.worldName;
	}

	// Token: 0x06005F3D RID: 24381 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetBiomeSizeFactor(float _factor)
	{
	}

	// Token: 0x06005F3E RID: 24382 RVA: 0x0025A534 File Offset: 0x00258734
	public IEnumerator InitData()
	{
		yield break;
	}

	// Token: 0x06005F3F RID: 24383 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(int _seed, string _worldName, WorldBiomes _biomes, string _params1, string _params2)
	{
	}

	// Token: 0x06005F40 RID: 24384 RVA: 0x000FEFA9 File Offset: 0x000FD1A9
	public int GetSubBiomeIdxAt(BiomeDefinition bd, int _x, int _y, int _z)
	{
		return -1;
	}

	// Token: 0x06005F41 RID: 24385 RVA: 0x0025A53C File Offset: 0x0025873C
	public BiomeDefinition GetBiomeAt(int x, int z, out float _intensity)
	{
		_intensity = 1f;
		return this.GetBiomeAt(x, z);
	}

	// Token: 0x06005F42 RID: 24386 RVA: 0x0025A54D File Offset: 0x0025874D
	public BiomeDefinition GetBiomeAt(int x, int z)
	{
		if (this.terrainGenerator.GetTerrainHeightAt(x, z) <= (float)this.waterLevel)
		{
			return this.waterBiome;
		}
		return this.otherBiome;
	}

	// Token: 0x06005F43 RID: 24387 RVA: 0x0025A572 File Offset: 0x00258772
	public Vector2i GetSize()
	{
		return new Vector2i(1, 1);
	}

	// Token: 0x06005F44 RID: 24388 RVA: 0x0025A57B File Offset: 0x0025877B
	public BiomeIntensity GetBiomeIntensityAt(int _x, int _z)
	{
		return BiomeIntensity.Default;
	}

	// Token: 0x06005F45 RID: 24389 RVA: 0x0003D066 File Offset: 0x0003B266
	public float GetHumidityAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005F46 RID: 24390 RVA: 0x0003D066 File Offset: 0x0003B266
	public float GetTemperatureAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005F47 RID: 24391 RVA: 0x0003D066 File Offset: 0x0003B266
	public float GetRadiationAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x001A7AE9 File Offset: 0x001A5CE9
	public BlockValue GetTopmostBlockValue(int xWorld, int zWorld)
	{
		return BlockValue.Air;
	}

	// Token: 0x04004A63 RID: 19043
	[PublicizedFrom(EAccessModifier.Private)]
	public string worldName;

	// Token: 0x04004A64 RID: 19044
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition waterBiome;

	// Token: 0x04004A65 RID: 19045
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition otherBiome;

	// Token: 0x04004A66 RID: 19046
	[PublicizedFrom(EAccessModifier.Private)]
	public ITerrainGenerator terrainGenerator;

	// Token: 0x04004A67 RID: 19047
	[PublicizedFrom(EAccessModifier.Private)]
	public int waterLevel;
}
