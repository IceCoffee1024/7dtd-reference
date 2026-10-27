using System;
using UnityEngine;

// Token: 0x0200117B RID: 4475
public class XUiV_Empty : XUiView
{
	// Token: 0x170010D6 RID: 4310
	// (get) Token: 0x06008DCF RID: 36303 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x06008DD0 RID: 36304 RVA: 0x0035C1E9 File Offset: 0x0035A3E9
	public XUiV_Empty(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x170010D7 RID: 4311
	// (get) Token: 0x06008DD1 RID: 36305 RVA: 0x0035CA3D File Offset: 0x0035AC3D
	public override Vector3[] WorldCorners
	{
		get
		{
			return XUiV_Empty.WorldCornersEmpty;
		}
	}

	// Token: 0x04006879 RID: 26745
	public static readonly Vector3[] WorldCornersEmpty = new Vector3[4];
}
