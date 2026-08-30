using System;

// Token: 0x02000B7D RID: 2941
public class RegionFileAccessRaw : RegionFileAccessMultipleChunks
{
	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x06005912 RID: 22802 RVA: 0x00080B69 File Offset: 0x0007ED69
	public override int ChunksPerRegionPerDimension
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x06005913 RID: 22803 RVA: 0x0022177E File Offset: 0x0021F97E
	public override void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler)
	{
		this.ReadDirectory(_dir, _chunkAndTimeStampHandler, 8);
	}

	// Token: 0x06005914 RID: 22804 RVA: 0x0022178C File Offset: 0x0021F98C
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

	// Token: 0x06005915 RID: 22805 RVA: 0x002217C2 File Offset: 0x0021F9C2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetRegionCoords(int _chunkX, int _chunkZ, out int _regionX, out int _regionZ)
	{
		_regionX = (int)Math.Floor((double)_chunkX / 8.0);
		_regionZ = (int)Math.Floor((double)_chunkZ / 8.0);
	}
}
