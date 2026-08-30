using System;

// Token: 0x02000BB5 RID: 2997
public interface IThreadingSemantics
{
	// Token: 0x06005A75 RID: 23157
	void Synchronize(AtomicActionDelegate _delegate);

	// Token: 0x06005A76 RID: 23158
	T Synchronize<T>(Func<T> _delegate);

	// Token: 0x06005A77 RID: 23159
	int InterlockedAdd(ref int _number, int _add);
}
