using System;

// Token: 0x02000B8F RID: 2959
public class RegionFileAccessRaw : RegionFileAccessMultipleChunks
{
	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x0600599A RID: 22938 RVA: 0x0008121D File Offset: 0x0007F41D
	public override int ChunksPerRegionPerDimension
	{
		get
		{
			return 8;
		}
	}

	// Token: 0x0600599B RID: 22939 RVA: 0x0022449A File Offset: 0x0022269A
	public override void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler)
	{
		this.ReadDirectory(_dir, _chunkAndTimeStampHandler, 8);
	}

	// Token: 0x0600599C RID: 22940 RVA: 0x002244A8 File Offset: 0x002226A8
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

	// Token: 0x0600599D RID: 22941 RVA: 0x002244DE File Offset: 0x002226DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void GetRegionCoords(int _chunkX, int _chunkZ, out int _regionX, out int _regionZ)
	{
		_regionX = (int)Math.Floor((double)_chunkX / 8.0);
		_regionZ = (int)Math.Floor((double)_chunkZ / 8.0);
	}
}
