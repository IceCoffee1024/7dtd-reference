using System;
using System.IO;

// Token: 0x02000B89 RID: 2953
public abstract class RegionFileAccessAbstract
{
	// Token: 0x06005972 RID: 22898 RVA: 0x0000640C File Offset: 0x0000460C
	public RegionFileAccessAbstract()
	{
	}

	// Token: 0x06005973 RID: 22899 RVA: 0x00223B14 File Offset: 0x00221D14
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

	// Token: 0x06005974 RID: 22900 RVA: 0x00223B99 File Offset: 0x00221D99
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

	// Token: 0x06005975 RID: 22901 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Close()
	{
	}

	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x06005976 RID: 22902
	public abstract int ChunksPerRegionPerDimension { get; }

	// Token: 0x06005977 RID: 22903
	public abstract void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler);

	// Token: 0x06005978 RID: 22904
	public abstract Stream GetOutputStream(string _dir, int _chunkX, int _chunkZ, string _ext);

	// Token: 0x06005979 RID: 22905
	public abstract Stream GetInputStream(string _dir, int _chunkX, int _chunkZ, string _ext);

	// Token: 0x0600597A RID: 22906
	public abstract void Remove(string _dir, int _chunkX, int _chunkZ);

	// Token: 0x0600597B RID: 22907 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OptimizeLayouts()
	{
	}

	// Token: 0x0600597C RID: 22908
	public abstract void ClearCache();

	// Token: 0x0600597D RID: 22909
	public abstract void RemoveRegionFromCache(int _regionX, int _regionZ, string _dir);

	// Token: 0x0600597E RID: 22910 RVA: 0x00087DB8 File Offset: 0x00085FB8
	public virtual int GetChunkByteCount(string _dir, int _chunkX, int _chunkZ)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600597F RID: 22911 RVA: 0x00087DB8 File Offset: 0x00085FB8
	public virtual long GetTotalByteCount(string _dir)
	{
		throw new NotImplementedException();
	}
}
