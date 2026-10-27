using System;
using System.IO;

// Token: 0x020014D8 RID: 5336
public class SmartArray
{
	// Token: 0x0600A6D2 RID: 42706 RVA: 0x003E1CD8 File Offset: 0x003DFED8
	public SmartArray(int xPow, int yPow, int zPos)
	{
		this._lXPow = xPow;
		this._lYPow = yPow;
		this._lZPow = zPos;
		this._size = (1 << this._lXPow) * (1 << this._lYPow) * (1 << this._lZPow);
		this._halfSize = this._size / 2;
		this._array = new byte[this._halfSize];
	}

	// Token: 0x0600A6D3 RID: 42707 RVA: 0x003E1D48 File Offset: 0x003DFF48
	public void clear()
	{
		for (int i = 0; i < this._array.Length; i++)
		{
			this._array[i] = 0;
		}
	}

	// Token: 0x0600A6D4 RID: 42708 RVA: 0x003E1D71 File Offset: 0x003DFF71
	public void write(BinaryWriter stream)
	{
		stream.Write(this._array);
	}

	// Token: 0x0600A6D5 RID: 42709 RVA: 0x003E1D7F File Offset: 0x003DFF7F
	public void read(BinaryReader stream)
	{
		this._array = stream.ReadBytes(this._halfSize);
	}

	// Token: 0x0600A6D6 RID: 42710 RVA: 0x003E1D94 File Offset: 0x003DFF94
	public byte get(int x, int y, int z)
	{
		int num = (x << this._lXPow << this._lYPow) + (y << this._lXPow) + z;
		if (num < this._halfSize)
		{
			return this._array[num] & 15;
		}
		return (byte)((this._array[num % this._halfSize] >> 4) & 15);
	}

	// Token: 0x0600A6D7 RID: 42711 RVA: 0x003E1DF0 File Offset: 0x003DFFF0
	public void set(int x, int y, int z, byte b)
	{
		int num = (x << this._lXPow << this._lYPow) + (y << this._lXPow) + z;
		int num2;
		if (num < this._halfSize)
		{
			num2 = (int)((this._array[num] & 240) | (b & 15));
			this._array[num] = (byte)num2;
			return;
		}
		num2 = (((int)b << 4) & 240) | (int)(this._array[num % this._halfSize] & 15);
		this._array[num % this._halfSize] = (byte)num2;
	}

	// Token: 0x0600A6D8 RID: 42712 RVA: 0x003E1E7B File Offset: 0x003E007B
	public int size()
	{
		return this._size;
	}

	// Token: 0x0600A6D9 RID: 42713 RVA: 0x003E1E83 File Offset: 0x003E0083
	public int sizePacked()
	{
		return this._halfSize;
	}

	// Token: 0x0600A6DA RID: 42714 RVA: 0x003E1E8B File Offset: 0x003E008B
	public void copyFrom(SmartArray _other)
	{
		_other._array.CopyTo(this._array, 0);
	}

	// Token: 0x0600A6DB RID: 42715 RVA: 0x003E1E9F File Offset: 0x003E009F
	public int GetUsedMem()
	{
		return this._array.Length;
	}

	// Token: 0x04007C44 RID: 31812
	public byte[] _array;

	// Token: 0x04007C45 RID: 31813
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lXPow;

	// Token: 0x04007C46 RID: 31814
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lYPow;

	// Token: 0x04007C47 RID: 31815
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lZPow;

	// Token: 0x04007C48 RID: 31816
	public int _size;

	// Token: 0x04007C49 RID: 31817
	public int _halfSize;
}
