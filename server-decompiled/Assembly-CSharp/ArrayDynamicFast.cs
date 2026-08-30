using System;
using System.Collections.Generic;

// Token: 0x02001367 RID: 4967
public class ArrayDynamicFast<T>
{
	// Token: 0x06009C97 RID: 40087 RVA: 0x003B3A4F File Offset: 0x003B1C4F
	public ArrayDynamicFast(int _size)
	{
		this.Size = _size;
		this.Data = new T[_size];
		this.DataAvail = new bool[_size];
		this.Count = 0;
	}

	// Token: 0x06009C98 RID: 40088 RVA: 0x003B3A80 File Offset: 0x003B1C80
	public int Contains(T _v)
	{
		if (this.Count == 0)
		{
			return -1;
		}
		if (_v == null)
		{
			for (int i = 0; i < this.Data.Length; i++)
			{
				if (this.DataAvail[i] && this.Data[i] == null)
				{
					return i;
				}
			}
		}
		else
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int j = 0; j < this.Data.Length; j++)
			{
				if (this.DataAvail[j] && @default.Equals(this.Data[j], _v))
				{
					return j;
				}
			}
		}
		return -1;
	}

	// Token: 0x06009C99 RID: 40089 RVA: 0x003B3B10 File Offset: 0x003B1D10
	public void Clear()
	{
		for (int i = 0; i < this.Data.Length; i++)
		{
			this.DataAvail[i] = false;
		}
	}

	// Token: 0x06009C9A RID: 40090 RVA: 0x003B3B3C File Offset: 0x003B1D3C
	public void Add(int _idx, T _texId)
	{
		if (_idx == -1)
		{
			for (int i = 0; i < this.Size; i++)
			{
				if (!this.DataAvail[i])
				{
					_idx = i;
					break;
				}
			}
		}
		if (_idx == -1)
		{
			return;
		}
		this.Data[_idx] = _texId;
		this.DataAvail[_idx] = true;
		this.Count++;
	}

	// Token: 0x040076CE RID: 30414
	public T[] Data;

	// Token: 0x040076CF RID: 30415
	public bool[] DataAvail;

	// Token: 0x040076D0 RID: 30416
	public int Count;

	// Token: 0x040076D1 RID: 30417
	public int Size;
}
