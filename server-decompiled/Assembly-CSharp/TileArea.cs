using System;
using System.Collections.Generic;

// Token: 0x02000BEA RID: 3050
public class TileArea<T> : ITileArea<T> where T : class
{
	// Token: 0x06005B4F RID: 23375 RVA: 0x00234A70 File Offset: 0x00232C70
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

	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x06005B50 RID: 23376 RVA: 0x00234AFC File Offset: 0x00232CFC
	public TileAreaConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x06005B51 RID: 23377 RVA: 0x00234B04 File Offset: 0x00232D04
	public void Remove(uint _key)
	{
		this.Data.Remove(_key);
	}

	// Token: 0x170009A9 RID: 2473
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

	// Token: 0x170009AA RID: 2474
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

	// Token: 0x0400471A RID: 18202
	public TileAreaConfig config;

	// Token: 0x0400471B RID: 18203
	public Dictionary<uint, T> Data = new Dictionary<uint, T>();
}
