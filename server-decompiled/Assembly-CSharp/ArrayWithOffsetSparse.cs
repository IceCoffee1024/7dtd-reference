using System;
using System.Collections.Generic;

// Token: 0x02001384 RID: 4996
public class ArrayWithOffsetSparse<T>
{
	// Token: 0x06009DA8 RID: 40360 RVA: 0x003B9265 File Offset: 0x003B7465
	public ArrayWithOffsetSparse(int _dimX, int _dimY, T _emptyValue)
	{
		this.DimX = _dimX;
		this.DimY = _dimY;
		this.EmptyValue = _emptyValue;
		this.myData = new Dictionary<long, T>();
		this.addX = _dimX / 2;
		this.addY = _dimY / 2;
	}

	// Token: 0x06009DA9 RID: 40361 RVA: 0x003B929F File Offset: 0x003B749F
	[PublicizedFrom(EAccessModifier.Private)]
	public long makeKey(int _x, int _y)
	{
		return (((long)(_y + this.addY) & (long)((ulong)(-1))) << 32) | ((long)(_x + this.addX) & (long)((ulong)(-1)));
	}

	// Token: 0x06009DAA RID: 40362 RVA: 0x003B92BD File Offset: 0x003B74BD
	public bool Contains(int _x, int _y)
	{
		return _x >= -this.addX && _y >= -this.addY && _x < this.addX - 1 && _y < this.addY - 1;
	}

	// Token: 0x1700129F RID: 4767
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

	// Token: 0x040077B6 RID: 30646
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, T> myData;

	// Token: 0x040077B7 RID: 30647
	public T EmptyValue;

	// Token: 0x040077B8 RID: 30648
	[PublicizedFrom(EAccessModifier.Private)]
	public int addX;

	// Token: 0x040077B9 RID: 30649
	[PublicizedFrom(EAccessModifier.Private)]
	public int addY;

	// Token: 0x040077BA RID: 30650
	public int DimX;

	// Token: 0x040077BB RID: 30651
	public int DimY;
}
