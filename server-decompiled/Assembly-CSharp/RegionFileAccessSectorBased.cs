using System;

// Token: 0x02000BAD RID: 2989
public class RegionFileAccessSectorBased : RegionFileAccessMultipleChunks
{
	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x060059DE RID: 23006 RVA: 0x00228CF9 File Offset: 0x00226EF9
	public override int ChunksPerRegionPerDimension
	{
		get
		{
			return 32;
		}
	}

	// Token: 0x060059DF RID: 23007 RVA: 0x00228CFD File Offset: 0x00226EFD
	public override void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler)
	{
		this.ReadDirectory(_dir, _chunkAndTimeStampHandler, 32);
	}

	// Token: 0x060059E0 RID: 23008 RVA: 0x00228D09 File Offset: 0x00226F09
	[PublicizedFrom(EAccessModifier.Protected)]
	public override RegionFile OpenRegionFile(string _dir, int _regionX, int _regionZ, string _ext)
	{
		return RegionFileSectorBased.Get(_dir, _regionX, _regionZ, _ext);
	}

	// Token: 0x060059E1 RID: 23009 RVA: 0x00228D15 File Offset: 0x00226F15
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetRegionCoords(int _chunkX, int _chunkZ, out int _regionX, out int _regionZ)
	{
		_regionX = (int)Math.Floor((double)_chunkX / 32.0);
		_regionZ = (int)Math.Floor((double)_chunkZ / 32.0);
	}
}
