using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020014EF RID: 5359
public readonly ref struct StringSpan
{
	// Token: 0x0600A7A8 RID: 42920 RVA: 0x003E78D3 File Offset: 0x003E5AD3
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan(ReadOnlySpan<char> span)
	{
		this.m_span = span;
	}

	// Token: 0x170013DA RID: 5082
	// (get) Token: 0x0600A7A9 RID: 42921 RVA: 0x003E78DC File Offset: 0x003E5ADC
	public int Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_span.Length;
		}
	}

	// Token: 0x0600A7AA RID: 42922 RVA: 0x003E78E9 File Offset: 0x003E5AE9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<char> AsSpan()
	{
		return this.m_span;
	}

	// Token: 0x0600A7AB RID: 42923 RVA: 0x003E78F1 File Offset: 0x003E5AF1
	public override string ToString()
	{
		return new string(this.m_span);
	}

	// Token: 0x170013DB RID: 5083
	public ref char this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_span[index];
		}
	}

	// Token: 0x0600A7AD RID: 42925 RVA: 0x003E790C File Offset: 0x003E5B0C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Slice(int start)
	{
		return this.m_span.Slice(start);
	}

	// Token: 0x0600A7AE RID: 42926 RVA: 0x003E791F File Offset: 0x003E5B1F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Slice(int start, int length)
	{
		return this.m_span.Slice(start, length);
	}

	// Token: 0x0600A7AF RID: 42927 RVA: 0x003E7933 File Offset: 0x003E5B33
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(StringSpan other, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return this.m_span == other.m_span || this.m_span.Equals(other.AsSpan(), comparisonType);
	}

	// Token: 0x0600A7B0 RID: 42928 RVA: 0x003E795D File Offset: 0x003E5B5D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(StringSpan other, StringComparison comparisonType = StringComparison.Ordinal)
	{
		if (!(this.m_span == other.m_span))
		{
			return this.m_span.CompareTo(other.AsSpan(), comparisonType);
		}
		return 0;
	}

	// Token: 0x0600A7B1 RID: 42929 RVA: 0x003E7987 File Offset: 0x003E5B87
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char value)
	{
		return this.IndexOf(value) >= 0;
	}

	// Token: 0x0600A7B2 RID: 42930 RVA: 0x003E7996 File Offset: 0x003E5B96
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char value, StringComparison comparisonType)
	{
		return this.IndexOf(value, comparisonType) >= 0;
	}

	// Token: 0x0600A7B3 RID: 42931 RVA: 0x003E79A6 File Offset: 0x003E5BA6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(string value)
	{
		return this.IndexOf(value, StringComparison.Ordinal) >= 0;
	}

	// Token: 0x0600A7B4 RID: 42932 RVA: 0x003E79BB File Offset: 0x003E5BBB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(string value, StringComparison comparisonType)
	{
		return this.IndexOf(value, comparisonType) >= 0;
	}

	// Token: 0x0600A7B5 RID: 42933 RVA: 0x003E79D0 File Offset: 0x003E5BD0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(char value)
	{
		return this.m_span.IndexOf(value);
	}

	// Token: 0x0600A7B6 RID: 42934 RVA: 0x003E79DE File Offset: 0x003E5BDE
	public int IndexOf(char value, StringComparison comparisonType)
	{
		return this.m_span.IndexOf(MemoryMarshal.CreateReadOnlySpan<char>(ref value, 1), comparisonType);
	}

	// Token: 0x0600A7B7 RID: 42935 RVA: 0x003E79F4 File Offset: 0x003E5BF4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(StringSpan value)
	{
		return this.m_span.IndexOf<char>(value.AsSpan());
	}

	// Token: 0x0600A7B8 RID: 42936 RVA: 0x003E7A08 File Offset: 0x003E5C08
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(StringSpan value, StringComparison comparisonType)
	{
		return this.m_span.IndexOf(value.AsSpan(), comparisonType);
	}

	// Token: 0x0600A7B9 RID: 42937 RVA: 0x003E7A1D File Offset: 0x003E5C1D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int LastIndexOf(char value)
	{
		return this.m_span.LastIndexOf(value);
	}

	// Token: 0x0600A7BA RID: 42938 RVA: 0x003E7A2B File Offset: 0x003E5C2B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int LastIndexOf(StringSpan value)
	{
		return this.m_span.LastIndexOf<char>(value.AsSpan());
	}

	// Token: 0x0600A7BB RID: 42939 RVA: 0x003E7A3F File Offset: 0x003E5C3F
	public int IndexOfAny(StringSpan value)
	{
		return this.m_span.IndexOfAny<char>(value.AsSpan());
	}

	// Token: 0x0600A7BC RID: 42940 RVA: 0x003E7A53 File Offset: 0x003E5C53
	public int LastIndexOfAny(StringSpan value)
	{
		return this.m_span.LastIndexOfAny<char>(value.AsSpan());
	}

	// Token: 0x0600A7BD RID: 42941 RVA: 0x003E7A67 File Offset: 0x003E5C67
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.WhitespaceSplitEnumerator GetSplitEnumerator(StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.WhitespaceSplitEnumerator(this.m_span, options);
	}

	// Token: 0x0600A7BE RID: 42942 RVA: 0x003E7A75 File Offset: 0x003E5C75
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.CharSplitEnumerator GetSplitEnumerator(char separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.CharSplitEnumerator(this.m_span, separator, options);
	}

	// Token: 0x0600A7BF RID: 42943 RVA: 0x003E7A84 File Offset: 0x003E5C84
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.StringSplitEnumerator GetSplitEnumerator(ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.StringSplitEnumerator(this.m_span, separator, options);
	}

	// Token: 0x0600A7C0 RID: 42944 RVA: 0x003E7A93 File Offset: 0x003E5C93
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.SeparatorSplitAnyEnumerator GetSplitAnyEnumerator(string[] separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.SeparatorSplitAnyEnumerator(this.m_span, options, separator);
	}

	// Token: 0x0600A7C1 RID: 42945 RVA: 0x003E7AA4 File Offset: 0x003E5CA4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Substring(int startIndex)
	{
		StringSpan stringSpan = this;
		return stringSpan.Slice(startIndex, stringSpan.Length - startIndex);
	}

	// Token: 0x0600A7C2 RID: 42946 RVA: 0x003E7ACC File Offset: 0x003E5CCC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Substring(int startIndex, int length)
	{
		StringSpan stringSpan = this;
		return stringSpan.Slice(startIndex, startIndex + length - startIndex);
	}

	// Token: 0x0600A7C3 RID: 42947 RVA: 0x003E7AF0 File Offset: 0x003E5CF0
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

	// Token: 0x0600A7C4 RID: 42948 RVA: 0x003E7B59 File Offset: 0x003E5D59
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator StringSpan(string str)
	{
		return new StringSpan(str);
	}

	// Token: 0x0600A7C5 RID: 42949 RVA: 0x003E7B66 File Offset: 0x003E5D66
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator StringSpan(ReadOnlySpan<char> span)
	{
		return new StringSpan(span);
	}

	// Token: 0x0600A7C6 RID: 42950 RVA: 0x003E78F1 File Offset: 0x003E5AF1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator string(StringSpan span)
	{
		return new string(span.m_span);
	}

	// Token: 0x0600A7C7 RID: 42951 RVA: 0x003E78E9 File Offset: 0x003E5AE9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator ReadOnlySpan<char>(StringSpan span)
	{
		return span.m_span;
	}

	// Token: 0x0600A7C8 RID: 42952 RVA: 0x003E7B6E File Offset: 0x003E5D6E
	public override bool Equals(object obj)
	{
		throw new NotSupportedException("StringSpan.Equals(object) is not supported. Use another method or the operator == instead.");
	}

	// Token: 0x0600A7C9 RID: 42953 RVA: 0x003E7B7A File Offset: 0x003E5D7A
	public override int GetHashCode()
	{
		return SpanUtils.GetHashCode<char>(this.m_span);
	}

	// Token: 0x0600A7CA RID: 42954 RVA: 0x003E7B87 File Offset: 0x003E5D87
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(StringSpan left, StringSpan right)
	{
		return left.Equals(right, StringComparison.Ordinal);
	}

	// Token: 0x0600A7CB RID: 42955 RVA: 0x003E7B92 File Offset: 0x003E5D92
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(StringSpan left, StringSpan right)
	{
		return !(left == right);
	}

	// Token: 0x04007C8A RID: 31882
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReadOnlySpan<char> m_span;

	// Token: 0x020014F0 RID: 5360
	public ref struct CharSplitEnumerator
	{
		// Token: 0x0600A7CC RID: 42956 RVA: 0x003E7B9E File Offset: 0x003E5D9E
		public CharSplitEnumerator(ReadOnlySpan<char> span, char separator, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = span;
			this.m_separator = separator;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A7CD RID: 42957 RVA: 0x003E7BD8 File Offset: 0x003E5DD8
		public StringSpan.CharSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x0600A7CE RID: 42958 RVA: 0x003E7BE0 File Offset: 0x003E5DE0
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A7CF RID: 42959 RVA: 0x003E7BE8 File Offset: 0x003E5DE8
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

		// Token: 0x0600A7D0 RID: 42960 RVA: 0x003E7C0C File Offset: 0x003E5E0C
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

		// Token: 0x04007C8B RID: 31883
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlySpan<char> m_remainder;

		// Token: 0x04007C8C RID: 31884
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly char m_separator;

		// Token: 0x04007C8D RID: 31885
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007C8E RID: 31886
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007C8F RID: 31887
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014F1 RID: 5361
	public ref struct StringSplitEnumerator
	{
		// Token: 0x0600A7D1 RID: 42961 RVA: 0x003E7CB2 File Offset: 0x003E5EB2
		public StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = span;
			this.m_separator = separator;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A7D2 RID: 42962 RVA: 0x003E7CEC File Offset: 0x003E5EEC
		public StringSpan.StringSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x0600A7D3 RID: 42963 RVA: 0x003E7CF4 File Offset: 0x003E5EF4
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A7D4 RID: 42964 RVA: 0x003E7CFC File Offset: 0x003E5EFC
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

		// Token: 0x0600A7D5 RID: 42965 RVA: 0x003E7D20 File Offset: 0x003E5F20
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

		// Token: 0x04007C90 RID: 31888
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlySpan<char> m_remainder;

		// Token: 0x04007C91 RID: 31889
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReadOnlySpan<char> m_separator;

		// Token: 0x04007C92 RID: 31890
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007C93 RID: 31891
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007C94 RID: 31892
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014F2 RID: 5362
	public ref struct WhitespaceSplitEnumerator
	{
		// Token: 0x0600A7D6 RID: 42966 RVA: 0x003E7E24 File Offset: 0x003E6024
		public WhitespaceSplitEnumerator(ReadOnlySpan<char> text, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = text;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A7D7 RID: 42967 RVA: 0x003E7E5C File Offset: 0x003E605C
		public StringSpan.WhitespaceSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x0600A7D8 RID: 42968 RVA: 0x003E7E64 File Offset: 0x003E6064
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A7D9 RID: 42969 RVA: 0x003E7E6C File Offset: 0x003E606C
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

		// Token: 0x0600A7DA RID: 42970 RVA: 0x003E7E90 File Offset: 0x003E6090
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

		// Token: 0x04007C95 RID: 31893
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_remainder;

		// Token: 0x04007C96 RID: 31894
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007C97 RID: 31895
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007C98 RID: 31896
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014F3 RID: 5363
	public ref struct SeparatorSplitAnyEnumerator
	{
		// Token: 0x0600A7DB RID: 42971 RVA: 0x003E7F2C File Offset: 0x003E612C
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

		// Token: 0x0600A7DC RID: 42972 RVA: 0x003E7FA7 File Offset: 0x003E61A7
		public StringSpan.SeparatorSplitAnyEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x0600A7DD RID: 42973 RVA: 0x003E7FAF File Offset: 0x003E61AF
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A7DE RID: 42974 RVA: 0x003E7FB7 File Offset: 0x003E61B7
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

		// Token: 0x0600A7DF RID: 42975 RVA: 0x003E7FDC File Offset: 0x003E61DC
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

		// Token: 0x04007C99 RID: 31897
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_remainder;

		// Token: 0x04007C9A RID: 31898
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007C9B RID: 31899
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string[] m_separators;

		// Token: 0x04007C9C RID: 31900
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007C9D RID: 31901
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}
}
