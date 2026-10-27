using System;
using System.Runtime.CompilerServices;

// Token: 0x0200152B RID: 5419
public class WorldGridCompressedData<[IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x0600A9F9 RID: 43513 RVA: 0x003F426B File Offset: 0x003F246B
	public WorldGridCompressedData(T[] _colors, int _dimX, int _dimY, int _gridSizeX, int _gridSizeY)
		: this(_colors, _dimX, _dimY, _gridSizeX, _gridSizeY, 0, 0)
	{
	}

	// Token: 0x0600A9FA RID: 43514 RVA: 0x003F427C File Offset: 0x003F247C
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

	// Token: 0x0600A9FB RID: 43515 RVA: 0x003F432C File Offset: 0x003F252C
	public WorldGridCompressedData(GridCompressedData<T> _data)
		: this(_data, 0, 0)
	{
	}

	// Token: 0x0600A9FC RID: 43516 RVA: 0x003F4338 File Offset: 0x003F2538
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

	// Token: 0x0600A9FD RID: 43517 RVA: 0x003F43E4 File Offset: 0x003F25E4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetData(int _x, int _y)
	{
		return this.colors.GetValue(_x + this.addXOffs + this.sizeXHalf, _y + this.addYOffs + this.sizeYHalf);
	}

	// Token: 0x0600A9FE RID: 43518 RVA: 0x003F440F File Offset: 0x003F260F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetData(int _offs)
	{
		return this.colors.GetValue(_offs);
	}

	// Token: 0x0600A9FF RID: 43519 RVA: 0x003F441D File Offset: 0x003F261D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(int _x, int _y)
	{
		return _x >= this.MinPos.x && _y >= this.MinPos.y && _x <= this.MaxPos.x && _y <= this.MaxPos.y;
	}

	// Token: 0x04007DCE RID: 32206
	public GridCompressedData<T> colors;

	// Token: 0x04007DCF RID: 32207
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeXHalf;

	// Token: 0x04007DD0 RID: 32208
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeYHalf;

	// Token: 0x04007DD1 RID: 32209
	[PublicizedFrom(EAccessModifier.Private)]
	public int addXOffs;

	// Token: 0x04007DD2 RID: 32210
	[PublicizedFrom(EAccessModifier.Private)]
	public int addYOffs;

	// Token: 0x04007DD3 RID: 32211
	public int DimX;

	// Token: 0x04007DD4 RID: 32212
	public int DimY;

	// Token: 0x04007DD5 RID: 32213
	public Vector2i MinPos;

	// Token: 0x04007DD6 RID: 32214
	public Vector2i MaxPos;
}
