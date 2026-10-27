using System;
using System.Collections.Generic;

// Token: 0x02001144 RID: 4420
public class XUiControllerComparerDepth : IComparer<XUiController>
{
	// Token: 0x06008B63 RID: 35683 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiControllerComparerDepth()
	{
	}

	// Token: 0x06008B64 RID: 35684 RVA: 0x003529CC File Offset: 0x00350BCC
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

	// Token: 0x17001050 RID: 4176
	// (get) Token: 0x06008B65 RID: 35685 RVA: 0x00352A07 File Offset: 0x00350C07
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

	// Token: 0x04006777 RID: 26487
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiControllerComparerDepth instance;
}
