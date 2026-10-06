using System;
using System.Runtime.CompilerServices;

// Token: 0x02000BE2 RID: 3042
public class TileFile<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x06005B3C RID: 23356 RVA: 0x002338A5 File Offset: 0x00231AA5
	public TileFile(FileBackedArray<T> _fileBackedArray, int _tileWidth, int _tileCountWidth, int _tileCountHeight)
	{
		this.fba = _fileBackedArray;
		this.tileWidth = _tileWidth;
		this.tileCountWidth = _tileCountWidth;
		this.tileCountHeight = _tileCountHeight;
		this.dataLength = this.tileWidth * this.tileWidth;
	}

	// Token: 0x06005B3D RID: 23357 RVA: 0x002338DD File Offset: 0x00231ADD
	public bool IsInDatabase(int _tileX, int _tileZ)
	{
		return _tileX >= 0 && _tileX < this.tileCountWidth && _tileZ >= 0 && _tileZ < this.tileCountHeight;
	}

	// Token: 0x06005B3E RID: 23358 RVA: 0x002338FC File Offset: 0x00231AFC
	public unsafe void LoadTile(int _tileX, int _tileZ, ref T[,] _tile)
	{
		if (_tile == null)
		{
			_tile = new T[this.tileWidth, this.tileWidth];
		}
		int num = _tileZ * this.tileCountWidth * this.dataLength + _tileX * this.dataLength;
		ReadOnlySpan<T> readOnlySpan2;
		using (this.fba.GetReadOnlySpan(num, this.dataLength, out readOnlySpan2))
		{
			T[,] array;
			T* ptr;
			if ((array = _tile) == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0, 0];
			}
			Span<T> span = new Span<T>((void*)ptr, this.dataLength);
			readOnlySpan2.CopyTo(span);
			array = null;
		}
	}

	// Token: 0x06005B3F RID: 23359 RVA: 0x002339A4 File Offset: 0x00231BA4
	public void Dispose()
	{
		this.fba.Dispose();
	}

	// Token: 0x040046FD RID: 18173
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<T> fba;

	// Token: 0x040046FE RID: 18174
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileWidth;

	// Token: 0x040046FF RID: 18175
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileCountWidth;

	// Token: 0x04004700 RID: 18176
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileCountHeight;

	// Token: 0x04004701 RID: 18177
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int dataLength;
}
