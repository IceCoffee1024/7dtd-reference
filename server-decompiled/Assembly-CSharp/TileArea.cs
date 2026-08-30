using System;
using System.Collections.Generic;

// Token: 0x02000BCD RID: 3021
public class TileArea<T> : ITileArea<T> where T : class
{
	// Token: 0x06005B11 RID: 23313 RVA: 0x00230218 File Offset: 0x0022E418
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

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06005B12 RID: 23314 RVA: 0x002302A4 File Offset: 0x0022E4A4
	public TileAreaConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x002302AC File Offset: 0x0022E4AC
	public void Remove(uint _key)
	{
		this.Data.Remove(_key);
	}

	// Token: 0x17000981 RID: 2433
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

	// Token: 0x17000982 RID: 2434
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

	// Token: 0x040046B6 RID: 18102
	public TileAreaConfig config;

	// Token: 0x040046B7 RID: 18103
	public Dictionary<uint, T> Data = new Dictionary<uint, T>();
}
