using System;
using UnityEngine;

// Token: 0x02001186 RID: 4486
public class XUiV_ScrollBar : XUiView
{
	// Token: 0x17001113 RID: 4371
	// (get) Token: 0x06008E86 RID: 36486 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x17001114 RID: 4372
	// (get) Token: 0x06008E87 RID: 36487 RVA: 0x0035E544 File Offset: 0x0035C744
	public UIScrollBar ScrollBar
	{
		get
		{
			return this.scrollBar;
		}
	}

	// Token: 0x17001115 RID: 4373
	// (get) Token: 0x06008E88 RID: 36488 RVA: 0x0035E54C File Offset: 0x0035C74C
	// (set) Token: 0x06008E89 RID: 36489 RVA: 0x0035E554 File Offset: 0x0035C754
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

	// Token: 0x17001116 RID: 4374
	// (get) Token: 0x06008E8A RID: 36490 RVA: 0x0035E572 File Offset: 0x0035C772
	// (set) Token: 0x06008E8B RID: 36491 RVA: 0x0035E57A File Offset: 0x0035C77A
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

	// Token: 0x17001117 RID: 4375
	// (get) Token: 0x06008E8C RID: 36492 RVA: 0x0035E598 File Offset: 0x0035C798
	// (set) Token: 0x06008E8D RID: 36493 RVA: 0x0035E5A0 File Offset: 0x0035C7A0
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

	// Token: 0x17001118 RID: 4376
	// (get) Token: 0x06008E8E RID: 36494 RVA: 0x0035E5BE File Offset: 0x0035C7BE
	// (set) Token: 0x06008E8F RID: 36495 RVA: 0x0035E5C6 File Offset: 0x0035C7C6
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

	// Token: 0x17001119 RID: 4377
	// (get) Token: 0x06008E90 RID: 36496 RVA: 0x0035E5E4 File Offset: 0x0035C7E4
	// (set) Token: 0x06008E91 RID: 36497 RVA: 0x0035E5EC File Offset: 0x0035C7EC
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

	// Token: 0x1700111A RID: 4378
	// (get) Token: 0x06008E92 RID: 36498 RVA: 0x0035E60A File Offset: 0x0035C80A
	// (set) Token: 0x06008E93 RID: 36499 RVA: 0x0035E612 File Offset: 0x0035C812
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

	// Token: 0x1700111B RID: 4379
	// (get) Token: 0x06008E94 RID: 36500 RVA: 0x0035E630 File Offset: 0x0035C830
	// (set) Token: 0x06008E95 RID: 36501 RVA: 0x0035E638 File Offset: 0x0035C838
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

	// Token: 0x1700111C RID: 4380
	// (get) Token: 0x06008E96 RID: 36502 RVA: 0x0035E656 File Offset: 0x0035C856
	// (set) Token: 0x06008E97 RID: 36503 RVA: 0x0035E663 File Offset: 0x0035C863
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

	// Token: 0x1700111D RID: 4381
	// (get) Token: 0x06008E98 RID: 36504 RVA: 0x0035E677 File Offset: 0x0035C877
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

	// Token: 0x1700111E RID: 4382
	// (get) Token: 0x06008E99 RID: 36505 RVA: 0x0035E69E File Offset: 0x0035C89E
	// (set) Token: 0x06008E9A RID: 36506 RVA: 0x0035E6A6 File Offset: 0x0035C8A6
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

	// Token: 0x1700111F RID: 4383
	// (get) Token: 0x06008E9B RID: 36507 RVA: 0x0035E6BF File Offset: 0x0035C8BF
	// (set) Token: 0x06008E9C RID: 36508 RVA: 0x0035E6C7 File Offset: 0x0035C8C7
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

	// Token: 0x06008E9D RID: 36509 RVA: 0x0035C1E9 File Offset: 0x0035A3E9
	public XUiV_ScrollBar(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E9E RID: 36510 RVA: 0x0035E6E0 File Offset: 0x0035C8E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UIScrollBar>();
	}

	// Token: 0x06008E9F RID: 36511 RVA: 0x0035E6F0 File Offset: 0x0035C8F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.scrollBar = this.uiTransform.gameObject.GetComponent<UIScrollBar>();
		this.scrollBar.value = 0f;
	}

	// Token: 0x06008EA0 RID: 36512 RVA: 0x0035E720 File Offset: 0x0035C920
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

	// Token: 0x06008EA1 RID: 36513 RVA: 0x0035E81B File Offset: 0x0035CA1B
	[PublicizedFrom(EAccessModifier.Private)]
	public void onHovered(XUiController _sender, bool _isOver)
	{
		this.ThumbHovered = _isOver;
	}

	// Token: 0x06008EA2 RID: 36514 RVA: 0x0035E824 File Offset: 0x0035CA24
	public override void OnOpen()
	{
		base.OnOpen();
		this.onOpenValue = this.scrollBar.value;
		this.openFrame = Time.frameCount;
	}

	// Token: 0x06008EA3 RID: 36515 RVA: 0x0035E848 File Offset: 0x0035CA48
	public override void OnVisibilityChanged(bool _visibleInScene)
	{
		base.OnVisibilityChanged(_visibleInScene);
		if (_visibleInScene)
		{
			this.onOpenValue = this.scrollBar.value;
			this.openFrame = Time.frameCount;
		}
	}

	// Token: 0x06008EA4 RID: 36516 RVA: 0x0035E870 File Offset: 0x0035CA70
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

	// Token: 0x06008EA5 RID: 36517 RVA: 0x0035E8BE File Offset: 0x0035CABE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.refreshBoxCollider();
		this.updateScrollColors();
		base.updateData();
	}

	// Token: 0x06008EA6 RID: 36518 RVA: 0x0035E8D4 File Offset: 0x0035CAD4
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

	// Token: 0x06008EA7 RID: 36519 RVA: 0x0035E974 File Offset: 0x0035CB74
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

	// Token: 0x040068C1 RID: 26817
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollBar scrollBar;

	// Token: 0x040068C2 RID: 26818
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite thumb;

	// Token: 0x040068C3 RID: 26819
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite border;

	// Token: 0x040068C4 RID: 26820
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x040068C5 RID: 26821
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 thumbColorHover;

	// Token: 0x040068C6 RID: 26822
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 thumbColorActive;

	// Token: 0x040068C7 RID: 26823
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 thumbColorInactive;

	// Token: 0x040068C8 RID: 26824
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 borderColorActive;

	// Token: 0x040068C9 RID: 26825
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 borderColorInactive;

	// Token: 0x040068CA RID: 26826
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 backgroundColorActive;

	// Token: 0x040068CB RID: 26827
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 backgroundColorInactive;

	// Token: 0x040068CC RID: 26828
	[PublicizedFrom(EAccessModifier.Private)]
	public bool barRequired;

	// Token: 0x040068CD RID: 26829
	[PublicizedFrom(EAccessModifier.Private)]
	public bool thumbHovered;

	// Token: 0x040068CE RID: 26830
	[PublicizedFrom(EAccessModifier.Private)]
	public float onOpenValue;

	// Token: 0x040068CF RID: 26831
	[PublicizedFrom(EAccessModifier.Private)]
	public int openFrame;
}
