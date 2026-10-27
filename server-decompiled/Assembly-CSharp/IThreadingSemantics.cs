using System;

// Token: 0x02000BE4 RID: 3044
public interface IThreadingSemantics
{
	// Token: 0x06005B3B RID: 23355
	void Synchronize(AtomicActionDelegate _delegate);

	// Token: 0x06005B3C RID: 23356
	T Synchronize<T>(Func<T> _delegate);

	// Token: 0x06005B3D RID: 23357
	int InterlockedAdd(ref int _number, int _add);
}
