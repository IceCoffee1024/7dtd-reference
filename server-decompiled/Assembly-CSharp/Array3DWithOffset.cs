using System;
using System.Runtime.CompilerServices;

// Token: 0x02001380 RID: 4992
public class Array3DWithOffset<T>
{
	// Token: 0x06009D8C RID: 40332 RVA: 0x0000640C File Offset: 0x0000460C
	public Array3DWithOffset()
	{
	}

	// Token: 0x06009D8D RID: 40333 RVA: 0x003B8C00 File Offset: 0x003B6E00
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

	// Token: 0x06009D8E RID: 40334 RVA: 0x003B8C53 File Offset: 0x003B6E53
	[PublicizedFrom(EAccessModifier.Protected)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetIndex(int _x, int _y, int _z)
	{
		return _x + this.addX + (_z + this.addZ) * this.DimX + (_y + this.addY) * this.DimZ * this.DimX;
	}

	// Token: 0x1700129C RID: 4764
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

	// Token: 0x06009D91 RID: 40337 RVA: 0x003B8CB4 File Offset: 0x003B6EB4
	public bool Contains(int _x, int _y, int _z)
	{
		return _x >= -this.addX && _y >= -this.addY && _x < this.addX - 1 && _y < this.addY - 1 && _z < this.addZ - 1 && _z < this.addZ - 1;
	}

	// Token: 0x0400779F RID: 30623
	[PublicizedFrom(EAccessModifier.Protected)]
	public T[] data;

	// Token: 0x040077A0 RID: 30624
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addX;

	// Token: 0x040077A1 RID: 30625
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addY;

	// Token: 0x040077A2 RID: 30626
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addZ;

	// Token: 0x040077A3 RID: 30627
	public int DimX;

	// Token: 0x040077A4 RID: 30628
	public int DimY;

	// Token: 0x040077A5 RID: 30629
	public int DimZ;
}
