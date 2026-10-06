using System;
using System.Collections.Generic;

// Token: 0x02001134 RID: 4404
public class XUiControllerComparerDepth : IComparer<XUiController>
{
	// Token: 0x06008B0F RID: 35599 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiControllerComparerDepth()
	{
	}

	// Token: 0x06008B10 RID: 35600 RVA: 0x0035091C File Offset: 0x0034EB1C
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

	// Token: 0x17001045 RID: 4165
	// (get) Token: 0x06008B11 RID: 35601 RVA: 0x00350957 File Offset: 0x0034EB57
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

	// Token: 0x04006737 RID: 26423
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiControllerComparerDepth instance;
}
