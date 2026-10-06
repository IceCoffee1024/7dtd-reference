using System;
using UnityEngine;

// Token: 0x0200116B RID: 4459
public class XUiV_Empty : XUiView
{
	// Token: 0x170010CB RID: 4299
	// (get) Token: 0x06008D7B RID: 36219 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x06008D7C RID: 36220 RVA: 0x0035A13D File Offset: 0x0035833D
	public XUiV_Empty(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x170010CC RID: 4300
	// (get) Token: 0x06008D7D RID: 36221 RVA: 0x0035A991 File Offset: 0x00358B91
	public override Vector3[] WorldCorners
	{
		get
		{
			return XUiV_Empty.WorldCornersEmpty;
		}
	}

	// Token: 0x04006839 RID: 26681
	public static readonly Vector3[] WorldCornersEmpty = new Vector3[4];
}
