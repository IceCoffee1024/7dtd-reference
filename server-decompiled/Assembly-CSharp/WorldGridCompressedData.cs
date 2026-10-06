using System;
using System.Runtime.CompilerServices;

// Token: 0x02001519 RID: 5401
public class WorldGridCompressedData<[IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x0600A9A1 RID: 43425 RVA: 0x003F1E67 File Offset: 0x003F0067
	public WorldGridCompressedData(T[] _colors, int _dimX, int _dimY, int _gridSizeX, int _gridSizeY)
		: this(_colors, _dimX, _dimY, _gridSizeX, _gridSizeY, 0, 0)
	{
	}

	// Token: 0x0600A9A2 RID: 43426 RVA: 0x003F1E78 File Offset: 0x003F0078
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

	// Token: 0x0600A9A3 RID: 43427 RVA: 0x003F1F28 File Offset: 0x003F0128
	public WorldGridCompressedData(GridCompressedData<T> _data)
		: this(_data, 0, 0)
	{
	}

	// Token: 0x0600A9A4 RID: 43428 RVA: 0x003F1F34 File Offset: 0x003F0134
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

	// Token: 0x0600A9A5 RID: 43429 RVA: 0x003F1FE0 File Offset: 0x003F01E0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetData(int _x, int _y)
	{
		return this.colors.GetValue(_x + this.addXOffs + this.sizeXHalf, _y + this.addYOffs + this.sizeYHalf);
	}

	// Token: 0x0600A9A6 RID: 43430 RVA: 0x003F200B File Offset: 0x003F020B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetData(int _offs)
	{
		return this.colors.GetValue(_offs);
	}

	// Token: 0x0600A9A7 RID: 43431 RVA: 0x003F2019 File Offset: 0x003F0219
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(int _x, int _y)
	{
		return _x >= this.MinPos.x && _y >= this.MinPos.y && _x <= this.MaxPos.x && _y <= this.MaxPos.y;
	}

	// Token: 0x04007D8D RID: 32141
	public GridCompressedData<T> colors;

	// Token: 0x04007D8E RID: 32142
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeXHalf;

	// Token: 0x04007D8F RID: 32143
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeYHalf;

	// Token: 0x04007D90 RID: 32144
	[PublicizedFrom(EAccessModifier.Private)]
	public int addXOffs;

	// Token: 0x04007D91 RID: 32145
	[PublicizedFrom(EAccessModifier.Private)]
	public int addYOffs;

	// Token: 0x04007D92 RID: 32146
	public int DimX;

	// Token: 0x04007D93 RID: 32147
	public int DimY;

	// Token: 0x04007D94 RID: 32148
	public Vector2i MinPos;

	// Token: 0x04007D95 RID: 32149
	public Vector2i MaxPos;
}
