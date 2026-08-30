using System;
using System.Runtime.CompilerServices;

// Token: 0x02001392 RID: 5010
public interface IBackedArray<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x170012B0 RID: 4784
	// (get) Token: 0x06009E0C RID: 40460
	int Length { get; }

	// Token: 0x06009E0D RID: 40461
	IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory);

	// Token: 0x06009E0E RID: 40462
	IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory);

	// Token: 0x06009E0F RID: 40463
	unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr);

	// Token: 0x06009E10 RID: 40464
	unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr);

	// Token: 0x06009E11 RID: 40465
	IBackedArrayHandle GetSpan(int start, int length, out Span<T> span);

	// Token: 0x06009E12 RID: 40466
	IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span);
}
