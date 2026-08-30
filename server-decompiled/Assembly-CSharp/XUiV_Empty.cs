using System;
using UnityEngine;

// Token: 0x0200113A RID: 4410
public class XUiV_Empty : XUiView
{
	// Token: 0x1700105D RID: 4189
	// (get) Token: 0x06008C1E RID: 35870 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x06008C1F RID: 35871 RVA: 0x003514E9 File Offset: 0x0034F6E9
	public XUiV_Empty(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x1700105E RID: 4190
	// (get) Token: 0x06008C20 RID: 35872 RVA: 0x00351D3D File Offset: 0x0034FF3D
	public override Vector3[] WorldCorners
	{
		get
		{
			return XUiV_Empty.WorldCornersEmpty;
		}
	}

	// Token: 0x0400671A RID: 26394
	public static readonly Vector3[] WorldCornersEmpty = new Vector3[4];
}
