using System;

// Token: 0x02000CC0 RID: 3264
public class GroundWaterHeightMap
{
	// Token: 0x06006450 RID: 25680 RVA: 0x002741F3 File Offset: 0x002723F3
	public GroundWaterHeightMap(World _world)
	{
		this.world = _world;
	}

	// Token: 0x06006451 RID: 25681 RVA: 0x00274204 File Offset: 0x00272404
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

	// Token: 0x06006452 RID: 25682 RVA: 0x0027427C File Offset: 0x0027247C
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

	// Token: 0x06006453 RID: 25683 RVA: 0x002742BC File Offset: 0x002724BC
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

	// Token: 0x04004E11 RID: 19985
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04004E12 RID: 19986
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldGridCompressedData<byte> poiColors;

	// Token: 0x04004E13 RID: 19987
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBiomes biomes;
}
