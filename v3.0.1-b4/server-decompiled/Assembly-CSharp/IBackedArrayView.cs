using System;
using System.Runtime.CompilerServices;

// Token: 0x0200137A RID: 4986
public interface IBackedArrayView<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17001279 RID: 4729
	// (get) Token: 0x06009D1A RID: 40218
	int Length { get; }

	// Token: 0x1700127A RID: 4730
	// (get) Token: 0x06009D1B RID: 40219
	BackedArrayHandleMode Mode { get; }

	// Token: 0x1700127B RID: 4731
	T this[int i] { get; set; }

	// Token: 0x06009D1E RID: 40222
	void Flush();
}
