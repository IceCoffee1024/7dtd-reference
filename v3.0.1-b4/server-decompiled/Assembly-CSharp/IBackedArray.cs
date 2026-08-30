using System;
using System.Runtime.CompilerServices;

// Token: 0x02001378 RID: 4984
public interface IBackedArray<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17001277 RID: 4727
	// (get) Token: 0x06009D11 RID: 40209
	int Length { get; }

	// Token: 0x06009D12 RID: 40210
	IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory);

	// Token: 0x06009D13 RID: 40211
	IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory);

	// Token: 0x06009D14 RID: 40212
	unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr);

	// Token: 0x06009D15 RID: 40213
	unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr);

	// Token: 0x06009D16 RID: 40214
	IBackedArrayHandle GetSpan(int start, int length, out Span<T> span);

	// Token: 0x06009D17 RID: 40215
	IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span);
}
