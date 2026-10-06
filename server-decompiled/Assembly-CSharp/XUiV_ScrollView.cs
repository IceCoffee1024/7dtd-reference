using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001177 RID: 4471
public class XUiV_ScrollView : XUiV_Panel
{
	// Token: 0x17001115 RID: 4373
	// (get) Token: 0x06008E54 RID: 36436 RVA: 0x0035C919 File Offset: 0x0035AB19
	// (set) Token: 0x06008E55 RID: 36437 RVA: 0x0035C921 File Offset: 0x0035AB21
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

	// Token: 0x17001116 RID: 4374
	// (get) Token: 0x06008E56 RID: 36438 RVA: 0x0035C93A File Offset: 0x0035AB3A
	// (set) Token: 0x06008E57 RID: 36439 RVA: 0x0035C942 File Offset: 0x0035AB42
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

	// Token: 0x17001117 RID: 4375
	// (get) Token: 0x06008E58 RID: 36440 RVA: 0x0035C95B File Offset: 0x0035AB5B
	// (set) Token: 0x06008E59 RID: 36441 RVA: 0x0035C963 File Offset: 0x0035AB63
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

	// Token: 0x17001118 RID: 4376
	// (get) Token: 0x06008E5A RID: 36442 RVA: 0x0035C97C File Offset: 0x0035AB7C
	// (set) Token: 0x06008E5B RID: 36443 RVA: 0x0035C984 File Offset: 0x0035AB84
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

	// Token: 0x17001119 RID: 4377
	// (get) Token: 0x06008E5C RID: 36444 RVA: 0x0035C9A2 File Offset: 0x0035ABA2
	// (set) Token: 0x06008E5D RID: 36445 RVA: 0x0035C9AA File Offset: 0x0035ABAA
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

	// Token: 0x1700111A RID: 4378
	// (get) Token: 0x06008E5E RID: 36446 RVA: 0x0035C9C3 File Offset: 0x0035ABC3
	// (set) Token: 0x06008E5F RID: 36447 RVA: 0x0035C9CB File Offset: 0x0035ABCB
	[XuiXmlAttribute("resetpositiononopen", false)]
	public bool ResetPositionOnOpen { get; set; } = true;

	// Token: 0x1700111B RID: 4379
	// (get) Token: 0x06008E60 RID: 36448 RVA: 0x0035C9D4 File Offset: 0x0035ABD4
	// (set) Token: 0x06008E61 RID: 36449 RVA: 0x0035C9DC File Offset: 0x0035ABDC
	[XuiXmlAttribute("updatepositiononopen", false)]
	public bool UpdatePositionOnOpen { get; set; } = true;

	// Token: 0x1700111C RID: 4380
	// (get) Token: 0x06008E62 RID: 36450 RVA: 0x0035C9E5 File Offset: 0x0035ABE5
	// (set) Token: 0x06008E63 RID: 36451 RVA: 0x0035C9ED File Offset: 0x0035ABED
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

	// Token: 0x06008E64 RID: 36452 RVA: 0x0035CA06 File Offset: 0x0035AC06
	public XUiV_ScrollView(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E65 RID: 36453 RVA: 0x0035CA3E File Offset: 0x0035AC3E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UIScrollView>();
	}

	// Token: 0x06008E66 RID: 36454 RVA: 0x0035CA4E File Offset: 0x0035AC4E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.scrollView = this.uiTransform.gameObject.GetComponent<UIScrollView>();
	}

	// Token: 0x06008E67 RID: 36455 RVA: 0x0035CA6C File Offset: 0x0035AC6C
	public override void InitView()
	{
		base.InitView();
		this.controller.Parent.OnScroll += this.OnScrolled;
		this.xui.OnBuilt += this.OnXuiLoadDone;
		base.SetDirty();
	}

	// Token: 0x06008E68 RID: 36456 RVA: 0x0035CAB8 File Offset: 0x0035ACB8
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.OnBuilt -= this.OnXuiLoadDone;
	}

	// Token: 0x06008E69 RID: 36457 RVA: 0x0035CAD8 File Offset: 0x0035ACD8
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

	// Token: 0x06008E6A RID: 36458 RVA: 0x0035CB4C File Offset: 0x0035AD4C
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

	// Token: 0x06008E6B RID: 36459 RVA: 0x0035CBC0 File Offset: 0x0035ADC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void scrollbarChanged()
	{
		this.updateScrollbarsNextUpdate = true;
	}

	// Token: 0x06008E6C RID: 36460 RVA: 0x0035CBCC File Offset: 0x0035ADCC
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
			if (!this.UpdateScrollbarsRequired())
			{
				this.ResetPosition();
			}
		}
	}

	// Token: 0x06008E6D RID: 36461 RVA: 0x0035CC48 File Offset: 0x0035AE48
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

	// Token: 0x06008E6E RID: 36462 RVA: 0x0035CD4C File Offset: 0x0035AF4C
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

	// Token: 0x1700111D RID: 4381
	// (get) Token: 0x06008E6F RID: 36463 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool updateClippingOffset
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return false;
		}
	}

	// Token: 0x06008E70 RID: 36464 RVA: 0x0035CE5A File Offset: 0x0035B05A
	public override void OnOpen()
	{
		base.OnOpen();
		this.openFrame = Time.frameCount;
	}

	// Token: 0x06008E71 RID: 36465 RVA: 0x0035CE6D File Offset: 0x0035B06D
	public override void OnVisibilityChanged(bool _visibleInScene)
	{
		base.OnVisibilityChanged(_visibleInScene);
		if (_visibleInScene)
		{
			this.openFrame = Time.frameCount;
		}
	}

	// Token: 0x06008E72 RID: 36466 RVA: 0x0035CE84 File Offset: 0x0035B084
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrolled(XUiController _sender, float _delta)
	{
		if (InputUtils.ShiftKeyPressed)
		{
			return;
		}
		this.scrollView.Scroll(_delta);
	}

	// Token: 0x06008E73 RID: 36467 RVA: 0x0035CE9A File Offset: 0x0035B09A
	public void ResetPosition()
	{
		this.scrollView.ResetPosition();
		this.UpdateScrollbarsRequired();
	}

	// Token: 0x06008E74 RID: 36468 RVA: 0x0035CEAE File Offset: 0x0035B0AE
	public void UpdatePosition()
	{
		this.scrollView.UpdatePosition();
		this.UpdateScrollbarsRequired();
	}

	// Token: 0x06008E75 RID: 36469 RVA: 0x0035CEC4 File Offset: 0x0035B0C4
	public bool UpdateScrollbarsRequired()
	{
		return this.scrollBar.BarRequired = ((this.Movement == UIScrollView.Movement.Vertical) ? this.scrollView.shouldMoveVertically : this.scrollView.shouldMoveHorizontally);
	}

	// Token: 0x06008E76 RID: 36470 RVA: 0x0035CF00 File Offset: 0x0035B100
	public void MakeVisible(XUiView _viewComponent)
	{
		if (_viewComponent == null)
		{
			return;
		}
		Vector3[] worldCorners = _viewComponent.WorldCorners;
		this.MakeVisible(worldCorners);
	}

	// Token: 0x06008E77 RID: 36471 RVA: 0x0035CF20 File Offset: 0x0035B120
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

	// Token: 0x06008E78 RID: 36472 RVA: 0x0035D0F0 File Offset: 0x0035B2F0
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

	// Token: 0x04006890 RID: 26768
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView scrollView;

	// Token: 0x04006891 RID: 26769
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_ScrollBar scrollBar;

	// Token: 0x04006892 RID: 26770
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView.Movement movement = UIScrollView.Movement.Vertical;

	// Token: 0x04006893 RID: 26771
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView.DragEffect dragEffect;

	// Token: 0x04006894 RID: 26772
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView.ShowCondition showCondition = UIScrollView.ShowCondition.OnlyIfNeeded;

	// Token: 0x04006895 RID: 26773
	[PublicizedFrom(EAccessModifier.Private)]
	public float scrollFactor = 2f;

	// Token: 0x04006896 RID: 26774
	[PublicizedFrom(EAccessModifier.Private)]
	public bool scrollWithoutFocus;

	// Token: 0x04006899 RID: 26777
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxBoundsDepth = -1;

	// Token: 0x0400689A RID: 26778
	[PublicizedFrom(EAccessModifier.Private)]
	public int openFrame;

	// Token: 0x0400689B RID: 26779
	[PublicizedFrom(EAccessModifier.Private)]
	public bool updateScrollbarsNextUpdate;
}
