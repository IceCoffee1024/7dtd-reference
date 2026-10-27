using System;

// Token: 0x0200114A RID: 4426
public interface IXUiElement
{
	// Token: 0x17001051 RID: 4177
	// (get) Token: 0x06008B6F RID: 35695
	XUiController Controller { get; }

	// Token: 0x06008B70 RID: 35696
	string GetXuiHierarchy();
}
