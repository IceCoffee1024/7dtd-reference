using System;
using System.Runtime.CompilerServices;

// Token: 0x02001447 RID: 5191
public class GridCompressedData<[IsUnmanaged] T> where T : struct, ValueType, IEquatable<T>
{
	// Token: 0x0600A27B RID: 41595 RVA: 0x003CFB6C File Offset: 0x003CDD6C
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

	// Token: 0x0600A27C RID: 41596 RVA: 0x003CFC24 File Offset: 0x003CDE24
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

	// Token: 0x0600A27D RID: 41597 RVA: 0x003CFCBC File Offset: 0x003CDEBC
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

	// Token: 0x0600A27E RID: 41598 RVA: 0x003CFD37 File Offset: 0x003CDF37
	public void SetSameValue(int _cellIndex, T _value)
	{
		this.sameValues[_cellIndex] = _value;
		this.cells[_cellIndex] = null;
	}

	// Token: 0x0600A27F RID: 41599 RVA: 0x003CFD50 File Offset: 0x003CDF50
	public void Fill(T _value)
	{
		for (int i = 0; i < this.cells.Length; i++)
		{
			this.sameValues[i] = _value;
			this.cells[i] = null;
		}
	}

	// Token: 0x0600A280 RID: 41600 RVA: 0x003CFD88 File Offset: 0x003CDF88
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

	// Token: 0x0600A281 RID: 41601 RVA: 0x003CFDE7 File Offset: 0x003CDFE7
	public T GetValue(int _offs)
	{
		return this.GetValue(_offs % this.width, _offs / this.width);
	}

	// Token: 0x0600A282 RID: 41602 RVA: 0x003CFE00 File Offset: 0x003CE000
	public void CheckSameValue(int _cellIndex)
	{
		T[] array = this.cells[_cellIndex];
		if (array != null && GridCompressedData<T>.CheckSameValue(this.cells[_cellIndex]))
		{
			this.sameValues[_cellIndex] = array[0];
			this.cells[_cellIndex] = null;
		}
	}

	// Token: 0x0600A283 RID: 41603 RVA: 0x003CFE44 File Offset: 0x003CE044
	public void CheckSameValues()
	{
		for (int i = 0; i < this.sameValues.Length; i++)
		{
			this.CheckSameValue(i);
		}
	}

	// Token: 0x0600A284 RID: 41604 RVA: 0x003CFE6C File Offset: 0x003CE06C
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

	// Token: 0x0600A285 RID: 41605 RVA: 0x003CFEA8 File Offset: 0x003CE0A8
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

	// Token: 0x0600A286 RID: 41606 RVA: 0x003CFFA4 File Offset: 0x003CE1A4
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

	// Token: 0x0600A287 RID: 41607 RVA: 0x003D00A0 File Offset: 0x003CE2A0
	public int EstimateOwnedBytes()
	{
		return 0 + MemoryTracker.GetSize<T>(this.sameValues) + MemoryTracker.GetSize<T>(this.cells);
	}

	// Token: 0x04007A40 RID: 31296
	[PublicizedFrom(EAccessModifier.Private)]
	public T[][] cells;

	// Token: 0x04007A41 RID: 31297
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] sameValues;

	// Token: 0x04007A42 RID: 31298
	public int cellSizeX;

	// Token: 0x04007A43 RID: 31299
	public int cellSizeY;

	// Token: 0x04007A44 RID: 31300
	public int widthCells;

	// Token: 0x04007A45 RID: 31301
	public int heightCells;

	// Token: 0x04007A46 RID: 31302
	public int width;

	// Token: 0x04007A47 RID: 31303
	public int height;
}
