using System;
using System.Collections.Generic;

// Token: 0x020013A1 RID: 5025
public class ArrayDynamicFast<T>
{
	// Token: 0x06009E3D RID: 40509 RVA: 0x003BD87F File Offset: 0x003BBA7F
	public ArrayDynamicFast(int _size)
	{
		this.Size = _size;
		this.Data = new T[_size];
		this.DataAvail = new bool[_size];
		this.Count = 0;
	}

	// Token: 0x06009E3E RID: 40510 RVA: 0x003BD8B0 File Offset: 0x003BBAB0
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

	// Token: 0x06009E3F RID: 40511 RVA: 0x003BD940 File Offset: 0x003BBB40
	public void Clear()
	{
		for (int i = 0; i < this.Data.Length; i++)
		{
			this.DataAvail[i] = false;
		}
	}

	// Token: 0x06009E40 RID: 40512 RVA: 0x003BD96C File Offset: 0x003BBB6C
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

	// Token: 0x04007821 RID: 30753
	public T[] Data;

	// Token: 0x04007822 RID: 30754
	public bool[] DataAvail;

	// Token: 0x04007823 RID: 30755
	public int Count;

	// Token: 0x04007824 RID: 30756
	public int Size;
}
