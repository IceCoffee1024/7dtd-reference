using System;

// Token: 0x02000DC5 RID: 3525
public interface IXUiWindowConditionalClosing
{
	// Token: 0x06006AF2 RID: 27378
	void TryClose(Action _onClosed, Action _onCancelled);
}
