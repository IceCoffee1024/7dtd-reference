using System;

// Token: 0x02001109 RID: 4361
public interface IXUiElement
{
	// Token: 0x17000FD8 RID: 4056
	// (get) Token: 0x060089BD RID: 35261
	XUiController Controller { get; }

	// Token: 0x060089BE RID: 35262
	string GetXuiHierarchy();
}
