using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02000AFF RID: 2815
public struct TextureFullArray : IEquatable<TextureFullArray>
{
	// Token: 0x17000913 RID: 2323
	public unsafe long this[int index]
	{
		get
		{
			if (index < 0 || index >= 1)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is outside of the valid range of min: 0, max: {1}.", index, 1));
			}
			return *((ref this.values.FixedElementField) + (IntPtr)index * 8);
		}
		set
		{
			if (index < 0 || index >= 1)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is outside of the valid range of min: 0, max: {1}.", index, 1));
			}
			*((ref this.values.FixedElementField) + (IntPtr)index * 8) = value;
		}
	}

	// Token: 0x060053F4 RID: 21492 RVA: 0x00200EC3 File Offset: 0x001FF0C3
	public TextureFullArray(long _fillValue)
	{
		this.Fill(_fillValue);
	}

	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x060053F5 RID: 21493 RVA: 0x00200ECC File Offset: 0x001FF0CC
	public static TextureFullArray Default
	{
		get
		{
			return TextureFullArray._default;
		}
	}

	// Token: 0x060053F6 RID: 21494 RVA: 0x00200ED4 File Offset: 0x001FF0D4
	public unsafe void Fill(long _fillValue)
	{
		for (int i = 0; i < 1; i++)
		{
			*((ref this.values.FixedElementField) + (IntPtr)i * 8) = _fillValue;
		}
	}

	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x060053F7 RID: 21495 RVA: 0x00200EFF File Offset: 0x001FF0FF
	public bool IsDefault
	{
		get
		{
			return this.Equals(TextureFullArray._default);
		}
	}

	// Token: 0x060053F8 RID: 21496 RVA: 0x00200F0C File Offset: 0x001FF10C
	public unsafe void Read(BinaryReader _br, int count = 1)
	{
		int i;
		for (i = 0; i < count; i++)
		{
			long num = _br.ReadInt64();
			if (i < 1)
			{
				*((ref this.values.FixedElementField) + (IntPtr)i * 8) = num;
			}
		}
		while (i < 1)
		{
			*((ref this.values.FixedElementField) + (IntPtr)i * 8) = this.values.FixedElementField;
			i++;
		}
	}

	// Token: 0x060053F9 RID: 21497 RVA: 0x00200F6C File Offset: 0x001FF16C
	public unsafe void Write(BinaryWriter _bw)
	{
		for (int i = 0; i < 1; i++)
		{
			_bw.Write(*((ref this.values.FixedElementField) + (IntPtr)i * 8));
		}
	}

	// Token: 0x060053FA RID: 21498 RVA: 0x00200F9C File Offset: 0x001FF19C
	public unsafe bool Equals(TextureFullArray other)
	{
		for (int i = 0; i < 1; i++)
		{
			if (*((ref this.values.FixedElementField) + (IntPtr)i * 8) != *((ref other.values.FixedElementField) + (IntPtr)i * 8))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060053FB RID: 21499 RVA: 0x00200FE0 File Offset: 0x001FF1E0
	public override bool Equals(object obj)
	{
		if (obj is TextureFullArray)
		{
			TextureFullArray textureFullArray = (TextureFullArray)obj;
			return this.Equals(textureFullArray);
		}
		return false;
	}

	// Token: 0x060053FC RID: 21500 RVA: 0x00201008 File Offset: 0x001FF208
	public override int GetHashCode()
	{
		int num = 17;
		for (int i = 0; i < 1; i++)
		{
			num = num * 31 + ((ref this.values.FixedElementField) + (IntPtr)i * 8).GetHashCode();
		}
		return num;
	}

	// Token: 0x060053FD RID: 21501 RVA: 0x00201040 File Offset: 0x001FF240
	public static bool operator ==(TextureFullArray left, TextureFullArray right)
	{
		return left.Equals(right);
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x0020104A File Offset: 0x001FF24A
	public static bool operator !=(TextureFullArray left, TextureFullArray right)
	{
		return !left.Equals(right);
	}

	// Token: 0x04004190 RID: 16784
	[FixedBuffer(typeof(long), 1)]
	[PublicizedFrom(EAccessModifier.Private)]
	public TextureFullArray.<values>e__FixedBuffer values;

	// Token: 0x04004191 RID: 16785
	public static readonly TextureFullArray _default = new TextureFullArray(0L);

	// Token: 0x02000B00 RID: 2816
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct <values>e__FixedBuffer
	{
		// Token: 0x04004192 RID: 16786
		public long FixedElementField;
	}
}
