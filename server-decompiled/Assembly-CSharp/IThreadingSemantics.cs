using System;

// Token: 0x02000BD9 RID: 3033
public interface IThreadingSemantics
{
	// Token: 0x06005B16 RID: 23318
	void Synchronize(AtomicActionDelegate _delegate);

	// Token: 0x06005B17 RID: 23319
	T Synchronize<T>(Func<T> _delegate);

	// Token: 0x06005B18 RID: 23320
	int InterlockedAdd(ref int _number, int _add);
}
