using System;
using UnityEngine;

// Token: 0x0200115D RID: 4445
public class XUiV_Rect : XUiView_WidgetBased
{
	// Token: 0x06008DBE RID: 36286 RVA: 0x00356E80 File Offset: 0x00355080
	public XUiV_Rect(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x170010D0 RID: 4304
	// (get) Token: 0x06008DBF RID: 36287 RVA: 0x00357F8B File Offset: 0x0035618B
	// (set) Token: 0x06008DC0 RID: 36288 RVA: 0x00357F93 File Offset: 0x00356193
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

	// Token: 0x170010D1 RID: 4305
	// (get) Token: 0x06008DC1 RID: 36289 RVA: 0x00357F9C File Offset: 0x0035619C
	public override bool HasAnyEvent
	{
		get
		{
			return this.disableFallthrough || base.HasAnyEvent;
		}
	}

	// Token: 0x06008DC2 RID: 36290 RVA: 0x00357FAE File Offset: 0x003561AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIWidget>();
	}

	// Token: 0x06008DC3 RID: 36291 RVA: 0x00357FB7 File Offset: 0x003561B7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = this.uiTransform.gameObject.GetComponent<UIWidget>();
	}

	// Token: 0x06008DC4 RID: 36292 RVA: 0x00357FD5 File Offset: 0x003561D5
	public override void InitView()
	{
		base.InitView();
		UIWidget widget = this.widget;
		widget.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(widget.onChange, new UIWidget.OnDimensionsChanged(base.SetDirty));
		this.updateData();
	}

	// Token: 0x06008DC5 RID: 36293 RVA: 0x0035800C File Offset: 0x0035620C
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

	// Token: 0x0400682B RID: 26667
	[PublicizedFrom(EAccessModifier.Private)]
	public bool disableFallthrough;
}
