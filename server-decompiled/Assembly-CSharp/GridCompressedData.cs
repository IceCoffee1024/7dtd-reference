using System;
using System.Runtime.CompilerServices;

// Token: 0x020013FA RID: 5114
public class GridCompressedData<[IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x0600A07B RID: 41083 RVA: 0x003C3888 File Offset: 0x003C1A88
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

	// Token: 0x0600A07C RID: 41084 RVA: 0x003C3940 File Offset: 0x003C1B40
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

	// Token: 0x0600A07D RID: 41085 RVA: 0x003C39D8 File Offset: 0x003C1BD8
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

	// Token: 0x0600A07E RID: 41086 RVA: 0x003C3A53 File Offset: 0x003C1C53
	public void SetSameValue(int _cellIndex, T _value)
	{
		this.sameValues[_cellIndex] = _value;
		this.cells[_cellIndex] = null;
	}

	// Token: 0x0600A07F RID: 41087 RVA: 0x003C3A6C File Offset: 0x003C1C6C
	public void Fill(T _value)
	{
		for (int i = 0; i < this.cells.Length; i++)
		{
			this.sameValues[i] = _value;
			this.cells[i] = null;
		}
	}

	// Token: 0x0600A080 RID: 41088 RVA: 0x003C3AA4 File Offset: 0x003C1CA4
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

	// Token: 0x0600A081 RID: 41089 RVA: 0x003C3B03 File Offset: 0x003C1D03
	public T GetValue(int _offs)
	{
		return this.GetValue(_offs % this.width, _offs / this.width);
	}

	// Token: 0x0600A082 RID: 41090 RVA: 0x003C3B1C File Offset: 0x003C1D1C
	public void CheckSameValue(int _cellIndex)
	{
		T[] array = this.cells[_cellIndex];
		if (array != null && GridCompressedData<T>.CheckSameValue(this.cells[_cellIndex]))
		{
			this.sameValues[_cellIndex] = array[0];
			this.cells[_cellIndex] = null;
		}
	}

	// Token: 0x0600A083 RID: 41091 RVA: 0x003C3B60 File Offset: 0x003C1D60
	public void CheckSameValues()
	{
		for (int i = 0; i < this.sameValues.Length; i++)
		{
			this.CheckSameValue(i);
		}
	}

	// Token: 0x0600A084 RID: 41092 RVA: 0x003C3B88 File Offset: 0x003C1D88
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

	// Token: 0x0600A085 RID: 41093 RVA: 0x003C3BC4 File Offset: 0x003C1DC4
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

	// Token: 0x0600A086 RID: 41094 RVA: 0x003C3CC0 File Offset: 0x003C1EC0
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

	// Token: 0x0600A087 RID: 41095 RVA: 0x003C3DBC File Offset: 0x003C1FBC
	public int EstimateOwnedBytes()
	{
		return 0 + MemoryTracker.GetSize<T>(this.sameValues) + MemoryTracker.GetSize<T>(this.cells);
	}

	// Token: 0x040078AB RID: 30891
	[PublicizedFrom(EAccessModifier.Private)]
	public T[][] cells;

	// Token: 0x040078AC RID: 30892
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] sameValues;

	// Token: 0x040078AD RID: 30893
	public int cellSizeX;

	// Token: 0x040078AE RID: 30894
	public int cellSizeY;

	// Token: 0x040078AF RID: 30895
	public int widthCells;

	// Token: 0x040078B0 RID: 30896
	public int heightCells;

	// Token: 0x040078B1 RID: 30897
	public int width;

	// Token: 0x040078B2 RID: 30898
	public int height;
}
