using System;
using UnityEngine;

// Token: 0x02001194 RID: 4500
public class XUiV_Window : XUiView
{
	// Token: 0x17001159 RID: 4441
	// (get) Token: 0x06008F7E RID: 36734 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x1700115A RID: 4442
	// (get) Token: 0x06008F7F RID: 36735 RVA: 0x00360B0B File Offset: 0x0035ED0B
	// (set) Token: 0x06008F80 RID: 36736 RVA: 0x00360B13 File Offset: 0x0035ED13
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

	// Token: 0x1700115B RID: 4443
	// (get) Token: 0x06008F81 RID: 36737 RVA: 0x00360B22 File Offset: 0x0035ED22
	// (set) Token: 0x06008F82 RID: 36738 RVA: 0x00360B2A File Offset: 0x0035ED2A
	[XuiXmlAttribute("cursor_area", false)]
	public bool IsCursorArea
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700115C RID: 4444
	// (get) Token: 0x06008F83 RID: 36739 RVA: 0x00360B33 File Offset: 0x0035ED33
	// (set) Token: 0x06008F84 RID: 36740 RVA: 0x00360B3B File Offset: 0x0035ED3B
	[XuiXmlAttribute("lock_navigation", false)]
	public bool LockNavigation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700115D RID: 4445
	// (get) Token: 0x06008F85 RID: 36741 RVA: 0x00360B44 File Offset: 0x0035ED44
	// (set) Token: 0x06008F86 RID: 36742 RVA: 0x00360B4C File Offset: 0x0035ED4C
	public bool IsOpen
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700115E RID: 4446
	// (get) Token: 0x06008F87 RID: 36743 RVA: 0x00360B55 File Offset: 0x0035ED55
	public bool IsInStackPanel
	{
		get
		{
			return this.stackPanel != null;
		}
	}

	// Token: 0x1700115F RID: 4447
	// (get) Token: 0x06008F88 RID: 36744 RVA: 0x00360B60 File Offset: 0x0035ED60
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.panel.worldCorners;
		}
	}

	// Token: 0x06008F89 RID: 36745 RVA: 0x00360B6D File Offset: 0x0035ED6D
	public XUiV_Window(XUi _xui, string _id)
		: base(_xui, _id)
	{
		this.xui.AddWindow(this);
	}

	// Token: 0x06008F8A RID: 36746 RVA: 0x0035E2D0 File Offset: 0x0035C4D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIPanel>();
	}

	// Token: 0x06008F8B RID: 36747 RVA: 0x00360BA0 File Offset: 0x0035EDA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.panel = this.uiTransform.gameObject.GetComponent<UIPanel>();
	}

	// Token: 0x06008F8C RID: 36748 RVA: 0x00360BC0 File Offset: 0x0035EDC0
	public override void InitView()
	{
		base.InitView();
		this.setRootNode();
		base.Controller.OnVisiblity += this.updateVisibility;
		this.panel.depth = base.Depth + 1;
		this.panel.alpha = 0f;
	}

	// Token: 0x06008F8D RID: 36749 RVA: 0x00360C13 File Offset: 0x0035EE13
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.uiTransform != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.uiTransform.gameObject);
		}
		this.xui.RemoveWindow(this);
	}

	// Token: 0x06008F8E RID: 36750 RVA: 0x00360C48 File Offset: 0x0035EE48
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

	// Token: 0x17001160 RID: 4448
	// (get) Token: 0x06008F8F RID: 36751 RVA: 0x00360D5A File Offset: 0x0035EF5A
	public float PanelAlpha
	{
		get
		{
			return this.panel.alpha;
		}
	}

	// Token: 0x17001161 RID: 4449
	// (get) Token: 0x06008F90 RID: 36752 RVA: 0x00360D67 File Offset: 0x0035EF67
	// (set) Token: 0x06008F91 RID: 36753 RVA: 0x00360D6F File Offset: 0x0035EF6F
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

	// Token: 0x17001162 RID: 4450
	// (get) Token: 0x06008F92 RID: 36754 RVA: 0x00360D7E File Offset: 0x0035EF7E
	// (set) Token: 0x06008F93 RID: 36755 RVA: 0x00360D86 File Offset: 0x0035EF86
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

	// Token: 0x17001163 RID: 4451
	// (get) Token: 0x06008F94 RID: 36756 RVA: 0x00360D95 File Offset: 0x0035EF95
	// (set) Token: 0x06008F95 RID: 36757 RVA: 0x00360D9D File Offset: 0x0035EF9D
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

	// Token: 0x06008F96 RID: 36758 RVA: 0x00360DAC File Offset: 0x0035EFAC
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

	// Token: 0x06008F97 RID: 36759 RVA: 0x00360EAA File Offset: 0x0035F0AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.setRootNode();
		base.updateData();
		this.panel.SetDirty();
	}

	// Token: 0x06008F98 RID: 36760 RVA: 0x00360EC4 File Offset: 0x0035F0C4
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

	// Token: 0x06008F99 RID: 36761 RVA: 0x00360F80 File Offset: 0x0035F180
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

	// Token: 0x06008F9A RID: 36762 RVA: 0x00360FD0 File Offset: 0x0035F1D0
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

	// Token: 0x06008F9B RID: 36763 RVA: 0x00361074 File Offset: 0x0035F274
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

	// Token: 0x06008F9C RID: 36764 RVA: 0x003610C5 File Offset: 0x0035F2C5
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

	// Token: 0x06008F9D RID: 36765 RVA: 0x003610EC File Offset: 0x0035F2EC
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

	// Token: 0x0400691E RID: 26910
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchor;

	// Token: 0x0400691F RID: 26911
	[PublicizedFrom(EAccessModifier.Private)]
	public UIPanel panel;

	// Token: 0x04006921 RID: 26913
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousNavigationTarget;

	// Token: 0x04006922 RID: 26914
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousNavigationLockView;

	// Token: 0x04006924 RID: 26916
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 oldTransformPosition;

	// Token: 0x04006926 RID: 26918
	[PublicizedFrom(EAccessModifier.Private)]
	public XUi.StackPanel stackPanel;

	// Token: 0x04006927 RID: 26919
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetAlpha;

	// Token: 0x04006928 RID: 26920
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fade = true;

	// Token: 0x04006929 RID: 26921
	[PublicizedFrom(EAccessModifier.Private)]
	public int frameClosed;

	// Token: 0x0400692A RID: 26922
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeInTime = 0.05f;

	// Token: 0x0400692B RID: 26923
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeTimer;

	// Token: 0x0400692C RID: 26924
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayToFadeTime = 0.1f;

	// Token: 0x0400692D RID: 26925
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayTimer;
}
