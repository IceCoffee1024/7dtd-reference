using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02000B11 RID: 2833
public struct TextureFullArray : IEquatable<TextureFullArray>
{
	// Token: 0x17000935 RID: 2357
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

	// Token: 0x0600540D RID: 21517 RVA: 0x00204123 File Offset: 0x00202323
	public TextureFullArray(long _fillValue)
	{
		this.Fill(_fillValue);
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x0600540E RID: 21518 RVA: 0x0020412C File Offset: 0x0020232C
	public static TextureFullArray Default
	{
		get
		{
			return TextureFullArray._default;
		}
	}

	// Token: 0x0600540F RID: 21519 RVA: 0x00204134 File Offset: 0x00202334
	public unsafe void Fill(long _fillValue)
	{
		for (int i = 0; i < 1; i++)
		{
			*((ref this.values.FixedElementField) + (IntPtr)i * 8) = _fillValue;
		}
	}

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x06005410 RID: 21520 RVA: 0x0020415F File Offset: 0x0020235F
	public bool IsDefault
	{
		get
		{
			return this.Equals(TextureFullArray._default);
		}
	}

	// Token: 0x06005411 RID: 21521 RVA: 0x0020416C File Offset: 0x0020236C
	public unsafe void Read(PooledBinaryReader _br, int count = 1)
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

	// Token: 0x06005412 RID: 21522 RVA: 0x002041CC File Offset: 0x002023CC
	public unsafe void Write(PooledBinaryWriter _bw)
	{
		for (int i = 0; i < 1; i++)
		{
			_bw.Write(*((ref this.values.FixedElementField) + (IntPtr)i * 8));
		}
	}

	// Token: 0x06005413 RID: 21523 RVA: 0x002041FC File Offset: 0x002023FC
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

	// Token: 0x06005414 RID: 21524 RVA: 0x00204240 File Offset: 0x00202440
	public override bool Equals(object obj)
	{
		if (obj is TextureFullArray)
		{
			TextureFullArray textureFullArray = (TextureFullArray)obj;
			return this.Equals(textureFullArray);
		}
		return false;
	}

	// Token: 0x06005415 RID: 21525 RVA: 0x00204268 File Offset: 0x00202468
	public override int GetHashCode()
	{
		int num = 17;
		for (int i = 0; i < 1; i++)
		{
			num = num * 31 + ((ref this.values.FixedElementField) + (IntPtr)i * 8).GetHashCode();
		}
		return num;
	}

	// Token: 0x06005416 RID: 21526 RVA: 0x002042A0 File Offset: 0x002024A0
	public static bool operator ==(TextureFullArray left, TextureFullArray right)
	{
		return left.Equals(right);
	}

	// Token: 0x06005417 RID: 21527 RVA: 0x002042AA File Offset: 0x002024AA
	public static bool operator !=(TextureFullArray left, TextureFullArray right)
	{
		return !left.Equals(right);
	}

	// Token: 0x040041CD RID: 16845
	[FixedBuffer(typeof(long), 1)]
	[PublicizedFrom(EAccessModifier.Private)]
	public TextureFullArray.<values>e__FixedBuffer values;

	// Token: 0x040041CE RID: 16846
	public static readonly TextureFullArray _default = new TextureFullArray(0L);

	// Token: 0x02000B12 RID: 2834
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct <values>e__FixedBuffer
	{
		// Token: 0x040041CF RID: 16847
		public long FixedElementField;
	}
}
