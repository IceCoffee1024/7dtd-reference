using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020014DD RID: 5341
public readonly ref struct StringSpan
{
	// Token: 0x0600A750 RID: 42832 RVA: 0x003E54CF File Offset: 0x003E36CF
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan(ReadOnlySpan<char> span)
	{
		this.m_span = span;
	}

	// Token: 0x170013CE RID: 5070
	// (get) Token: 0x0600A751 RID: 42833 RVA: 0x003E54D8 File Offset: 0x003E36D8
	public int Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_span.Length;
		}
	}

	// Token: 0x0600A752 RID: 42834 RVA: 0x003E54E5 File Offset: 0x003E36E5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<char> AsSpan()
	{
		return this.m_span;
	}

	// Token: 0x0600A753 RID: 42835 RVA: 0x003E54ED File Offset: 0x003E36ED
	public override string ToString()
	{
		return new string(this.m_span);
	}

	// Token: 0x170013CF RID: 5071
	public ref char this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_span[index];
		}
	}

	// Token: 0x0600A755 RID: 42837 RVA: 0x003E5508 File Offset: 0x003E3708
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Slice(int start)
	{
		return this.m_span.Slice(start);
	}

	// Token: 0x0600A756 RID: 42838 RVA: 0x003E551B File Offset: 0x003E371B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Slice(int start, int length)
	{
		return this.m_span.Slice(start, length);
	}

	// Token: 0x0600A757 RID: 42839 RVA: 0x003E552F File Offset: 0x003E372F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(StringSpan other, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return this.m_span == other.m_span || this.m_span.Equals(other.AsSpan(), comparisonType);
	}

	// Token: 0x0600A758 RID: 42840 RVA: 0x003E5559 File Offset: 0x003E3759
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(StringSpan other, StringComparison comparisonType = StringComparison.Ordinal)
	{
		if (!(this.m_span == other.m_span))
		{
			return this.m_span.CompareTo(other.AsSpan(), comparisonType);
		}
		return 0;
	}

	// Token: 0x0600A759 RID: 42841 RVA: 0x003E5583 File Offset: 0x003E3783
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char value)
	{
		return this.IndexOf(value) >= 0;
	}

	// Token: 0x0600A75A RID: 42842 RVA: 0x003E5592 File Offset: 0x003E3792
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char value, StringComparison comparisonType)
	{
		return this.IndexOf(value, comparisonType) >= 0;
	}

	// Token: 0x0600A75B RID: 42843 RVA: 0x003E55A2 File Offset: 0x003E37A2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(string value)
	{
		return this.IndexOf(value, StringComparison.Ordinal) >= 0;
	}

	// Token: 0x0600A75C RID: 42844 RVA: 0x003E55B7 File Offset: 0x003E37B7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(string value, StringComparison comparisonType)
	{
		return this.IndexOf(value, comparisonType) >= 0;
	}

	// Token: 0x0600A75D RID: 42845 RVA: 0x003E55CC File Offset: 0x003E37CC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(char value)
	{
		return this.m_span.IndexOf(value);
	}

	// Token: 0x0600A75E RID: 42846 RVA: 0x003E55DA File Offset: 0x003E37DA
	public int IndexOf(char value, StringComparison comparisonType)
	{
		return this.m_span.IndexOf(MemoryMarshal.CreateReadOnlySpan<char>(ref value, 1), comparisonType);
	}

	// Token: 0x0600A75F RID: 42847 RVA: 0x003E55F0 File Offset: 0x003E37F0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(StringSpan value)
	{
		return this.m_span.IndexOf<char>(value.AsSpan());
	}

	// Token: 0x0600A760 RID: 42848 RVA: 0x003E5604 File Offset: 0x003E3804
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(StringSpan value, StringComparison comparisonType)
	{
		return this.m_span.IndexOf(value.AsSpan(), comparisonType);
	}

	// Token: 0x0600A761 RID: 42849 RVA: 0x003E5619 File Offset: 0x003E3819
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int LastIndexOf(char value)
	{
		return this.m_span.LastIndexOf(value);
	}

	// Token: 0x0600A762 RID: 42850 RVA: 0x003E5627 File Offset: 0x003E3827
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int LastIndexOf(StringSpan value)
	{
		return this.m_span.LastIndexOf<char>(value.AsSpan());
	}

	// Token: 0x0600A763 RID: 42851 RVA: 0x003E563B File Offset: 0x003E383B
	public int IndexOfAny(StringSpan value)
	{
		return this.m_span.IndexOfAny<char>(value.AsSpan());
	}

	// Token: 0x0600A764 RID: 42852 RVA: 0x003E564F File Offset: 0x003E384F
	public int LastIndexOfAny(StringSpan value)
	{
		return this.m_span.LastIndexOfAny<char>(value.AsSpan());
	}

	// Token: 0x0600A765 RID: 42853 RVA: 0x003E5663 File Offset: 0x003E3863
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.WhitespaceSplitEnumerator GetSplitEnumerator(StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.WhitespaceSplitEnumerator(this.m_span, options);
	}

	// Token: 0x0600A766 RID: 42854 RVA: 0x003E5671 File Offset: 0x003E3871
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.CharSplitEnumerator GetSplitEnumerator(char separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.CharSplitEnumerator(this.m_span, separator, options);
	}

	// Token: 0x0600A767 RID: 42855 RVA: 0x003E5680 File Offset: 0x003E3880
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.StringSplitEnumerator GetSplitEnumerator(ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.StringSplitEnumerator(this.m_span, separator, options);
	}

	// Token: 0x0600A768 RID: 42856 RVA: 0x003E568F File Offset: 0x003E388F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.SeparatorSplitAnyEnumerator GetSplitAnyEnumerator(string[] separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.SeparatorSplitAnyEnumerator(this.m_span, options, separator);
	}

	// Token: 0x0600A769 RID: 42857 RVA: 0x003E56A0 File Offset: 0x003E38A0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Substring(int startIndex)
	{
		StringSpan stringSpan = this;
		return stringSpan.Slice(startIndex, stringSpan.Length - startIndex);
	}

	// Token: 0x0600A76A RID: 42858 RVA: 0x003E56C8 File Offset: 0x003E38C8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Substring(int startIndex, int length)
	{
		StringSpan stringSpan = this;
		return stringSpan.Slice(startIndex, startIndex + length - startIndex);
	}

	// Token: 0x0600A76B RID: 42859 RVA: 0x003E56EC File Offset: 0x003E38EC
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

	// Token: 0x0600A76C RID: 42860 RVA: 0x003E5755 File Offset: 0x003E3955
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator StringSpan(string str)
	{
		return new StringSpan(str);
	}

	// Token: 0x0600A76D RID: 42861 RVA: 0x003E5762 File Offset: 0x003E3962
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator StringSpan(ReadOnlySpan<char> span)
	{
		return new StringSpan(span);
	}

	// Token: 0x0600A76E RID: 42862 RVA: 0x003E54ED File Offset: 0x003E36ED
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator string(StringSpan span)
	{
		return new string(span.m_span);
	}

	// Token: 0x0600A76F RID: 42863 RVA: 0x003E54E5 File Offset: 0x003E36E5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator ReadOnlySpan<char>(StringSpan span)
	{
		return span.m_span;
	}

	// Token: 0x0600A770 RID: 42864 RVA: 0x003E576A File Offset: 0x003E396A
	public override bool Equals(object obj)
	{
		throw new NotSupportedException("StringSpan.Equals(object) is not supported. Use another method or the operator == instead.");
	}

	// Token: 0x0600A771 RID: 42865 RVA: 0x003E5776 File Offset: 0x003E3976
	public override int GetHashCode()
	{
		return SpanUtils.GetHashCode<char>(this.m_span);
	}

	// Token: 0x0600A772 RID: 42866 RVA: 0x003E5783 File Offset: 0x003E3983
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(StringSpan left, StringSpan right)
	{
		return left.Equals(right, StringComparison.Ordinal);
	}

	// Token: 0x0600A773 RID: 42867 RVA: 0x003E578E File Offset: 0x003E398E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(StringSpan left, StringSpan right)
	{
		return !(left == right);
	}

	// Token: 0x04007C49 RID: 31817
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReadOnlySpan<char> m_span;

	// Token: 0x020014DE RID: 5342
	public ref struct CharSplitEnumerator
	{
		// Token: 0x0600A774 RID: 42868 RVA: 0x003E579A File Offset: 0x003E399A
		public CharSplitEnumerator(ReadOnlySpan<char> span, char separator, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = span;
			this.m_separator = separator;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A775 RID: 42869 RVA: 0x003E57D4 File Offset: 0x003E39D4
		public StringSpan.CharSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x0600A776 RID: 42870 RVA: 0x003E57DC File Offset: 0x003E39DC
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A777 RID: 42871 RVA: 0x003E57E4 File Offset: 0x003E39E4
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

		// Token: 0x0600A778 RID: 42872 RVA: 0x003E5808 File Offset: 0x003E3A08
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

		// Token: 0x04007C4A RID: 31818
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlySpan<char> m_remainder;

		// Token: 0x04007C4B RID: 31819
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly char m_separator;

		// Token: 0x04007C4C RID: 31820
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007C4D RID: 31821
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007C4E RID: 31822
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014DF RID: 5343
	public ref struct StringSplitEnumerator
	{
		// Token: 0x0600A779 RID: 42873 RVA: 0x003E58AE File Offset: 0x003E3AAE
		public StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = span;
			this.m_separator = separator;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A77A RID: 42874 RVA: 0x003E58E8 File Offset: 0x003E3AE8
		public StringSpan.StringSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x0600A77B RID: 42875 RVA: 0x003E58F0 File Offset: 0x003E3AF0
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A77C RID: 42876 RVA: 0x003E58F8 File Offset: 0x003E3AF8
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

		// Token: 0x0600A77D RID: 42877 RVA: 0x003E591C File Offset: 0x003E3B1C
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

		// Token: 0x04007C4F RID: 31823
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlySpan<char> m_remainder;

		// Token: 0x04007C50 RID: 31824
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReadOnlySpan<char> m_separator;

		// Token: 0x04007C51 RID: 31825
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007C52 RID: 31826
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007C53 RID: 31827
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014E0 RID: 5344
	public ref struct WhitespaceSplitEnumerator
	{
		// Token: 0x0600A77E RID: 42878 RVA: 0x003E5A20 File Offset: 0x003E3C20
		public WhitespaceSplitEnumerator(ReadOnlySpan<char> text, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = text;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A77F RID: 42879 RVA: 0x003E5A58 File Offset: 0x003E3C58
		public StringSpan.WhitespaceSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x0600A780 RID: 42880 RVA: 0x003E5A60 File Offset: 0x003E3C60
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A781 RID: 42881 RVA: 0x003E5A68 File Offset: 0x003E3C68
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

		// Token: 0x0600A782 RID: 42882 RVA: 0x003E5A8C File Offset: 0x003E3C8C
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

		// Token: 0x04007C54 RID: 31828
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_remainder;

		// Token: 0x04007C55 RID: 31829
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007C56 RID: 31830
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007C57 RID: 31831
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014E1 RID: 5345
	public ref struct SeparatorSplitAnyEnumerator
	{
		// Token: 0x0600A783 RID: 42883 RVA: 0x003E5B28 File Offset: 0x003E3D28
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

		// Token: 0x0600A784 RID: 42884 RVA: 0x003E5BA3 File Offset: 0x003E3DA3
		public StringSpan.SeparatorSplitAnyEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x0600A785 RID: 42885 RVA: 0x003E5BAB File Offset: 0x003E3DAB
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A786 RID: 42886 RVA: 0x003E5BB3 File Offset: 0x003E3DB3
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

		// Token: 0x0600A787 RID: 42887 RVA: 0x003E5BD8 File Offset: 0x003E3DD8
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

		// Token: 0x04007C58 RID: 31832
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_remainder;

		// Token: 0x04007C59 RID: 31833
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007C5A RID: 31834
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string[] m_separators;

		// Token: 0x04007C5B RID: 31835
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007C5C RID: 31836
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}
}
