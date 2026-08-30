using System;
using UnityEngine;

// Token: 0x02001153 RID: 4435
public class XUiV_Empty : XUiView
{
	// Token: 0x17001096 RID: 4246
	// (get) Token: 0x06008D10 RID: 36112 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x06008D11 RID: 36113 RVA: 0x00355D1D File Offset: 0x00353F1D
	public XUiV_Empty(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x17001097 RID: 4247
	// (get) Token: 0x06008D12 RID: 36114 RVA: 0x00356571 File Offset: 0x00354771
	public override Vector3[] WorldCorners
	{
		get
		{
			return XUiV_Empty.WorldCornersEmpty;
		}
	}

	// Token: 0x040067E4 RID: 26596
	public static readonly Vector3[] WorldCornersEmpty = new Vector3[4];
}
