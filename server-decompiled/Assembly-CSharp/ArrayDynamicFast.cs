using System;
using System.Collections.Generic;

// Token: 0x02001381 RID: 4993
public class ArrayDynamicFast<T>
{
	// Token: 0x06009D92 RID: 40338 RVA: 0x003B8D03 File Offset: 0x003B6F03
	public ArrayDynamicFast(int _size)
	{
		this.Size = _size;
		this.Data = new T[_size];
		this.DataAvail = new bool[_size];
		this.Count = 0;
	}

	// Token: 0x06009D93 RID: 40339 RVA: 0x003B8D34 File Offset: 0x003B6F34
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

	// Token: 0x06009D94 RID: 40340 RVA: 0x003B8DC4 File Offset: 0x003B6FC4
	public void Clear()
	{
		for (int i = 0; i < this.Data.Length; i++)
		{
			this.DataAvail[i] = false;
		}
	}

	// Token: 0x06009D95 RID: 40341 RVA: 0x003B8DF0 File Offset: 0x003B6FF0
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

	// Token: 0x040077A6 RID: 30630
	public T[] Data;

	// Token: 0x040077A7 RID: 30631
	public bool[] DataAvail;

	// Token: 0x040077A8 RID: 30632
	public int Count;

	// Token: 0x040077A9 RID: 30633
	public int Size;
}
