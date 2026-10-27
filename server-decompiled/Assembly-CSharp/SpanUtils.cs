using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020014D9 RID: 5337
public static class SpanUtils
{
	// Token: 0x0600A6DC RID: 42716 RVA: 0x003E1EA9 File Offset: 0x003E00A9
	public static int GetHashCode<[IsUnmanaged] T>(ReadOnlySpan<T> span) where T : struct, ValueType
	{
		return SpanUtils.GetHashCodeInternal(MemoryMarshal.Cast<T, byte>(span));
	}

	// Token: 0x0600A6DD RID: 42717 RVA: 0x003E1EB8 File Offset: 0x003E00B8
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static int GetHashCodeInternal(ReadOnlySpan<byte> span)
	{
		int num = span.Length / 4 * 4;
		ReadOnlySpan<int> readOnlySpan = MemoryMarshal.Cast<byte, int>(span.Slice(0, num));
		int num2 = 1009;
		ReadOnlySpan<int> readOnlySpan2 = readOnlySpan;
		int i;
		for (i = 0; i < readOnlySpan2.Length; i++)
		{
			int num3 = *readOnlySpan2[i];
			num2 = num2 * 9176 + num3;
		}
		if (num == span.Length)
		{
			return num2;
		}
		int num4 = span.Length - num;
		switch (num4)
		{
		case 1:
			i = (int)(*span[num]);
			break;
		case 2:
			i = (int)(*span[num]) | ((int)(*span[num + 1]) << 8);
			break;
		case 3:
			i = (int)(*span[num]) | ((int)(*span[num + 1]) << 8) | ((int)(*span[num + 2]) << 16);
			break;
		default:
			throw new InvalidOperationException(string.Format("Remainder should be 1, 2 or 3, but was: {0}", num4));
		}
		int num5 = i;
		return num2 * 9176 + num5;
	}

	// Token: 0x0600A6DE RID: 42718 RVA: 0x003E1FB4 File Offset: 0x003E01B4
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ConcatAccLength(int length, ref int totalLength)
	{
		totalLength += length;
		return length;
	}

	// Token: 0x0600A6DF RID: 42719 RVA: 0x003E1FC0 File Offset: 0x003E01C0
	[PublicizedFrom(EAccessModifier.Private)]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ConcatCopyThenSlice(ref Span<char> dest, [TupleElementNames(new string[] { "ptr", "len" })] ValueTuple<IntPtr, int> src)
	{
		ReadOnlySpan<char> readOnlySpan = new ReadOnlySpan<char>(src.Item1.ToPointer(), src.Item2);
		readOnlySpan.CopyTo(dest);
		Span<char> span = dest;
		int item = src.Item2;
		dest = span.Slice(item, span.Length - item);
	}

	// Token: 0x0600A6E0 RID: 42720 RVA: 0x003E2018 File Offset: 0x003E0218
	public unsafe static string Concat(StringSpan s0, StringSpan s1)
	{
		fixed (char* pinnableReference = s0.AsSpan().GetPinnableReference())
		{
			void* ptr = (void*)pinnableReference;
			fixed (char* pinnableReference2 = s1.AsSpan().GetPinnableReference())
			{
				void* ptr2 = (void*)pinnableReference2;
				int num = 0;
				ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>> valueTuple = new ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>>(new ValueTuple<IntPtr, int>((IntPtr)ptr, SpanUtils.ConcatAccLength(s0.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr2, SpanUtils.ConcatAccLength(s1.Length, ref num)));
				return string.Create<ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>>>(num, valueTuple, delegate(Span<char> span, [TupleElementNames(new string[] { "d0", "d1", null, null, null, null })] ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>> data)
				{
					SpanUtils.ConcatCopyThenSlice(ref span, data.Item1);
					SpanUtils.ConcatCopyThenSlice(ref span, data.Item2);
				});
			}
		}
	}

	// Token: 0x0600A6E1 RID: 42721 RVA: 0x003E20B4 File Offset: 0x003E02B4
	public unsafe static string Concat(StringSpan s0, StringSpan s1, StringSpan s2)
	{
		fixed (char* pinnableReference = s0.AsSpan().GetPinnableReference())
		{
			void* ptr = (void*)pinnableReference;
			fixed (char* pinnableReference2 = s1.AsSpan().GetPinnableReference())
			{
				void* ptr2 = (void*)pinnableReference2;
				fixed (char* pinnableReference3 = s2.AsSpan().GetPinnableReference())
				{
					void* ptr3 = (void*)pinnableReference3;
					int num = 0;
					ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>> valueTuple = new ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>>(new ValueTuple<IntPtr, int>((IntPtr)ptr, SpanUtils.ConcatAccLength(s0.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr2, SpanUtils.ConcatAccLength(s1.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr3, SpanUtils.ConcatAccLength(s2.Length, ref num)));
					return string.Create<ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>>>(num, valueTuple, delegate(Span<char> span, [TupleElementNames(new string[] { "d0", "d1", "d2", null, null, null, null, null, null })] ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>> data)
					{
						SpanUtils.ConcatCopyThenSlice(ref span, data.Item1);
						SpanUtils.ConcatCopyThenSlice(ref span, data.Item2);
						SpanUtils.ConcatCopyThenSlice(ref span, data.Item3);
					});
				}
			}
		}
	}

	// Token: 0x0600A6E2 RID: 42722 RVA: 0x003E2180 File Offset: 0x003E0380
	public unsafe static string Concat(StringSpan s0, StringSpan s1, StringSpan s2, StringSpan s3)
	{
		fixed (char* pinnableReference = s0.AsSpan().GetPinnableReference())
		{
			void* ptr = (void*)pinnableReference;
			fixed (char* pinnableReference2 = s1.AsSpan().GetPinnableReference())
			{
				void* ptr2 = (void*)pinnableReference2;
				fixed (char* pinnableReference3 = s2.AsSpan().GetPinnableReference())
				{
					void* ptr3 = (void*)pinnableReference3;
					fixed (char* pinnableReference4 = s3.AsSpan().GetPinnableReference())
					{
						void* ptr4 = (void*)pinnableReference4;
						int num = 0;
						ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>> valueTuple = new ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>>(new ValueTuple<IntPtr, int>((IntPtr)ptr, SpanUtils.ConcatAccLength(s0.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr2, SpanUtils.ConcatAccLength(s1.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr3, SpanUtils.ConcatAccLength(s2.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr4, SpanUtils.ConcatAccLength(s3.Length, ref num)));
						return string.Create<ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>>>(num, valueTuple, delegate(Span<char> span, [TupleElementNames(new string[]
						{
							"d0", "d1", "d2", "d3", null, null, null, null, null, null,
							null, null
						})] ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>> data)
						{
							SpanUtils.ConcatCopyThenSlice(ref span, data.Item1);
							SpanUtils.ConcatCopyThenSlice(ref span, data.Item2);
							SpanUtils.ConcatCopyThenSlice(ref span, data.Item3);
							SpanUtils.ConcatCopyThenSlice(ref span, data.Item4);
						});
					}
				}
			}
		}
	}

	// Token: 0x0600A6E3 RID: 42723 RVA: 0x003E227C File Offset: 0x003E047C
	public unsafe static string Concat(StringSpan s0, StringSpan s1, StringSpan s2, StringSpan s3, StringSpan s4)
	{
		fixed (char* pinnableReference = s0.AsSpan().GetPinnableReference())
		{
			void* ptr = (void*)pinnableReference;
			fixed (char* pinnableReference2 = s1.AsSpan().GetPinnableReference())
			{
				void* ptr2 = (void*)pinnableReference2;
				fixed (char* pinnableReference3 = s2.AsSpan().GetPinnableReference())
				{
					void* ptr3 = (void*)pinnableReference3;
					fixed (char* pinnableReference4 = s3.AsSpan().GetPinnableReference())
					{
						void* ptr4 = (void*)pinnableReference4;
						fixed (char* pinnableReference5 = s4.AsSpan().GetPinnableReference())
						{
							void* ptr5 = (void*)pinnableReference5;
							int num = 0;
							ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>> valueTuple = new ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>>(new ValueTuple<IntPtr, int>((IntPtr)ptr, SpanUtils.ConcatAccLength(s0.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr2, SpanUtils.ConcatAccLength(s1.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr3, SpanUtils.ConcatAccLength(s2.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr4, SpanUtils.ConcatAccLength(s3.Length, ref num)), new ValueTuple<IntPtr, int>((IntPtr)ptr5, SpanUtils.ConcatAccLength(s4.Length, ref num)));
							return string.Create<ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>>>(num, valueTuple, delegate(Span<char> span, [TupleElementNames(new string[]
							{
								"d0", "d1", "d2", "d3", "d4", null, null, null, null, null,
								null, null, null, null, null
							})] ValueTuple<ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>, ValueTuple<IntPtr, int>> data)
							{
								SpanUtils.ConcatCopyThenSlice(ref span, data.Item1);
								SpanUtils.ConcatCopyThenSlice(ref span, data.Item2);
								SpanUtils.ConcatCopyThenSlice(ref span, data.Item3);
								SpanUtils.ConcatCopyThenSlice(ref span, data.Item4);
								SpanUtils.ConcatCopyThenSlice(ref span, data.Item5);
							});
						}
					}
				}
			}
		}
	}
}
