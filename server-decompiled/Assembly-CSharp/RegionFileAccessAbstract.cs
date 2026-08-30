using System;
using System.IO;

// Token: 0x02000B77 RID: 2935
public abstract class RegionFileAccessAbstract
{
	// Token: 0x060058EA RID: 22762 RVA: 0x0000640C File Offset: 0x0000460C
	public RegionFileAccessAbstract()
	{
	}

	// Token: 0x060058EB RID: 22763 RVA: 0x00220DF8 File Offset: 0x0021EFF8
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

	// Token: 0x060058EC RID: 22764 RVA: 0x00220E7D File Offset: 0x0021F07D
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

	// Token: 0x060058ED RID: 22765 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Close()
	{
	}

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x060058EE RID: 22766
	public abstract int ChunksPerRegionPerDimension { get; }

	// Token: 0x060058EF RID: 22767
	public abstract void ReadDirectory(string _dir, Action<long, string, uint> _chunkAndTimeStampHandler);

	// Token: 0x060058F0 RID: 22768
	public abstract Stream GetOutputStream(string _dir, int _chunkX, int _chunkZ, string _ext);

	// Token: 0x060058F1 RID: 22769
	public abstract Stream GetInputStream(string _dir, int _chunkX, int _chunkZ, string _ext);

	// Token: 0x060058F2 RID: 22770
	public abstract void Remove(string _dir, int _chunkX, int _chunkZ);

	// Token: 0x060058F3 RID: 22771 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OptimizeLayouts()
	{
	}

	// Token: 0x060058F4 RID: 22772
	public abstract void ClearCache();

	// Token: 0x060058F5 RID: 22773
	public abstract void RemoveRegionFromCache(int _regionX, int _regionZ, string _dir);

	// Token: 0x060058F6 RID: 22774 RVA: 0x000876FD File Offset: 0x000858FD
	public virtual int GetChunkByteCount(string _dir, int _chunkX, int _chunkZ)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060058F7 RID: 22775 RVA: 0x000876FD File Offset: 0x000858FD
	public virtual long GetTotalByteCount(string _dir)
	{
		throw new NotImplementedException();
	}
}
