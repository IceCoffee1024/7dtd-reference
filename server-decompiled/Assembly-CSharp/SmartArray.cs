using System;
using System.IO;

// Token: 0x020014A6 RID: 5286
public class SmartArray
{
	// Token: 0x0600A5CC RID: 42444 RVA: 0x003DAC48 File Offset: 0x003D8E48
	public SmartArray(int xPow, int yPow, int zPos)
	{
		this._lXPow = xPow;
		this._lYPow = yPow;
		this._lZPow = zPos;
		this._size = (1 << this._lXPow) * (1 << this._lYPow) * (1 << this._lZPow);
		this._halfSize = this._size / 2;
		this._array = new byte[this._halfSize];
	}

	// Token: 0x0600A5CD RID: 42445 RVA: 0x003DACB8 File Offset: 0x003D8EB8
	public void clear()
	{
		for (int i = 0; i < this._array.Length; i++)
		{
			this._array[i] = 0;
		}
	}

	// Token: 0x0600A5CE RID: 42446 RVA: 0x003DACE1 File Offset: 0x003D8EE1
	public void write(BinaryWriter stream)
	{
		stream.Write(this._array);
	}

	// Token: 0x0600A5CF RID: 42447 RVA: 0x003DACEF File Offset: 0x003D8EEF
	public void read(BinaryReader stream)
	{
		this._array = stream.ReadBytes(this._halfSize);
	}

	// Token: 0x0600A5D0 RID: 42448 RVA: 0x003DAD04 File Offset: 0x003D8F04
	public byte get(int x, int y, int z)
	{
		int num = (x << this._lXPow << this._lYPow) + (y << this._lXPow) + z;
		if (num < this._halfSize)
		{
			return this._array[num] & 15;
		}
		return (byte)((this._array[num % this._halfSize] >> 4) & 15);
	}

	// Token: 0x0600A5D1 RID: 42449 RVA: 0x003DAD60 File Offset: 0x003D8F60
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

	// Token: 0x0600A5D2 RID: 42450 RVA: 0x003DADEB File Offset: 0x003D8FEB
	public int size()
	{
		return this._size;
	}

	// Token: 0x0600A5D3 RID: 42451 RVA: 0x003DADF3 File Offset: 0x003D8FF3
	public int sizePacked()
	{
		return this._halfSize;
	}

	// Token: 0x0600A5D4 RID: 42452 RVA: 0x003DADFB File Offset: 0x003D8FFB
	public void copyFrom(SmartArray _other)
	{
		_other._array.CopyTo(this._array, 0);
	}

	// Token: 0x0600A5D5 RID: 42453 RVA: 0x003DAE0F File Offset: 0x003D900F
	public int GetUsedMem()
	{
		return this._array.Length;
	}

	// Token: 0x04007B88 RID: 31624
	public byte[] _array;

	// Token: 0x04007B89 RID: 31625
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lXPow;

	// Token: 0x04007B8A RID: 31626
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lYPow;

	// Token: 0x04007B8B RID: 31627
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lZPow;

	// Token: 0x04007B8C RID: 31628
	public int _size;

	// Token: 0x04007B8D RID: 31629
	public int _halfSize;
}
