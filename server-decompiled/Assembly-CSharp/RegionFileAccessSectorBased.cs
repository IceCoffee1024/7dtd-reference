using System;

// Token: 0x02000BA2 RID: 2978
public class RegionFileAccessSectorBased : RegionFileAccessMultipleChunks
{
	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x060059B9 RID: 22969 RVA: 0x0022779D File Offset: 0x0022599D
	public override int ChunksPerRegionPerDimension
	{
		get
		{
			return 32;
		}
	}

	// Token: 0x060059BA RID: 22970 RVA: 0x002277A1 File Offset: 0x002259A1
	public override void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler)
	{
		this.ReadDirectory(_dir, _chunkAndTimeStampHandler, 32);
	}

	// Token: 0x060059BB RID: 22971 RVA: 0x002277AD File Offset: 0x002259AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override RegionFile OpenRegionFile(string _dir, int _regionX, int _regionZ, string _ext)
	{
		return RegionFileSectorBased.Get(_dir, _regionX, _regionZ, _ext);
	}

	// Token: 0x060059BC RID: 22972 RVA: 0x002277B9 File Offset: 0x002259B9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetRegionCoords(int _chunkX, int _chunkZ, out int _regionX, out int _regionZ)
	{
		_regionX = (int)Math.Floor((double)_chunkX / 32.0);
		_regionZ = (int)Math.Floor((double)_chunkZ / 32.0);
	}
}
