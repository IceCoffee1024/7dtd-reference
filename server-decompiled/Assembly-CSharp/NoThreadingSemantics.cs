using System;

// Token: 0x02000BE6 RID: 3046
public class NoThreadingSemantics : IThreadingSemantics
{
	// Token: 0x06005B43 RID: 23363 RVA: 0x0023494C File Offset: 0x00232B4C
	public void Synchronize(AtomicActionDelegate _delegate)
	{
		_delegate();
	}

	// Token: 0x06005B44 RID: 23364 RVA: 0x00234954 File Offset: 0x00232B54
	public T Synchronize<T>(Func<T> _delegate)
	{
		return _delegate();
	}

	// Token: 0x06005B45 RID: 23365 RVA: 0x0023495C File Offset: 0x00232B5C
	public int InterlockedAdd(ref int _number, int _add)
	{
		_number += _add;
		return _number;
	}
}
