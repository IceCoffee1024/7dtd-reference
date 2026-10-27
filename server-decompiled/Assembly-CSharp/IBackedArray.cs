using System;
using System.Runtime.CompilerServices;

// Token: 0x020013C4 RID: 5060
public interface IBackedArray<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17001301 RID: 4865
	// (get) Token: 0x06009F0F RID: 40719
	int Length { get; }

	// Token: 0x06009F10 RID: 40720
	IBackedArrayHandle GetMemory(int start, int length, out Memory<T> memory);

	// Token: 0x06009F11 RID: 40721
	IBackedArrayHandle GetReadOnlyMemory(int start, int length, out ReadOnlyMemory<T> memory);

	// Token: 0x06009F12 RID: 40722
	unsafe IBackedArrayHandle GetMemoryUnsafe(int start, int length, out T* arrayPtr);

	// Token: 0x06009F13 RID: 40723
	unsafe IBackedArrayHandle GetReadOnlyMemoryUnsafe(int start, int length, out T* arrayPtr);

	// Token: 0x06009F14 RID: 40724
	IBackedArrayHandle GetSpan(int start, int length, out Span<T> span);

	// Token: 0x06009F15 RID: 40725
	IBackedArrayHandle GetReadOnlySpan(int start, int length, out ReadOnlySpan<T> span);
}
