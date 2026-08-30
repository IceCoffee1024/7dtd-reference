using System;
using System.Runtime.CompilerServices;

// Token: 0x02000BBE RID: 3006
public class TileFile<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x06005A9B RID: 23195 RVA: 0x0022D841 File Offset: 0x0022BA41
	public TileFile(FileBackedArray<T> _fileBackedArray, int _tileWidth, int _tileCountWidth, int _tileCountHeight)
	{
		this.fba = _fileBackedArray;
		this.tileWidth = _tileWidth;
		this.tileCountWidth = _tileCountWidth;
		this.tileCountHeight = _tileCountHeight;
		this.dataLength = this.tileWidth * this.tileWidth;
	}

	// Token: 0x06005A9C RID: 23196 RVA: 0x0022D879 File Offset: 0x0022BA79
	public bool IsInDatabase(int _tileX, int _tileZ)
	{
		return _tileX >= 0 && _tileX < this.tileCountWidth && _tileZ >= 0 && _tileZ < this.tileCountHeight;
	}

	// Token: 0x06005A9D RID: 23197 RVA: 0x0022D898 File Offset: 0x0022BA98
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

	// Token: 0x06005A9E RID: 23198 RVA: 0x0022D940 File Offset: 0x0022BB40
	public void Dispose()
	{
		this.fba.Dispose();
	}

	// Token: 0x04004622 RID: 17954
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<T> fba;

	// Token: 0x04004623 RID: 17955
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileWidth;

	// Token: 0x04004624 RID: 17956
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileCountWidth;

	// Token: 0x04004625 RID: 17957
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int tileCountHeight;

	// Token: 0x04004626 RID: 17958
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int dataLength;
}
