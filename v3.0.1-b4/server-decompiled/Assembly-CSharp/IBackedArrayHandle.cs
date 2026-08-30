using System;

// Token: 0x02001379 RID: 4985
public interface IBackedArrayHandle : IDisposable
{
	// Token: 0x17001278 RID: 4728
	// (get) Token: 0x06009D18 RID: 40216
	BackedArrayHandleMode Mode { get; }

	// Token: 0x06009D19 RID: 40217
	void Flush();
}
