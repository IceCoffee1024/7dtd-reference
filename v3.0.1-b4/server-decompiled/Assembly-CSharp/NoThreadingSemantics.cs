using System;

// Token: 0x02000BB7 RID: 2999
public class NoThreadingSemantics : IThreadingSemantics
{
	// Token: 0x06005A7D RID: 23165 RVA: 0x0022D38C File Offset: 0x0022B58C
	public void Synchronize(AtomicActionDelegate _delegate)
	{
		_delegate();
	}

	// Token: 0x06005A7E RID: 23166 RVA: 0x0022D394 File Offset: 0x0022B594
	public T Synchronize<T>(Func<T> _delegate)
	{
		return _delegate();
	}

	// Token: 0x06005A7F RID: 23167 RVA: 0x0022D39C File Offset: 0x0022B59C
	public int InterlockedAdd(ref int _number, int _add)
	{
		_number += _add;
		return _number;
	}
}
