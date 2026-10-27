using System;
using System.Collections.Generic;

// Token: 0x020013B6 RID: 5046
public class ArrayWithOffsetSparse<T>
{
	// Token: 0x06009EAB RID: 40619 RVA: 0x003C01E5 File Offset: 0x003BE3E5
	public ArrayWithOffsetSparse(int _dimX, int _dimY, T _emptyValue)
	{
		this.DimX = _dimX;
		this.DimY = _dimY;
		this.EmptyValue = _emptyValue;
		this.myData = new Dictionary<long, T>();
		this.addX = _dimX / 2;
		this.addY = _dimY / 2;
	}

	// Token: 0x06009EAC RID: 40620 RVA: 0x003C021F File Offset: 0x003BE41F
	[PublicizedFrom(EAccessModifier.Private)]
	public long makeKey(int _x, int _y)
	{
		return (((long)(_y + this.addY) & (long)((ulong)(-1))) << 32) | ((long)(_x + this.addX) & (long)((ulong)(-1)));
	}

	// Token: 0x06009EAD RID: 40621 RVA: 0x003C023D File Offset: 0x003BE43D
	public bool Contains(int _x, int _y)
	{
		return _x >= -this.addX && _y >= -this.addY && _x < this.addX - 1 && _y < this.addY - 1;
	}

	// Token: 0x170012F0 RID: 4848
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

	// Token: 0x04007872 RID: 30834
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, T> myData;

	// Token: 0x04007873 RID: 30835
	public T EmptyValue;

	// Token: 0x04007874 RID: 30836
	[PublicizedFrom(EAccessModifier.Private)]
	public int addX;

	// Token: 0x04007875 RID: 30837
	[PublicizedFrom(EAccessModifier.Private)]
	public int addY;

	// Token: 0x04007876 RID: 30838
	public int DimX;

	// Token: 0x04007877 RID: 30839
	public int DimY;
}
