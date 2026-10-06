using System;

// Token: 0x02000DBA RID: 3514
public interface IXUiWindowConditionalClosing
{
	// Token: 0x06006ACD RID: 27341
	void TryClose(Action _onClosed, Action _onCancelled);
}
