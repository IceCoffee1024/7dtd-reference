using System;

// Token: 0x020013B5 RID: 5045
public class ArrayWithOffset<T>
{
	// Token: 0x06009EA4 RID: 40612 RVA: 0x0000640C File Offset: 0x0000460C
	public ArrayWithOffset()
	{
	}

	// Token: 0x06009EA5 RID: 40613 RVA: 0x003C006E File Offset: 0x003BE26E
	public ArrayWithOffset(int _dimX, int _dimY)
		: this(_dimX, _dimY, 0, 0)
	{
	}

	// Token: 0x06009EA6 RID: 40614 RVA: 0x003C007C File Offset: 0x003BE27C
	public ArrayWithOffset(int _dimX, int _dimY, int _addXOffs, int _addYOffs)
	{
		this.DimX = _dimX;
		this.DimY = _dimY;
		this.data = new T[_dimX, _dimY];
		this.sizeXHalf = _dimX / 2;
		this.sizeYHalf = _dimY / 2;
		this.MinPos = new Vector2i(-this.sizeXHalf - _addXOffs, -this.sizeYHalf - _addYOffs);
		this.MaxPos = new Vector2i(this.sizeXHalf - _addXOffs - 1, this.sizeYHalf - _addYOffs - 1);
		this.addXOffs = _addXOffs + this.sizeXHalf;
		this.addYOffs = _addYOffs + this.sizeXHalf;
	}

	// Token: 0x170012EF RID: 4847
	public virtual T this[int _x, int _y]
	{
		get
		{
			return this.data[_x + this.addXOffs, _y + this.addYOffs];
		}
		set
		{
			this.data[_x + this.addXOffs, _y + this.addYOffs] = value;
		}
	}

	// Token: 0x06009EA9 RID: 40617 RVA: 0x003C0152 File Offset: 0x003BE352
	public bool Contains(int _x, int _y)
	{
		return _x >= this.MinPos.x && _y >= this.MinPos.y && _x < this.MaxPos.x && _y < this.MaxPos.y;
	}

	// Token: 0x06009EAA RID: 40618 RVA: 0x003C0190 File Offset: 0x003BE390
	public void CopyInto(ArrayWithOffset<T> _other)
	{
		for (int i = 0; i < this.data.GetLength(0); i++)
		{
			for (int j = 0; j < this.data.GetLength(1); j++)
			{
				_other.data[i, j] = this.data[i, j];
			}
		}
	}

	// Token: 0x04007869 RID: 30825
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] data;

	// Token: 0x0400786A RID: 30826
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeXHalf;

	// Token: 0x0400786B RID: 30827
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeYHalf;

	// Token: 0x0400786C RID: 30828
	[PublicizedFrom(EAccessModifier.Private)]
	public int addXOffs;

	// Token: 0x0400786D RID: 30829
	[PublicizedFrom(EAccessModifier.Private)]
	public int addYOffs;

	// Token: 0x0400786E RID: 30830
	public int DimX;

	// Token: 0x0400786F RID: 30831
	public int DimY;

	// Token: 0x04007870 RID: 30832
	public Vector2i MinPos;

	// Token: 0x04007871 RID: 30833
	public Vector2i MaxPos;
}
