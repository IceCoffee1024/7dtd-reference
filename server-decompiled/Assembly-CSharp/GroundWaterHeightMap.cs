using System;

// Token: 0x02000CCE RID: 3278
public class GroundWaterHeightMap
{
	// Token: 0x06006443 RID: 25667 RVA: 0x002775F7 File Offset: 0x002757F7
	public GroundWaterHeightMap(World _world)
	{
		this.world = _world;
	}

	// Token: 0x06006444 RID: 25668 RVA: 0x00277608 File Offset: 0x00275808
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

	// Token: 0x06006445 RID: 25669 RVA: 0x00277680 File Offset: 0x00275880
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

	// Token: 0x06006446 RID: 25670 RVA: 0x002776C0 File Offset: 0x002758C0
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

	// Token: 0x04004E47 RID: 20039
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04004E48 RID: 20040
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldGridCompressedData<byte> poiColors;

	// Token: 0x04004E49 RID: 20041
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBiomes biomes;
}
