using System;
using System.IO;

// Token: 0x0200148B RID: 5259
public class SmartArray
{
	// Token: 0x0600A4CF RID: 42191 RVA: 0x003D5904 File Offset: 0x003D3B04
	public SmartArray(int xPow, int yPow, int zPos)
	{
		this._lXPow = xPow;
		this._lYPow = yPow;
		this._lZPow = zPos;
		this._size = (1 << this._lXPow) * (1 << this._lYPow) * (1 << this._lZPow);
		this._halfSize = this._size / 2;
		this._array = new byte[this._halfSize];
	}

	// Token: 0x0600A4D0 RID: 42192 RVA: 0x003D5974 File Offset: 0x003D3B74
	public void clear()
	{
		for (int i = 0; i < this._array.Length; i++)
		{
			this._array[i] = 0;
		}
	}

	// Token: 0x0600A4D1 RID: 42193 RVA: 0x003D599D File Offset: 0x003D3B9D
	public void write(BinaryWriter stream)
	{
		stream.Write(this._array);
	}

	// Token: 0x0600A4D2 RID: 42194 RVA: 0x003D59AB File Offset: 0x003D3BAB
	public void read(BinaryReader stream)
	{
		this._array = stream.ReadBytes(this._halfSize);
	}

	// Token: 0x0600A4D3 RID: 42195 RVA: 0x003D59C0 File Offset: 0x003D3BC0
	public byte get(int x, int y, int z)
	{
		int num = (x << this._lXPow << this._lYPow) + (y << this._lXPow) + z;
		if (num < this._halfSize)
		{
			return this._array[num] & 15;
		}
		return (byte)((this._array[num % this._halfSize] >> 4) & 15);
	}

	// Token: 0x0600A4D4 RID: 42196 RVA: 0x003D5A1C File Offset: 0x003D3C1C
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

	// Token: 0x0600A4D5 RID: 42197 RVA: 0x003D5AA7 File Offset: 0x003D3CA7
	public int size()
	{
		return this._size;
	}

	// Token: 0x0600A4D6 RID: 42198 RVA: 0x003D5AAF File Offset: 0x003D3CAF
	public int sizePacked()
	{
		return this._halfSize;
	}

	// Token: 0x0600A4D7 RID: 42199 RVA: 0x003D5AB7 File Offset: 0x003D3CB7
	public void copyFrom(SmartArray _other)
	{
		_other._array.CopyTo(this._array, 0);
	}

	// Token: 0x0600A4D8 RID: 42200 RVA: 0x003D5ACB File Offset: 0x003D3CCB
	public int GetUsedMem()
	{
		return this._array.Length;
	}

	// Token: 0x04007AAF RID: 31407
	public byte[] _array;

	// Token: 0x04007AB0 RID: 31408
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lXPow;

	// Token: 0x04007AB1 RID: 31409
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lYPow;

	// Token: 0x04007AB2 RID: 31410
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lZPow;

	// Token: 0x04007AB3 RID: 31411
	public int _size;

	// Token: 0x04007AB4 RID: 31412
	public int _halfSize;
}
