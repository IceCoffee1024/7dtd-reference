using System;

// Token: 0x02000CD9 RID: 3289
public class GroundWaterHeightMap
{
	// Token: 0x06006468 RID: 25704 RVA: 0x00278B1F File Offset: 0x00276D1F
	public GroundWaterHeightMap(World _world)
	{
		this.world = _world;
	}

	// Token: 0x06006469 RID: 25705 RVA: 0x00278B30 File Offset: 0x00276D30
	public bool TryInit()
	{
		if (this.poiColors != null && this.biomes != null)
		{
			return true;
		}
		ChunkProviderGenerateWorldFromRaw chunkProviderGenerateWorldFromRaw = this.world.ChunkCache.ChunkProvider as ChunkProviderGenerateWorldFromRaw;
		if (chunkProviderGenerateWorldFromRaw == null)
		{
			return false;
		}
		WorldDecoratorPOIFromImage poiFromImage = chunkProviderGenerateWorldFromRaw.poiFromImage;
		if (poiFromImage == null)
		{
			return false;
		}
		this.poiColors = poiFromImage.m_Poi;
		this.biomes = this.world.Biomes;
		return this.poiColors != null && this.biomes != null;
	}

	// Token: 0x0600646A RID: 25706 RVA: 0x00278BA8 File Offset: 0x00276DA8
	[PublicizedFrom(EAccessModifier.Private)]
	public PoiMapElement GetPoiMapElement(int _worldX, int _worldZ)
	{
		if (!this.poiColors.Contains(_worldX, _worldZ))
		{
			return null;
		}
		byte data = this.poiColors.GetData(_worldX, _worldZ);
		if (data == 0)
		{
			return null;
		}
		return this.biomes.getPoiForColor((uint)data);
	}

	// Token: 0x0600646B RID: 25707 RVA: 0x00278BE8 File Offset: 0x00276DE8
	public bool TryGetWaterHeightAt(int _worldX, int _worldZ, out int _height)
	{
		PoiMapElement poiMapElement = this.GetPoiMapElement(_worldX, _worldZ);
		if (poiMapElement == null)
		{
			_height = 0;
			return false;
		}
		if (poiMapElement.m_BlockValue.type != 240)
		{
			_height = 0;
			return false;
		}
		_height = poiMapElement.m_YPosFill;
		return true;
	}

	// Token: 0x04004E6C RID: 20076
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04004E6D RID: 20077
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldGridCompressedData<byte> poiColors;

	// Token: 0x04004E6E RID: 20078
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBiomes biomes;
}
