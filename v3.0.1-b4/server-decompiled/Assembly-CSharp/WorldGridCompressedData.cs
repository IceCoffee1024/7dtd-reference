using System;
using System.Runtime.CompilerServices;

// Token: 0x020014DC RID: 5340
public class WorldGridCompressedData<[IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x0600A7F1 RID: 42993 RVA: 0x003E7CD3 File Offset: 0x003E5ED3
	public WorldGridCompressedData(T[] _colors, int _dimX, int _dimY, int _gridSizeX, int _gridSizeY)
		: this(_colors, _dimX, _dimY, _gridSizeX, _gridSizeY, 0, 0)
	{
	}

	// Token: 0x0600A7F2 RID: 42994 RVA: 0x003E7CE4 File Offset: 0x003E5EE4
	public WorldGridCompressedData(T[] _colors, int _dimX, int _dimY, int _gridSizeX, int _gridSizeY, int _addXOffs, int _addYOffs)
	{
		this.colors = new GridCompressedData<T>(_dimX, _dimY, _gridSizeX, _gridSizeY);
		this.colors.FromArray(_colors);
		this.DimX = _dimX;
		this.DimY = _dimY;
		this.sizeXHalf = _dimX / 2;
		this.sizeYHalf = _dimY / 2;
		this.addXOffs = _addXOffs;
		this.addYOffs = _addYOffs;
		this.MinPos = new Vector2i(-this.sizeXHalf - this.addXOffs, -this.sizeYHalf - this.addYOffs);
		this.MaxPos = new Vector2i(this.sizeXHalf - this.addXOffs - 1, this.sizeYHalf - this.addYOffs - 1);
	}

	// Token: 0x0600A7F3 RID: 42995 RVA: 0x003E7D94 File Offset: 0x003E5F94
	public WorldGridCompressedData(GridCompressedData<T> _data)
		: this(_data, 0, 0)
	{
	}

	// Token: 0x0600A7F4 RID: 42996 RVA: 0x003E7DA0 File Offset: 0x003E5FA0
	public WorldGridCompressedData(GridCompressedData<T> _data, int _addXOffs, int _addYOffs)
	{
		this.colors = _data;
		this.DimX = _data.width;
		this.DimY = _data.height;
		this.sizeXHalf = this.DimX / 2;
		this.sizeYHalf = this.DimY / 2;
		this.addXOffs = _addXOffs;
		this.addYOffs = _addYOffs;
		this.MinPos = new Vector2i(-this.sizeXHalf - this.addXOffs, -this.sizeYHalf - this.addYOffs);
		this.MaxPos = new Vector2i(this.sizeXHalf - this.addXOffs - 1, this.sizeYHalf - this.addYOffs - 1);
	}

	// Token: 0x0600A7F5 RID: 42997 RVA: 0x003E7E4C File Offset: 0x003E604C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetData(int _x, int _y)
	{
		return this.colors.GetValue(_x + this.addXOffs + this.sizeXHalf, _y + this.addYOffs + this.sizeYHalf);
	}

	// Token: 0x0600A7F6 RID: 42998 RVA: 0x003E7E77 File Offset: 0x003E6077
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetData(int _offs)
	{
		return this.colors.GetValue(_offs);
	}

	// Token: 0x0600A7F7 RID: 42999 RVA: 0x003E7E85 File Offset: 0x003E6085
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(int _x, int _y)
	{
		return _x >= this.MinPos.x && _y >= this.MinPos.y && _x <= this.MaxPos.x && _y <= this.MaxPos.y;
	}

	// Token: 0x04007C31 RID: 31793
	public GridCompressedData<T> colors;

	// Token: 0x04007C32 RID: 31794
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeXHalf;

	// Token: 0x04007C33 RID: 31795
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeYHalf;

	// Token: 0x04007C34 RID: 31796
	[PublicizedFrom(EAccessModifier.Private)]
	public int addXOffs;

	// Token: 0x04007C35 RID: 31797
	[PublicizedFrom(EAccessModifier.Private)]
	public int addYOffs;

	// Token: 0x04007C36 RID: 31798
	public int DimX;

	// Token: 0x04007C37 RID: 31799
	public int DimY;

	// Token: 0x04007C38 RID: 31800
	public Vector2i MinPos;

	// Token: 0x04007C39 RID: 31801
	public Vector2i MaxPos;
}
