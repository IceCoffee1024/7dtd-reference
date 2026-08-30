using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001146 RID: 4422
public class XUiV_ScrollView : XUiV_Panel
{
	// Token: 0x1700109D RID: 4253
	// (get) Token: 0x06008CE2 RID: 36066 RVA: 0x00353AC7 File Offset: 0x00351CC7
	// (set) Token: 0x06008CE3 RID: 36067 RVA: 0x00353ACF File Offset: 0x00351CCF
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

	// Token: 0x1700109E RID: 4254
	// (get) Token: 0x06008CE4 RID: 36068 RVA: 0x00353AE8 File Offset: 0x00351CE8
	// (set) Token: 0x06008CE5 RID: 36069 RVA: 0x00353AF0 File Offset: 0x00351CF0
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

	// Token: 0x1700109F RID: 4255
	// (get) Token: 0x06008CE6 RID: 36070 RVA: 0x00353B09 File Offset: 0x00351D09
	// (set) Token: 0x06008CE7 RID: 36071 RVA: 0x00353B11 File Offset: 0x00351D11
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

	// Token: 0x170010A0 RID: 4256
	// (get) Token: 0x06008CE8 RID: 36072 RVA: 0x00353B2F File Offset: 0x00351D2F
	// (set) Token: 0x06008CE9 RID: 36073 RVA: 0x00353B37 File Offset: 0x00351D37
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

	// Token: 0x06008CEA RID: 36074 RVA: 0x00353B50 File Offset: 0x00351D50
	public XUiV_ScrollView(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008CEB RID: 36075 RVA: 0x00353B6C File Offset: 0x00351D6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UIScrollView>();
	}

	// Token: 0x06008CEC RID: 36076 RVA: 0x00353B7C File Offset: 0x00351D7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.scrollView = this.uiTransform.gameObject.GetComponent<UIScrollView>();
	}

	// Token: 0x06008CED RID: 36077 RVA: 0x00353B9C File Offset: 0x00351D9C
	public override void InitView()
	{
		base.InitView();
		this.controller.Parent.OnScroll += this.OnScrolled;
		this.xui.OnBuilt += this.OnXuiLoadDone;
		base.SetDirty();
	}

	// Token: 0x06008CEE RID: 36078 RVA: 0x00353BE8 File Offset: 0x00351DE8
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.OnBuilt -= this.OnXuiLoadDone;
	}

	// Token: 0x06008CEF RID: 36079 RVA: 0x00353C08 File Offset: 0x00351E08
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
		xuiV_ScrollBar.Connect(new XUiEvent_OnScrollEventHandler(this.OnScrolled));
	}

	// Token: 0x06008CF0 RID: 36080 RVA: 0x00353C70 File Offset: 0x00351E70
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

	// Token: 0x06008CF1 RID: 36081 RVA: 0x00353CE4 File Offset: 0x00351EE4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.controllerScroll(Time.unscaledDeltaTime);
	}

	// Token: 0x06008CF2 RID: 36082 RVA: 0x00353CF8 File Offset: 0x00351EF8
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

	// Token: 0x06008CF3 RID: 36083 RVA: 0x00353DFC File Offset: 0x00351FFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		base.Clipping = UIDrawCall.Clipping.SoftClip;
		this.scrollView.movement = this.movement;
		this.scrollView.dragEffect = this.dragEffect;
		this.scrollView.scrollWheelFactor = this.scrollFactor;
		this.scrollView.constrainOnDrag = true;
		this.scrollView.disableDragIfFits = true;
		this.scrollView.smoothDragStart = true;
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

	// Token: 0x06008CF4 RID: 36084 RVA: 0x00353EE8 File Offset: 0x003520E8
	public override void OnOpen()
	{
		base.OnOpen();
		this.ResetPosition();
	}

	// Token: 0x06008CF5 RID: 36085 RVA: 0x00353EF6 File Offset: 0x003520F6
	public override void OnVisibilityChanged(bool _visibleInScene)
	{
		base.OnVisibilityChanged(_visibleInScene);
		if (_visibleInScene)
		{
			this.UpdatePosition();
		}
	}

	// Token: 0x06008CF6 RID: 36086 RVA: 0x00353F08 File Offset: 0x00352108
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrolled(XUiController _sender, float _delta)
	{
		if (InputUtils.ShiftKeyPressed)
		{
			return;
		}
		this.scrollView.Scroll(_delta);
	}

	// Token: 0x06008CF7 RID: 36087 RVA: 0x00353F1E File Offset: 0x0035211E
	public void ResetPosition()
	{
		this.scrollView.ResetPosition();
	}

	// Token: 0x06008CF8 RID: 36088 RVA: 0x00353F2B File Offset: 0x0035212B
	public void UpdatePosition()
	{
		this.scrollView.UpdatePosition();
	}

	// Token: 0x06008CF9 RID: 36089 RVA: 0x00353F38 File Offset: 0x00352138
	public void MakeVisible(XUiView _viewComponent)
	{
		if (_viewComponent == null)
		{
			return;
		}
		Vector3[] worldCorners = _viewComponent.WorldCorners;
		this.MakeVisible(worldCorners);
	}

	// Token: 0x06008CFA RID: 36090 RVA: 0x00353F58 File Offset: 0x00352158
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
	}

	// Token: 0x06008CFB RID: 36091 RVA: 0x0035411C File Offset: 0x0035231C
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
	}

	// Token: 0x04006767 RID: 26471
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView scrollView;

	// Token: 0x04006768 RID: 26472
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_ScrollBar scrollBar;

	// Token: 0x04006769 RID: 26473
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView.Movement movement = UIScrollView.Movement.Vertical;

	// Token: 0x0400676A RID: 26474
	[PublicizedFrom(EAccessModifier.Private)]
	public UIScrollView.DragEffect dragEffect;

	// Token: 0x0400676B RID: 26475
	[PublicizedFrom(EAccessModifier.Private)]
	public float scrollFactor = 2f;

	// Token: 0x0400676C RID: 26476
	[PublicizedFrom(EAccessModifier.Private)]
	public bool scrollWithoutFocus;
}
