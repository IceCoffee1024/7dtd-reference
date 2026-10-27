using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001187 RID: 4487
public class XUiV_ScrollView : XUiV_Panel
{
	// Token: 0x17001120 RID: 4384
	// (get) Token: 0x06008EA8 RID: 36520 RVA: 0x0035E9E9 File Offset: 0x0035CBE9
	// (set) Token: 0x06008EA9 RID: 36521 RVA: 0x0035E9F1 File Offset: 0x0035CBF1
	[XuiXmlAttribute("movement", false)]
	public UIScrollView.Movement Movement
	{
		get
		{
			return this.movement;
		}
		set
		{
			if (value == this.movement)
			{
				return;
			}
			this.movement = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001121 RID: 4385
	// (get) Token: 0x06008EAA RID: 36522 RVA: 0x0035EA0A File Offset: 0x0035CC0A
	// (set) Token: 0x06008EAB RID: 36523 RVA: 0x0035EA12 File Offset: 0x0035CC12
	[XuiXmlAttribute("drageffect", false)]
	public UIScrollView.DragEffect DragEffect
	{
		get
		{
			return this.dragEffect;
		}
		set
		{
			if (value == this.dragEffect)
			{
				return;
			}
			this.dragEffect = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001122 RID: 4386
	// (get) Token: 0x06008EAC RID: 36524 RVA: 0x0035EA2B File Offset: 0x0035CC2B
	// (set) Token: 0x06008EAD RID: 36525 RVA: 0x0035EA33 File Offset: 0x0035CC33
	[XuiXmlAttribute("showcondition", false)]
	public UIScrollView.ShowCondition ShowCondition
	{
		get
		{
			return this.showCondition;
		}
		set
		{
			if (value == this.showCondition)
			{
				return;
			}
			this.showCondition = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001123 RID: 4387
	// (get) Token: 0x06008EAE RID: 36526 RVA: 0x0035EA4C File Offset: 0x0035CC4C
	// (set) Token: 0x06008EAF RID: 36527 RVA: 0x0035EA54 File Offset: 0x0035CC54
	[XuiXmlAttribute("scrollfactor", false)]
	public float ScrollFactor
	{
		get
		{
			return this.scrollFactor;
		}
		set
		{
			if (Mathf.Approximately(value, this.scrollFactor))
			{
				return;
			}
			this.scrollFactor = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001124 RID: 4388
	// (get) Token: 0x06008EB0 RID: 36528 RVA: 0x0035EA72 File Offset: 0x0035CC72
	// (set) Token: 0x06008EB1 RID: 36529 RVA: 0x0035EA7A File Offset: 0x0035CC7A
	[XuiXmlAttribute("scrollwithoutfocus", false)]
	public bool ScrollWithoutFocus
	{
		get
		{
			return this.scrollWithoutFocus;
		}
		set
		{
			if (value == this.scrollWithoutFocus)
			{
				return;
			}
			this.scrollWithoutFocus = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001125 RID: 4389
	// (get) Token: 0x06008EB2 RID: 36530 RVA: 0x0035EA93 File Offset: 0x0035CC93
	// (set) Token: 0x06008EB3 RID: 36531 RVA: 0x0035EA9B File Offset: 0x0035CC9B
	[XuiXmlAttribute("resetpositiononopen", false)]
	public bool ResetPositionOnOpen { get; set; } = true;

	// Token: 0x17001126 RID: 4390
	// (get) Token: 0x06008EB4 RID: 36532 RVA: 0x0035EAA4 File Offset: 0x0035CCA4
	// (set) Token: 0x06008EB5 RID: 36533 RVA: 0x0035EAAC File Offset: 0x0035CCAC
	[XuiXmlAttribute("updatepositiononopen", false)]
	public bool UpdatePositionOnOpen { get; set; } = true;

	// Token: 0x17001127 RID: 4391
	// (get) Token: 0x06008EB6 RID: 36534 RVA: 0x0035EAB5 File Offset: 0x0035CCB5
	// (set) Token: 0x06008EB7 RID: 36535 RVA: 0x0035EABD File Offset: 0x0035CCBD
	[XuiXmlAttribute("maxboundsdepth", false)]
	public int MaxBoundsDepth
	{
		get
		{
			return this.maxBoundsDepth;
		}
		set
		{
			if (value == this.maxBoundsDepth)
			{
				return;
			}
			this.maxBoundsDepth = value;
			base.SetDirty();
		}
	}

	// Token: 0x06008EB8 RID: 36536 RVA: 0x0035EAD6 File Offset: 0x0035CCD6
	public XUiV_ScrollView(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008EB9 RID: 36537 RVA: 0x0035EB0E File Offset: 0x0035CD0E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UIScrollView>();
	}

	// Token: 0x06008EBA RID: 36538 RVA: 0x0035EB1E File Offset: 0x0035CD1E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.scrollView = this.uiTransform.gameObject.GetComponent<UIScrollView>();
	}

	// Token: 0x06008EBB RID: 36539 RVA: 0x0035EB3C File Offset: 0x0035CD3C
	public override void InitView()
	{
		base.InitView();
		this.controller.Parent.OnScroll += this.OnScrolled;
		this.xui.OnBuilt += this.OnXuiLoadDone;
		base.SetDirty();
	}

	// Token: 0x06008EBC RID: 36540 RVA: 0x0035EB88 File Offset: 0x0035CD88
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.OnBuilt -= this.OnXuiLoadDone;
	}

	// Token: 0x06008EBD RID: 36541 RVA: 0x0035EBA8 File Offset: 0x0035CDA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnXuiLoadDone()
	{
		this.xui.OnBuilt -= this.OnXuiLoadDone;
		this.applyScrollEventToChildren(this.controller);
		this.controller.Parent.TryGetChildView<XUiV_ScrollBar>("", out this.scrollBar);
		XUiV_ScrollBar xuiV_ScrollBar = this.scrollBar;
		if (xuiV_ScrollBar == null)
		{
			return;
		}
		xuiV_ScrollBar.Connect(new XUiEvent_OnScrollEventHandler(this.OnScrolled), new EventDelegate.Callback(this.scrollbarChanged));
	}

	// Token: 0x06008EBE RID: 36542 RVA: 0x0035EC1C File Offset: 0x0035CE1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyScrollEventToChildren(XUiController _controller)
	{
		foreach (XUiController xuiController in _controller.Children)
		{
			this.applyScrollEventToChildren(xuiController);
			xuiController.ViewComponent.EventOnScroll = true;
			xuiController.OnScroll += this.OnScrolled;
		}
	}

	// Token: 0x06008EBF RID: 36543 RVA: 0x0035EC90 File Offset: 0x0035CE90
	[PublicizedFrom(EAccessModifier.Private)]
	public void scrollbarChanged()
	{
		this.updateScrollbarsNextUpdate = true;
	}

	// Token: 0x06008EC0 RID: 36544 RVA: 0x0035EC9C File Offset: 0x0035CE9C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.controllerScroll(Time.unscaledDeltaTime);
		if (Time.frameCount <= this.openFrame + 1)
		{
			this.scrollView.CheckScrollbars();
			this.UpdateScrollbarsRequired();
			if (this.ResetPositionOnOpen)
			{
				this.ResetPosition();
			}
			if (this.UpdatePositionOnOpen)
			{
				this.UpdatePosition();
			}
		}
		if (this.updateScrollbarsNextUpdate)
		{
			this.updateScrollbarsNextUpdate = false;
			this.scrollView.RestrictWithinBounds(true, this.scrollView.canMoveHorizontally, this.scrollView.canMoveVertically);
			if (!this.UpdateScrollbarsRequired())
			{
				this.ResetPosition();
			}
		}
	}

	// Token: 0x06008EC1 RID: 36545 RVA: 0x0035ED38 File Offset: 0x0035CF38
	[PublicizedFrom(EAccessModifier.Private)]
	public void controllerScroll(float _dt)
	{
		if (!XUiUtils.HotkeysAllowedFor(this.controller.Parent.ViewComponent, false))
		{
			return;
		}
		XUiView currentTarget = this.xui.playerUI.CursorController.CurrentTarget;
		if (!this.ScrollWithoutFocus && (currentTarget == null || !currentTarget.Controller.IsSelfOrChildOf(this.controller.Parent)))
		{
			return;
		}
		Vector2 vector = this.xui.playerUI.playerInput.GUIActions.Camera.Vector;
		float num = Mathf.Abs(vector.x);
		float num2 = Mathf.Abs(vector.y);
		if (this.movement == UIScrollView.Movement.Vertical)
		{
			if ((double)num2 < (double)num * 1.8)
			{
				return;
			}
			if ((double)num2 > 0.1)
			{
				this.scrollView.Scroll(vector.y * _dt);
				return;
			}
		}
		else
		{
			if ((double)num < (double)num2 * 1.8)
			{
				return;
			}
			if ((double)num > 0.1)
			{
				this.scrollView.Scroll(vector.x * _dt);
			}
		}
	}

	// Token: 0x06008EC2 RID: 36546 RVA: 0x0035EE3C File Offset: 0x0035D03C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		base.Clipping = UIDrawCall.Clipping.SoftClip;
		this.scrollView.movement = this.movement;
		this.scrollView.dragEffect = this.dragEffect;
		this.scrollView.showScrollBars = this.showCondition;
		this.scrollView.scrollWheelFactor = this.scrollFactor;
		this.scrollView.constrainOnDrag = true;
		this.scrollView.disableDragIfFits = true;
		this.scrollView.smoothDragStart = true;
		this.scrollView.maxBoundsDepth = this.MaxBoundsDepth;
		XUiV_ScrollBar xuiV_ScrollBar = this.scrollBar;
		UIScrollBar uiscrollBar = ((xuiV_ScrollBar != null) ? xuiV_ScrollBar.ScrollBar : null);
		if (uiscrollBar != null)
		{
			uiscrollBar.fillDirection = ((this.movement == UIScrollView.Movement.Horizontal) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.TopToBottom);
			UIScrollView.Movement movement = this.movement;
			if (movement != UIScrollView.Movement.Horizontal)
			{
				if (movement == UIScrollView.Movement.Vertical)
				{
					this.scrollView.horizontalScrollBar = null;
					this.scrollView.verticalScrollBar = uiscrollBar;
				}
			}
			else
			{
				this.scrollView.horizontalScrollBar = uiscrollBar;
				this.scrollView.verticalScrollBar = null;
			}
			this.scrollView.CheckScrollbars();
		}
		base.updateData();
	}

	// Token: 0x17001128 RID: 4392
	// (get) Token: 0x06008EC3 RID: 36547 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool updateClippingOffset
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x06008EC4 RID: 36548 RVA: 0x0035EF4A File Offset: 0x0035D14A
	public override void OnOpen()
	{
		base.OnOpen();
		this.openFrame = Time.frameCount;
	}

	// Token: 0x06008EC5 RID: 36549 RVA: 0x0035EF5D File Offset: 0x0035D15D
	public override void OnVisibilityChanged(bool _visibleInScene)
	{
		base.OnVisibilityChanged(_visibleInScene);
		if (_visibleInScene)
		{
			this.openFrame = Time.frameCount;
		}
	}

	// Token: 0x06008EC6 RID: 36550 RVA: 0x0035EF74 File Offset: 0x0035D174
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrolled(XUiController _sender, float _delta)
	{
		if (InputUtils.ShiftKeyPressed)
		{
			return;
		}
		this.scrollView.Scroll(_delta);
	}

	// Token: 0x06008EC7 RID: 36551 RVA: 0x0035EF8A File Offset: 0x0035D18A
	public void ResetPosition()
	{
		this.scrollView.ResetPosition();
		this.UpdateScrollbarsRequired();
	}

	// Token: 0x06008EC8 RID: 36552 RVA: 0x0035EF9E File Offset: 0x0035D19E
	public void UpdatePosition()
	{
		this.scrollView.UpdatePosition();
		this.UpdateScrollbarsRequired();
	}

	// Token: 0x06008EC9 RID: 36553 RVA: 0x0035EFB4 File Offset: 0x0035D1B4
	public bool UpdateScrollbarsRequired()
	{
		return this.scrollBar.BarRequired = ((this.Movement == UIScrollView.Movement.Vertical) ? this.scrollView.shouldMoveVertically : this.scrollView.shouldMoveHorizontally);
	}

	// Token: 0x06008ECA RID: 36554 RVA: 0x0035EFF0 File Offset: 0x0035D1F0
	public void MakeVisible(XUiView _viewComponent)
	{
		if (_viewComponent == null)
		{
			return;
		}
		Vector3[] worldCorners = _viewComponent.WorldCorners;
		this.MakeVisible(worldCorners);
	}

	// Token: 0x06008ECB RID: 36555 RVA: 0x0035F010 File Offset: 0x0035D210
	public void MakeVisible(IList<Vector3> _worldCorners)
	{
		if (_worldCorners == null || _worldCorners.Count < 4)
		{
			return;
		}
		Transform cachedTransform = this.panel.cachedTransform;
		Vector3[] worldCorners = this.panel.worldCorners;
		Vector3 vector = cachedTransform.InverseTransformPoint(worldCorners[1]);
		Vector3 vector2 = cachedTransform.InverseTransformPoint(worldCorners[3]);
		Vector3 vector3 = cachedTransform.InverseTransformPoint(_worldCorners[1]);
		Vector3 vector4 = cachedTransform.InverseTransformPoint(_worldCorners[3]);
		Vector3 zero = Vector3.zero;
		if (!this.scrollView.canMoveHorizontally)
		{
			zero.x = 0f;
		}
		else if (vector3.x < vector.x)
		{
			zero.x = vector.x - vector3.x + base.ClippingSoftness.x;
		}
		else if (vector4.x > vector2.x)
		{
			zero.x = vector2.x - vector4.x - base.ClippingSoftness.x;
		}
		if (!this.scrollView.canMoveVertically)
		{
			zero.y = 0f;
		}
		else if (vector3.y > vector.y)
		{
			zero.y = vector.y - vector3.y - base.ClippingSoftness.y;
		}
		else if (vector4.y < vector2.y)
		{
			zero.y = vector2.y - vector4.y + base.ClippingSoftness.y;
		}
		zero.z = 0f;
		cachedTransform.localPosition += zero;
		Vector2 clipOffset = this.panel.clipOffset;
		clipOffset.x -= zero.x;
		clipOffset.y -= zero.y;
		this.panel.clipOffset = clipOffset;
		this.scrollView.UpdateScrollbars();
	}

	// Token: 0x06008ECC RID: 36556 RVA: 0x0035F1E0 File Offset: 0x0035D3E0
	public void CenterOn(XUiView _target)
	{
		if (_target == null)
		{
			return;
		}
		Vector3[] worldCorners = this.panel.worldCorners;
		Vector3 vector = (worldCorners[2] + worldCorners[0]) * 0.5f;
		Transform cachedTransform = this.panel.cachedTransform;
		Vector3 vector2 = cachedTransform.InverseTransformPoint(_target.UiTransform.position);
		Vector3 vector3 = cachedTransform.InverseTransformPoint(vector);
		Vector3 vector4 = vector2 - vector3;
		if (!this.scrollView.canMoveHorizontally)
		{
			vector4.x = 0f;
		}
		if (!this.scrollView.canMoveVertically)
		{
			vector4.y = 0f;
		}
		vector4.z = 0f;
		cachedTransform.localPosition -= vector4;
		Vector2 clipOffset = this.panel.clipOffset;
		clipOffset.x += vector4.x;
		clipOffset.y += vector4.y;
		this.panel.clipOffset = clipOffset;
		this.scrollView.UpdateScrollbars();
	}

	// Token: 0x040068D0 RID: 26832
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView scrollView;

	// Token: 0x040068D1 RID: 26833
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_ScrollBar scrollBar;

	// Token: 0x040068D2 RID: 26834
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView.Movement movement = UIScrollView.Movement.Vertical;

	// Token: 0x040068D3 RID: 26835
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView.DragEffect dragEffect;

	// Token: 0x040068D4 RID: 26836
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView.ShowCondition showCondition = UIScrollView.ShowCondition.OnlyIfNeeded;

	// Token: 0x040068D5 RID: 26837
	[PublicizedFrom(EAccessModifier.Private)]
	public float scrollFactor = 2f;

	// Token: 0x040068D6 RID: 26838
	[PublicizedFrom(EAccessModifier.Private)]
	public bool scrollWithoutFocus;

	// Token: 0x040068D9 RID: 26841
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxBoundsDepth = -1;

	// Token: 0x040068DA RID: 26842
	[PublicizedFrom(EAccessModifier.Private)]
	public int openFrame;

	// Token: 0x040068DB RID: 26843
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateScrollbarsNextUpdate;
}
