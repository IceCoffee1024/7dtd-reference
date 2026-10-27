using System;

// Token: 0x020013C5 RID: 5061
public interface IBackedArrayHandle : IDisposable
{
	// Token: 0x17001302 RID: 4866
	// (get) Token: 0x06009F16 RID: 40726
	BackedArrayHandleMode Mode { get; }

	// Token: 0x06009F17 RID: 40727
	void Flush();
}
