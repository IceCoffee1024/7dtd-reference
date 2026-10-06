using System;
using System.Runtime.CompilerServices;

// Token: 0x020013A0 RID: 5024
public class Array3DWithOffset<T>
{
	// Token: 0x06009E37 RID: 40503 RVA: 0x0000640C File Offset: 0x0000460C
	public Array3DWithOffset()
	{
	}

	// Token: 0x06009E38 RID: 40504 RVA: 0x003BD77C File Offset: 0x003BB97C
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

	// Token: 0x06009E39 RID: 40505 RVA: 0x003BD7CF File Offset: 0x003BB9CF
	[PublicizedFrom(EAccessModifier.Protected)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetIndex(int _x, int _y, int _z)
	{
		return _x + this.addX + (_z + this.addZ) * this.DimX + (_y + this.addY) * this.DimZ * this.DimX;
	}

	// Token: 0x170012E1 RID: 4833
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

	// Token: 0x06009E3C RID: 40508 RVA: 0x003BD830 File Offset: 0x003BBA30
	public bool Contains(int _x, int _y, int _z)
	{
		return _x >= -this.addX && _y >= -this.addY && _x < this.addX - 1 && _y < this.addY - 1 && _z < this.addZ - 1 && _z < this.addZ - 1;
	}

	// Token: 0x0400781A RID: 30746
	[PublicizedFrom(EAccessModifier.Protected)]
	public T[] data;

	// Token: 0x0400781B RID: 30747
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addX;

	// Token: 0x0400781C RID: 30748
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addY;

	// Token: 0x0400781D RID: 30749
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addZ;

	// Token: 0x0400781E RID: 30750
	public int DimX;

	// Token: 0x0400781F RID: 30751
	public int DimY;

	// Token: 0x04007820 RID: 30752
	public int DimZ;
}
