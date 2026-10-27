using System;
using System.Runtime.CompilerServices;

// Token: 0x020013C6 RID: 5062
public interface IBackedArrayView<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x17001303 RID: 4867
	// (get) Token: 0x06009F18 RID: 40728
	int Length { get; }

	// Token: 0x17001304 RID: 4868
	// (get) Token: 0x06009F19 RID: 40729
	BackedArrayHandleMode Mode { get; }

	// Token: 0x17001305 RID: 4869
	T this[int i] { get; set; }

	// Token: 0x06009F1C RID: 40732
	void Flush();
}
