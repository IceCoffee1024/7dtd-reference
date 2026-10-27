using System;
using UnityEngine;

// Token: 0x02001185 RID: 4485
public class XUiV_Rect : XUiView_WidgetBased
{
	// Token: 0x06008E7E RID: 36478 RVA: 0x0035D34C File Offset: 0x0035B54C
	public XUiV_Rect(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x17001111 RID: 4369
	// (get) Token: 0x06008E7F RID: 36479 RVA: 0x0035E483 File Offset: 0x0035C683
	// (set) Token: 0x06008E80 RID: 36480 RVA: 0x0035E48B File Offset: 0x0035C68B
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

	// Token: 0x17001112 RID: 4370
	// (get) Token: 0x06008E81 RID: 36481 RVA: 0x0035E494 File Offset: 0x0035C694
	public override bool HasAnyEvent
	{
		get
		{
			return this.disableFallthrough || base.HasAnyEvent;
		}
	}

	// Token: 0x06008E82 RID: 36482 RVA: 0x0035E4A6 File Offset: 0x0035C6A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIWidget>();
	}

	// Token: 0x06008E83 RID: 36483 RVA: 0x0035E4AF File Offset: 0x0035C6AF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = this.uiTransform.gameObject.GetComponent<UIWidget>();
	}

	// Token: 0x06008E84 RID: 36484 RVA: 0x0035E4CD File Offset: 0x0035C6CD
	public override void InitView()
	{
		base.InitView();
		UIWidget widget = this.widget;
		widget.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(widget.onChange, new UIWidget.OnDimensionsChanged(base.SetDirty));
		this.updateData();
	}

	// Token: 0x06008E85 RID: 36485 RVA: 0x0035E504 File Offset: 0x0035C704
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

	// Token: 0x040068C0 RID: 26816
	[PublicizedFrom(EAccessModifier.Private)]
	public bool disableFallthrough;
}
