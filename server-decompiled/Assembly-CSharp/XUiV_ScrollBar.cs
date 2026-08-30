using System;
using UnityEngine;

// Token: 0x02001145 RID: 4421
public class XUiV_ScrollBar : XUiView
{
	// Token: 0x17001099 RID: 4249
	// (get) Token: 0x06008CD4 RID: 36052 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x1700109A RID: 4250
	// (get) Token: 0x06008CD5 RID: 36053 RVA: 0x00353810 File Offset: 0x00351A10
	public UIScrollBar ScrollBar
	{
		get
		{
			return this.scrollBar;
		}
	}

	// Token: 0x1700109B RID: 4251
	// (get) Token: 0x06008CD6 RID: 36054 RVA: 0x00353818 File Offset: 0x00351A18
	// (set) Token: 0x06008CD7 RID: 36055 RVA: 0x00353825 File Offset: 0x00351A25
	public float ScrollPosition
	{
		get
		{
			return this.scrollBar.value;
		}
		set
		{
			this.scrollBar.value = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700109C RID: 4252
	// (get) Token: 0x06008CD8 RID: 36056 RVA: 0x00353839 File Offset: 0x00351A39
	public override Vector3[] WorldCorners
	{
		get
		{
			if (this.background != null)
			{
				return this.background.WorldCorners;
			}
			XUiV_Sprite xuiV_Sprite = this.thumb;
			if (xuiV_Sprite == null)
			{
				return null;
			}
			return xuiV_Sprite.WorldCorners;
		}
	}

	// Token: 0x06008CD9 RID: 36057 RVA: 0x003514E9 File Offset: 0x0034F6E9
	public XUiV_ScrollBar(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008CDA RID: 36058 RVA: 0x00353860 File Offset: 0x00351A60
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UIScrollBar>();
	}

	// Token: 0x06008CDB RID: 36059 RVA: 0x00353870 File Offset: 0x00351A70
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.scrollBar = this.uiTransform.gameObject.GetComponent<UIScrollBar>();
		this.scrollBar.value = 0f;
	}

	// Token: 0x06008CDC RID: 36060 RVA: 0x003538A0 File Offset: 0x00351AA0
	public override void InitView()
	{
		base.InitView();
		if (!base.Controller.TryGetChildView<XUiV_Sprite>("thumb", out this.thumb))
		{
			Log.Error("[XUi] XUiV_ScrollBar without child sprite 'thumb'. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		if (!this.thumb.HasAnyEvent)
		{
			Log.Error("[XUi] XUiV_ScrollBar child sprite 'thumb' has no events enabled. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		if (base.Controller.TryGetChildView<XUiV_Sprite>("background", out this.background) && !this.background.HasAnyEvent)
		{
			Log.Error("[XUi] XUiV_ScrollBar child sprite 'background' has no events enabled. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		base.SetDirty();
	}

	// Token: 0x06008CDD RID: 36061 RVA: 0x00353948 File Offset: 0x00351B48
	public override void OnOpen()
	{
		base.OnOpen();
		this.scrollBar.alpha = 0f;
		if (this.thumb != null)
		{
			Color color = this.thumb.Color;
			color.a = 0f;
			this.thumb.Color = color;
		}
		if (this.background != null)
		{
			Color color2 = this.background.Color;
			color2.a = 0f;
			this.background.Color = color2;
		}
		this.onOpenValue = this.scrollBar.value;
		this.openFrame = Time.frameCount;
	}

	// Token: 0x06008CDE RID: 36062 RVA: 0x003539DF File Offset: 0x00351BDF
	public override void OnVisibilityChanged(bool _visibleInScene)
	{
		base.OnVisibilityChanged(_visibleInScene);
		if (_visibleInScene)
		{
			this.onOpenValue = this.scrollBar.value;
			this.openFrame = Time.frameCount;
		}
	}

	// Token: 0x06008CDF RID: 36063 RVA: 0x00353A08 File Offset: 0x00351C08
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (Time.frameCount <= this.openFrame + 1)
		{
			float value = this.scrollBar.value;
			if (!Mathf.Approximately(this.onOpenValue, value))
			{
				this.scrollBar.value = this.onOpenValue;
			}
		}
	}

	// Token: 0x06008CE0 RID: 36064 RVA: 0x00353A56 File Offset: 0x00351C56
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.refreshBoxCollider();
		base.updateData();
	}

	// Token: 0x06008CE1 RID: 36065 RVA: 0x00353A64 File Offset: 0x00351C64
	public void Connect(XUiEvent_OnScrollEventHandler _onScrolled)
	{
		this.scrollBar.foregroundWidget = this.thumb.Sprite;
		this.thumb.Controller.OnScroll += _onScrolled;
		if (this.background != null)
		{
			this.scrollBar.backgroundWidget = this.background.Sprite;
			this.background.Controller.OnScroll += _onScrolled;
		}
	}

	// Token: 0x04006762 RID: 26466
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollBar scrollBar;

	// Token: 0x04006763 RID: 26467
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite thumb;

	// Token: 0x04006764 RID: 26468
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006765 RID: 26469
	[PublicizedFrom(EAccessModifier.Private)]
	public float onOpenValue;

	// Token: 0x04006766 RID: 26470
	[PublicizedFrom(EAccessModifier.Private)]
	public int openFrame;
}
