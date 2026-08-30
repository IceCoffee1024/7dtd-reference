using System;
using System.Collections.Generic;

// Token: 0x02001103 RID: 4355
public class XUiControllerComparerDepth : IComparer<XUiController>
{
	// Token: 0x060089B1 RID: 35249 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiControllerComparerDepth()
	{
	}

	// Token: 0x060089B2 RID: 35250 RVA: 0x00347C4C File Offset: 0x00345E4C
	public int Compare(XUiController _x, XUiController _y)
	{
		if (_x == _y)
		{
			return 0;
		}
		if (_y == null)
		{
			return 1;
		}
		if (_x == null)
		{
			return -1;
		}
		return _x.ViewComponent.Depth.CompareTo(_y.ViewComponent.Depth);
	}

	// Token: 0x17000FD7 RID: 4055
	// (get) Token: 0x060089B3 RID: 35251 RVA: 0x00347C87 File Offset: 0x00345E87
	public static XUiControllerComparerDepth Instance
	{
		get
		{
			XUiControllerComparerDepth xuiControllerComparerDepth;
			if ((xuiControllerComparerDepth = XUiControllerComparerDepth.instance) == null)
			{
				xuiControllerComparerDepth = (XUiControllerComparerDepth.instance = new XUiControllerComparerDepth());
			}
			return xuiControllerComparerDepth;
		}
	}

	// Token: 0x0400661B RID: 26139
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiControllerComparerDepth instance;
}
