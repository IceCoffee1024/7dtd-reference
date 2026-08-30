using System;

// Token: 0x02000B7E RID: 2942
public class RegionFileAccessSectorBased : RegionFileAccessMultipleChunks
{
	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x06005917 RID: 22807 RVA: 0x001818A1 File Offset: 0x0017FAA1
	public override int ChunksPerRegionPerDimension
	{
		get
		{
			return 32;
		}
	}

	// Token: 0x06005918 RID: 22808 RVA: 0x002217F5 File Offset: 0x0021F9F5
	public override void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler)
	{
		this.ReadDirectory(_dir, _chunkAndTimeStampHandler, 32);
	}

	// Token: 0x06005919 RID: 22809 RVA: 0x00221801 File Offset: 0x0021FA01
	[PublicizedFrom(EAccessModifier.Protected)]
	public override RegionFile OpenRegionFile(string _dir, int _regionX, int _regionZ, string _ext)
	{
		return RegionFileSectorBased.Get(_dir, _regionX, _regionZ, _ext);
	}

	// Token: 0x0600591A RID: 22810 RVA: 0x0022180D File Offset: 0x0021FA0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetRegionCoords(int _chunkX, int _chunkZ, out int _regionX, out int _regionZ)
	{
		_regionX = (int)Math.Floor((double)_chunkX / 32.0);
		_regionZ = (int)Math.Floor((double)_chunkZ / 32.0);
	}
}
