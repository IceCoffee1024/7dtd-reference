using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020014BB RID: 5307
public readonly ref struct StringSpan
{
	// Token: 0x0600A69D RID: 42653 RVA: 0x003E067B File Offset: 0x003DE87B
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan(ReadOnlySpan<char> span)
	{
		this.m_span = span;
	}

	// Token: 0x17001389 RID: 5001
	// (get) Token: 0x0600A69E RID: 42654 RVA: 0x003E0684 File Offset: 0x003DE884
	public int Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_span.Length;
		}
	}

	// Token: 0x0600A69F RID: 42655 RVA: 0x003E0691 File Offset: 0x003DE891
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<char> AsSpan()
	{
		return this.m_span;
	}

	// Token: 0x0600A6A0 RID: 42656 RVA: 0x003E0699 File Offset: 0x003DE899
	public override string ToString()
	{
		return new string(this.m_span);
	}

	// Token: 0x1700138A RID: 5002
	public ref char this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_span[index];
		}
	}

	// Token: 0x0600A6A2 RID: 42658 RVA: 0x003E06B4 File Offset: 0x003DE8B4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Slice(int start)
	{
		return this.m_span.Slice(start);
	}

	// Token: 0x0600A6A3 RID: 42659 RVA: 0x003E06C7 File Offset: 0x003DE8C7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Slice(int start, int length)
	{
		return this.m_span.Slice(start, length);
	}

	// Token: 0x0600A6A4 RID: 42660 RVA: 0x003E06DB File Offset: 0x003DE8DB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(StringSpan other, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return this.m_span == other.m_span || this.m_span.Equals(other.AsSpan(), comparisonType);
	}

	// Token: 0x0600A6A5 RID: 42661 RVA: 0x003E0705 File Offset: 0x003DE905
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(StringSpan other, StringComparison comparisonType = StringComparison.Ordinal)
	{
		if (!(this.m_span == other.m_span))
		{
			return this.m_span.CompareTo(other.AsSpan(), comparisonType);
		}
		return 0;
	}

	// Token: 0x0600A6A6 RID: 42662 RVA: 0x003E072F File Offset: 0x003DE92F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char value)
	{
		return this.IndexOf(value) >= 0;
	}

	// Token: 0x0600A6A7 RID: 42663 RVA: 0x003E073E File Offset: 0x003DE93E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char value, StringComparison comparisonType)
	{
		return this.IndexOf(value, comparisonType) >= 0;
	}

	// Token: 0x0600A6A8 RID: 42664 RVA: 0x003E074E File Offset: 0x003DE94E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(string value)
	{
		return this.IndexOf(value, StringComparison.Ordinal) >= 0;
	}

	// Token: 0x0600A6A9 RID: 42665 RVA: 0x003E0763 File Offset: 0x003DE963
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(string value, StringComparison comparisonType)
	{
		return this.IndexOf(value, comparisonType) >= 0;
	}

	// Token: 0x0600A6AA RID: 42666 RVA: 0x003E0778 File Offset: 0x003DE978
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(char value)
	{
		return this.m_span.IndexOf(value);
	}

	// Token: 0x0600A6AB RID: 42667 RVA: 0x003E0786 File Offset: 0x003DE986
	public int IndexOf(char value, StringComparison comparisonType)
	{
		return this.m_span.IndexOf(MemoryMarshal.CreateReadOnlySpan<char>(ref value, 1), comparisonType);
	}

	// Token: 0x0600A6AC RID: 42668 RVA: 0x003E079C File Offset: 0x003DE99C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(StringSpan value)
	{
		return this.m_span.IndexOf<char>(value.AsSpan());
	}

	// Token: 0x0600A6AD RID: 42669 RVA: 0x003E07B0 File Offset: 0x003DE9B0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(StringSpan value, StringComparison comparisonType)
	{
		return this.m_span.IndexOf(value.AsSpan(), comparisonType);
	}

	// Token: 0x0600A6AE RID: 42670 RVA: 0x003E07C5 File Offset: 0x003DE9C5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int LastIndexOf(char value)
	{
		return this.m_span.LastIndexOf(value);
	}

	// Token: 0x0600A6AF RID: 42671 RVA: 0x003E07D3 File Offset: 0x003DE9D3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int LastIndexOf(StringSpan value)
	{
		return this.m_span.LastIndexOf<char>(value.AsSpan());
	}

	// Token: 0x0600A6B0 RID: 42672 RVA: 0x003E07E7 File Offset: 0x003DE9E7
	public int IndexOfAny(StringSpan value)
	{
		return this.m_span.IndexOfAny<char>(value.AsSpan());
	}

	// Token: 0x0600A6B1 RID: 42673 RVA: 0x003E07FB File Offset: 0x003DE9FB
	public int LastIndexOfAny(StringSpan value)
	{
		return this.m_span.LastIndexOfAny<char>(value.AsSpan());
	}

	// Token: 0x0600A6B2 RID: 42674 RVA: 0x003E080F File Offset: 0x003DEA0F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.WhitespaceSplitEnumerator GetSplitEnumerator(StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.WhitespaceSplitEnumerator(this.m_span, options);
	}

	// Token: 0x0600A6B3 RID: 42675 RVA: 0x003E081D File Offset: 0x003DEA1D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.CharSplitEnumerator GetSplitEnumerator(char separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.CharSplitEnumerator(this.m_span, separator, options);
	}

	// Token: 0x0600A6B4 RID: 42676 RVA: 0x003E082C File Offset: 0x003DEA2C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.StringSplitEnumerator GetSplitEnumerator(ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.StringSplitEnumerator(this.m_span, separator, options);
	}

	// Token: 0x0600A6B5 RID: 42677 RVA: 0x003E083B File Offset: 0x003DEA3B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.SeparatorSplitAnyEnumerator GetSplitAnyEnumerator(string[] separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.SeparatorSplitAnyEnumerator(this.m_span, options, separator);
	}

	// Token: 0x0600A6B6 RID: 42678 RVA: 0x003E084C File Offset: 0x003DEA4C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Substring(int startIndex)
	{
		StringSpan stringSpan = this;
		return stringSpan.Slice(startIndex, stringSpan.Length - startIndex);
	}

	// Token: 0x0600A6B7 RID: 42679 RVA: 0x003E0874 File Offset: 0x003DEA74
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Substring(int startIndex, int length)
	{
		StringSpan stringSpan = this;
		return stringSpan.Slice(startIndex, startIndex + length - startIndex);
	}

	// Token: 0x0600A6B8 RID: 42680 RVA: 0x003E0898 File Offset: 0x003DEA98
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe StringSpan Trim()
	{
		int num = 0;
		while (num < this.Length && char.IsWhiteSpace((char)(*this[num])))
		{
			num++;
		}
		int num2 = this.Length - 1;
		while (num2 >= num && char.IsWhiteSpace((char)(*this[num2])))
		{
			num2--;
		}
		if (num > num2)
		{
			return default(StringSpan);
		}
		return this.Slice(num, num2 - num + 1);
	}

	// Token: 0x0600A6B9 RID: 42681 RVA: 0x003E0901 File Offset: 0x003DEB01
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator StringSpan(string str)
	{
		return new StringSpan(str);
	}

	// Token: 0x0600A6BA RID: 42682 RVA: 0x003E090E File Offset: 0x003DEB0E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator StringSpan(ReadOnlySpan<char> span)
	{
		return new StringSpan(span);
	}

	// Token: 0x0600A6BB RID: 42683 RVA: 0x003E0699 File Offset: 0x003DE899
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator string(StringSpan span)
	{
		return new string(span.m_span);
	}

	// Token: 0x0600A6BC RID: 42684 RVA: 0x003E0691 File Offset: 0x003DE891
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator ReadOnlySpan<char>(StringSpan span)
	{
		return span.m_span;
	}

	// Token: 0x0600A6BD RID: 42685 RVA: 0x003E0916 File Offset: 0x003DEB16
	public override bool Equals(object obj)
	{
		throw new NotSupportedException("StringSpan.Equals(object) is not supported. Use another method or the operator == instead.");
	}

	// Token: 0x0600A6BE RID: 42686 RVA: 0x003E0922 File Offset: 0x003DEB22
	public override int GetHashCode()
	{
		return SpanUtils.GetHashCode<char>(this.m_span);
	}

	// Token: 0x0600A6BF RID: 42687 RVA: 0x003E092F File Offset: 0x003DEB2F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(StringSpan left, StringSpan right)
	{
		return left.Equals(right, StringComparison.Ordinal);
	}

	// Token: 0x0600A6C0 RID: 42688 RVA: 0x003E093A File Offset: 0x003DEB3A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(StringSpan left, StringSpan right)
	{
		return !(left == right);
	}

	// Token: 0x04007BC6 RID: 31686
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReadOnlySpan<char> m_span;

	// Token: 0x020014BC RID: 5308
	public ref struct CharSplitEnumerator
	{
		// Token: 0x0600A6C1 RID: 42689 RVA: 0x003E0946 File Offset: 0x003DEB46
		public CharSplitEnumerator(ReadOnlySpan<char> span, char separator, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = span;
			this.m_separator = separator;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A6C2 RID: 42690 RVA: 0x003E0980 File Offset: 0x003DEB80
		public StringSpan.CharSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x0600A6C3 RID: 42691 RVA: 0x003E0988 File Offset: 0x003DEB88
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A6C4 RID: 42692 RVA: 0x003E0990 File Offset: 0x003DEB90
		public bool MoveNext()
		{
			while (this.MoveToNextInternal())
			{
				if (!this.m_removeEmptyEntries || this.m_current.Length != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A6C5 RID: 42693 RVA: 0x003E09B4 File Offset: 0x003DEBB4
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe bool MoveToNextInternal()
		{
			if (this.m_done)
			{
				return false;
			}
			for (int i = 0; i < this.m_remainder.Length; i++)
			{
				if (*this.m_remainder[i] == (ushort)this.m_separator)
				{
					ReadOnlySpan<char> readOnlySpan = this.m_remainder;
					this.m_current = readOnlySpan.Slice(0, i);
					readOnlySpan = this.m_remainder;
					int num = i + 1;
					this.m_remainder = readOnlySpan.Slice(num, readOnlySpan.Length - num);
					return true;
				}
			}
			this.m_current = this.m_remainder;
			this.m_remainder = default(ReadOnlySpan<char>);
			this.m_done = true;
			return true;
		}

		// Token: 0x04007BC7 RID: 31687
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlySpan<char> m_remainder;

		// Token: 0x04007BC8 RID: 31688
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly char m_separator;

		// Token: 0x04007BC9 RID: 31689
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007BCA RID: 31690
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007BCB RID: 31691
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014BD RID: 5309
	public ref struct StringSplitEnumerator
	{
		// Token: 0x0600A6C6 RID: 42694 RVA: 0x003E0A5A File Offset: 0x003DEC5A
		public StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = span;
			this.m_separator = separator;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A6C7 RID: 42695 RVA: 0x003E0A94 File Offset: 0x003DEC94
		public StringSpan.StringSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x0600A6C8 RID: 42696 RVA: 0x003E0A9C File Offset: 0x003DEC9C
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A6C9 RID: 42697 RVA: 0x003E0AA4 File Offset: 0x003DECA4
		public bool MoveNext()
		{
			while (this.MoveToNextInternal())
			{
				if (!this.m_removeEmptyEntries || this.m_current.Length != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A6CA RID: 42698 RVA: 0x003E0AC8 File Offset: 0x003DECC8
		[PublicizedFrom(EAccessModifier.Private)]
		public bool MoveToNextInternal()
		{
			if (this.m_done)
			{
				return false;
			}
			if (this.m_separator.Length <= 0)
			{
				this.m_current = this.m_remainder;
				this.m_remainder = default(ReadOnlySpan<char>);
				this.m_done = true;
				return true;
			}
			int num = this.m_remainder.Length + 1 - this.m_separator.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.m_remainder.Slice(i, this.m_separator.Length).CompareTo(this.m_separator, StringComparison.Ordinal) == 0)
				{
					ReadOnlySpan<char> readOnlySpan = this.m_remainder;
					this.m_current = readOnlySpan.Slice(0, i);
					readOnlySpan = this.m_remainder;
					int num2 = i + this.m_separator.Length;
					this.m_remainder = readOnlySpan.Slice(num2, readOnlySpan.Length - num2);
					return true;
				}
			}
			this.m_current = this.m_remainder;
			this.m_remainder = default(ReadOnlySpan<char>);
			this.m_done = true;
			return true;
		}

		// Token: 0x04007BCC RID: 31692
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlySpan<char> m_remainder;

		// Token: 0x04007BCD RID: 31693
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReadOnlySpan<char> m_separator;

		// Token: 0x04007BCE RID: 31694
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007BCF RID: 31695
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007BD0 RID: 31696
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014BE RID: 5310
	public ref struct WhitespaceSplitEnumerator
	{
		// Token: 0x0600A6CB RID: 42699 RVA: 0x003E0BCC File Offset: 0x003DEDCC
		public WhitespaceSplitEnumerator(ReadOnlySpan<char> text, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = text;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A6CC RID: 42700 RVA: 0x003E0C04 File Offset: 0x003DEE04
		public StringSpan.WhitespaceSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x0600A6CD RID: 42701 RVA: 0x003E0C0C File Offset: 0x003DEE0C
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A6CE RID: 42702 RVA: 0x003E0C14 File Offset: 0x003DEE14
		public bool MoveNext()
		{
			while (this.MoveToNextInternal())
			{
				if (!this.m_removeEmptyEntries || this.m_current.Length != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A6CF RID: 42703 RVA: 0x003E0C38 File Offset: 0x003DEE38
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe bool MoveToNextInternal()
		{
			if (this.m_done)
			{
				return false;
			}
			for (int i = 0; i < this.m_remainder.Length; i++)
			{
				if (char.IsWhiteSpace((char)(*this.m_remainder[i])))
				{
					StringSpan stringSpan = this.m_remainder;
					this.m_current = stringSpan.Slice(0, i);
					stringSpan = this.m_remainder;
					int num = i + 1;
					this.m_remainder = stringSpan.Slice(num, stringSpan.Length - num);
					return true;
				}
			}
			this.m_current = this.m_remainder;
			this.m_remainder = default(StringSpan);
			this.m_done = true;
			return true;
		}

		// Token: 0x04007BD1 RID: 31697
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_remainder;

		// Token: 0x04007BD2 RID: 31698
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007BD3 RID: 31699
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007BD4 RID: 31700
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014BF RID: 5311
	public ref struct SeparatorSplitAnyEnumerator
	{
		// Token: 0x0600A6D0 RID: 42704 RVA: 0x003E0CD4 File Offset: 0x003DEED4
		public SeparatorSplitAnyEnumerator(ReadOnlySpan<char> text, StringSplitOptions options = StringSplitOptions.None, params string[] separators)
		{
			this.m_remainder = text;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			bool flag = false;
			if (separators != null)
			{
				for (int i = 0; i < separators.Length; i++)
				{
					if (!string.IsNullOrEmpty(separators[i]))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				throw new ArgumentException("StringSplitEnumerator requires at least one non-empty separator");
			}
			this.m_separators = separators;
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A6D1 RID: 42705 RVA: 0x003E0D4F File Offset: 0x003DEF4F
		public StringSpan.SeparatorSplitAnyEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x0600A6D2 RID: 42706 RVA: 0x003E0D57 File Offset: 0x003DEF57
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A6D3 RID: 42707 RVA: 0x003E0D5F File Offset: 0x003DEF5F
		public bool MoveNext()
		{
			while (this.MoveToNextInternal())
			{
				if (!this.m_removeEmptyEntries || this.m_current.Length != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A6D4 RID: 42708 RVA: 0x003E0D84 File Offset: 0x003DEF84
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe bool MoveToNextInternal()
		{
			if (this.m_done)
			{
				return false;
			}
			for (int i = 0; i < this.m_remainder.Length; i++)
			{
				foreach (string text in this.m_separators)
				{
					if (!string.IsNullOrEmpty(text) && *this.m_remainder[i] == (ushort)text[0] && i + text.Length <= this.m_remainder.Length && this.m_remainder.Slice(i, text.Length).CompareTo(text.AsSpan(), StringComparison.Ordinal) == 0)
					{
						StringSpan stringSpan = this.m_remainder;
						this.m_current = stringSpan.Slice(0, i);
						stringSpan = this.m_remainder;
						int num = i + text.Length;
						this.m_remainder = stringSpan.Slice(num, stringSpan.Length - num);
						return true;
					}
				}
			}
			this.m_current = this.m_remainder;
			this.m_remainder = default(StringSpan);
			this.m_done = true;
			return true;
		}

		// Token: 0x04007BD5 RID: 31701
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_remainder;

		// Token: 0x04007BD6 RID: 31702
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007BD7 RID: 31703
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string[] m_separators;

		// Token: 0x04007BD8 RID: 31704
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007BD9 RID: 31705
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}
}
