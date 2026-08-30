using System;

// Token: 0x02000BC9 RID: 3017
public class NoThreadingSemantics : IThreadingSemantics
{
	// Token: 0x06005B05 RID: 23301 RVA: 0x002300F4 File Offset: 0x0022E2F4
	public void Synchronize(AtomicActionDelegate _delegate)
	{
		_delegate();
	}

	// Token: 0x06005B06 RID: 23302 RVA: 0x002300FC File Offset: 0x0022E2FC
	public T Synchronize<T>(Func<T> _delegate)
	{
		return _delegate();
	}

	// Token: 0x06005B07 RID: 23303 RVA: 0x00230104 File Offset: 0x0022E304
	public int InterlockedAdd(ref int _number, int _add)
	{
		_number += _add;
		return _number;
	}
}
