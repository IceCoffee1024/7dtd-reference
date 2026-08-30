using System;

// Token: 0x02001369 RID: 4969
public class ArrayWithOffset<T>
{
	// Token: 0x06009CA6 RID: 40102 RVA: 0x0000640C File Offset: 0x0000460C
	public ArrayWithOffset()
	{
	}

	// Token: 0x06009CA7 RID: 40103 RVA: 0x003B3E3A File Offset: 0x003B203A
	public ArrayWithOffset(int _dimX, int _dimY)
		: this(_dimX, _dimY, 0, 0)
	{
	}

	// Token: 0x06009CA8 RID: 40104 RVA: 0x003B3E48 File Offset: 0x003B2048
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

	// Token: 0x17001265 RID: 4709
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

	// Token: 0x06009CAB RID: 40107 RVA: 0x003B3F1E File Offset: 0x003B211E
	public bool Contains(int _x, int _y)
	{
		return _x >= this.MinPos.x && _y >= this.MinPos.y && _x < this.MaxPos.x && _y < this.MaxPos.y;
	}

	// Token: 0x06009CAC RID: 40108 RVA: 0x003B3F5C File Offset: 0x003B215C
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

	// Token: 0x040076D5 RID: 30421
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] data;

	// Token: 0x040076D6 RID: 30422
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeXHalf;

	// Token: 0x040076D7 RID: 30423
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeYHalf;

	// Token: 0x040076D8 RID: 30424
	[PublicizedFrom(EAccessModifier.Private)]
	public int addXOffs;

	// Token: 0x040076D9 RID: 30425
	[PublicizedFrom(EAccessModifier.Private)]
	public int addYOffs;

	// Token: 0x040076DA RID: 30426
	public int DimX;

	// Token: 0x040076DB RID: 30427
	public int DimY;

	// Token: 0x040076DC RID: 30428
	public Vector2i MinPos;

	// Token: 0x040076DD RID: 30429
	public Vector2i MaxPos;
}
