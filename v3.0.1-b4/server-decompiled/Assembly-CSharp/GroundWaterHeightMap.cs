using System;

// Token: 0x02000CAE RID: 3246
public class GroundWaterHeightMap
{
	// Token: 0x060063C4 RID: 25540 RVA: 0x002712F7 File Offset: 0x0026F4F7
	public GroundWaterHeightMap(World _world)
	{
		this.world = _world;
	}

	// Token: 0x060063C5 RID: 25541 RVA: 0x00271308 File Offset: 0x0026F508
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

	// Token: 0x060063C6 RID: 25542 RVA: 0x00271380 File Offset: 0x0026F580
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

	// Token: 0x060063C7 RID: 25543 RVA: 0x002713C0 File Offset: 0x0026F5C0
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

	// Token: 0x04004D73 RID: 19827
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04004D74 RID: 19828
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldGridCompressedData<byte> poiColors;

	// Token: 0x04004D75 RID: 19829
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBiomes biomes;
}
