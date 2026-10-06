using System;
using UnityEngine;

// Token: 0x02001176 RID: 4470
public class XUiV_ScrollBar : XUiView
{
	// Token: 0x17001108 RID: 4360
	// (get) Token: 0x06008E32 RID: 36402 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x17001109 RID: 4361
	// (get) Token: 0x06008E33 RID: 36403 RVA: 0x0035C474 File Offset: 0x0035A674
	public UIScrollBar ScrollBar
	{
		get
		{
			return this.scrollBar;
		}
	}

	// Token: 0x1700110A RID: 4362
	// (get) Token: 0x06008E34 RID: 36404 RVA: 0x0035C47C File Offset: 0x0035A67C
	// (set) Token: 0x06008E35 RID: 36405 RVA: 0x0035C484 File Offset: 0x0035A684
	[XuiXmlAttribute("thumb_color_hover", false)]
	public Color32 ThumbColorHover
	{
		get
		{
			return this.thumbColorHover;
		}
		set
		{
			if (this.thumbColorHover.ColorEquals(value))
			{
				return;
			}
			this.thumbColorHover = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110B RID: 4363
	// (get) Token: 0x06008E36 RID: 36406 RVA: 0x0035C4A2 File Offset: 0x0035A6A2
	// (set) Token: 0x06008E37 RID: 36407 RVA: 0x0035C4AA File Offset: 0x0035A6AA
	[XuiXmlAttribute("thumb_color_active", false)]
	public Color32 ThumbColorActive
	{
		get
		{
			return this.thumbColorActive;
		}
		set
		{
			if (this.thumbColorActive.ColorEquals(value))
			{
				return;
			}
			this.thumbColorActive = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110C RID: 4364
	// (get) Token: 0x06008E38 RID: 36408 RVA: 0x0035C4C8 File Offset: 0x0035A6C8
	// (set) Token: 0x06008E39 RID: 36409 RVA: 0x0035C4D0 File Offset: 0x0035A6D0
	[XuiXmlAttribute("thumb_color_inactive", false)]
	public Color32 ThumbColorInactive
	{
		get
		{
			return this.thumbColorInactive;
		}
		set
		{
			if (this.thumbColorInactive.ColorEquals(value))
			{
				return;
			}
			this.thumbColorInactive = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110D RID: 4365
	// (get) Token: 0x06008E3A RID: 36410 RVA: 0x0035C4EE File Offset: 0x0035A6EE
	// (set) Token: 0x06008E3B RID: 36411 RVA: 0x0035C4F6 File Offset: 0x0035A6F6
	[XuiXmlAttribute("border_color_active", false)]
	public Color32 BorderColorActive
	{
		get
		{
			return this.borderColorActive;
		}
		set
		{
			if (this.borderColorActive.ColorEquals(value))
			{
				return;
			}
			this.borderColorActive = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110E RID: 4366
	// (get) Token: 0x06008E3C RID: 36412 RVA: 0x0035C514 File Offset: 0x0035A714
	// (set) Token: 0x06008E3D RID: 36413 RVA: 0x0035C51C File Offset: 0x0035A71C
	[XuiXmlAttribute("border_color_inactive", false)]
	public Color32 BorderColorInactive
	{
		get
		{
			return this.borderColorInactive;
		}
		set
		{
			if (this.borderColorInactive.ColorEquals(value))
			{
				return;
			}
			this.borderColorInactive = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110F RID: 4367
	// (get) Token: 0x06008E3E RID: 36414 RVA: 0x0035C53A File Offset: 0x0035A73A
	// (set) Token: 0x06008E3F RID: 36415 RVA: 0x0035C542 File Offset: 0x0035A742
	[XuiXmlAttribute("background_color_active", false)]
	public Color32 BackgroundColorActive
	{
		get
		{
			return this.backgroundColorActive;
		}
		set
		{
			if (this.backgroundColorActive.ColorEquals(value))
			{
				return;
			}
			this.backgroundColorActive = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001110 RID: 4368
	// (get) Token: 0x06008E40 RID: 36416 RVA: 0x0035C560 File Offset: 0x0035A760
	// (set) Token: 0x06008E41 RID: 36417 RVA: 0x0035C568 File Offset: 0x0035A768
	[XuiXmlAttribute("background_color_inactive", false)]
	public Color32 BackgroundColorInactive
	{
		get
		{
			return this.backgroundColorInactive;
		}
		set
		{
			if (this.backgroundColorInactive.ColorEquals(value))
			{
				return;
			}
			this.backgroundColorInactive = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001111 RID: 4369
	// (get) Token: 0x06008E42 RID: 36418 RVA: 0x0035C586 File Offset: 0x0035A786
	// (set) Token: 0x06008E43 RID: 36419 RVA: 0x0035C593 File Offset: 0x0035A793
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

	// Token: 0x17001112 RID: 4370
	// (get) Token: 0x06008E44 RID: 36420 RVA: 0x0035C5A7 File Offset: 0x0035A7A7
	public override Vector3[] WorldCorners
	{
		get
		{
			if (this.border != null)
			{
				return this.border.WorldCorners;
			}
			XUiV_Sprite xuiV_Sprite = this.thumb;
			if (xuiV_Sprite == null)
			{
				return null;
			}
			return xuiV_Sprite.WorldCorners;
		}
	}

	// Token: 0x17001113 RID: 4371
	// (get) Token: 0x06008E45 RID: 36421 RVA: 0x0035C5CE File Offset: 0x0035A7CE
	// (set) Token: 0x06008E46 RID: 36422 RVA: 0x0035C5D6 File Offset: 0x0035A7D6
	public bool BarRequired
	{
		get
		{
			return this.barRequired;
		}
		set
		{
			if (this.barRequired == value)
			{
				return;
			}
			this.barRequired = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001114 RID: 4372
	// (get) Token: 0x06008E47 RID: 36423 RVA: 0x0035C5EF File Offset: 0x0035A7EF
	// (set) Token: 0x06008E48 RID: 36424 RVA: 0x0035C5F7 File Offset: 0x0035A7F7
	public bool ThumbHovered
	{
		get
		{
			return this.thumbHovered;
		}
		set
		{
			if (this.thumbHovered == value)
			{
				return;
			}
			this.thumbHovered = value;
			base.SetDirty();
		}
	}

	// Token: 0x06008E49 RID: 36425 RVA: 0x0035A13D File Offset: 0x0035833D
	public XUiV_ScrollBar(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E4A RID: 36426 RVA: 0x0035C610 File Offset: 0x0035A810
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UIScrollBar>();
	}

	// Token: 0x06008E4B RID: 36427 RVA: 0x0035C620 File Offset: 0x0035A820
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.scrollBar = this.uiTransform.gameObject.GetComponent<UIScrollBar>();
		this.scrollBar.value = 0f;
	}

	// Token: 0x06008E4C RID: 36428 RVA: 0x0035C650 File Offset: 0x0035A850
	public override void InitView()
	{
		base.InitView();
		if (!base.Controller.TryGetChildView<XUiV_Sprite>("scrollbarthumb", out this.thumb))
		{
			Log.Error("[XUi] XUiV_ScrollBar without child sprite 'scrollbarthumb'. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		if (!this.thumb.HasAnyEvent)
		{
			Log.Error("[XUi] XUiV_ScrollBar child sprite 'scrollbarthumb' has no events enabled. Hierarchy: " + base.GetXuiHierarchy());
			return;
		}
		if (base.Controller.TryGetChildView<XUiV_Sprite>("scrollbarborder", out this.border) && !this.border.HasAnyEvent)
		{
			Log.Out("[XUi] XUiV_ScrollBar child sprite 'scrollbarborder' has no events enabled. Hierarchy: " + base.GetXuiHierarchy());
		}
		this.thumb.Controller.OnHover += this.onHovered;
		if (this.border != null)
		{
			this.border.Controller.OnHover += this.onHovered;
		}
		base.Controller.TryGetChildView<XUiV_Sprite>("scrollbarbackground", out this.background);
		base.SetDirty();
	}

	// Token: 0x06008E4D RID: 36429 RVA: 0x0035C74B File Offset: 0x0035A94B
	[PublicizedFrom(EAccessModifier.Private)]
	public void onHovered(XUiController _sender, bool _isOver)
	{
		this.ThumbHovered = _isOver;
	}

	// Token: 0x06008E4E RID: 36430 RVA: 0x0035C754 File Offset: 0x0035A954
	public override void OnOpen()
	{
		base.OnOpen();
		this.onOpenValue = this.scrollBar.value;
		this.openFrame = Time.frameCount;
	}

	// Token: 0x06008E4F RID: 36431 RVA: 0x0035C778 File Offset: 0x0035A978
	public override void OnVisibilityChanged(bool _visibleInScene)
	{
		base.OnVisibilityChanged(_visibleInScene);
		if (_visibleInScene)
		{
			this.onOpenValue = this.scrollBar.value;
			this.openFrame = Time.frameCount;
		}
	}

	// Token: 0x06008E50 RID: 36432 RVA: 0x0035C7A0 File Offset: 0x0035A9A0
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

	// Token: 0x06008E51 RID: 36433 RVA: 0x0035C7EE File Offset: 0x0035A9EE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.refreshBoxCollider();
		this.updateScrollColors();
		base.updateData();
	}

	// Token: 0x06008E52 RID: 36434 RVA: 0x0035C804 File Offset: 0x0035AA04
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateScrollColors()
	{
		this.thumb.Color = ((!this.BarRequired) ? this.thumbColorInactive : (this.ThumbHovered ? this.thumbColorHover : this.thumbColorActive));
		if (this.border != null)
		{
			this.border.Color = (this.BarRequired ? this.borderColorActive : this.borderColorInactive);
		}
		if (this.background != null)
		{
			this.background.Color = (this.BarRequired ? this.backgroundColorActive : this.backgroundColorInactive);
		}
	}

	// Token: 0x06008E53 RID: 36435 RVA: 0x0035C8A4 File Offset: 0x0035AAA4
	public void Connect(XUiEvent_OnScrollEventHandler _onScrolled, EventDelegate.Callback scrollbarChanged)
	{
		EventDelegate.Add(this.scrollBar.onChange, scrollbarChanged);
		this.scrollBar.foregroundWidget = this.thumb.Sprite;
		this.thumb.Controller.OnScroll += _onScrolled;
		if (this.border != null)
		{
			this.scrollBar.backgroundWidget = this.border.Sprite;
			this.border.Controller.OnScroll += _onScrolled;
		}
	}

	// Token: 0x04006881 RID: 26753
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollBar scrollBar;

	// Token: 0x04006882 RID: 26754
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite thumb;

	// Token: 0x04006883 RID: 26755
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x04006884 RID: 26756
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006885 RID: 26757
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 thumbColorHover;

	// Token: 0x04006886 RID: 26758
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 thumbColorActive;

	// Token: 0x04006887 RID: 26759
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 thumbColorInactive;

	// Token: 0x04006888 RID: 26760
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 borderColorActive;

	// Token: 0x04006889 RID: 26761
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 borderColorInactive;

	// Token: 0x0400688A RID: 26762
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 backgroundColorActive;

	// Token: 0x0400688B RID: 26763
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 backgroundColorInactive;

	// Token: 0x0400688C RID: 26764
	[PublicizedFrom(EAccessModifier.Private)]
	public bool barRequired;

	// Token: 0x0400688D RID: 26765
	[PublicizedFrom(EAccessModifier.Private)]
	public bool thumbHovered;

	// Token: 0x0400688E RID: 26766
	[PublicizedFrom(EAccessModifier.Private)]
	public float onOpenValue;

	// Token: 0x0400688F RID: 26767
	[PublicizedFrom(EAccessModifier.Private)]
	public int openFrame;
}
