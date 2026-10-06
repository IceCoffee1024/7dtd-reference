using System;
using UnityEngine;

// Token: 0x02001184 RID: 4484
public class XUiV_Window : XUiView
{
	// Token: 0x1700114E RID: 4430
	// (get) Token: 0x06008F2A RID: 36650 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return null;
		}
	}

	// Token: 0x1700114F RID: 4431
	// (get) Token: 0x06008F2B RID: 36651 RVA: 0x0035EA1B File Offset: 0x0035CC1B
	// (set) Token: 0x06008F2C RID: 36652 RVA: 0x0035EA23 File Offset: 0x0035CC23
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

	// Token: 0x17001150 RID: 4432
	// (get) Token: 0x06008F2D RID: 36653 RVA: 0x0035EA32 File Offset: 0x0035CC32
	// (set) Token: 0x06008F2E RID: 36654 RVA: 0x0035EA3A File Offset: 0x0035CC3A
	[XuiXmlAttribute("cursor_area", false)]
	public bool IsCursorArea
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17001151 RID: 4433
	// (get) Token: 0x06008F2F RID: 36655 RVA: 0x0035EA43 File Offset: 0x0035CC43
	// (set) Token: 0x06008F30 RID: 36656 RVA: 0x0035EA4B File Offset: 0x0035CC4B
	[XuiXmlAttribute("lock_navigation", false)]
	public bool LockNavigation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17001152 RID: 4434
	// (get) Token: 0x06008F31 RID: 36657 RVA: 0x0035EA54 File Offset: 0x0035CC54
	// (set) Token: 0x06008F32 RID: 36658 RVA: 0x0035EA5C File Offset: 0x0035CC5C
	public bool IsOpen
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17001153 RID: 4435
	// (get) Token: 0x06008F33 RID: 36659 RVA: 0x0035EA65 File Offset: 0x0035CC65
	public bool IsInStackPanel
	{
		get
		{
			return this.stackPanel != null;
		}
	}

	// Token: 0x17001154 RID: 4436
	// (get) Token: 0x06008F34 RID: 36660 RVA: 0x0035EA70 File Offset: 0x0035CC70
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.panel.worldCorners;
		}
	}

	// Token: 0x06008F35 RID: 36661 RVA: 0x0035EA7D File Offset: 0x0035CC7D
	public XUiV_Window(XUi _xui, string _id)
		: base(_xui, _id)
	{
		this.xui.AddWindow(this);
	}

	// Token: 0x06008F36 RID: 36662 RVA: 0x0035C200 File Offset: 0x0035A400
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIPanel>();
	}

	// Token: 0x06008F37 RID: 36663 RVA: 0x0035EAB0 File Offset: 0x0035CCB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.panel = this.uiTransform.gameObject.GetComponent<UIPanel>();
	}

	// Token: 0x06008F38 RID: 36664 RVA: 0x0035EAD0 File Offset: 0x0035CCD0
	public override void InitView()
	{
		base.InitView();
		this.setRootNode();
		base.Controller.OnVisiblity += this.updateVisibility;
		this.panel.depth = base.Depth + 1;
		this.panel.alpha = 0f;
	}

	// Token: 0x06008F39 RID: 36665 RVA: 0x0035EB23 File Offset: 0x0035CD23
	public override void Cleanup()
	{
		base.Cleanup();
		if (this.uiTransform != null)
		{
			global::UnityEngine.Object.DestroyImmediate(this.uiTransform.gameObject);
		}
		this.xui.RemoveWindow(this);
	}

	// Token: 0x06008F3A RID: 36666 RVA: 0x0035EB58 File Offset: 0x0035CD58
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

	// Token: 0x17001155 RID: 4437
	// (get) Token: 0x06008F3B RID: 36667 RVA: 0x0035EC6A File Offset: 0x0035CE6A
	public float PanelAlpha
	{
		get
		{
			return this.panel.alpha;
		}
	}

	// Token: 0x17001156 RID: 4438
	// (get) Token: 0x06008F3C RID: 36668 RVA: 0x0035EC77 File Offset: 0x0035CE77
	// (set) Token: 0x06008F3D RID: 36669 RVA: 0x0035EC7F File Offset: 0x0035CE7F
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

	// Token: 0x17001157 RID: 4439
	// (get) Token: 0x06008F3E RID: 36670 RVA: 0x0035EC8E File Offset: 0x0035CE8E
	// (set) Token: 0x06008F3F RID: 36671 RVA: 0x0035EC96 File Offset: 0x0035CE96
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

	// Token: 0x17001158 RID: 4440
	// (get) Token: 0x06008F40 RID: 36672 RVA: 0x0035ECA5 File Offset: 0x0035CEA5
	// (set) Token: 0x06008F41 RID: 36673 RVA: 0x0035ECAD File Offset: 0x0035CEAD
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

	// Token: 0x06008F42 RID: 36674 RVA: 0x0035ECBC File Offset: 0x0035CEBC
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

	// Token: 0x06008F43 RID: 36675 RVA: 0x0035EDBA File Offset: 0x0035CFBA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.setRootNode();
		base.updateData();
		this.panel.SetDirty();
	}

	// Token: 0x06008F44 RID: 36676 RVA: 0x0035EDD4 File Offset: 0x0035CFD4
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

	// Token: 0x06008F45 RID: 36677 RVA: 0x0035EE90 File Offset: 0x0035D090
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

	// Token: 0x06008F46 RID: 36678 RVA: 0x0035EEE0 File Offset: 0x0035D0E0
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

	// Token: 0x06008F47 RID: 36679 RVA: 0x0035EF84 File Offset: 0x0035D184
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

	// Token: 0x06008F48 RID: 36680 RVA: 0x0035EFD5 File Offset: 0x0035D1D5
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

	// Token: 0x06008F49 RID: 36681 RVA: 0x0035EFFC File Offset: 0x0035D1FC
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

	// Token: 0x040068DE RID: 26846
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchor;

	// Token: 0x040068DF RID: 26847
	[PublicizedFrom(EAccessModifier.Private)]
	public UIPanel panel;

	// Token: 0x040068E1 RID: 26849
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousNavigationTarget;

	// Token: 0x040068E2 RID: 26850
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView previousNavigationLockView;

	// Token: 0x040068E4 RID: 26852
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 oldTransformPosition;

	// Token: 0x040068E6 RID: 26854
	[PublicizedFrom(EAccessModifier.Private)]
	public XUi.StackPanel stackPanel;

	// Token: 0x040068E7 RID: 26855
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetAlpha;

	// Token: 0x040068E8 RID: 26856
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fade = true;

	// Token: 0x040068E9 RID: 26857
	[PublicizedFrom(EAccessModifier.Private)]
	public int frameClosed;

	// Token: 0x040068EA RID: 26858
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeInTime = 0.05f;

	// Token: 0x040068EB RID: 26859
	[PublicizedFrom(EAccessModifier.Private)]
	public float fadeTimer;

	// Token: 0x040068EC RID: 26860
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayToFadeTime = 0.1f;

	// Token: 0x040068ED RID: 26861
	[PublicizedFrom(EAccessModifier.Private)]
	public float delayTimer;
}
