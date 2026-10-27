using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02000B1C RID: 2844
public struct TextureFullArray : IEquatable<TextureFullArray>
{
	// Token: 0x1700093B RID: 2363
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

	// Token: 0x06005432 RID: 21554 RVA: 0x002056A7 File Offset: 0x002038A7
	public TextureFullArray(long _fillValue)
	{
		this.Fill(_fillValue);
	}

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x06005433 RID: 21555 RVA: 0x002056B0 File Offset: 0x002038B0
	public static TextureFullArray Default
	{
		get
		{
			return TextureFullArray._default;
		}
	}

	// Token: 0x06005434 RID: 21556 RVA: 0x002056B8 File Offset: 0x002038B8
	public unsafe void Fill(long _fillValue)
	{
		for (int i = 0; i < 1; i++)
		{
			*((ref this.values.FixedElementField) + (IntPtr)i * 8) = _fillValue;
		}
	}

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x06005435 RID: 21557 RVA: 0x002056E3 File Offset: 0x002038E3
	public bool IsDefault
	{
		get
		{
			return this.Equals(TextureFullArray._default);
		}
	}

	// Token: 0x06005436 RID: 21558 RVA: 0x002056F0 File Offset: 0x002038F0
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

	// Token: 0x06005437 RID: 21559 RVA: 0x00205750 File Offset: 0x00203950
	public unsafe void Write(PooledBinaryWriter _bw)
	{
		for (int i = 0; i < 1; i++)
		{
			_bw.Write(*((ref this.values.FixedElementField) + (IntPtr)i * 8));
		}
	}

	// Token: 0x06005438 RID: 21560 RVA: 0x00205780 File Offset: 0x00203980
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

	// Token: 0x06005439 RID: 21561 RVA: 0x002057C4 File Offset: 0x002039C4
	public override bool Equals(object obj)
	{
		if (obj is TextureFullArray)
		{
			TextureFullArray textureFullArray = (TextureFullArray)obj;
			return this.Equals(textureFullArray);
		}
		return false;
	}

	// Token: 0x0600543A RID: 21562 RVA: 0x002057EC File Offset: 0x002039EC
	public override int GetHashCode()
	{
		int num = 17;
		for (int i = 0; i < 1; i++)
		{
			num = num * 31 + ((ref this.values.FixedElementField) + (IntPtr)i * 8).GetHashCode();
		}
		return num;
	}

	// Token: 0x0600543B RID: 21563 RVA: 0x00205824 File Offset: 0x00203A24
	public static bool operator ==(TextureFullArray left, TextureFullArray right)
	{
		return left.Equals(right);
	}

	// Token: 0x0600543C RID: 21564 RVA: 0x0020582E File Offset: 0x00203A2E
	public static bool operator !=(TextureFullArray left, TextureFullArray right)
	{
		return !left.Equals(right);
	}

	// Token: 0x040041F2 RID: 16882
	[FixedBuffer(typeof(long), 1)]
	[PublicizedFrom(EAccessModifier.Private)]
	public TextureFullArray.<values>e__FixedBuffer values;

	// Token: 0x040041F3 RID: 16883
	public static readonly TextureFullArray _default = new TextureFullArray(0L);

	// Token: 0x02000B1D RID: 2845
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct <values>e__FixedBuffer
	{
		// Token: 0x040041F4 RID: 16884
		public long FixedElementField;
	}
}
