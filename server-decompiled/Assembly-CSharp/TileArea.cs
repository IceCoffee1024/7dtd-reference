using System;
using System.Collections.Generic;

// Token: 0x02000BDF RID: 3039
public class TileArea<T> : ITileArea<T> where T : class
{
	// Token: 0x06005B2A RID: 23338 RVA: 0x00233514 File Offset: 0x00231714
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

	// Token: 0x170009A2 RID: 2466
	// (get) Token: 0x06005B2B RID: 23339 RVA: 0x002335A0 File Offset: 0x002317A0
	public TileAreaConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x06005B2C RID: 23340 RVA: 0x002335A8 File Offset: 0x002317A8
	public void Remove(uint _key)
	{
		this.Data.Remove(_key);
	}

	// Token: 0x170009A3 RID: 2467
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

	// Token: 0x170009A4 RID: 2468
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

	// Token: 0x040046F5 RID: 18165
	public TileAreaConfig config;

	// Token: 0x040046F6 RID: 18166
	public Dictionary<uint, T> Data = new Dictionary<uint, T>();
}
