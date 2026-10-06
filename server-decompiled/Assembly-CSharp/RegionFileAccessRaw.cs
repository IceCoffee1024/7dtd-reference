using System;

// Token: 0x02000BA1 RID: 2977
public class RegionFileAccessRaw : RegionFileAccessMultipleChunks
{
	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x060059B4 RID: 22964 RVA: 0x000814D1 File Offset: 0x0007F6D1
	public override int ChunksPerRegionPerDimension
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x060059B5 RID: 22965 RVA: 0x00227726 File Offset: 0x00225926
	public override void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler)
	{
		this.ReadDirectory(_dir, _chunkAndTimeStampHandler, 8);
	}

	// Token: 0x060059B6 RID: 22966 RVA: 0x00227734 File Offset: 0x00225934
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

	// Token: 0x060059B7 RID: 22967 RVA: 0x0022776A File Offset: 0x0022596A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetRegionCoords(int _chunkX, int _chunkZ, out int _regionX, out int _regionZ)
	{
		_regionX = (int)Math.Floor((double)_chunkX / 8.0);
		_regionZ = (int)Math.Floor((double)_chunkZ / 8.0);
	}
}
