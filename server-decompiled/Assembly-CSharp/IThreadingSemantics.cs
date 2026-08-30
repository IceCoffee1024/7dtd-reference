using System;

// Token: 0x02000BC7 RID: 3015
public interface IThreadingSemantics
{
	// Token: 0x06005AFD RID: 23293
	void Synchronize(AtomicActionDelegate _delegate);

	// Token: 0x06005AFE RID: 23294
	T Synchronize<T>(Func<T> _delegate);

	// Token: 0x06005AFF RID: 23295
	int InterlockedAdd(ref int _number, int _add);
}
