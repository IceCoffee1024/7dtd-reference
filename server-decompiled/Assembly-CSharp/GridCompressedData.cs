using System;
using System.Runtime.CompilerServices;

// Token: 0x02001435 RID: 5173
public class GridCompressedData<[IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x0600A223 RID: 41507 RVA: 0x003CD768 File Offset: 0x003CB968
	public GridCompressedData(int _width, int _height, int _cellSizeX, int _cellSizeY)
	{
		if (_width % _cellSizeX != 0)
		{
			throw new Exception(string.Format("Cell width must be a multiple of data width. Cell width: {0}, width: {1}", _cellSizeX, _width));
		}
		if (_height % _cellSizeY != 0)
		{
			throw new Exception(string.Format("Cell height must be a multiple of data height. Cell height: {0}, height: {1}", _cellSizeX, _width));
		}
		this.width = _width;
		this.height = _height;
		this.cellSizeX = _cellSizeX;
		this.cellSizeY = _cellSizeY;
		this.widthCells = _width / this.cellSizeX;
		this.heightCells = _height / this.cellSizeY;
		int num = this.widthCells * this.heightCells;
		this.cells = new T[num][];
		this.sameValues = new T[num];
	}

	// Token: 0x0600A224 RID: 41508 RVA: 0x003CD820 File Offset: 0x003CBA20
	public void SetValue(int _x, int _y, T value)
	{
		int num = _x / this.cellSizeX + _y / this.cellSizeY * this.widthCells;
		T[] array = this.cells[num];
		if (array == null)
		{
			T t = this.sameValues[num];
			if (value.Equals(t))
			{
				return;
			}
			array = (this.cells[num] = new T[this.cellSizeX * this.cellSizeY]);
			Array.Fill<T>(array, t);
		}
		int num2 = _x % this.cellSizeX + _y % this.cellSizeY * this.cellSizeX;
		array[num2] = value;
	}

	// Token: 0x0600A225 RID: 41509 RVA: 0x003CD8B8 File Offset: 0x003CBAB8
	public void SetValue(int _cellIndex, int _cellX, int _cellY, T _value)
	{
		T[] array = this.cells[_cellIndex];
		if (array == null)
		{
			T t = this.sameValues[_cellIndex];
			if (_value.Equals(this.sameValues[_cellIndex]))
			{
				return;
			}
			array = (this.cells[_cellIndex] = new T[this.cellSizeX * this.cellSizeY]);
			Array.Fill<T>(array, t);
		}
		int num = _cellX + _cellY * this.cellSizeX;
		array[num] = _value;
	}

	// Token: 0x0600A226 RID: 41510 RVA: 0x003CD933 File Offset: 0x003CBB33
	public void SetSameValue(int _cellIndex, T _value)
	{
		this.sameValues[_cellIndex] = _value;
		this.cells[_cellIndex] = null;
	}

	// Token: 0x0600A227 RID: 41511 RVA: 0x003CD94C File Offset: 0x003CBB4C
	public void Fill(T _value)
	{
		for (int i = 0; i < this.cells.Length; i++)
		{
			this.sameValues[i] = _value;
			this.cells[i] = null;
		}
	}

	// Token: 0x0600A228 RID: 41512 RVA: 0x003CD984 File Offset: 0x003CBB84
	public T GetValue(int _x, int _y)
	{
		int num = _x / this.cellSizeX + _y / this.cellSizeY * this.widthCells;
		T[] array = this.cells[num];
		if (array == null)
		{
			return this.sameValues[num];
		}
		int num2 = _x % this.cellSizeX + _y % this.cellSizeY * this.cellSizeX;
		return array[num2];
	}

	// Token: 0x0600A229 RID: 41513 RVA: 0x003CD9E3 File Offset: 0x003CBBE3
	public T GetValue(int _offs)
	{
		return this.GetValue(_offs % this.width, _offs / this.width);
	}

	// Token: 0x0600A22A RID: 41514 RVA: 0x003CD9FC File Offset: 0x003CBBFC
	public void CheckSameValue(int _cellIndex)
	{
		T[] array = this.cells[_cellIndex];
		if (array != null && GridCompressedData<T>.CheckSameValue(this.cells[_cellIndex]))
		{
			this.sameValues[_cellIndex] = array[0];
			this.cells[_cellIndex] = null;
		}
	}

	// Token: 0x0600A22B RID: 41515 RVA: 0x003CDA40 File Offset: 0x003CBC40
	public void CheckSameValues()
	{
		for (int i = 0; i < this.sameValues.Length; i++)
		{
			this.CheckSameValue(i);
		}
	}

	// Token: 0x0600A22C RID: 41516 RVA: 0x003CDA68 File Offset: 0x003CBC68
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool CheckSameValue(T[] _cell)
	{
		T t = _cell[0];
		for (int i = 1; i < _cell.Length; i++)
		{
			if (!t.Equals(_cell[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600A22D RID: 41517 RVA: 0x003CDAA4 File Offset: 0x003CBCA4
	public T[] ToArray()
	{
		T[] array = new T[this.widthCells * this.cellSizeX * this.heightCells * this.cellSizeY];
		for (int i = 0; i < this.cells.Length; i++)
		{
			int num = i % this.widthCells;
			int num2 = i / this.widthCells;
			int num3 = num * this.cellSizeX + num2 * this.cellSizeY * this.width;
			T[] array2 = this.cells[i];
			if (array2 == null)
			{
				T t = this.sameValues[i];
				for (int j = 0; j < this.cellSizeY; j++)
				{
					Array.Fill<T>(array, t, num3, this.cellSizeX);
					num3 += this.width;
				}
			}
			else
			{
				int num4 = 0;
				for (int k = 0; k < this.cellSizeY; k++)
				{
					Array.Copy(array2, num4, array, num3, this.cellSizeX);
					num3 += this.width;
					num4 += this.cellSizeX;
				}
			}
		}
		return array;
	}

	// Token: 0x0600A22E RID: 41518 RVA: 0x003CDBA0 File Offset: 0x003CBDA0
	public void FromArray(T[] _pixs)
	{
		if (_pixs.Length != this.width * this.height)
		{
			throw new Exception(string.Format("Source array does not contain enough data. Expected length: {0}, Actual length: {1}", this.width * this.height, _pixs.Length));
		}
		int num = 0;
		for (int i = 0; i < this.heightCells; i++)
		{
			for (int j = 0; j < this.widthCells; j++)
			{
				int num2 = i * this.cellSizeY;
				int num3 = j * this.cellSizeX;
				T t = _pixs[num3 + num2 * this.width];
				this.SetSameValue(num, t);
				for (int k = 0; k < this.cellSizeY; k++)
				{
					for (int l = 0; l < this.cellSizeX; l++)
					{
						int num4 = num3 + l + (num2 + k) * this.width;
						this.SetValue(num, l, k, _pixs[num4]);
					}
				}
				num++;
			}
		}
	}

	// Token: 0x0600A22F RID: 41519 RVA: 0x003CDC9C File Offset: 0x003CBE9C
	public int EstimateOwnedBytes()
	{
		return 0 + MemoryTracker.GetSize<T>(this.sameValues) + MemoryTracker.GetSize<T>(this.cells);
	}

	// Token: 0x040079FF RID: 31231
	[PublicizedFrom(EAccessModifier.Private)]
	public T[][] cells;

	// Token: 0x04007A00 RID: 31232
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] sameValues;

	// Token: 0x04007A01 RID: 31233
	public int cellSizeX;

	// Token: 0x04007A02 RID: 31234
	public int cellSizeY;

	// Token: 0x04007A03 RID: 31235
	public int widthCells;

	// Token: 0x04007A04 RID: 31236
	public int heightCells;

	// Token: 0x04007A05 RID: 31237
	public int width;

	// Token: 0x04007A06 RID: 31238
	public int height;
}
