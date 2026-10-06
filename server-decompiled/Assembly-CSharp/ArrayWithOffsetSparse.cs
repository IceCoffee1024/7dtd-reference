using System;
using System.Collections.Generic;

// Token: 0x020013A4 RID: 5028
public class ArrayWithOffsetSparse<T>
{
	// Token: 0x06009E53 RID: 40531 RVA: 0x003BDDE1 File Offset: 0x003BBFE1
	public ArrayWithOffsetSparse(int _dimX, int _dimY, T _emptyValue)
	{
		this.DimX = _dimX;
		this.DimY = _dimY;
		this.EmptyValue = _emptyValue;
		this.myData = new Dictionary<long, T>();
		this.addX = _dimX / 2;
		this.addY = _dimY / 2;
	}

	// Token: 0x06009E54 RID: 40532 RVA: 0x003BDE1B File Offset: 0x003BC01B
	[PublicizedFrom(EAccessModifier.Private)]
	public long makeKey(int _x, int _y)
	{
		return (((long)(_y + this.addY) & (long)((ulong)(-1))) << 32) | ((long)(_x + this.addX) & (long)((ulong)(-1)));
	}

	// Token: 0x06009E55 RID: 40533 RVA: 0x003BDE39 File Offset: 0x003BC039
	public bool Contains(int _x, int _y)
	{
		return _x >= -this.addX && _y >= -this.addY && _x < this.addX - 1 && _y < this.addY - 1;
	}

	// Token: 0x170012E4 RID: 4836
	public virtual T this[int _x, int _y]
	{
		get
		{
			if (!this.myData.ContainsKey(this.makeKey(_x, _y)))
			{
				return this.EmptyValue;
			}
			return this.myData[this.makeKey(_x, _y)];
		}
		set
		{
			this.myData[this.makeKey(_x, _y)] = value;
		}
	}

	// Token: 0x04007831 RID: 30769
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, T> myData;

	// Token: 0x04007832 RID: 30770
	public T EmptyValue;

	// Token: 0x04007833 RID: 30771
	[PublicizedFrom(EAccessModifier.Private)]
	public int addX;

	// Token: 0x04007834 RID: 30772
	[PublicizedFrom(EAccessModifier.Private)]
	public int addY;

	// Token: 0x04007835 RID: 30773
	public int DimX;

	// Token: 0x04007836 RID: 30774
	public int DimY;
}
