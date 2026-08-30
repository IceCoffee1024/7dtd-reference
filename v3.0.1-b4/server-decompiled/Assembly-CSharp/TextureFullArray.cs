using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02000AED RID: 2797
public struct TextureFullArray : IEquatable<TextureFullArray>
{
	// Token: 0x17000901 RID: 2305
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

	// Token: 0x0600536C RID: 21356 RVA: 0x001FE253 File Offset: 0x001FC453
	public TextureFullArray(long _fillValue)
	{
		this.Fill(_fillValue);
	}

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x0600536D RID: 21357 RVA: 0x001FE25C File Offset: 0x001FC45C
	public static TextureFullArray Default
	{
		get
		{
			return TextureFullArray._default;
		}
	}

	// Token: 0x0600536E RID: 21358 RVA: 0x001FE264 File Offset: 0x001FC464
	public unsafe void Fill(long _fillValue)
	{
		for (int i = 0; i < 1; i++)
		{
			*((ref this.values.FixedElementField) + (IntPtr)i * 8) = _fillValue;
		}
	}

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x0600536F RID: 21359 RVA: 0x001FE28F File Offset: 0x001FC48F
	public bool IsDefault
	{
		get
		{
			return this.Equals(TextureFullArray._default);
		}
	}

	// Token: 0x06005370 RID: 21360 RVA: 0x001FE29C File Offset: 0x001FC49C
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

	// Token: 0x06005371 RID: 21361 RVA: 0x001FE2FC File Offset: 0x001FC4FC
	public unsafe void Write(BinaryWriter _bw)
	{
		for (int i = 0; i < 1; i++)
		{
			_bw.Write(*((ref this.values.FixedElementField) + (IntPtr)i * 8));
		}
	}

	// Token: 0x06005372 RID: 21362 RVA: 0x001FE32C File Offset: 0x001FC52C
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

	// Token: 0x06005373 RID: 21363 RVA: 0x001FE370 File Offset: 0x001FC570
	public override bool Equals(object obj)
	{
		if (obj is TextureFullArray)
		{
			TextureFullArray textureFullArray = (TextureFullArray)obj;
			return this.Equals(textureFullArray);
		}
		return false;
	}

	// Token: 0x06005374 RID: 21364 RVA: 0x001FE398 File Offset: 0x001FC598
	public override int GetHashCode()
	{
		int num = 17;
		for (int i = 0; i < 1; i++)
		{
			num = num * 31 + ((ref this.values.FixedElementField) + (IntPtr)i * 8).GetHashCode();
		}
		return num;
	}

	// Token: 0x06005375 RID: 21365 RVA: 0x001FE3D0 File Offset: 0x001FC5D0
	public static bool operator ==(TextureFullArray left, TextureFullArray right)
	{
		return left.Equals(right);
	}

	// Token: 0x06005376 RID: 21366 RVA: 0x001FE3DA File Offset: 0x001FC5DA
	public static bool operator !=(TextureFullArray left, TextureFullArray right)
	{
		return !left.Equals(right);
	}

	// Token: 0x040040FA RID: 16634
	[FixedBuffer(typeof(long), 1)]
	[PublicizedFrom(EAccessModifier.Private)]
	public TextureFullArray.<values>e__FixedBuffer values;

	// Token: 0x040040FB RID: 16635
	public static readonly TextureFullArray _default = new TextureFullArray(0L);

	// Token: 0x02000AEE RID: 2798
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 8)]
	public struct <values>e__FixedBuffer
	{
		// Token: 0x040040FC RID: 16636
		public long FixedElementField;
	}
}
