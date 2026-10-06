using System;
using System.Runtime.CompilerServices;

// Token: 0x020013B2 RID: 5042
public interface IBackedArray<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x170012F5 RID: 4853
	// (get) Token: 0x06009EB7 RID: 40631
	int Length { get; }

	// Token: 0x06009EB8 RID: 40632
	IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory);

	// Token: 0x06009EB9 RID: 40633
	IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory);

	// Token: 0x06009EBA RID: 40634
	unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr);

	// Token: 0x06009EBB RID: 40635
	unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr);

	// Token: 0x06009EBC RID: 40636
	IBackedArrayHandle GetSpan(int start, int length, out Span<T> span);

	// Token: 0x06009EBD RID: 40637
	IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span);
}
