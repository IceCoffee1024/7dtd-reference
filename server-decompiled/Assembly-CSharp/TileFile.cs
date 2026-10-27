using System;
using System.Runtime.CompilerServices;

// Token: 0x02000BED RID: 3053
public class TileFile<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x06005B61 RID: 23393 RVA: 0x00234E01 File Offset: 0x00233001
	public TileFile(FileBackedArray<T> _fileBackedArray, int _tileWidth, int _tileCountWidth, int _tileCountHeight)
	{
		this.fba = _fileBackedArray;
		this.tileWidth = _tileWidth;
		this.tileCountWidth = _tileCountWidth;
		this.tileCountHeight = _tileCountHeight;
		this.dataLength = this.tileWidth * this.tileWidth;
	}

	// Token: 0x06005B62 RID: 23394 RVA: 0x00234E39 File Offset: 0x00233039
	public bool IsInDatabase(int _tileX, int _tileZ)
	{
		return _tileX >= 0 && _tileX < this.tileCountWidth && _tileZ >= 0 && _tileZ < this.tileCountHeight;
	}

	// Token: 0x06005B63 RID: 23395 RVA: 0x00234E58 File Offset: 0x00233058
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

	// Token: 0x06005B64 RID: 23396 RVA: 0x00234F00 File Offset: 0x00233100
	public void Dispose()
	{
		this.fba.Dispose();
	}

	// Token: 0x04004722 RID: 18210
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<T> fba;

	// Token: 0x04004723 RID: 18211
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileWidth;

	// Token: 0x04004724 RID: 18212
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileCountWidth;

	// Token: 0x04004725 RID: 18213
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileCountHeight;

	// Token: 0x04004726 RID: 18214
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int dataLength;
}
