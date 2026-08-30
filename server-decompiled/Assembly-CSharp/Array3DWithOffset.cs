using System;
using System.Runtime.CompilerServices;

// Token: 0x02001366 RID: 4966
public class Array3DWithOffset<T>
{
	// Token: 0x06009C91 RID: 40081 RVA: 0x0000640C File Offset: 0x0000460C
	public Array3DWithOffset()
	{
	}

	// Token: 0x06009C92 RID: 40082 RVA: 0x003B394C File Offset: 0x003B1B4C
	public Array3DWithOffset(int _dimX, int _dimY, int _dimZ)
	{
		this.DimX = _dimX;
		this.DimY = _dimY;
		this.DimZ = _dimZ;
		this.data = new T[_dimX * _dimY * _dimZ];
		this.addX = _dimX / 2;
		this.addY = _dimY / 2;
		this.addZ = _dimZ / 2;
	}

	// Token: 0x06009C93 RID: 40083 RVA: 0x003B399F File Offset: 0x003B1B9F
	[PublicizedFrom(EAccessModifier.Protected)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetIndex(int _x, int _y, int _z)
	{
		return _x + this.addX + (_z + this.addZ) * this.DimX + (_y + this.addY) * this.DimZ * this.DimX;
	}

	// Token: 0x17001263 RID: 4707
	public virtual T this[int _x, int _y, int _z]
	{
		get
		{
			return this.data[this.GetIndex(_x, _y, _z)];
		}
		set
		{
			this.data[this.GetIndex(_x, _y, _z)] = value;
		}
	}

	// Token: 0x06009C96 RID: 40086 RVA: 0x003B3A00 File Offset: 0x003B1C00
	public bool Contains(int _x, int _y, int _z)
	{
		return _x >= -this.addX && _y >= -this.addY && _x < this.addX - 1 && _y < this.addY - 1 && _z < this.addZ - 1 && _z < this.addZ - 1;
	}

	// Token: 0x040076C7 RID: 30407
	[PublicizedFrom(EAccessModifier.Protected)]
	public T[] data;

	// Token: 0x040076C8 RID: 30408
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addX;

	// Token: 0x040076C9 RID: 30409
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addY;

	// Token: 0x040076CA RID: 30410
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addZ;

	// Token: 0x040076CB RID: 30411
	public int DimX;

	// Token: 0x040076CC RID: 30412
	public int DimY;

	// Token: 0x040076CD RID: 30413
	public int DimZ;
}
