using System;
using UnityEngine;

// Token: 0x02001175 RID: 4469
public class XUiV_Rect : XUiView_WidgetBased
{
	// Token: 0x06008E2A RID: 36394 RVA: 0x0035B2A0 File Offset: 0x003594A0
	public XUiV_Rect(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x17001106 RID: 4358
	// (get) Token: 0x06008E2B RID: 36395 RVA: 0x0035C3B3 File Offset: 0x0035A5B3
	// (set) Token: 0x06008E2C RID: 36396 RVA: 0x0035C3BB File Offset: 0x0035A5BB
	[XuiXmlAttribute("disablefallthrough", false)]
	public bool DisableFallthrough
	{
		get
		{
			return this.disableFallthrough;
		}
		set
		{
			this.disableFallthrough = value;
		}
	}

	// Token: 0x17001107 RID: 4359
	// (get) Token: 0x06008E2D RID: 36397 RVA: 0x0035C3C4 File Offset: 0x0035A5C4
	public override bool HasAnyEvent
	{
		get
		{
			return this.disableFallthrough || base.HasAnyEvent;
		}
	}

	// Token: 0x06008E2E RID: 36398 RVA: 0x0035C3D6 File Offset: 0x0035A5D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIWidget>();
	}

	// Token: 0x06008E2F RID: 36399 RVA: 0x0035C3DF File Offset: 0x0035A5DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = this.uiTransform.gameObject.GetComponent<UIWidget>();
	}

	// Token: 0x06008E30 RID: 36400 RVA: 0x0035C3FD File Offset: 0x0035A5FD
	public override void InitView()
	{
		base.InitView();
		UIWidget widget = this.widget;
		widget.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(widget.onChange, new UIWidget.OnDimensionsChanged(base.SetDirty));
		this.updateData();
	}

	// Token: 0x06008E31 RID: 36401 RVA: 0x0035C434 File Offset: 0x0035A634
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void refreshBoxCollider()
	{
		base.refreshBoxCollider();
		if (!this.disableFallthrough)
		{
			return;
		}
		Vector3 center = this.collider.center;
		center.z = 100f;
		this.collider.center = center;
	}

	// Token: 0x04006880 RID: 26752
	[PublicizedFrom(EAccessModifier.Private)]
	public bool disableFallthrough;
}
