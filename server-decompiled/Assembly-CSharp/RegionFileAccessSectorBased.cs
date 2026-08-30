using System;

// Token: 0x02000B90 RID: 2960
public class RegionFileAccessSectorBased : RegionFileAccessMultipleChunks
{
	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x0600599F RID: 22943 RVA: 0x00183C79 File Offset: 0x00181E79
	public override int ChunksPerRegionPerDimension
	{
		get
		{
			return 32;
		}
	}

	// Token: 0x060059A0 RID: 22944 RVA: 0x00224511 File Offset: 0x00222711
	public override void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler)
	{
		this.ReadDirectory(_dir, _chunkAndTimeStampHandler, 32);
	}

	// Token: 0x060059A1 RID: 22945 RVA: 0x0022451D File Offset: 0x0022271D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override RegionFile OpenRegionFile(string _dir, int _regionX, int _regionZ, string _ext)
	{
		return RegionFileSectorBased.Get(_dir, _regionX, _regionZ, _ext);
	}

	// Token: 0x060059A2 RID: 22946 RVA: 0x00224529 File Offset: 0x00222729
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetRegionCoords(int _chunkX, int _chunkZ, out int _regionX, out int _regionZ)
	{
		_regionX = (int)Math.Floor((double)_chunkX / 32.0);
		_regionZ = (int)Math.Floor((double)_chunkZ / 32.0);
	}
}
