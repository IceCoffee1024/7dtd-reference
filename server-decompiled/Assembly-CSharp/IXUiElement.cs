using System;

// Token: 0x0200113A RID: 4410
public interface IXUiElement
{
	// Token: 0x17001046 RID: 4166
	// (get) Token: 0x06008B1B RID: 35611
	XUiController Controller { get; }

	// Token: 0x06008B1C RID: 35612
	string GetXuiHierarchy();
}
