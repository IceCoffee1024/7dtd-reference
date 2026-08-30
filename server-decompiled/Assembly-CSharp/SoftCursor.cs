using System;
using System.Collections;
using Audio;
using InControl;
using Platform;
using UnityEngine;

// Token: 0x020012B7 RID: 4791
public class SoftCursor : CursorControllerAbs
{
	// Token: 0x170011DD RID: 4573
	// (get) Token: 0x060097C1 RID: 38849 RVA: 0x00399BDD File Offset: 0x00397DDD
	// (set) Token: 0x060097C2 RID: 38850 RVA: 0x00399BE5 File Offset: 0x00397DE5
	public PlayerInputManager.InputStyle LastInputStyle
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_lastInputStyle;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			if (value != this.m_lastInputStyle)
			{
				this.m_lastInputStyle = value;
				this.SetVisible(this.SoftcursorAllowed);
			}
		}
	}

	// Token: 0x170011DE RID: 4574
	// (get) Token: 0x060097C3 RID: 38851 RVA: 0x00399C03 File Offset: 0x00397E03
	// (set) Token: 0x060097C4 RID: 38852 RVA: 0x00399C0B File Offset: 0x00397E0B
	public bool SoftcursorEnabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.m_softcursorEnabled;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value != this.m_softcursorEnabled)
			{
				this.m_softcursorEnabled = value;
				this.SetVisible(this.SoftcursorAllowed);
			}
		}
	}

	// Token: 0x170011DF RID: 4575
	// (get) Token: 0x060097C5 RID: 38853 RVA: 0x00399C29 File Offset: 0x00397E29
	// (set) Token: 0x060097C6 RID: 38854 RVA: 0x00399C34 File Offset: 0x00397E34
	public override XUiView HoverTarget
	{
		get
		{
			return base.HoverTarget;
		}
		set
		{
			base.HoverTarget = value;
			if (this.hoverTarget != null && !this.CursorModeActive && this.LastInputStyle != PlayerInputManager.InputStyle.Keyboard && !this.movingMouse && this.hoverTarget.IsNavigatable && !this.hoverTarget.HasHoverSound)
			{
				Manager.PlayXUiSound(this.cursorSelectSound, 0.75f);
			}
		}
	}

	// Token: 0x170011E0 RID: 4576
	// (get) Token: 0x060097C7 RID: 38855 RVA: 0x00399C93 File Offset: 0x00397E93
	public override bool CursorModeActive
	{
		get
		{
			return this.cursorModeActive;
		}
	}

	// Token: 0x170011E1 RID: 4577
	// (get) Token: 0x060097C8 RID: 38856 RVA: 0x00399C9B File Offset: 0x00397E9B
	public bool SoftcursorAllowed
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.guiActions != null && this.guiActions.Enabled && LocalPlayerUI.AnyModalWindowOpen();
		}
	}

	// Token: 0x170011E2 RID: 4578
	// (get) Token: 0x060097C9 RID: 38857 RVA: 0x00399CB9 File Offset: 0x00397EB9
	// (set) Token: 0x060097CA RID: 38858 RVA: 0x00399CCB File Offset: 0x00397ECB
	public Vector3 Position
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.cursor.transform.position;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.cursor.transform.position = value;
			this.lastMousePosition = Input.mousePosition;
		}
	}

	// Token: 0x170011E3 RID: 4579
	// (get) Token: 0x060097CB RID: 38859 RVA: 0x00399CEE File Offset: 0x00397EEE
	// (set) Token: 0x060097CC RID: 38860 RVA: 0x00399D00 File Offset: 0x00397F00
	public Vector3 LocalPosition
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.cursor.transform.localPosition;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.cursor.transform.localPosition = value;
			this.lastMousePosition = Input.mousePosition;
		}
	}

	// Token: 0x170011E4 RID: 4580
	// (get) Token: 0x060097CD RID: 38861 RVA: 0x00010E62 File Offset: 0x0000F062
	public static CursorLockMode DefaultCursorLockState
	{
		get
		{
			return CursorLockMode.None;
		}
	}

	// Token: 0x060097CE RID: 38862 RVA: 0x00399D24 File Offset: 0x00397F24
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.AwakeBase();
		Cursor.lockState = SoftCursor.DefaultCursorLockState;
		foreach (UISprite uisprite in base.GetComponentsInChildren<UISprite>())
		{
			string name = uisprite.gameObject.name;
			if (name == "Cursor")
			{
				this.cursor = uisprite;
			}
			else if (name == "SelectionBox")
			{
				this.selectionBox = uisprite;
			}
		}
		this.cursorPanel = base.GetComponent<UIPanel>();
		GameObject gameObject = new GameObject("cursorMouse");
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (SoftCursor.defaultMouseCursorAtlas == null)
		{
			foreach (UIAtlas uiatlas in Resources.FindObjectsOfTypeAll<UIAtlas>())
			{
				if (uiatlas.name.EqualsCaseInsensitive(SoftCursor.defaultMouseCursorAtlasName))
				{
					SoftCursor.defaultMouseCursorAtlas = uiatlas;
				}
				if (uiatlas.name.EqualsCaseInsensitive(SoftCursor.emptyCursorAtlasName))
				{
					SoftCursor.emptyCursorAtlas = uiatlas;
				}
				if (uiatlas.name.EqualsCaseInsensitive(SoftCursor.defaultControllerCursorAtlasName))
				{
					SoftCursor.defaultControllerCursorAtlas = uiatlas;
				}
				if (uiatlas.name.EqualsCaseInsensitive(SoftCursor.mapCursorAtlasName))
				{
					SoftCursor.mapCursorAtlas = uiatlas;
				}
			}
		}
		LocalPlayerUI.primaryUI.xui.LoadData<AudioClip>("@:Sounds/UI/ui_hover.wav", delegate(AudioClip _o)
		{
			this.cursorSelectSound = _o;
		});
		LocalPlayerUI.primaryUI.xui.LoadData<AudioClip>("@:Sounds/UI/ui_tab.wav", delegate(AudioClip _o)
		{
			this.pagingSound = _o;
		});
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
	}

	// Token: 0x060097CF RID: 38863 RVA: 0x00399EC4 File Offset: 0x003980C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		if (_style != PlayerInputManager.InputStyle.Keyboard)
		{
			if (this.LastInputStyle == PlayerInputManager.InputStyle.Keyboard)
			{
				this.LocalPosition = this.cursorPanel.transform.worldToLocalMatrix.MultiplyPoint3x4(this.uiCamera.cachedCamera.ScreenToWorldPoint(Input.mousePosition));
			}
			this.RefreshSelection();
		}
		this.SetVisible(this.SoftcursorAllowed);
	}

	// Token: 0x060097D0 RID: 38864 RVA: 0x00399F23 File Offset: 0x00398123
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.uiCamera = base.GetComponentInParent<UICamera>();
		this.UpdateMoveSpeed();
		base.InitCursorBounds();
	}

	// Token: 0x060097D1 RID: 38865 RVA: 0x00399F3D File Offset: 0x0039813D
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		base.DestroyBase();
	}

	// Token: 0x060097D2 RID: 38866 RVA: 0x00399F48 File Offset: 0x00398148
	public override void UpdateMoveSpeed()
	{
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		this.speed = 500f + 2000f * @float;
		this.mouseSpeed = (500f + 2000f * @float) / 1000f * (1f / MouseBindingSource.ScaleX) * 4f;
	}

	// Token: 0x060097D3 RID: 38867 RVA: 0x00399FA0 File Offset: 0x003981A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (Application.isPlaying && !GameManager.Instance.IsQuitting && !GUIWindowConsole.IsOpen())
		{
			this.LastInputStyle = PlatformManager.NativePlatform.Input.CurrentInputStyle;
			if (this.guiActions != null && this.windowManager != null)
			{
				if (this.SoftcursorAllowed != this.SoftcursorEnabled)
				{
					this.SoftcursorEnabled = this.SoftcursorAllowed & !this.hidden;
				}
				if (this.SoftcursorEnabled)
				{
					this.HandleMovement();
				}
			}
			this.LastFrameTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x060097D4 RID: 38868 RVA: 0x0039A02F File Offset: 0x0039822F
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.selectionBox.enabled)
		{
			this.RefreshSelection();
		}
	}

	// Token: 0x060097D5 RID: 38869 RVA: 0x0039A044 File Offset: 0x00398244
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleMovement()
	{
		if (this.guiActions == null)
		{
			return;
		}
		if (base.Locked || this.hidden)
		{
			return;
		}
		this.movingMouse = false;
		Vector2 vector = Input.mousePosition;
		if (vector != this.lastMousePosition)
		{
			this.movingMouse = true;
		}
		this.lastMousePosition = vector;
		if (this.LastInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			if (this.movingMouse)
			{
				this.LocalPosition = this.cursorPanel.transform.worldToLocalMatrix.MultiplyPoint3x4(this.uiCamera.cachedCamera.ScreenToWorldPoint(Input.mousePosition));
				this.SetVisible(true);
			}
			else
			{
				if (!CursorControllerAbs.FreeCursorEnabled)
				{
					return;
				}
				Vector2 vector2 = new Vector2(this.guiActions.Right.RawValue - this.guiActions.Left.RawValue, this.guiActions.Up.RawValue - this.guiActions.Down.RawValue);
				float magnitude = vector2.magnitude;
				if (magnitude > 0f)
				{
					this.cursorModeActive = true;
					this.SetVisible(true);
					this.SetNavigationTarget(null);
				}
				Vector3 localPosition = this.LocalPosition;
				Vector3 localPosition2 = this.LocalPosition;
				if (this.bHasHoverTarget && (this.HoverTarget == null || !this.HoverTarget.ColliderEnabled || !this.HoverTarget.IsActiveInHierarchy))
				{
					this.HoverTarget = null;
				}
				float num = (this.bHasHoverTarget ? CursorControllerAbs.hoverSpeed : 1f);
				this.currentAcceleration = Mathf.Clamp(this.currentAcceleration + magnitude * Time.unscaledDeltaTime, 0f, Mathf.Min(magnitude, num));
				this.speedMultiplier = Mathf.MoveTowards(this.speedMultiplier, (this.bHasHoverTarget && this.HoverTarget.IsSnappable) ? CursorControllerAbs.hoverSpeed : 1f, Time.unscaledDeltaTime * (this.bHasHoverTarget ? 10f : 1f));
				float num2 = Time.unscaledDeltaTime * this.speed * this.speedMultiplier * this.accelerationCurve.Evaluate(this.currentAcceleration);
				vector2.x *= num2;
				vector2.y *= num2;
				localPosition2.x += vector2.x;
				localPosition2.y += vector2.y;
				this.LocalPosition = localPosition2;
				if (CursorControllerAbs.bSnapCursor && localPosition == localPosition2)
				{
					if (!this.snapped)
					{
						this.Snap();
						this.snapped = true;
					}
				}
				else
				{
					this.snapped = false;
				}
			}
			this.ConstrainCursor();
			return;
		}
		this.LocalPosition = this.cursorPanel.transform.worldToLocalMatrix.MultiplyPoint3x4(this.uiCamera.cachedCamera.ScreenToWorldPoint(Input.mousePosition));
		this.ConstrainCursor();
	}

	// Token: 0x060097D6 RID: 38870 RVA: 0x0039A308 File Offset: 0x00398508
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 ConstrainToBounds(Vector3 _newPosition)
	{
		Vector3 vector = _newPosition;
		if (this.LastInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			vector.z = this.currentBounds.center.z;
			vector = this.currentBounds.ClosestPoint(vector);
		}
		return vector;
	}

	// Token: 0x060097D7 RID: 38871 RVA: 0x0039A348 File Offset: 0x00398548
	[PublicizedFrom(EAccessModifier.Private)]
	public void ConstrainCursor()
	{
		Vector3 vector = this.uiCamera.cachedCamera.WorldToScreenPoint(this.Position);
		Vector3 vector2 = this.ConstrainToBounds(vector);
		vector2 = this.uiCamera.cachedCamera.ScreenToViewportPoint(vector2);
		if (vector2.x < 0f)
		{
			vector2.x = 0f;
		}
		else if (vector2.x > 1f)
		{
			vector2.x = 1f;
		}
		if (vector2.y < 0f)
		{
			vector2.y = 0f;
		}
		else if (vector2.y > 1f)
		{
			vector2.y = 1f;
		}
		this.Position = this.uiCamera.cachedCamera.ViewportToWorldPoint(vector2);
	}

	// Token: 0x060097D8 RID: 38872 RVA: 0x0039A408 File Offset: 0x00398608
	[PublicizedFrom(EAccessModifier.Private)]
	public void Snap()
	{
		if (GamePrefs.GetBool(EnumGamePrefs.OptionsControllerCursorSnap) && this.hoverTarget != null && this.hoverTarget.IsSnappable && this.hoverTarget.Enabled && this.hoverTarget.ColliderEnabled && this.hoverTarget.IsActiveInHierarchy)
		{
			Bounds colliderBounds = this.hoverTarget.ColliderBounds;
			if (this.cursorWorldBounds.extents.x > colliderBounds.extents.x - this.OffsetSnapBounds)
			{
				this.Position = colliderBounds.center;
				return;
			}
			Vector3 vector = colliderBounds.ClosestPoint(this.Position);
			Vector3 vector2 = Vector3.right * this.cursorWorldBounds.extents.x;
			Vector3 vector3 = vector - vector2;
			Vector3 vector4 = vector + vector2;
			if (!colliderBounds.Contains(vector3))
			{
				vector = colliderBounds.ClosestPoint(vector3) + vector2;
			}
			else if (!colliderBounds.Contains(vector4))
			{
				vector = colliderBounds.ClosestPoint(vector4) - vector2;
			}
			vector.y = colliderBounds.center.y;
			this.Position = vector;
		}
	}

	// Token: 0x060097D9 RID: 38873 RVA: 0x0039A538 File Offset: 0x00398738
	public override Vector2 GetScreenPosition()
	{
		Vector2 vector = default(Vector2);
		if (this.uiCamera != null)
		{
			vector = this.uiCamera.cachedCamera.WorldToScreenPoint(this.Position);
		}
		return vector;
	}

	// Token: 0x060097DA RID: 38874 RVA: 0x0039A578 File Offset: 0x00398778
	public override Vector2 GetLocalScreenPosition()
	{
		Vector2 vector = default(Vector2);
		if (this.uiCamera != null)
		{
			vector = this.uiCamera.cachedCamera.WorldToViewportPoint(this.Position);
			vector.x *= (float)this.uiCamera.cachedCamera.pixelWidth;
			vector.y *= (float)this.uiCamera.cachedCamera.pixelHeight;
		}
		return vector;
	}

	// Token: 0x060097DB RID: 38875 RVA: 0x0039A5F6 File Offset: 0x003987F6
	public override void SetScreenPosition(Vector2 _newPosition)
	{
		this.Position = this.uiCamera.cachedCamera.ScreenToWorldPoint(_newPosition);
	}

	// Token: 0x060097DC RID: 38876 RVA: 0x0039A614 File Offset: 0x00398814
	public override void SetScreenPosition(float _x, float _y)
	{
		this.SetScreenPosition(new Vector2(_x, _y));
	}

	// Token: 0x060097DD RID: 38877 RVA: 0x0039A623 File Offset: 0x00398823
	public override void ResetToCenter()
	{
		this.LocalPosition = Vector3.zero;
	}

	// Token: 0x060097DE RID: 38878 RVA: 0x0039A630 File Offset: 0x00398830
	public override void SetCursorHidden(bool _hidden)
	{
		this.hidden = _hidden;
		this.SoftcursorEnabled &= !this.hidden;
		SoftCursor.SetCursor(_hidden ? CursorControllerAbs.ECursorType.None : CursorControllerAbs.ECursorType.Default);
		Cursor.visible = PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && !this.hidden;
		if (this.LastInputStyle != PlayerInputManager.InputStyle.Keyboard && !this.hidden)
		{
			this.RefreshSelection();
		}
	}

	// Token: 0x060097DF RID: 38879 RVA: 0x0039A6A0 File Offset: 0x003988A0
	public override bool GetCursorHidden()
	{
		return this.hidden;
	}

	// Token: 0x060097E0 RID: 38880 RVA: 0x0039A6A8 File Offset: 0x003988A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetCursorSprite(INGUIAtlas _atlasMouse, string _spriteMouse, UIWidget.Pivot _pivotMouse, INGUIAtlas _atlasController, string _spriteController, UIWidget.Pivot _pivotController)
	{
		this.cursor.atlas = _atlasController;
		this.cursor.spriteName = _spriteController;
		this.cursor.pivot = _pivotController;
	}

	// Token: 0x060097E1 RID: 38881 RVA: 0x0039A6D1 File Offset: 0x003988D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetCursorSpriteNone()
	{
		this.cursor.atlas = null;
		this.cursor.spriteName = "";
		this.SetSelectionBoxEnabled(false);
	}

	// Token: 0x060097E2 RID: 38882 RVA: 0x0039A6F8 File Offset: 0x003988F8
	public new static void SetCursor(CursorControllerAbs.ECursorType _cursorType)
	{
		switch (_cursorType)
		{
		case CursorControllerAbs.ECursorType.None:
		{
			for (int i = 0; i < CursorControllerAbs.softCursors.Count; i++)
			{
				(CursorControllerAbs.softCursors[i] as SoftCursor).SetCursorSpriteNone();
			}
			return;
		}
		case CursorControllerAbs.ECursorType.Default:
		{
			for (int j = 0; j < CursorControllerAbs.softCursors.Count; j++)
			{
				(CursorControllerAbs.softCursors[j] as SoftCursor).SetCursorSprite(SoftCursor.defaultMouseCursorAtlas, SoftCursor.defaultMouseCursorSprite, SoftCursor.defaultMouseCursorPivot, SoftCursor.defaultControllerCursorAtlas, SoftCursor.defaultControllerCursorSprite, SoftCursor.defaultControllerCursorPivot);
			}
			return;
		}
		case CursorControllerAbs.ECursorType.Map:
		{
			for (int k = 0; k < CursorControllerAbs.softCursors.Count; k++)
			{
				(CursorControllerAbs.softCursors[k] as SoftCursor).SetCursorSprite(SoftCursor.mapCursorAtlas, SoftCursor.mapCursorSprite, SoftCursor.mapCursorPivot, SoftCursor.mapCursorAtlas, SoftCursor.mapCursorSprite, SoftCursor.mapCursorPivot);
			}
			return;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x060097E3 RID: 38883 RVA: 0x0039A7E0 File Offset: 0x003989E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetVisible(bool _visible)
	{
		Cursor.lockState = (_visible ? SoftCursor.DefaultCursorLockState : CursorLockMode.Locked);
		if (this.LastInputStyle == PlayerInputManager.InputStyle.Keyboard || this.movingMouse)
		{
			Cursor.visible = _visible;
			this.cursor.enabled = false;
			this.SetSelectionBoxEnabled(false);
		}
		else
		{
			Cursor.visible = false;
			this.cursor.enabled = _visible && CursorControllerAbs.FreeCursorEnabled && !base.Locked && this.cursorModeActive && !base.VirtualCursorHidden;
			this.SetSelectionBoxEnabled(_visible && !this.cursorModeActive && !this.hidden && base.navigationTarget != null && base.navigationTarget.UseSelectionBox);
		}
		GameManager.Instance.bCursorVisible = _visible;
		this.lastMousePosition = Input.mousePosition;
	}

	// Token: 0x060097E4 RID: 38884 RVA: 0x0039A8AD File Offset: 0x00398AAD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnVirtualCursorVisibleChanged()
	{
		if (this.LastInputStyle == PlayerInputManager.InputStyle.Keyboard || this.movingMouse)
		{
			return;
		}
		this.cursor.enabled = !base.VirtualCursorHidden && !base.Locked && this.cursorModeActive;
	}

	// Token: 0x060097E5 RID: 38885 RVA: 0x0039A8E8 File Offset: 0x00398AE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetSelectionBoxEnabled(bool _enabled)
	{
		if (this.selectionBox == null)
		{
			return;
		}
		this.selectionBox.enabled = _enabled;
		this.selectionBox.alpha = 0f;
		if (!_enabled)
		{
			this.selectionBox.transform.parent = null;
		}
	}

	// Token: 0x060097E6 RID: 38886 RVA: 0x0039A934 File Offset: 0x00398B34
	public static void SetCursorVisible(bool _visible)
	{
		for (int i = 0; i < CursorControllerAbs.softCursors.Count; i++)
		{
			(CursorControllerAbs.softCursors[i] as SoftCursor).SetVisible(_visible);
		}
	}

	// Token: 0x060097E7 RID: 38887 RVA: 0x0039A96C File Offset: 0x00398B6C
	public Vector2 GetFlatPosition()
	{
		return new Vector2(this.Position.x, this.Position.y);
	}

	// Token: 0x060097E8 RID: 38888 RVA: 0x0039A98C File Offset: 0x00398B8C
	public override void SetNavigationTarget(XUiView _view)
	{
		if (_view != null && !_view.IsNavigatable)
		{
			this.SetNavigationTarget(null);
			return;
		}
		if (_view == base.navigationTarget)
		{
			return;
		}
		if (base.lockNavigationToView != null && (_view == null || !_view.Controller.IsChildOf(base.lockNavigationToView.Controller)))
		{
			return;
		}
		if (base.navigationTarget != null)
		{
			base.navigationTarget.Controller.OnCursorUnSelected();
		}
		if (_view != null && PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			_view.Controller.OnCursorSelected(true);
			this.cursorModeActive = false;
			this.SetSelectionBoxEnabled(_view.UseSelectionBox);
			this.PositionSelectionBox(_view);
		}
		else
		{
			this.SetSelectionBoxEnabled(false);
		}
		base.navigationTarget = _view;
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (nativePlatform == null || nativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			this.SetVisible(base.navigationTarget != null);
		}
	}

	// Token: 0x060097E9 RID: 38889 RVA: 0x0039AA6C File Offset: 0x00398C6C
	public override void RefreshSelection()
	{
		if (base.navigationTarget == null)
		{
			this.selectionBox.alpha = 0f;
			return;
		}
		this.PositionSelectionBox(base.navigationTarget);
		this.selectionBox.alpha = (base.navigationTarget.UseSelectionBox ? base.navigationTarget.Controller.GetParentWindow().PanelAlpha : 0f);
	}

	// Token: 0x060097EA RID: 38890 RVA: 0x0039AAD4 File Offset: 0x00398CD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void PositionSelectionBox(XUiView _view)
	{
		this.Position = _view.ColliderCenter;
		if (_view.UseSelectionBox)
		{
			this.selectionBox.width = _view.Size.x + SoftCursor.selectionBoxMargin;
			this.selectionBox.height = _view.Size.y + SoftCursor.selectionBoxMargin;
			this.selectionBox.depth = 1000000;
			this.selectionBox.transform.localScale = Vector3.one;
			this.selectionBox.transform.localPosition = _view.LocalCenter;
			this.selectionBox.transform.localRotation = Quaternion.identity;
			this.selectionBox.transform.parent = _view.UiTransform;
			this.selectionBox.RemoveFromPanel();
		}
	}

	// Token: 0x060097EB RID: 38891 RVA: 0x0039ABA4 File Offset: 0x00398DA4
	public override void SetNavigationLockView(XUiView _view, XUiView _viewToSelect = null)
	{
		if (_view != null && (!_view.IsVisible || !_view.IsActiveInHierarchy))
		{
			this.SetNavigationLockView(null, null);
			return;
		}
		base.lockNavigationToView = _view;
		if (_viewToSelect != null)
		{
			_viewToSelect.Controller.SelectCursorElement(true, false);
			return;
		}
		if (_view != null)
		{
			_view.Controller.SelectCursorElement(true, false);
		}
	}

	// Token: 0x060097EC RID: 38892 RVA: 0x0039ABF7 File Offset: 0x00398DF7
	public override bool IsWithinNavigationLockView(XUiView _view)
	{
		return base.lockNavigationToView == null || _view.Controller.IsSelfOrChildOf(base.lockNavigationToView.Controller);
	}

	// Token: 0x060097ED RID: 38893 RVA: 0x0039AC19 File Offset: 0x00398E19
	public override void SetNavigationTargetLater(XUiView _view)
	{
		if (_view == null)
		{
			this.SetNavigationTarget(null);
			return;
		}
		if (base.isActiveAndEnabled)
		{
			base.StartCoroutine(this.SetNavigationTargetWithDelay(_view));
		}
	}

	// Token: 0x060097EE RID: 38894 RVA: 0x0039AC3C File Offset: 0x00398E3C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator SetNavigationTargetWithDelay(XUiView _view)
	{
		base.navigationTargetLater = _view;
		base.Locked = true;
		int num;
		for (int i = 0; i < 3; i = num + 1)
		{
			yield return null;
			num = i;
		}
		base.Locked = false;
		if (_view != null)
		{
			this.SetNavigationTarget(_view);
		}
		base.navigationTargetLater = null;
		this.lastMousePosition = Input.mousePosition;
		yield break;
	}

	// Token: 0x060097EF RID: 38895 RVA: 0x0039AC52 File Offset: 0x00398E52
	public override void ResetNavigationTarget()
	{
		if (base.navigationTarget != null)
		{
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
			{
				this.Position = base.navigationTarget.ColliderCenter;
				return;
			}
			this.SetNavigationTarget(null);
		}
	}

	// Token: 0x04007202 RID: 29186
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float BaseSpeed = 500f;

	// Token: 0x04007203 RID: 29187
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float SpeedModRange = 2000f;

	// Token: 0x04007204 RID: 29188
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string emptyCursorAtlasName = "UIAtlas";

	// Token: 0x04007205 RID: 29189
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string emptyCursorSprite = "";

	// Token: 0x04007206 RID: 29190
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot emptyCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x04007207 RID: 29191
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas emptyCursorAtlas;

	// Token: 0x04007208 RID: 29192
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorAtlasName = "UIAtlas";

	// Token: 0x04007209 RID: 29193
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorSprite = "soft_cursor";

	// Token: 0x0400720A RID: 29194
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot defaultControllerCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x0400720B RID: 29195
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas defaultControllerCursorAtlas;

	// Token: 0x0400720C RID: 29196
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorAtlasName = "UIAtlas";

	// Token: 0x0400720D RID: 29197
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorSprite = "cursor01";

	// Token: 0x0400720E RID: 29198
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot defaultMouseCursorPivot = UIWidget.Pivot.TopLeft;

	// Token: 0x0400720F RID: 29199
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas defaultMouseCursorAtlas;

	// Token: 0x04007210 RID: 29200
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorAtlasName = "UIAtlas";

	// Token: 0x04007211 RID: 29201
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorSprite = "map_cursor";

	// Token: 0x04007212 RID: 29202
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot mapCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x04007213 RID: 29203
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas mapCursorAtlas;

	// Token: 0x04007214 RID: 29204
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speed;

	// Token: 0x04007215 RID: 29205
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mouseSpeed;

	// Token: 0x04007216 RID: 29206
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float LastFrameTime;

	// Token: 0x04007217 RID: 29207
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hidden;

	// Token: 0x04007218 RID: 29208
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool snapped;

	// Token: 0x04007219 RID: 29209
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float OffsetSnapBounds = 0.1f;

	// Token: 0x0400721A RID: 29210
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UIPanel cursorPanel;

	// Token: 0x0400721B RID: 29211
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 lastMousePosition = Vector2.zero;

	// Token: 0x0400721C RID: 29212
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float currentAcceleration;

	// Token: 0x0400721D RID: 29213
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedMultiplier = 1f;

	// Token: 0x0400721E RID: 29214
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool movingMouse;

	// Token: 0x0400721F RID: 29215
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UISprite selectionBox;

	// Token: 0x04007220 RID: 29216
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int selectionBoxMargin = 10;

	// Token: 0x04007221 RID: 29217
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public PlayerInputManager.InputStyle m_lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x04007222 RID: 29218
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_softcursorEnabled;

	// Token: 0x04007223 RID: 29219
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool cursorModeActive;

	// Token: 0x04007224 RID: 29220
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CursorControllerAbs.ECursorType currentCursorType = CursorControllerAbs.ECursorType.Default;
}
