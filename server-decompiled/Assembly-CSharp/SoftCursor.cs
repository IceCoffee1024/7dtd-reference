using System;
using System.Collections;
using Audio;
using InControl;
using Platform;
using UnityEngine;

// Token: 0x020012D1 RID: 4817
public class SoftCursor : CursorControllerAbs
{
	// Token: 0x17001216 RID: 4630
	// (get) Token: 0x060098BC RID: 39100 RVA: 0x0039EE21 File Offset: 0x0039D021
	// (set) Token: 0x060098BD RID: 39101 RVA: 0x0039EE29 File Offset: 0x0039D029
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

	// Token: 0x17001217 RID: 4631
	// (get) Token: 0x060098BE RID: 39102 RVA: 0x0039EE47 File Offset: 0x0039D047
	// (set) Token: 0x060098BF RID: 39103 RVA: 0x0039EE4F File Offset: 0x0039D04F
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

	// Token: 0x17001218 RID: 4632
	// (get) Token: 0x060098C0 RID: 39104 RVA: 0x0039EE6D File Offset: 0x0039D06D
	// (set) Token: 0x060098C1 RID: 39105 RVA: 0x0039EE78 File Offset: 0x0039D078
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

	// Token: 0x17001219 RID: 4633
	// (get) Token: 0x060098C2 RID: 39106 RVA: 0x0039EED7 File Offset: 0x0039D0D7
	public override bool CursorModeActive
	{
		get
		{
			return this.cursorModeActive;
		}
	}

	// Token: 0x1700121A RID: 4634
	// (get) Token: 0x060098C3 RID: 39107 RVA: 0x0039EEDF File Offset: 0x0039D0DF
	public bool SoftcursorAllowed
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.guiActions != null && this.guiActions.Enabled && LocalPlayerUI.AnyModalWindowOpen();
		}
	}

	// Token: 0x1700121B RID: 4635
	// (get) Token: 0x060098C4 RID: 39108 RVA: 0x0039EEFD File Offset: 0x0039D0FD
	// (set) Token: 0x060098C5 RID: 39109 RVA: 0x0039EF0F File Offset: 0x0039D10F
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

	// Token: 0x1700121C RID: 4636
	// (get) Token: 0x060098C6 RID: 39110 RVA: 0x0039EF32 File Offset: 0x0039D132
	// (set) Token: 0x060098C7 RID: 39111 RVA: 0x0039EF44 File Offset: 0x0039D144
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

	// Token: 0x1700121D RID: 4637
	// (get) Token: 0x060098C8 RID: 39112 RVA: 0x00010E62 File Offset: 0x0000F062
	public static CursorLockMode DefaultCursorLockState
	{
		get
		{
			return CursorLockMode.None;
		}
	}

	// Token: 0x060098C9 RID: 39113 RVA: 0x0039EF68 File Offset: 0x0039D168
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

	// Token: 0x060098CA RID: 39114 RVA: 0x0039F108 File Offset: 0x0039D308
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

	// Token: 0x060098CB RID: 39115 RVA: 0x0039F167 File Offset: 0x0039D367
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.uiCamera = base.GetComponentInParent<UICamera>();
		this.UpdateMoveSpeed();
		base.InitCursorBounds();
	}

	// Token: 0x060098CC RID: 39116 RVA: 0x0039F181 File Offset: 0x0039D381
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		base.DestroyBase();
	}

	// Token: 0x060098CD RID: 39117 RVA: 0x0039F18C File Offset: 0x0039D38C
	public override void UpdateMoveSpeed()
	{
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		this.speed = 500f + 2000f * @float;
		this.mouseSpeed = (500f + 2000f * @float) / 1000f * (1f / MouseBindingSource.ScaleX) * 4f;
	}

	// Token: 0x060098CE RID: 39118 RVA: 0x0039F1E4 File Offset: 0x0039D3E4
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

	// Token: 0x060098CF RID: 39119 RVA: 0x0039F273 File Offset: 0x0039D473
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.selectionBox.enabled)
		{
			this.RefreshSelection();
		}
	}

	// Token: 0x060098D0 RID: 39120 RVA: 0x0039F288 File Offset: 0x0039D488
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

	// Token: 0x060098D1 RID: 39121 RVA: 0x0039F54C File Offset: 0x0039D74C
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

	// Token: 0x060098D2 RID: 39122 RVA: 0x0039F58C File Offset: 0x0039D78C
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

	// Token: 0x060098D3 RID: 39123 RVA: 0x0039F64C File Offset: 0x0039D84C
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

	// Token: 0x060098D4 RID: 39124 RVA: 0x0039F77C File Offset: 0x0039D97C
	public override Vector2 GetScreenPosition()
	{
		Vector2 vector = default(Vector2);
		if (this.uiCamera != null)
		{
			vector = this.uiCamera.cachedCamera.WorldToScreenPoint(this.Position);
		}
		return vector;
	}

	// Token: 0x060098D5 RID: 39125 RVA: 0x0039F7BC File Offset: 0x0039D9BC
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

	// Token: 0x060098D6 RID: 39126 RVA: 0x0039F83A File Offset: 0x0039DA3A
	public override void SetScreenPosition(Vector2 _newPosition)
	{
		this.Position = this.uiCamera.cachedCamera.ScreenToWorldPoint(_newPosition);
	}

	// Token: 0x060098D7 RID: 39127 RVA: 0x0039F858 File Offset: 0x0039DA58
	public override void SetScreenPosition(float _x, float _y)
	{
		this.SetScreenPosition(new Vector2(_x, _y));
	}

	// Token: 0x060098D8 RID: 39128 RVA: 0x0039F867 File Offset: 0x0039DA67
	public override void ResetToCenter()
	{
		this.LocalPosition = Vector3.zero;
	}

	// Token: 0x060098D9 RID: 39129 RVA: 0x0039F874 File Offset: 0x0039DA74
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

	// Token: 0x060098DA RID: 39130 RVA: 0x0039F8E4 File Offset: 0x0039DAE4
	public override bool GetCursorHidden()
	{
		return this.hidden;
	}

	// Token: 0x060098DB RID: 39131 RVA: 0x0039F8EC File Offset: 0x0039DAEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetCursorSprite(INGUIAtlas _atlasMouse, string _spriteMouse, UIWidget.Pivot _pivotMouse, INGUIAtlas _atlasController, string _spriteController, UIWidget.Pivot _pivotController)
	{
		this.cursor.atlas = _atlasController;
		this.cursor.spriteName = _spriteController;
		this.cursor.pivot = _pivotController;
	}

	// Token: 0x060098DC RID: 39132 RVA: 0x0039F915 File Offset: 0x0039DB15
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetCursorSpriteNone()
	{
		this.cursor.atlas = null;
		this.cursor.spriteName = "";
		this.SetSelectionBoxEnabled(false);
	}

	// Token: 0x060098DD RID: 39133 RVA: 0x0039F93C File Offset: 0x0039DB3C
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

	// Token: 0x060098DE RID: 39134 RVA: 0x0039FA24 File Offset: 0x0039DC24
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetVisible(bool _visible)
	{
		_visible &= !this.hidden;
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

	// Token: 0x060098DF RID: 39135 RVA: 0x0039FAFE File Offset: 0x0039DCFE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnVirtualCursorVisibleChanged()
	{
		if (this.LastInputStyle == PlayerInputManager.InputStyle.Keyboard || this.movingMouse)
		{
			return;
		}
		this.cursor.enabled = !base.VirtualCursorHidden && !base.Locked && this.cursorModeActive;
	}

	// Token: 0x060098E0 RID: 39136 RVA: 0x0039FB38 File Offset: 0x0039DD38
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

	// Token: 0x060098E1 RID: 39137 RVA: 0x0039FB84 File Offset: 0x0039DD84
	public static void SetCursorVisible(bool _visible)
	{
		for (int i = 0; i < CursorControllerAbs.softCursors.Count; i++)
		{
			(CursorControllerAbs.softCursors[i] as SoftCursor).SetVisible(_visible);
		}
	}

	// Token: 0x060098E2 RID: 39138 RVA: 0x0039FBBC File Offset: 0x0039DDBC
	public Vector2 GetFlatPosition()
	{
		return new Vector2(this.Position.x, this.Position.y);
	}

	// Token: 0x060098E3 RID: 39139 RVA: 0x0039FBDC File Offset: 0x0039DDDC
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

	// Token: 0x060098E4 RID: 39140 RVA: 0x0039FCBC File Offset: 0x0039DEBC
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

	// Token: 0x060098E5 RID: 39141 RVA: 0x0039FD24 File Offset: 0x0039DF24
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

	// Token: 0x060098E6 RID: 39142 RVA: 0x0039FDF4 File Offset: 0x0039DFF4
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

	// Token: 0x060098E7 RID: 39143 RVA: 0x0039FE47 File Offset: 0x0039E047
	public override bool IsWithinNavigationLockView(XUiView _view)
	{
		return base.lockNavigationToView == null || _view.Controller.IsSelfOrChildOf(base.lockNavigationToView.Controller);
	}

	// Token: 0x060098E8 RID: 39144 RVA: 0x0039FE69 File Offset: 0x0039E069
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

	// Token: 0x060098E9 RID: 39145 RVA: 0x0039FE8C File Offset: 0x0039E08C
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

	// Token: 0x060098EA RID: 39146 RVA: 0x0039FEA2 File Offset: 0x0039E0A2
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

	// Token: 0x040072D9 RID: 29401
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float BaseSpeed = 500f;

	// Token: 0x040072DA RID: 29402
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float SpeedModRange = 2000f;

	// Token: 0x040072DB RID: 29403
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string emptyCursorAtlasName = "UIAtlas";

	// Token: 0x040072DC RID: 29404
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string emptyCursorSprite = "";

	// Token: 0x040072DD RID: 29405
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot emptyCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x040072DE RID: 29406
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas emptyCursorAtlas;

	// Token: 0x040072DF RID: 29407
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorAtlasName = "UIAtlas";

	// Token: 0x040072E0 RID: 29408
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorSprite = "soft_cursor";

	// Token: 0x040072E1 RID: 29409
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot defaultControllerCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x040072E2 RID: 29410
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas defaultControllerCursorAtlas;

	// Token: 0x040072E3 RID: 29411
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorAtlasName = "UIAtlas";

	// Token: 0x040072E4 RID: 29412
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorSprite = "cursor01";

	// Token: 0x040072E5 RID: 29413
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot defaultMouseCursorPivot = UIWidget.Pivot.TopLeft;

	// Token: 0x040072E6 RID: 29414
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas defaultMouseCursorAtlas;

	// Token: 0x040072E7 RID: 29415
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorAtlasName = "UIAtlas";

	// Token: 0x040072E8 RID: 29416
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorSprite = "map_cursor";

	// Token: 0x040072E9 RID: 29417
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot mapCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x040072EA RID: 29418
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas mapCursorAtlas;

	// Token: 0x040072EB RID: 29419
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speed;

	// Token: 0x040072EC RID: 29420
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mouseSpeed;

	// Token: 0x040072ED RID: 29421
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float LastFrameTime;

	// Token: 0x040072EE RID: 29422
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hidden;

	// Token: 0x040072EF RID: 29423
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool snapped;

	// Token: 0x040072F0 RID: 29424
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float OffsetSnapBounds = 0.1f;

	// Token: 0x040072F1 RID: 29425
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UIPanel cursorPanel;

	// Token: 0x040072F2 RID: 29426
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 lastMousePosition = Vector2.zero;

	// Token: 0x040072F3 RID: 29427
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float currentAcceleration;

	// Token: 0x040072F4 RID: 29428
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedMultiplier = 1f;

	// Token: 0x040072F5 RID: 29429
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool movingMouse;

	// Token: 0x040072F6 RID: 29430
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UISprite selectionBox;

	// Token: 0x040072F7 RID: 29431
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int selectionBoxMargin = 10;

	// Token: 0x040072F8 RID: 29432
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public PlayerInputManager.InputStyle m_lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x040072F9 RID: 29433
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_softcursorEnabled;

	// Token: 0x040072FA RID: 29434
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool cursorModeActive;

	// Token: 0x040072FB RID: 29435
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CursorControllerAbs.ECursorType currentCursorType = CursorControllerAbs.ECursorType.Default;
}
