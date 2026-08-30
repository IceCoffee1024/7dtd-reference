using System;
using System.Runtime.CompilerServices;

// Token: 0x02000BD0 RID: 3024
public class TileFile<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x06005B23 RID: 23331 RVA: 0x002305A9 File Offset: 0x0022E7A9
	public TileFile(FileBackedArray<T> _fileBackedArray, int _tileWidth, int _tileCountWidth, int _tileCountHeight)
	{
		this.fba = _fileBackedArray;
		this.tileWidth = _tileWidth;
		this.tileCountWidth = _tileCountWidth;
		this.tileCountHeight = _tileCountHeight;
		this.dataLength = this.tileWidth * this.tileWidth;
	}

	// Token: 0x06005B24 RID: 23332 RVA: 0x002305E1 File Offset: 0x0022E7E1
	public bool IsInDatabase(int _tileX, int _tileZ)
	{
		return _tileX >= 0 && _tileX < this.tileCountWidth && _tileZ >= 0 && _tileZ < this.tileCountHeight;
	}

	// Token: 0x06005B25 RID: 23333 RVA: 0x00230600 File Offset: 0x0022E800
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

	// Token: 0x06005B26 RID: 23334 RVA: 0x002306A8 File Offset: 0x0022E8A8
	public void Dispose()
	{
		this.fba.Dispose();
	}

	// Token: 0x040046BE RID: 18110
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<T> fba;

	// Token: 0x040046BF RID: 18111
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileWidth;

	// Token: 0x040046C0 RID: 18112
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileCountWidth;

	// Token: 0x040046C1 RID: 18113
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileCountHeight;

	// Token: 0x040046C2 RID: 18114
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int dataLength;
}
