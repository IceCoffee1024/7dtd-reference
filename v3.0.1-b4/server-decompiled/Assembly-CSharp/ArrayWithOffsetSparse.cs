using System;
using System.Collections.Generic;

// Token: 0x0200136A RID: 4970
public class ArrayWithOffsetSparse<T>
{
	// Token: 0x06009CAD RID: 40109 RVA: 0x003B3FB1 File Offset: 0x003B21B1
	public ArrayWithOffsetSparse(int _dimX, int _dimY, T _emptyValue)
	{
		this.DimX = _dimX;
		this.DimY = _dimY;
		this.EmptyValue = _emptyValue;
		this.myData = new Dictionary<long, T>();
		this.addX = _dimX / 2;
		this.addY = _dimY / 2;
	}

	// Token: 0x06009CAE RID: 40110 RVA: 0x003B3FEB File Offset: 0x003B21EB
	[PublicizedFrom(EAccessModifier.Private)]
	public long makeKey(int _x, int _y)
	{
		return (((long)(_y + this.addY) & (long)((ulong)(-1))) << 32) | ((long)(_x + this.addX) & (long)((ulong)(-1)));
	}

	// Token: 0x06009CAF RID: 40111 RVA: 0x003B4009 File Offset: 0x003B2209
	public bool Contains(int _x, int _y)
	{
		return _x >= -this.addX && _y >= -this.addY && _x < this.addX - 1 && _y < this.addY - 1;
	}

	// Token: 0x17001266 RID: 4710
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

	// Token: 0x040076DE RID: 30430
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, T> myData;

	// Token: 0x040076DF RID: 30431
	public T EmptyValue;

	// Token: 0x040076E0 RID: 30432
	[PublicizedFrom(EAccessModifier.Private)]
	public int addX;

	// Token: 0x040076E1 RID: 30433
	[PublicizedFrom(EAccessModifier.Private)]
	public int addY;

	// Token: 0x040076E2 RID: 30434
	public int DimX;

	// Token: 0x040076E3 RID: 30435
	public int DimY;
}
