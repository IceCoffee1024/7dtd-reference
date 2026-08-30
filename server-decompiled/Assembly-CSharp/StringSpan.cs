using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020014A0 RID: 5280
public readonly ref struct StringSpan
{
	// Token: 0x0600A5A0 RID: 42400 RVA: 0x003DB33B File Offset: 0x003D953B
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan(ReadOnlySpan<char> span)
	{
		this.m_span = span;
	}

	// Token: 0x17001350 RID: 4944
	// (get) Token: 0x0600A5A1 RID: 42401 RVA: 0x003DB344 File Offset: 0x003D9544
	public int Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_span.Length;
		}
	}

	// Token: 0x0600A5A2 RID: 42402 RVA: 0x003DB351 File Offset: 0x003D9551
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<char> AsSpan()
	{
		return this.m_span;
	}

	// Token: 0x0600A5A3 RID: 42403 RVA: 0x003DB359 File Offset: 0x003D9559
	public override string ToString()
	{
		return new string(this.m_span);
	}

	// Token: 0x17001351 RID: 4945
	public ref char this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.m_span[index];
		}
	}

	// Token: 0x0600A5A5 RID: 42405 RVA: 0x003DB374 File Offset: 0x003D9574
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Slice(int start)
	{
		return this.m_span.Slice(start);
	}

	// Token: 0x0600A5A6 RID: 42406 RVA: 0x003DB387 File Offset: 0x003D9587
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Slice(int start, int length)
	{
		return this.m_span.Slice(start, length);
	}

	// Token: 0x0600A5A7 RID: 42407 RVA: 0x003DB39B File Offset: 0x003D959B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(StringSpan other, StringComparison comparisonType = StringComparison.Ordinal)
	{
		return this.m_span == other.m_span || this.m_span.Equals(other.AsSpan(), comparisonType);
	}

	// Token: 0x0600A5A8 RID: 42408 RVA: 0x003DB3C5 File Offset: 0x003D95C5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(StringSpan other, StringComparison comparisonType = StringComparison.Ordinal)
	{
		if (!(this.m_span == other.m_span))
		{
			return this.m_span.CompareTo(other.AsSpan(), comparisonType);
		}
		return 0;
	}

	// Token: 0x0600A5A9 RID: 42409 RVA: 0x003DB3EF File Offset: 0x003D95EF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char value)
	{
		return this.IndexOf(value) >= 0;
	}

	// Token: 0x0600A5AA RID: 42410 RVA: 0x003DB3FE File Offset: 0x003D95FE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(char value, StringComparison comparisonType)
	{
		return this.IndexOf(value, comparisonType) >= 0;
	}

	// Token: 0x0600A5AB RID: 42411 RVA: 0x003DB40E File Offset: 0x003D960E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(string value)
	{
		return this.IndexOf(value, StringComparison.Ordinal) >= 0;
	}

	// Token: 0x0600A5AC RID: 42412 RVA: 0x003DB423 File Offset: 0x003D9623
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(string value, StringComparison comparisonType)
	{
		return this.IndexOf(value, comparisonType) >= 0;
	}

	// Token: 0x0600A5AD RID: 42413 RVA: 0x003DB438 File Offset: 0x003D9638
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(char value)
	{
		return this.m_span.IndexOf(value);
	}

	// Token: 0x0600A5AE RID: 42414 RVA: 0x003DB446 File Offset: 0x003D9646
	public int IndexOf(char value, StringComparison comparisonType)
	{
		return this.m_span.IndexOf(MemoryMarshal.CreateReadOnlySpan<char>(ref value, 1), comparisonType);
	}

	// Token: 0x0600A5AF RID: 42415 RVA: 0x003DB45C File Offset: 0x003D965C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(StringSpan value)
	{
		return this.m_span.IndexOf<char>(value.AsSpan());
	}

	// Token: 0x0600A5B0 RID: 42416 RVA: 0x003DB470 File Offset: 0x003D9670
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int IndexOf(StringSpan value, StringComparison comparisonType)
	{
		return this.m_span.IndexOf(value.AsSpan(), comparisonType);
	}

	// Token: 0x0600A5B1 RID: 42417 RVA: 0x003DB485 File Offset: 0x003D9685
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int LastIndexOf(char value)
	{
		return this.m_span.LastIndexOf(value);
	}

	// Token: 0x0600A5B2 RID: 42418 RVA: 0x003DB493 File Offset: 0x003D9693
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int LastIndexOf(StringSpan value)
	{
		return this.m_span.LastIndexOf<char>(value.AsSpan());
	}

	// Token: 0x0600A5B3 RID: 42419 RVA: 0x003DB4A7 File Offset: 0x003D96A7
	public int IndexOfAny(StringSpan value)
	{
		return this.m_span.IndexOfAny<char>(value.AsSpan());
	}

	// Token: 0x0600A5B4 RID: 42420 RVA: 0x003DB4BB File Offset: 0x003D96BB
	public int LastIndexOfAny(StringSpan value)
	{
		return this.m_span.LastIndexOfAny<char>(value.AsSpan());
	}

	// Token: 0x0600A5B5 RID: 42421 RVA: 0x003DB4CF File Offset: 0x003D96CF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.WhitespaceSplitEnumerator GetSplitEnumerator(StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.WhitespaceSplitEnumerator(this.m_span, options);
	}

	// Token: 0x0600A5B6 RID: 42422 RVA: 0x003DB4DD File Offset: 0x003D96DD
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.CharSplitEnumerator GetSplitEnumerator(char separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.CharSplitEnumerator(this.m_span, separator, options);
	}

	// Token: 0x0600A5B7 RID: 42423 RVA: 0x003DB4EC File Offset: 0x003D96EC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.StringSplitEnumerator GetSplitEnumerator(ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.StringSplitEnumerator(this.m_span, separator, options);
	}

	// Token: 0x0600A5B8 RID: 42424 RVA: 0x003DB4FB File Offset: 0x003D96FB
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan.SeparatorSplitAnyEnumerator GetSplitAnyEnumerator(string[] separator, StringSplitOptions options = StringSplitOptions.None)
	{
		return new StringSpan.SeparatorSplitAnyEnumerator(this.m_span, options, separator);
	}

	// Token: 0x0600A5B9 RID: 42425 RVA: 0x003DB50C File Offset: 0x003D970C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Substring(int startIndex)
	{
		StringSpan stringSpan = this;
		return stringSpan.Slice(startIndex, stringSpan.Length - startIndex);
	}

	// Token: 0x0600A5BA RID: 42426 RVA: 0x003DB534 File Offset: 0x003D9734
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public StringSpan Substring(int startIndex, int length)
	{
		StringSpan stringSpan = this;
		return stringSpan.Slice(startIndex, startIndex + length - startIndex);
	}

	// Token: 0x0600A5BB RID: 42427 RVA: 0x003DB558 File Offset: 0x003D9758
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

	// Token: 0x0600A5BC RID: 42428 RVA: 0x003DB5C1 File Offset: 0x003D97C1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator StringSpan(string str)
	{
		return new StringSpan(str);
	}

	// Token: 0x0600A5BD RID: 42429 RVA: 0x003DB5CE File Offset: 0x003D97CE
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator StringSpan(ReadOnlySpan<char> span)
	{
		return new StringSpan(span);
	}

	// Token: 0x0600A5BE RID: 42430 RVA: 0x003DB359 File Offset: 0x003D9559
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator string(StringSpan span)
	{
		return new string(span.m_span);
	}

	// Token: 0x0600A5BF RID: 42431 RVA: 0x003DB351 File Offset: 0x003D9551
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static explicit operator ReadOnlySpan<char>(StringSpan span)
	{
		return span.m_span;
	}

	// Token: 0x0600A5C0 RID: 42432 RVA: 0x003DB5D6 File Offset: 0x003D97D6
	public override bool Equals(object obj)
	{
		throw new NotSupportedException("StringSpan.Equals(object) is not supported. Use another method or the operator == instead.");
	}

	// Token: 0x0600A5C1 RID: 42433 RVA: 0x003DB5E2 File Offset: 0x003D97E2
	public override int GetHashCode()
	{
		return SpanUtils.GetHashCode<char>(this.m_span);
	}

	// Token: 0x0600A5C2 RID: 42434 RVA: 0x003DB5EF File Offset: 0x003D97EF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(StringSpan left, StringSpan right)
	{
		return left.Equals(right, StringComparison.Ordinal);
	}

	// Token: 0x0600A5C3 RID: 42435 RVA: 0x003DB5FA File Offset: 0x003D97FA
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(StringSpan left, StringSpan right)
	{
		return !(left == right);
	}

	// Token: 0x04007AED RID: 31469
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ReadOnlySpan<char> m_span;

	// Token: 0x020014A1 RID: 5281
	public ref struct CharSplitEnumerator
	{
		// Token: 0x0600A5C4 RID: 42436 RVA: 0x003DB606 File Offset: 0x003D9806
		public CharSplitEnumerator(ReadOnlySpan<char> span, char separator, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = span;
			this.m_separator = separator;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A5C5 RID: 42437 RVA: 0x003DB640 File Offset: 0x003D9840
		public StringSpan.CharSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x0600A5C6 RID: 42438 RVA: 0x003DB648 File Offset: 0x003D9848
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A5C7 RID: 42439 RVA: 0x003DB650 File Offset: 0x003D9850
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

		// Token: 0x0600A5C8 RID: 42440 RVA: 0x003DB674 File Offset: 0x003D9874
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

		// Token: 0x04007AEE RID: 31470
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlySpan<char> m_remainder;

		// Token: 0x04007AEF RID: 31471
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly char m_separator;

		// Token: 0x04007AF0 RID: 31472
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007AF1 RID: 31473
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007AF2 RID: 31474
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014A2 RID: 5282
	public ref struct StringSplitEnumerator
	{
		// Token: 0x0600A5C9 RID: 42441 RVA: 0x003DB71A File Offset: 0x003D991A
		public StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = span;
			this.m_separator = separator;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A5CA RID: 42442 RVA: 0x003DB754 File Offset: 0x003D9954
		public StringSpan.StringSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x0600A5CB RID: 42443 RVA: 0x003DB75C File Offset: 0x003D995C
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A5CC RID: 42444 RVA: 0x003DB764 File Offset: 0x003D9964
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

		// Token: 0x0600A5CD RID: 42445 RVA: 0x003DB788 File Offset: 0x003D9988
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

		// Token: 0x04007AF3 RID: 31475
		[PublicizedFrom(EAccessModifier.Private)]
		public ReadOnlySpan<char> m_remainder;

		// Token: 0x04007AF4 RID: 31476
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ReadOnlySpan<char> m_separator;

		// Token: 0x04007AF5 RID: 31477
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007AF6 RID: 31478
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007AF7 RID: 31479
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014A3 RID: 5283
	public ref struct WhitespaceSplitEnumerator
	{
		// Token: 0x0600A5CE RID: 42446 RVA: 0x003DB88C File Offset: 0x003D9A8C
		public WhitespaceSplitEnumerator(ReadOnlySpan<char> text, StringSplitOptions options = StringSplitOptions.None)
		{
			this.m_remainder = text;
			this.m_removeEmptyEntries = options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
			this.m_current = default(StringSpan);
			this.m_done = false;
		}

		// Token: 0x0600A5CF RID: 42447 RVA: 0x003DB8C4 File Offset: 0x003D9AC4
		public StringSpan.WhitespaceSplitEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x0600A5D0 RID: 42448 RVA: 0x003DB8CC File Offset: 0x003D9ACC
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A5D1 RID: 42449 RVA: 0x003DB8D4 File Offset: 0x003D9AD4
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

		// Token: 0x0600A5D2 RID: 42450 RVA: 0x003DB8F8 File Offset: 0x003D9AF8
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

		// Token: 0x04007AF8 RID: 31480
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_remainder;

		// Token: 0x04007AF9 RID: 31481
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007AFA RID: 31482
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007AFB RID: 31483
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}

	// Token: 0x020014A4 RID: 5284
	public ref struct SeparatorSplitAnyEnumerator
	{
		// Token: 0x0600A5D3 RID: 42451 RVA: 0x003DB994 File Offset: 0x003D9B94
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

		// Token: 0x0600A5D4 RID: 42452 RVA: 0x003DBA0F File Offset: 0x003D9C0F
		public StringSpan.SeparatorSplitAnyEnumerator GetEnumerator()
		{
			return this;
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x0600A5D5 RID: 42453 RVA: 0x003DBA17 File Offset: 0x003D9C17
		public StringSpan Current
		{
			get
			{
				return this.m_current;
			}
		}

		// Token: 0x0600A5D6 RID: 42454 RVA: 0x003DBA1F File Offset: 0x003D9C1F
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

		// Token: 0x0600A5D7 RID: 42455 RVA: 0x003DBA44 File Offset: 0x003D9C44
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

		// Token: 0x04007AFC RID: 31484
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_remainder;

		// Token: 0x04007AFD RID: 31485
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_removeEmptyEntries;

		// Token: 0x04007AFE RID: 31486
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string[] m_separators;

		// Token: 0x04007AFF RID: 31487
		[PublicizedFrom(EAccessModifier.Private)]
		public StringSpan m_current;

		// Token: 0x04007B00 RID: 31488
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_done;
	}
}
