using System;
using System.Collections.Generic;

// Token: 0x02000BBB RID: 3003
public class TileArea<T> : ITileArea<T> where T : class
{
	// Token: 0x06005A89 RID: 23177 RVA: 0x0022D4B0 File Offset: 0x0022B6B0
	public TileArea(TileAreaConfig _config, T[,] _data = null)
	{
		this.config = _config;
		if (_data != null)
		{
			for (int i = 0; i < _data.GetLength(0); i++)
			{
				for (int j = 0; j < _data.GetLength(1); j++)
				{
					int num = i + this.config.tileStart.x;
					int num2 = j + this.config.tileStart.y;
					uint num3 = TileAreaUtils.MakeKey(num, num2);
					this.Data[num3] = _data[i, j];
				}
			}
		}
	}

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06005A8A RID: 23178 RVA: 0x0022D53C File Offset: 0x0022B73C
	public TileAreaConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x06005A8B RID: 23179 RVA: 0x0022D544 File Offset: 0x0022B744
	public void Remove(uint _key)
	{
		this.Data.Remove(_key);
	}

	// Token: 0x1700096F RID: 2415
	public T this[int _tileX, int _tileZ]
	{
		get
		{
			this.config.checkCoordinates(ref _tileX, ref _tileZ);
			uint num = TileAreaUtils.MakeKey(_tileX, _tileZ);
			T t;
			if (!this.Data.TryGetValue(num, out t))
			{
				return default(T);
			}
			return t;
		}
		set
		{
			this.config.checkCoordinates(ref _tileX, ref _tileZ);
			uint num = TileAreaUtils.MakeKey(_tileX, _tileZ);
			this.Data[num] = value;
		}
	}

	// Token: 0x17000970 RID: 2416
	public T this[uint _key]
	{
		get
		{
			T t;
			if (!this.Data.TryGetValue(_key, out t))
			{
				return default(T);
			}
			return t;
		}
		set
		{
			this.Data[_key] = value;
		}
	}

	// Token: 0x0400461A RID: 17946
	public TileAreaConfig config;

	// Token: 0x0400461B RID: 17947
	public Dictionary<uint, T> Data = new Dictionary<uint, T>();
}
