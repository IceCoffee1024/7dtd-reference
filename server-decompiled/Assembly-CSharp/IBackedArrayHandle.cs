using System;

// Token: 0x02001393 RID: 5011
public interface IBackedArrayHandle : IDisposable
{
	// Token: 0x170012B1 RID: 4785
	// (get) Token: 0x06009E13 RID: 40467
	BackedArrayHandleMode Mode { get; }

	// Token: 0x06009E14 RID: 40468
	void Flush();
}
