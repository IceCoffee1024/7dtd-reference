using System;

// Token: 0x02001383 RID: 4995
public class ArrayWithOffset<T>
{
	// Token: 0x06009DA1 RID: 40353 RVA: 0x0000640C File Offset: 0x0000460C
	public ArrayWithOffset()
	{
	}

	// Token: 0x06009DA2 RID: 40354 RVA: 0x003B90EE File Offset: 0x003B72EE
	public ArrayWithOffset(int _dimX, int _dimY)
		: this(_dimX, _dimY, 0, 0)
	{
	}

	// Token: 0x06009DA3 RID: 40355 RVA: 0x003B90FC File Offset: 0x003B72FC
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

	// Token: 0x1700129E RID: 4766
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

	// Token: 0x06009DA6 RID: 40358 RVA: 0x003B91D2 File Offset: 0x003B73D2
	public bool Contains(int _x, int _y)
	{
		return _x >= this.MinPos.x && _y >= this.MinPos.y && _x < this.MaxPos.x && _y < this.MaxPos.y;
	}

	// Token: 0x06009DA7 RID: 40359 RVA: 0x003B9210 File Offset: 0x003B7410
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

	// Token: 0x040077AD RID: 30637
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] data;

	// Token: 0x040077AE RID: 30638
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeXHalf;

	// Token: 0x040077AF RID: 30639
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeYHalf;

	// Token: 0x040077B0 RID: 30640
	[PublicizedFrom(EAccessModifier.Private)]
	public int addXOffs;

	// Token: 0x040077B1 RID: 30641
	[PublicizedFrom(EAccessModifier.Private)]
	public int addYOffs;

	// Token: 0x040077B2 RID: 30642
	public int DimX;

	// Token: 0x040077B3 RID: 30643
	public int DimY;

	// Token: 0x040077B4 RID: 30644
	public Vector2i MinPos;

	// Token: 0x040077B5 RID: 30645
	public Vector2i MaxPos;
}
