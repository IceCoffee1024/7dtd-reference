using System;
using System.Collections.Generic;

// Token: 0x020013B3 RID: 5043
public class ArrayDynamicFast<T>
{
	// Token: 0x06009E95 RID: 40597 RVA: 0x003BFC83 File Offset: 0x003BDE83
	public ArrayDynamicFast(int _size)
	{
		this.Size = _size;
		this.Data = new T[_size];
		this.DataAvail = new bool[_size];
		this.Count = 0;
	}

	// Token: 0x06009E96 RID: 40598 RVA: 0x003BFCB4 File Offset: 0x003BDEB4
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

	// Token: 0x06009E97 RID: 40599 RVA: 0x003BFD44 File Offset: 0x003BDF44
	public void Clear()
	{
		for (int i = 0; i < this.Data.Length; i++)
		{
			this.DataAvail[i] = false;
		}
	}

	// Token: 0x06009E98 RID: 40600 RVA: 0x003BFD70 File Offset: 0x003BDF70
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

	// Token: 0x04007862 RID: 30818
	public T[] Data;

	// Token: 0x04007863 RID: 30819
	public bool[] DataAvail;

	// Token: 0x04007864 RID: 30820
	public int Count;

	// Token: 0x04007865 RID: 30821
	public int Size;
}
