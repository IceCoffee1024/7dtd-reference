using System;

// Token: 0x02000BDB RID: 3035
public class NoThreadingSemantics : IThreadingSemantics
{
	// Token: 0x06005B1E RID: 23326 RVA: 0x002333F0 File Offset: 0x002315F0
	public void Synchronize(AtomicActionDelegate _delegate)
	{
		_delegate();
	}

	// Token: 0x06005B1F RID: 23327 RVA: 0x002333F8 File Offset: 0x002315F8
	public T Synchronize<T>(Func<T> _delegate)
	{
		return _delegate();
	}

	// Token: 0x06005B20 RID: 23328 RVA: 0x00233400 File Offset: 0x00231600
	public int InterlockedAdd(ref int _number, int _add)
	{
		_number += _add;
		return _number;
	}
}
