using System;

// Token: 0x02001122 RID: 4386
public interface IXUiElement
{
	// Token: 0x17001011 RID: 4113
	// (get) Token: 0x06008AAF RID: 35503
	XUiController Controller { get; }

	// Token: 0x06008AB0 RID: 35504
	string GetXuiHierarchy();
}
