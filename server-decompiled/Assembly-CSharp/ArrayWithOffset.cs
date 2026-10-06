using System;

// Token: 0x020013A3 RID: 5027
public class ArrayWithOffset<T>
{
	// Token: 0x06009E4C RID: 40524 RVA: 0x0000640C File Offset: 0x0000460C
	public ArrayWithOffset()
	{
	}

	// Token: 0x06009E4D RID: 40525 RVA: 0x003BDC6A File Offset: 0x003BBE6A
	public ArrayWithOffset(int _dimX, int _dimY)
		: this(_dimX, _dimY, 0, 0)
	{
	}

	// Token: 0x06009E4E RID: 40526 RVA: 0x003BDC78 File Offset: 0x003BBE78
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

	// Token: 0x170012E3 RID: 4835
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

	// Token: 0x06009E51 RID: 40529 RVA: 0x003BDD4E File Offset: 0x003BBF4E
	public bool Contains(int _x, int _y)
	{
		return _x >= this.MinPos.x && _y >= this.MinPos.y && _x < this.MaxPos.x && _y < this.MaxPos.y;
	}

	// Token: 0x06009E52 RID: 40530 RVA: 0x003BDD8C File Offset: 0x003BBF8C
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

	// Token: 0x04007828 RID: 30760
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] data;

	// Token: 0x04007829 RID: 30761
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeXHalf;

	// Token: 0x0400782A RID: 30762
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeYHalf;

	// Token: 0x0400782B RID: 30763
	[PublicizedFrom(EAccessModifier.Private)]
	public int addXOffs;

	// Token: 0x0400782C RID: 30764
	[PublicizedFrom(EAccessModifier.Private)]
	public int addYOffs;

	// Token: 0x0400782D RID: 30765
	public int DimX;

	// Token: 0x0400782E RID: 30766
	public int DimY;

	// Token: 0x0400782F RID: 30767
	public Vector2i MinPos;

	// Token: 0x04007830 RID: 30768
	public Vector2i MaxPos;
}
