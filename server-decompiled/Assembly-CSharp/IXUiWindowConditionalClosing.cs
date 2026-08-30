using System;

// Token: 0x02000DAB RID: 3499
public interface IXUiWindowConditionalClosing
{
	// Token: 0x06006AD5 RID: 27349
	void TryClose(Action _onClosed, Action _onCancelled);
}
