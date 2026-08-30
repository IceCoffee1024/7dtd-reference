using System;
using UnityEngine;

// Token: 0x02001153 RID: 4435
public class XUiV_Window : XUiView
{
	// Token: 0x170010D1 RID: 4305
	// (get) Token: 0x06008DAD RID: 36269 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x170010D2 RID: 4306
	// (get) Token: 0x06008DAE RID: 36270 RVA: 0x00355A23 File Offset: 0x00353C23
	// (set) Token: 0x06008DAF RID: 36271 RVA: 0x00355A2B File Offset: 0x00353C2B
	[XuiXmlAttribute("anchor", false)]
	public string Anchor
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchor;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.anchor = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010D3 RID: 4307
	// (get) Token: 0x06008DB0 RID: 36272 RVA: 0x00355A3A File Offset: 0x00353C3A
	// (set) Token: 0x06008DB1 RID: 36273 RVA: 0x00355A42 File Offset: 0x00353C42
	[XuiXmlAttribute("cursor_area", false)]
	public bool IsCursorArea
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170010D4 RID: 4308
	// (get) Token: 0x06008DB2 RID: 36274 RVA: 0x00355A4B File Offset: 0x00353C4B
	// (set) Token: 0x06008DB3 RID: 36275 RVA: 0x00355A53 File Offset: 0x00353C53
	[XuiXmlAttribute("lock_navigation", false)]
	public bool LockNavigation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170010D5 RID: 4309
	// (get) Token: 0x06008DB4 RID: 36276 RVA: 0x00355A5C File Offset: 0x00353C5C
	// (set) Token: 0x06008DB5 RID: 36277 RVA: 0x00355A64 File Offset: 0x00353C64
	public bool IsOpen
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170010D6 RID: 4310
	// (get) Token: 0x06008DB6 RID: 36278 RVA: 0x00355A6D File Offset: 0x00353C6D
	public bool IsInStackPanel
	{
		get
		{
			return this.stackPanel != null;
		}
	}

	// Token: 0x170010D7 RID: 4311
	// (get) Token: 0x06008DB7 RID: 36279 RVA: 0x00355A78 File Offset: 0x00353C78
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.panel.worldCorners;
		}
	}

	// Token: 0x06008DB8 RID: 36280 RVA: 0x00355A85 File Offset: 0x00353C85
	public XUiV_Window(XUi _xui, string _id)
		: base(_xui, _id)
	{
		this.xui.AddWindow(this);
	}

	// Token: 0x06008DB9 RID: 36281 RVA: 0x003535A4 File Offset: 0x003517A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIPanel>();
	}

	// Token: 0x06008DBA RID: 36282 RVA: 0x00355AB8 File Offset: 0x00353CB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.panel = this.uiTransform.gameObject.GetComponent<UIPanel>();
	}

	// Token: 0x06008DBB RID: 36283 RVA: 0x00355AD8 File Offset: 0x00353CD8
	public override void InitView()
	{
		base.InitView();
		this.setRootNode();
		base.Controller.OnVisiblity += this.updateVisibility;
		this.panel.depth = base.Depth + 1;
		this.panel.alpha = 0f;
	}

	// Token: 0x06008DBC RID: 36284 RVA: 0x00355B2B File Offset: 0x00353D2B
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.uiTransform != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.uiTransform.gameObject);
		}
		this.xui.RemoveWindow(this);
	}

	// Token: 0x06008DBD RID: 36285 RVA: 0x00355B60 File Offset: 0x00353D60
	[PublicizedFrom(EAccessModifier.Protected)]
	public void setRootNode()
	{
		Transform transform;
		if (this.stackPanel != null)
		{
			transform = this.stackPanel.Transform;
		}
		else if (!string.IsNullOrEmpty(this.Anchor))
		{
			transform = this.xui.GetAnchor(this.Anchor);
			if (transform == null)
			{
				Log.Error(string.Concat(new string[] { "Specified window anchor \"", this.Anchor, "\" not found for window \"", base.ID, "\"" }));
				throw new Exception();
			}
		}
		else
		{
			transform = this.xui.transform;
		}
		this.uiTransform.parent = transform;
		this.uiTransform.gameObject.layer = 12;
		this.uiTransform.localScale = Vector3.one;
		this.uiTransform.localPosition = new Vector3((float)base.PaddedPosition.x, (float)base.PaddedPosition.y, 0f);
		this.uiTransform.localEulerAngles = new Vector3(0f, 0f, this.rotation);
	}

	// Token: 0x170010D8 RID: 4312
	// (get) Token: 0x06008DBE RID: 36286 RVA: 0x00355C72 File Offset: 0x00353E72
	public float PanelAlpha
	{
		get
		{
			return this.panel.alpha;
		}
	}

	// Token: 0x170010D9 RID: 4313
	// (get) Token: 0x06008DBF RID: 36287 RVA: 0x00355C7F File Offset: 0x00353E7F
	// (set) Token: 0x06008DC0 RID: 36288 RVA: 0x00355C87 File Offset: 0x00353E87
	[XuiXmlAttribute("fade_window", false)]
	public bool Fade
	{
		get
		{
			return this.fade;
		}
		set
		{
			this.fade = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010DA RID: 4314
	// (get) Token: 0x06008DC1 RID: 36289 RVA: 0x00355C96 File Offset: 0x00353E96
	// (set) Token: 0x06008DC2 RID: 36290 RVA: 0x00355C9E File Offset: 0x00353E9E
	[XuiXmlAttribute("fade_time", false)]
	public float FadeInTime
	{
		get
		{
			return this.fadeInTime;
		}
		set
		{
			this.fadeInTime = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010DB RID: 4315
	// (get) Token: 0x06008DC3 RID: 36291 RVA: 0x00355CAD File Offset: 0x00353EAD
	// (set) Token: 0x06008DC4 RID: 36292 RVA: 0x00355CB5 File Offset: 0x00353EB5
	[XuiXmlAttribute("fade_delay", false)]
	public float DelayToFadeTime
	{
		get
		{
			return this.delayToFadeTime;
		}
		set
		{
			this.delayToFadeTime = value;
			base.SetDirty();
		}
	}

	// Token: 0x06008DC5 RID: 36293 RVA: 0x00355CC4 File Offset: 0x00353EC4
	public override void Update(float _dt)
	{
		if ((double)Time.timeScale < 0.01 || !this.fade)
		{
			this.delayTimer = this.delayToFadeTime + 1f;
			this.fadeTimer = this.fadeInTime;
		}
		if (this.delayTimer < this.delayToFadeTime)
		{
			this.delayTimer += _dt;
		}
		if (this.delayTimer < this.delayToFadeTime)
		{
			base.Update(_dt);
			return;
		}
		this.panel.alpha = Mathf.Lerp(this.panel.alpha, this.targetAlpha, this.fadeTimer / this.fadeInTime);
		this.fadeTimer += _dt;
		if (this.IsCursorArea && this.oldTransformPosition != this.uiTransform.position && base.IsVisible)
		{
			this.xui.UpdateWindowSoftCursorBounds(this);
			this.oldTransformPosition = this.uiTransform.position;
		}
		base.Update(_dt);
	}

	// Token: 0x06008DC6 RID: 36294 RVA: 0x00355DC2 File Offset: 0x00353FC2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.setRootNode();
		base.updateData();
		this.panel.SetDirty();
	}

	// Token: 0x06008DC7 RID: 36295 RVA: 0x00355DDC File Offset: 0x00353FDC
	public override void OnOpen()
	{
		base.OnOpen();
		base.IsVisible = true;
		if (this.frameClosed == Time.frameCount)
		{
			this.panel.alpha = 1f;
			this.targetAlpha = 1f;
			this.fadeTimer = this.fadeInTime + 1f;
			this.delayTimer = this.delayToFadeTime + 1f;
		}
		else
		{
			this.panel.alpha = 0f;
			this.targetAlpha = 1f;
			this.fadeTimer = 0f;
			this.delayTimer = 0f;
		}
		if (this.IsCursorArea)
		{
			this.oldTransformPosition = Vector3.zero;
		}
		this.applyNavigationChanges();
		this.IsOpen = true;
	}

	// Token: 0x06008DC8 RID: 36296 RVA: 0x00355E98 File Offset: 0x00354098
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyNavigationChanges()
	{
		CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
		this.previousNavigationTarget = cursorController.navigationTargetLater ?? cursorController.CurrentTarget;
		if (this.LockNavigation)
		{
			this.previousNavigationLockView = cursorController.lockNavigationToView;
			cursorController.SetNavigationLockView(this, null);
		}
	}

	// Token: 0x06008DC9 RID: 36297 RVA: 0x00355EE8 File Offset: 0x003540E8
	public override void OnClose()
	{
		base.OnClose();
		base.IsVisible = false;
		this.panel.alpha = 0f;
		this.targetAlpha = 0f;
		this.fadeTimer = this.fadeInTime + 1f;
		this.delayTimer = this.delayToFadeTime + 1f;
		this.frameClosed = Time.frameCount;
		this.IsOpen = false;
		if (this.LockNavigation)
		{
			this.xui.playerUI.CursorController.SetNavigationLockView(this.previousNavigationLockView, this.previousNavigationTarget);
		}
		this.previousNavigationTarget = null;
		this.previousNavigationLockView = null;
	}

	// Token: 0x06008DCA RID: 36298 RVA: 0x00355F8C File Offset: 0x0035418C
	public void ForceVisible(float _alpha = -1f)
	{
		this.delayTimer = this.delayToFadeTime + 1f;
		this.fadeTimer = this.fadeInTime + 1f;
		if (_alpha >= 0f)
		{
			this.targetAlpha = _alpha;
		}
		this.panel.alpha = this.targetAlpha;
	}

	// Token: 0x06008DCB RID: 36299 RVA: 0x00355FDD File Offset: 0x003541DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateVisibility(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (!this.IsCursorArea)
		{
			return;
		}
		if (_visibleInScene)
		{
			this.xui.UpdateWindowSoftCursorBounds(this);
			return;
		}
		this.xui.RemoveWindowFromSoftCursorBounds(this);
	}

	// Token: 0x06008DCC RID: 36300 RVA: 0x00356004 File Offset: 0x00354204
	[XuiXmlAttribute("panel", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePanel(string _value)
	{
		if (string.IsNullOrEmpty(_value))
		{
			return;
		}
		if (!this.xui.StackPanels.dict.TryGetValue(_value, out this.stackPanel))
		{
			Log.Error(string.Concat(new string[]
			{
				"[XUi] Could not find StackPanel '",
				_value,
				"' for view '",
				this.id,
				"' in window group '",
				this.controller.WindowGroup.Id,
				"'"
			}));
		}
	}

	// Token: 0x040067AE RID: 26542
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchor;

	// Token: 0x040067AF RID: 26543
	[PublicizedFrom(EAccessModifier.Private)]
	public UIPanel panel;

	// Token: 0x040067B1 RID: 26545
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousNavigationTarget;

	// Token: 0x040067B2 RID: 26546
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousNavigationLockView;

	// Token: 0x040067B4 RID: 26548
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 oldTransformPosition;

	// Token: 0x040067B6 RID: 26550
	[PublicizedFrom(EAccessModifier.Private)]
	public XUi.StackPanel stackPanel;

	// Token: 0x040067B7 RID: 26551
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetAlpha;

	// Token: 0x040067B8 RID: 26552
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fade = true;

	// Token: 0x040067B9 RID: 26553
	[PublicizedFrom(EAccessModifier.Private)]
	public int frameClosed;

	// Token: 0x040067BA RID: 26554
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeInTime = 0.05f;

	// Token: 0x040067BB RID: 26555
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeTimer;

	// Token: 0x040067BC RID: 26556
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayToFadeTime = 0.1f;

	// Token: 0x040067BD RID: 26557
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayTimer;
}
