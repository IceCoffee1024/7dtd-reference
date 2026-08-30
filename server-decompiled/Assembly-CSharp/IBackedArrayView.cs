using System;
using System.Runtime.CompilerServices;

// Token: 0x02001394 RID: 5012
public interface IBackedArrayView<[IsUnmanaged] T> : IDisposable where T : struct, ValueType
{
	// Token: 0x170012B2 RID: 4786
	// (get) Token: 0x06009E15 RID: 40469
	int Length { get; }

	// Token: 0x170012B3 RID: 4787
	// (get) Token: 0x06009E16 RID: 40470
	BackedArrayHandleMode Mode { get; }

	// Token: 0x170012B4 RID: 4788
	T this[int i] { get; set; }

	// Token: 0x06009E19 RID: 40473
	void Flush();
}
