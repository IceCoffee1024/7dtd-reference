using System;
using System.Runtime.CompilerServices;

// Token: 0x020013B4 RID: 5044
public interface IBackedArrayView<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x170012F7 RID: 4855
	// (get) Token: 0x06009EC0 RID: 40640
	int Length { get; }

	// Token: 0x170012F8 RID: 4856
	// (get) Token: 0x06009EC1 RID: 40641
	BackedArrayHandleMode Mode { get; }

	// Token: 0x170012F9 RID: 4857
	T this[int i] { get; set; }

	// Token: 0x06009EC4 RID: 40644
	void Flush();
}
