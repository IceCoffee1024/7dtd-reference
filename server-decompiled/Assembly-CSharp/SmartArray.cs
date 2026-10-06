using System;
using System.IO;

// Token: 0x020014C6 RID: 5318
public class SmartArray
{
	// Token: 0x0600A67A RID: 42618 RVA: 0x003DF8D4 File Offset: 0x003DDAD4
	public SmartArray(int xPow, int yPow, int zPos)
	{
		this._lXPow = xPow;
		this._lYPow = yPow;
		this._lZPow = zPos;
		this._size = (1 << this._lXPow) * (1 << this._lYPow) * (1 << this._lZPow);
		this._halfSize = this._size / 2;
		this._array = new byte[this._halfSize];
	}

	// Token: 0x0600A67B RID: 42619 RVA: 0x003DF944 File Offset: 0x003DDB44
	public void clear()
	{
		for (int i = 0; i < this._array.Length; i++)
		{
			this._array[i] = 0;
		}
	}

	// Token: 0x0600A67C RID: 42620 RVA: 0x003DF96D File Offset: 0x003DDB6D
	public void write(BinaryWriter stream)
	{
		stream.Write(this._array);
	}

	// Token: 0x0600A67D RID: 42621 RVA: 0x003DF97B File Offset: 0x003DDB7B
	public void read(BinaryReader stream)
	{
		this._array = stream.ReadBytes(this._halfSize);
	}

	// Token: 0x0600A67E RID: 42622 RVA: 0x003DF990 File Offset: 0x003DDB90
	public byte get(int x, int y, int z)
	{
		int num = (x << this._lXPow << this._lYPow) + (y << this._lXPow) + z;
		if (num < this._halfSize)
		{
			return this._array[num] & 15;
		}
		return (byte)((this._array[num % this._halfSize] >> 4) & 15);
	}

	// Token: 0x0600A67F RID: 42623 RVA: 0x003DF9EC File Offset: 0x003DDBEC
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

	// Token: 0x0600A680 RID: 42624 RVA: 0x003DFA77 File Offset: 0x003DDC77
	public int size()
	{
		return this._size;
	}

	// Token: 0x0600A681 RID: 42625 RVA: 0x003DFA7F File Offset: 0x003DDC7F
	public int sizePacked()
	{
		return this._halfSize;
	}

	// Token: 0x0600A682 RID: 42626 RVA: 0x003DFA87 File Offset: 0x003DDC87
	public void copyFrom(SmartArray _other)
	{
		_other._array.CopyTo(this._array, 0);
	}

	// Token: 0x0600A683 RID: 42627 RVA: 0x003DFA9B File Offset: 0x003DDC9B
	public int GetUsedMem()
	{
		return this._array.Length;
	}

	// Token: 0x04007C03 RID: 31747
	public byte[] _array;

	// Token: 0x04007C04 RID: 31748
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lXPow;

	// Token: 0x04007C05 RID: 31749
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lYPow;

	// Token: 0x04007C06 RID: 31750
	[PublicizedFrom(EAccessModifier.Private)]
	public int _lZPow;

	// Token: 0x04007C07 RID: 31751
	public int _size;

	// Token: 0x04007C08 RID: 31752
	public int _halfSize;
}
