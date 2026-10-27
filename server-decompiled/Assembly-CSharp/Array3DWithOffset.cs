using System;
using System.Runtime.CompilerServices;

// Token: 0x020013B2 RID: 5042
public class Array3DWithOffset<T>
{
	// Token: 0x06009E8F RID: 40591 RVA: 0x0000640C File Offset: 0x0000460C
	public Array3DWithOffset()
	{
	}

	// Token: 0x06009E90 RID: 40592 RVA: 0x003BFB80 File Offset: 0x003BDD80
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

	// Token: 0x06009E91 RID: 40593 RVA: 0x003BFBD3 File Offset: 0x003BDDD3
	[PublicizedFrom(EAccessModifier.Protected)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetIndex(int _x, int _y, int _z)
	{
		return _x + this.addX + (_z + this.addZ) * this.DimX + (_y + this.addY) * this.DimZ * this.DimX;
	}

	// Token: 0x170012ED RID: 4845
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

	// Token: 0x06009E94 RID: 40596 RVA: 0x003BFC34 File Offset: 0x003BDE34
	public bool Contains(int _x, int _y, int _z)
	{
		return _x >= -this.addX && _y >= -this.addY && _x < this.addX - 1 && _y < this.addY - 1 && _z < this.addZ - 1 && _z < this.addZ - 1;
	}

	// Token: 0x0400785B RID: 30811
	[PublicizedFrom(EAccessModifier.Protected)]
	public T[] data;

	// Token: 0x0400785C RID: 30812
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addX;

	// Token: 0x0400785D RID: 30813
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addY;

	// Token: 0x0400785E RID: 30814
	[PublicizedFrom(EAccessModifier.Protected)]
	public int addZ;

	// Token: 0x0400785F RID: 30815
	public int DimX;

	// Token: 0x04007860 RID: 30816
	public int DimY;

	// Token: 0x04007861 RID: 30817
	public int DimZ;
}
