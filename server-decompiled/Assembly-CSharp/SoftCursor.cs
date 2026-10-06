using System;
using System.Collections;
using Audio;
using InControl;
using Platform;
using UnityEngine;

// Token: 0x020012F1 RID: 4849
public class SoftCursor : CursorControllerAbs
{
	// Token: 0x1700125B RID: 4699
	// (get) Token: 0x06009968 RID: 39272 RVA: 0x003A39D1 File Offset: 0x003A1BD1
	// (set) Token: 0x06009969 RID: 39273 RVA: 0x003A39D9 File Offset: 0x003A1BD9
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

	// Token: 0x1700125C RID: 4700
	// (get) Token: 0x0600996A RID: 39274 RVA: 0x003A39F7 File Offset: 0x003A1BF7
	// (set) Token: 0x0600996B RID: 39275 RVA: 0x003A39FF File Offset: 0x003A1BFF
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

	// Token: 0x1700125D RID: 4701
	// (get) Token: 0x0600996C RID: 39276 RVA: 0x003A3A1D File Offset: 0x003A1C1D
	// (set) Token: 0x0600996D RID: 39277 RVA: 0x003A3A28 File Offset: 0x003A1C28
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

	// Token: 0x1700125E RID: 4702
	// (get) Token: 0x0600996E RID: 39278 RVA: 0x003A3A87 File Offset: 0x003A1C87
	public override bool CursorModeActive
	{
		get
		{
			return this.cursorModeActive;
		}
	}

	// Token: 0x1700125F RID: 4703
	// (get) Token: 0x0600996F RID: 39279 RVA: 0x003A3A8F File Offset: 0x003A1C8F
	public bool SoftcursorAllowed
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.guiActions != null && this.guiActions.Enabled && LocalPlayerUI.AnyModalWindowOpen();
		}
	}

	// Token: 0x17001260 RID: 4704
	// (get) Token: 0x06009970 RID: 39280 RVA: 0x003A3AAD File Offset: 0x003A1CAD
	// (set) Token: 0x06009971 RID: 39281 RVA: 0x003A3ABF File Offset: 0x003A1CBF
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

	// Token: 0x17001261 RID: 4705
	// (get) Token: 0x06009972 RID: 39282 RVA: 0x003A3AE2 File Offset: 0x003A1CE2
	// (set) Token: 0x06009973 RID: 39283 RVA: 0x003A3AF4 File Offset: 0x003A1CF4
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

	// Token: 0x17001262 RID: 4706
	// (get) Token: 0x06009974 RID: 39284 RVA: 0x00010E62 File Offset: 0x0000F062
	public static CursorLockMode DefaultCursorLockState
	{
		get
		{
			return CursorLockMode.None;
		}
	}

	// Token: 0x06009975 RID: 39285 RVA: 0x003A3B18 File Offset: 0x003A1D18
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

	// Token: 0x06009976 RID: 39286 RVA: 0x003A3CB8 File Offset: 0x003A1EB8
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

	// Token: 0x06009977 RID: 39287 RVA: 0x003A3D17 File Offset: 0x003A1F17
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.uiCamera = base.GetComponentInParent<UICamera>();
		this.UpdateMoveSpeed();
		base.InitCursorBounds();
	}

	// Token: 0x06009978 RID: 39288 RVA: 0x003A3D31 File Offset: 0x003A1F31
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		base.DestroyBase();
	}

	// Token: 0x06009979 RID: 39289 RVA: 0x003A3D3C File Offset: 0x003A1F3C
	public override void UpdateMoveSpeed()
	{
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		this.speed = 500f + 2000f * @float;
		this.mouseSpeed = (500f + 2000f * @float) / 1000f * (1f / MouseBindingSource.ScaleX) * 4f;
	}

	// Token: 0x0600997A RID: 39290 RVA: 0x003A3D94 File Offset: 0x003A1F94
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

	// Token: 0x0600997B RID: 39291 RVA: 0x003A3E23 File Offset: 0x003A2023
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.selectionBox.enabled)
		{
			this.RefreshSelection();
		}
	}

	// Token: 0x0600997C RID: 39292 RVA: 0x003A3E38 File Offset: 0x003A2038
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

	// Token: 0x0600997D RID: 39293 RVA: 0x003A40FC File Offset: 0x003A22FC
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

	// Token: 0x0600997E RID: 39294 RVA: 0x003A413C File Offset: 0x003A233C
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

	// Token: 0x0600997F RID: 39295 RVA: 0x003A41FC File Offset: 0x003A23FC
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

	// Token: 0x06009980 RID: 39296 RVA: 0x003A432C File Offset: 0x003A252C
	public override Vector2 GetScreenPosition()
	{
		Vector2 vector = default(Vector2);
		if (this.uiCamera != null)
		{
			vector = this.uiCamera.cachedCamera.WorldToScreenPoint(this.Position);
		}
		return vector;
	}

	// Token: 0x06009981 RID: 39297 RVA: 0x003A436C File Offset: 0x003A256C
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

	// Token: 0x06009982 RID: 39298 RVA: 0x003A43EA File Offset: 0x003A25EA
	public override void SetScreenPosition(Vector2 _newPosition)
	{
		this.Position = this.uiCamera.cachedCamera.ScreenToWorldPoint(_newPosition);
	}

	// Token: 0x06009983 RID: 39299 RVA: 0x003A4408 File Offset: 0x003A2608
	public override void SetScreenPosition(float _x, float _y)
	{
		this.SetScreenPosition(new Vector2(_x, _y));
	}

	// Token: 0x06009984 RID: 39300 RVA: 0x003A4417 File Offset: 0x003A2617
	public override void ResetToCenter()
	{
		this.LocalPosition = Vector3.zero;
	}

	// Token: 0x06009985 RID: 39301 RVA: 0x003A4424 File Offset: 0x003A2624
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

	// Token: 0x06009986 RID: 39302 RVA: 0x003A4494 File Offset: 0x003A2694
	public override bool GetCursorHidden()
	{
		return this.hidden;
	}

	// Token: 0x06009987 RID: 39303 RVA: 0x003A449C File Offset: 0x003A269C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetCursorSprite(INGUIAtlas _atlasMouse, string _spriteMouse, UIWidget.Pivot _pivotMouse, INGUIAtlas _atlasController, string _spriteController, UIWidget.Pivot _pivotController)
	{
		this.cursor.atlas = _atlasController;
		this.cursor.spriteName = _spriteController;
		this.cursor.pivot = _pivotController;
	}

	// Token: 0x06009988 RID: 39304 RVA: 0x003A44C5 File Offset: 0x003A26C5
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetCursorSpriteNone()
	{
		this.cursor.atlas = null;
		this.cursor.spriteName = "";
		this.SetSelectionBoxEnabled(false);
	}

	// Token: 0x06009989 RID: 39305 RVA: 0x003A44EC File Offset: 0x003A26EC
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

	// Token: 0x0600998A RID: 39306 RVA: 0x003A45D4 File Offset: 0x003A27D4
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

	// Token: 0x0600998B RID: 39307 RVA: 0x003A46AE File Offset: 0x003A28AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnVirtualCursorVisibleChanged()
	{
		if (this.LastInputStyle == PlayerInputManager.InputStyle.Keyboard || this.movingMouse)
		{
			return;
		}
		this.cursor.enabled = !base.VirtualCursorHidden && !base.Locked && this.cursorModeActive;
	}

	// Token: 0x0600998C RID: 39308 RVA: 0x003A46E8 File Offset: 0x003A28E8
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

	// Token: 0x0600998D RID: 39309 RVA: 0x003A4734 File Offset: 0x003A2934
	public static void SetCursorVisible(bool _visible)
	{
		for (int i = 0; i < CursorControllerAbs.softCursors.Count; i++)
		{
			(CursorControllerAbs.softCursors[i] as SoftCursor).SetVisible(_visible);
		}
	}

	// Token: 0x0600998E RID: 39310 RVA: 0x003A476C File Offset: 0x003A296C
	public Vector2 GetFlatPosition()
	{
		return new Vector2(this.Position.x, this.Position.y);
	}

	// Token: 0x0600998F RID: 39311 RVA: 0x003A478C File Offset: 0x003A298C
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

	// Token: 0x06009990 RID: 39312 RVA: 0x003A486C File Offset: 0x003A2A6C
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

	// Token: 0x06009991 RID: 39313 RVA: 0x003A48D4 File Offset: 0x003A2AD4
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

	// Token: 0x06009992 RID: 39314 RVA: 0x003A49A4 File Offset: 0x003A2BA4
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

	// Token: 0x06009993 RID: 39315 RVA: 0x003A49F7 File Offset: 0x003A2BF7
	public override bool IsWithinNavigationLockView(XUiView _view)
	{
		return base.lockNavigationToView == null || _view.Controller.IsSelfOrChildOf(base.lockNavigationToView.Controller);
	}

	// Token: 0x06009994 RID: 39316 RVA: 0x003A4A19 File Offset: 0x003A2C19
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

	// Token: 0x06009995 RID: 39317 RVA: 0x003A4A3C File Offset: 0x003A2C3C
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

	// Token: 0x06009996 RID: 39318 RVA: 0x003A4A52 File Offset: 0x003A2C52
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

	// Token: 0x04007354 RID: 29524
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float BaseSpeed = 500f;

	// Token: 0x04007355 RID: 29525
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float SpeedModRange = 2000f;

	// Token: 0x04007356 RID: 29526
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string emptyCursorAtlasName = "UIAtlas";

	// Token: 0x04007357 RID: 29527
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string emptyCursorSprite = "";

	// Token: 0x04007358 RID: 29528
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot emptyCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x04007359 RID: 29529
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas emptyCursorAtlas;

	// Token: 0x0400735A RID: 29530
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorAtlasName = "UIAtlas";

	// Token: 0x0400735B RID: 29531
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorSprite = "soft_cursor";

	// Token: 0x0400735C RID: 29532
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot defaultControllerCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x0400735D RID: 29533
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas defaultControllerCursorAtlas;

	// Token: 0x0400735E RID: 29534
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorAtlasName = "UIAtlas";

	// Token: 0x0400735F RID: 29535
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorSprite = "cursor01";

	// Token: 0x04007360 RID: 29536
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot defaultMouseCursorPivot = UIWidget.Pivot.TopLeft;

	// Token: 0x04007361 RID: 29537
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas defaultMouseCursorAtlas;

	// Token: 0x04007362 RID: 29538
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorAtlasName = "UIAtlas";

	// Token: 0x04007363 RID: 29539
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorSprite = "map_cursor";

	// Token: 0x04007364 RID: 29540
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot mapCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x04007365 RID: 29541
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas mapCursorAtlas;

	// Token: 0x04007366 RID: 29542
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speed;

	// Token: 0x04007367 RID: 29543
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mouseSpeed;

	// Token: 0x04007368 RID: 29544
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float LastFrameTime;

	// Token: 0x04007369 RID: 29545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hidden;

	// Token: 0x0400736A RID: 29546
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool snapped;

	// Token: 0x0400736B RID: 29547
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float OffsetSnapBounds = 0.1f;

	// Token: 0x0400736C RID: 29548
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UIPanel cursorPanel;

	// Token: 0x0400736D RID: 29549
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 lastMousePosition = Vector2.zero;

	// Token: 0x0400736E RID: 29550
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float currentAcceleration;

	// Token: 0x0400736F RID: 29551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedMultiplier = 1f;

	// Token: 0x04007370 RID: 29552
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool movingMouse;

	// Token: 0x04007371 RID: 29553
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UISprite selectionBox;

	// Token: 0x04007372 RID: 29554
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int selectionBoxMargin = 10;

	// Token: 0x04007373 RID: 29555
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public PlayerInputManager.InputStyle m_lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x04007374 RID: 29556
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_softcursorEnabled;

	// Token: 0x04007375 RID: 29557
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool cursorModeActive;

	// Token: 0x04007376 RID: 29558
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CursorControllerAbs.ECursorType currentCursorType = CursorControllerAbs.ECursorType.Default;
}
