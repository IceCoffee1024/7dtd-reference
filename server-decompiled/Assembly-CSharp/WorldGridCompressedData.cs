using System;
using System.Runtime.CompilerServices;

// Token: 0x020014F7 RID: 5367
public class WorldGridCompressedData<[IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x0600A8EE RID: 43246 RVA: 0x003ED013 File Offset: 0x003EB213
	public WorldGridCompressedData(T[] _colors, int _dimX, int _dimY, int _gridSizeX, int _gridSizeY)
		: this(_colors, _dimX, _dimY, _gridSizeX, _gridSizeY, 0, 0)
	{
	}

	// Token: 0x0600A8EF RID: 43247 RVA: 0x003ED024 File Offset: 0x003EB224
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

	// Token: 0x0600A8F0 RID: 43248 RVA: 0x003ED0D4 File Offset: 0x003EB2D4
	public WorldGridCompressedData(GridCompressedData<T> _data)
		: this(_data, 0, 0)
	{
	}

	// Token: 0x0600A8F1 RID: 43249 RVA: 0x003ED0E0 File Offset: 0x003EB2E0
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

	// Token: 0x0600A8F2 RID: 43250 RVA: 0x003ED18C File Offset: 0x003EB38C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetData(int _x, int _y)
	{
		return this.colors.GetValue(_x + this.addXOffs + this.sizeXHalf, _y + this.addYOffs + this.sizeYHalf);
	}

	// Token: 0x0600A8F3 RID: 43251 RVA: 0x003ED1B7 File Offset: 0x003EB3B7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T GetData(int _offs)
	{
		return this.colors.GetValue(_offs);
	}

	// Token: 0x0600A8F4 RID: 43252 RVA: 0x003ED1C5 File Offset: 0x003EB3C5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(int _x, int _y)
	{
		return _x >= this.MinPos.x && _y >= this.MinPos.y && _x <= this.MaxPos.x && _y <= this.MaxPos.y;
	}

	// Token: 0x04007D0A RID: 32010
	public GridCompressedData<T> colors;

	// Token: 0x04007D0B RID: 32011
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeXHalf;

	// Token: 0x04007D0C RID: 32012
	[PublicizedFrom(EAccessModifier.Private)]
	public int sizeYHalf;

	// Token: 0x04007D0D RID: 32013
	[PublicizedFrom(EAccessModifier.Private)]
	public int addXOffs;

	// Token: 0x04007D0E RID: 32014
	[PublicizedFrom(EAccessModifier.Private)]
	public int addYOffs;

	// Token: 0x04007D0F RID: 32015
	public int DimX;

	// Token: 0x04007D10 RID: 32016
	public int DimY;

	// Token: 0x04007D11 RID: 32017
	public Vector2i MinPos;

	// Token: 0x04007D12 RID: 32018
	public Vector2i MaxPos;
}
