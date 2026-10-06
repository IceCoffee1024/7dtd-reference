using System;

// Token: 0x020013B3 RID: 5043
public interface IBackedArrayHandle : IDisposable
{
	// Token: 0x170012F6 RID: 4854
	// (get) Token: 0x06009EBE RID: 40638
	BackedArrayHandleMode Mode { get; }

	// Token: 0x06009EBF RID: 40639
	void Flush();
}
