using System;
using System.IO;

// Token: 0x02000BA6 RID: 2982
public abstract class RegionFileAccessAbstract
{
	// Token: 0x060059B1 RID: 22961 RVA: 0x0000640C File Offset: 0x0000460C
	public RegionFileAccessAbstract()
	{
	}

	// Token: 0x060059B2 RID: 22962 RVA: 0x002282FC File Offset: 0x002264FC
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

	// Token: 0x060059B3 RID: 22963 RVA: 0x00228381 File Offset: 0x00226581
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

	// Token: 0x060059B4 RID: 22964 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Close()
	{
	}

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x060059B5 RID: 22965
	public abstract int ChunksPerRegionPerDimension { get; }

	// Token: 0x060059B6 RID: 22966
	public abstract void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler);

	// Token: 0x060059B7 RID: 22967
	public abstract Stream GetOutputStream(string _dir, int _chunkX, int _chunkZ, string _ext);

	// Token: 0x060059B8 RID: 22968
	public abstract Stream GetInputStream(string _dir, int _chunkX, int _chunkZ, string _ext);

	// Token: 0x060059B9 RID: 22969
	public abstract void Remove(string _dir, int _chunkX, int _chunkZ);

	// Token: 0x060059BA RID: 22970 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OptimizeLayouts()
	{
	}

	// Token: 0x060059BB RID: 22971
	public abstract void ClearCache();

	// Token: 0x060059BC RID: 22972
	public abstract void RemoveRegionFromCache(int _regionX, int _regionZ, string _dir);

	// Token: 0x060059BD RID: 22973 RVA: 0x000882A0 File Offset: 0x000864A0
	public virtual int GetChunkByteCount(string _dir, int _chunkX, int _chunkZ)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060059BE RID: 22974 RVA: 0x000882A0 File Offset: 0x000864A0
	public virtual long GetTotalByteCount(string _dir)
	{
		throw new NotImplementedException();
	}
}
