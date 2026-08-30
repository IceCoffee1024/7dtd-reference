using System;

// Token: 0x02000D97 RID: 3479
public interface IXUiWindowConditionalClosing
{
	// Token: 0x06006A3B RID: 27195
	void TryClose(Action _onClosed, Action _onCancelled);
}
