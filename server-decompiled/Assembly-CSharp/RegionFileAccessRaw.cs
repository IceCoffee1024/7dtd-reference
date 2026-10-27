using System;

// Token: 0x02000BAC RID: 2988
public class RegionFileAccessRaw : RegionFileAccessMultipleChunks
{
	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x060059D9 RID: 23001 RVA: 0x000816F5 File Offset: 0x0007F8F5
	public override int ChunksPerRegionPerDimension
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x060059DA RID: 23002 RVA: 0x00228C82 File Offset: 0x00226E82
	public override void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler)
	{
		this.ReadDirectory(_dir, _chunkAndTimeStampHandler, 8);
	}

	// Token: 0x060059DB RID: 23003 RVA: 0x00228C90 File Offset: 0x00226E90
	[PublicizedFrom(EAccessModifier.Protected)]
	public override RegionFile OpenRegionFile(string _dir, int _regionX, int _regionZ, string _ext)
	{
		string text = RegionFile.ConstructFullFilePath(_dir, _regionX, _regionZ, _ext);
		if (SdFile.Exists(text))
		{
			return RegionFileRaw.Load(text, _regionX, _regionZ);
		}
		return RegionFileRaw.New(text, _regionX, _regionZ, 1024);
	}

	// Token: 0x060059DC RID: 23004 RVA: 0x00228CC6 File Offset: 0x00226EC6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetRegionCoords(int _chunkX, int _chunkZ, out int _regionX, out int _regionZ)
	{
		_regionX = (int)Math.Floor((double)_chunkX / 8.0);
		_regionZ = (int)Math.Floor((double)_chunkZ / 8.0);
	}
}
