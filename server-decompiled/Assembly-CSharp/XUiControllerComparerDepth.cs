using System;
using System.Collections.Generic;

// Token: 0x0200111C RID: 4380
public class XUiControllerComparerDepth : IComparer<XUiController>
{
	// Token: 0x06008AA3 RID: 35491 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiControllerComparerDepth()
	{
	}

	// Token: 0x06008AA4 RID: 35492 RVA: 0x0034C470 File Offset: 0x0034A670
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

	// Token: 0x17001010 RID: 4112
	// (get) Token: 0x06008AA5 RID: 35493 RVA: 0x0034C4AB File Offset: 0x0034A6AB
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

	// Token: 0x040066E2 RID: 26338
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiControllerComparerDepth instance;
}
