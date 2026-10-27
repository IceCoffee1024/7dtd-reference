using System;
using System.Collections;

// Token: 0x02000C67 RID: 3175
public class WorldBiomeProviderFromHeight : IBiomeProvider
{
	// Token: 0x06006003 RID: 24579 RVA: 0x00261BD3 File Offset: 0x0025FDD3
	public WorldBiomeProviderFromHeight(string _levelName, ITerrainGenerator _terrainGenerator, int _waterLevel, BiomeDefinition _waterBiome, BiomeDefinition _otherBiome)
	{
		this.worldName = _levelName;
		this.waterBiome = _waterBiome;
		this.otherBiome = _otherBiome;
		this.waterLevel = _waterLevel;
		this.terrainGenerator = _terrainGenerator;
	}

	// Token: 0x06006004 RID: 24580 RVA: 0x00261C00 File Offset: 0x0025FE00
	public string GetWorldName()
	{
		return this.worldName;
	}

	// Token: 0x06006005 RID: 24581 RVA: 0x000027FC File Offset: 0x000009FC
	public void SetBiomeSizeFactor(float _factor)
	{
	}

	// Token: 0x06006006 RID: 24582 RVA: 0x00261C08 File Offset: 0x0025FE08
	public IEnumerator InitData()
	{
		yield break;
	}

	// Token: 0x06006007 RID: 24583 RVA: 0x000027FC File Offset: 0x000009FC
	public void Init(int _seed, string _worldName, WorldBiomes _biomes, string _params1, string _params2)
	{
	}

	// Token: 0x06006008 RID: 24584 RVA: 0x00100DD1 File Offset: 0x000FEFD1
	public int GetSubBiomeIdxAt(BiomeDefinition bd, int _x, int _y, int _z)
	{
		return -1;
	}

	// Token: 0x06006009 RID: 24585 RVA: 0x00261C10 File Offset: 0x0025FE10
	public BiomeDefinition GetBiomeAt(int x, int z, out float _intensity)
	{
		_intensity = 1f;
		return this.GetBiomeAt(x, z);
	}

	// Token: 0x0600600A RID: 24586 RVA: 0x00261C21 File Offset: 0x0025FE21
	public BiomeDefinition GetBiomeAt(int x, int z)
	{
		if (this.terrainGenerator.GetTerrainHeightAt(x, z) <= (float)this.waterLevel)
		{
			return this.waterBiome;
		}
		return this.otherBiome;
	}

	// Token: 0x0600600B RID: 24587 RVA: 0x00261C46 File Offset: 0x0025FE46
	public Vector2i GetSize()
	{
		return new Vector2i(1, 1);
	}

	// Token: 0x0600600C RID: 24588 RVA: 0x00261C4F File Offset: 0x0025FE4F
	public BiomeIntensity GetBiomeIntensityAt(int _x, int _z)
	{
		return BiomeIntensity.Default;
	}

	// Token: 0x0600600D RID: 24589 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetHumidityAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x0600600E RID: 24590 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetTemperatureAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x0600600F RID: 24591 RVA: 0x0003D372 File Offset: 0x0003B572
	public float GetRadiationAt(int x, int z)
	{
		return 0f;
	}

	// Token: 0x06006010 RID: 24592 RVA: 0x001AB1F9 File Offset: 0x001A93F9
	public BlockValue GetTopmostBlockValue(int xWorld, int zWorld)
	{
		return BlockValue.Air;
	}

	// Token: 0x04004B64 RID: 19300
	[PublicizedFrom(EAccessModifier.Private)]
	public string worldName;

	// Token: 0x04004B65 RID: 19301
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition waterBiome;

	// Token: 0x04004B66 RID: 19302
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition otherBiome;

	// Token: 0x04004B67 RID: 19303
	[PublicizedFrom(EAccessModifier.Private)]
	public ITerrainGenerator terrainGenerator;

	// Token: 0x04004B68 RID: 19304
	[PublicizedFrom(EAccessModifier.Private)]
	public int waterLevel;
}
