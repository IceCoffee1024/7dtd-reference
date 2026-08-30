using System;
using UnityEngine;

// Token: 0x02001144 RID: 4420
public class XUiV_Rect : XUiView_WidgetBased
{
	// Token: 0x06008CCC RID: 36044 RVA: 0x00352644 File Offset: 0x00350844
	public XUiV_Rect(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x17001097 RID: 4247
	// (get) Token: 0x06008CCD RID: 36045 RVA: 0x0035374F File Offset: 0x0035194F
	// (set) Token: 0x06008CCE RID: 36046 RVA: 0x00353757 File Offset: 0x00351957
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

	// Token: 0x17001098 RID: 4248
	// (get) Token: 0x06008CCF RID: 36047 RVA: 0x00353760 File Offset: 0x00351960
	public override bool HasAnyEvent
	{
		get
		{
			return this.disableFallthrough || base.HasAnyEvent;
		}
	}

	// Token: 0x06008CD0 RID: 36048 RVA: 0x00353772 File Offset: 0x00351972
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIWidget>();
	}

	// Token: 0x06008CD1 RID: 36049 RVA: 0x0035377B File Offset: 0x0035197B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = this.uiTransform.gameObject.GetComponent<UIWidget>();
	}

	// Token: 0x06008CD2 RID: 36050 RVA: 0x00353799 File Offset: 0x00351999
	public override void InitView()
	{
		base.InitView();
		UIWidget widget = this.widget;
		widget.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(widget.onChange, new UIWidget.OnDimensionsChanged(base.SetDirty));
		this.updateData();
	}

	// Token: 0x06008CD3 RID: 36051 RVA: 0x003537D0 File Offset: 0x003519D0
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

	// Token: 0x04006761 RID: 26465
	[PublicizedFrom(EAccessModifier.Private)]
	public bool disableFallthrough;
}
