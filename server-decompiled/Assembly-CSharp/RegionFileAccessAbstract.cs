using System;
using System.IO;

// Token: 0x02000B9B RID: 2971
public abstract class RegionFileAccessAbstract
{
	// Token: 0x0600598C RID: 22924 RVA: 0x0000640C File Offset: 0x0000460C
	public RegionFileAccessAbstract()
	{
	}

	// Token: 0x0600598D RID: 22925 RVA: 0x00226DA0 File Offset: 0x00224FA0
	public static bool ExtractKey(string _filename, out long _key)
	{
		_key = 0L;
		if (_filename.Length > 0 && _filename.StartsWith("r.") && _filename.EndsWith(".ttc"))
		{
			string text = GameIO.RemoveExtension(_filename.Substring(2), ".ttc");
			int num = text.IndexOf(".");
			int num2;
			int num3;
			if (num > 0 && int.TryParse(text.Substring(0, num), out num2) && int.TryParse(text.Substring(num + 1), out num3))
			{
				_key = WorldChunkCache.MakeChunkKey(num2, num3);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600598E RID: 22926 RVA: 0x00226E25 File Offset: 0x00225025
	public static string MakeFilename(int _x, int _z)
	{
		return string.Concat(new string[]
		{
			"r.",
			_x.ToString(),
			".",
			_z.ToString(),
			".ttc"
		});
	}

	// Token: 0x0600598F RID: 22927 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Close()
	{
	}

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x06005990 RID: 22928
	public abstract int ChunksPerRegionPerDimension { get; }

	// Token: 0x06005991 RID: 22929
	public abstract void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler);

	// Token: 0x06005992 RID: 22930
	public abstract Stream GetOutputStream(string _dir, int _chunkX, int _chunkZ, string _ext);

	// Token: 0x06005993 RID: 22931
	public abstract Stream GetInputStream(string _dir, int _chunkX, int _chunkZ, string _ext);

	// Token: 0x06005994 RID: 22932
	public abstract void Remove(string _dir, int _chunkX, int _chunkZ);

	// Token: 0x06005995 RID: 22933 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OptimizeLayouts()
	{
	}

	// Token: 0x06005996 RID: 22934
	public abstract void ClearCache();

	// Token: 0x06005997 RID: 22935
	public abstract void RemoveRegionFromCache(int _regionX, int _regionZ, string _dir);

	// Token: 0x06005998 RID: 22936 RVA: 0x0008806C File Offset: 0x0008626C
	public virtual int GetChunkByteCount(string _dir, int _chunkX, int _chunkZ)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005999 RID: 22937 RVA: 0x0008806C File Offset: 0x0008626C
	public virtual long GetTotalByteCount(string _dir)
	{
		throw new NotImplementedException();
	}
}
