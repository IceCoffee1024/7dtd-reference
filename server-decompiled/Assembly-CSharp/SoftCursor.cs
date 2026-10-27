using System;
using System.Collections;
using Audio;
using InControl;
using Platform;
using UnityEngine;

// Token: 0x02001303 RID: 4867
public class SoftCursor : CursorControllerAbs
{
	// Token: 0x17001267 RID: 4711
	// (get) Token: 0x060099C0 RID: 39360 RVA: 0x003A5DD5 File Offset: 0x003A3FD5
	// (set) Token: 0x060099C1 RID: 39361 RVA: 0x003A5DDD File Offset: 0x003A3FDD
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

	// Token: 0x17001268 RID: 4712
	// (get) Token: 0x060099C2 RID: 39362 RVA: 0x003A5DFB File Offset: 0x003A3FFB
	// (set) Token: 0x060099C3 RID: 39363 RVA: 0x003A5E03 File Offset: 0x003A4003
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

	// Token: 0x17001269 RID: 4713
	// (get) Token: 0x060099C4 RID: 39364 RVA: 0x003A5E21 File Offset: 0x003A4021
	// (set) Token: 0x060099C5 RID: 39365 RVA: 0x003A5E2C File Offset: 0x003A402C
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

	// Token: 0x1700126A RID: 4714
	// (get) Token: 0x060099C6 RID: 39366 RVA: 0x003A5E8B File Offset: 0x003A408B
	public override bool CursorModeActive
	{
		get
		{
			return this.cursorModeActive;
		}
	}

	// Token: 0x1700126B RID: 4715
	// (get) Token: 0x060099C7 RID: 39367 RVA: 0x003A5E93 File Offset: 0x003A4093
	public bool SoftcursorAllowed
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.guiActions != null && this.guiActions.Enabled && LocalPlayerUI.AnyModalWindowOpen();
		}
	}

	// Token: 0x1700126C RID: 4716
	// (get) Token: 0x060099C8 RID: 39368 RVA: 0x003A5EB1 File Offset: 0x003A40B1
	// (set) Token: 0x060099C9 RID: 39369 RVA: 0x003A5EC3 File Offset: 0x003A40C3
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

	// Token: 0x1700126D RID: 4717
	// (get) Token: 0x060099CA RID: 39370 RVA: 0x003A5EE6 File Offset: 0x003A40E6
	// (set) Token: 0x060099CB RID: 39371 RVA: 0x003A5EF8 File Offset: 0x003A40F8
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

	// Token: 0x1700126E RID: 4718
	// (get) Token: 0x060099CC RID: 39372 RVA: 0x00010E62 File Offset: 0x0000F062
	public static CursorLockMode DefaultCursorLockState
	{
		get
		{
			return CursorLockMode.None;
		}
	}

	// Token: 0x060099CD RID: 39373 RVA: 0x003A5F1C File Offset: 0x003A411C
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

	// Token: 0x060099CE RID: 39374 RVA: 0x003A60BC File Offset: 0x003A42BC
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

	// Token: 0x060099CF RID: 39375 RVA: 0x003A611B File Offset: 0x003A431B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.uiCamera = base.GetComponentInParent<UICamera>();
		this.UpdateMoveSpeed();
		base.InitCursorBounds();
	}

	// Token: 0x060099D0 RID: 39376 RVA: 0x003A6135 File Offset: 0x003A4335
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		base.DestroyBase();
	}

	// Token: 0x060099D1 RID: 39377 RVA: 0x003A6140 File Offset: 0x003A4340
	public override void UpdateMoveSpeed()
	{
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		this.speed = 500f + 2000f * @float;
		this.mouseSpeed = (500f + 2000f * @float) / 1000f * (1f / MouseBindingSource.ScaleX) * 4f;
	}

	// Token: 0x060099D2 RID: 39378 RVA: 0x003A6198 File Offset: 0x003A4398
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

	// Token: 0x060099D3 RID: 39379 RVA: 0x003A6227 File Offset: 0x003A4427
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		if (this.selectionBox.enabled)
		{
			this.RefreshSelection();
		}
	}

	// Token: 0x060099D4 RID: 39380 RVA: 0x003A623C File Offset: 0x003A443C
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

	// Token: 0x060099D5 RID: 39381 RVA: 0x003A6500 File Offset: 0x003A4700
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

	// Token: 0x060099D6 RID: 39382 RVA: 0x003A6540 File Offset: 0x003A4740
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

	// Token: 0x060099D7 RID: 39383 RVA: 0x003A6600 File Offset: 0x003A4800
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

	// Token: 0x060099D8 RID: 39384 RVA: 0x003A6730 File Offset: 0x003A4930
	public override Vector2 GetScreenPosition()
	{
		Vector2 vector = default(Vector2);
		if (this.uiCamera != null)
		{
			vector = this.uiCamera.cachedCamera.WorldToScreenPoint(this.Position);
		}
		return vector;
	}

	// Token: 0x060099D9 RID: 39385 RVA: 0x003A6770 File Offset: 0x003A4970
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

	// Token: 0x060099DA RID: 39386 RVA: 0x003A67EE File Offset: 0x003A49EE
	public override void SetScreenPosition(Vector2 _newPosition)
	{
		this.Position = this.uiCamera.cachedCamera.ScreenToWorldPoint(_newPosition);
	}

	// Token: 0x060099DB RID: 39387 RVA: 0x003A680C File Offset: 0x003A4A0C
	public override void SetScreenPosition(float _x, float _y)
	{
		this.SetScreenPosition(new Vector2(_x, _y));
	}

	// Token: 0x060099DC RID: 39388 RVA: 0x003A681B File Offset: 0x003A4A1B
	public override void ResetToCenter()
	{
		this.LocalPosition = Vector3.zero;
	}

	// Token: 0x060099DD RID: 39389 RVA: 0x003A6828 File Offset: 0x003A4A28
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

	// Token: 0x060099DE RID: 39390 RVA: 0x003A6898 File Offset: 0x003A4A98
	public override bool GetCursorHidden()
	{
		return this.hidden;
	}

	// Token: 0x060099DF RID: 39391 RVA: 0x003A68A0 File Offset: 0x003A4AA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetCursorSprite(INGUIAtlas _atlasMouse, string _spriteMouse, UIWidget.Pivot _pivotMouse, INGUIAtlas _atlasController, string _spriteController, UIWidget.Pivot _pivotController)
	{
		this.cursor.atlas = _atlasController;
		this.cursor.spriteName = _spriteController;
		this.cursor.pivot = _pivotController;
	}

	// Token: 0x060099E0 RID: 39392 RVA: 0x003A68C9 File Offset: 0x003A4AC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetCursorSpriteNone()
	{
		this.cursor.atlas = null;
		this.cursor.spriteName = "";
		this.SetSelectionBoxEnabled(false);
	}

	// Token: 0x060099E1 RID: 39393 RVA: 0x003A68F0 File Offset: 0x003A4AF0
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

	// Token: 0x060099E2 RID: 39394 RVA: 0x003A69D8 File Offset: 0x003A4BD8
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

	// Token: 0x060099E3 RID: 39395 RVA: 0x003A6AB2 File Offset: 0x003A4CB2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnVirtualCursorVisibleChanged()
	{
		if (this.LastInputStyle == PlayerInputManager.InputStyle.Keyboard || this.movingMouse)
		{
			return;
		}
		this.cursor.enabled = !base.VirtualCursorHidden && !base.Locked && this.cursorModeActive;
	}

	// Token: 0x060099E4 RID: 39396 RVA: 0x003A6AEC File Offset: 0x003A4CEC
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

	// Token: 0x060099E5 RID: 39397 RVA: 0x003A6B38 File Offset: 0x003A4D38
	public static void SetCursorVisible(bool _visible)
	{
		for (int i = 0; i < CursorControllerAbs.softCursors.Count; i++)
		{
			(CursorControllerAbs.softCursors[i] as SoftCursor).SetVisible(_visible);
		}
	}

	// Token: 0x060099E6 RID: 39398 RVA: 0x003A6B70 File Offset: 0x003A4D70
	public Vector2 GetFlatPosition()
	{
		return new Vector2(this.Position.x, this.Position.y);
	}

	// Token: 0x060099E7 RID: 39399 RVA: 0x003A6B90 File Offset: 0x003A4D90
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

	// Token: 0x060099E8 RID: 39400 RVA: 0x003A6C70 File Offset: 0x003A4E70
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

	// Token: 0x060099E9 RID: 39401 RVA: 0x003A6CD8 File Offset: 0x003A4ED8
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

	// Token: 0x060099EA RID: 39402 RVA: 0x003A6DA8 File Offset: 0x003A4FA8
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

	// Token: 0x060099EB RID: 39403 RVA: 0x003A6DFB File Offset: 0x003A4FFB
	public override bool IsWithinNavigationLockView(XUiView _view)
	{
		return base.lockNavigationToView == null || _view.Controller.IsSelfOrChildOf(base.lockNavigationToView.Controller);
	}

	// Token: 0x060099EC RID: 39404 RVA: 0x003A6E1D File Offset: 0x003A501D
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

	// Token: 0x060099ED RID: 39405 RVA: 0x003A6E40 File Offset: 0x003A5040
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

	// Token: 0x060099EE RID: 39406 RVA: 0x003A6E56 File Offset: 0x003A5056
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

	// Token: 0x04007395 RID: 29589
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float BaseSpeed = 500f;

	// Token: 0x04007396 RID: 29590
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float SpeedModRange = 2000f;

	// Token: 0x04007397 RID: 29591
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string emptyCursorAtlasName = "UIAtlas";

	// Token: 0x04007398 RID: 29592
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string emptyCursorSprite = "";

	// Token: 0x04007399 RID: 29593
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot emptyCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x0400739A RID: 29594
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas emptyCursorAtlas;

	// Token: 0x0400739B RID: 29595
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorAtlasName = "UIAtlas";

	// Token: 0x0400739C RID: 29596
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorSprite = "soft_cursor";

	// Token: 0x0400739D RID: 29597
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot defaultControllerCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x0400739E RID: 29598
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas defaultControllerCursorAtlas;

	// Token: 0x0400739F RID: 29599
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorAtlasName = "UIAtlas";

	// Token: 0x040073A0 RID: 29600
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorSprite = "cursor01";

	// Token: 0x040073A1 RID: 29601
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot defaultMouseCursorPivot = UIWidget.Pivot.TopLeft;

	// Token: 0x040073A2 RID: 29602
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas defaultMouseCursorAtlas;

	// Token: 0x040073A3 RID: 29603
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorAtlasName = "UIAtlas";

	// Token: 0x040073A4 RID: 29604
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorSprite = "map_cursor";

	// Token: 0x040073A5 RID: 29605
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly UIWidget.Pivot mapCursorPivot = UIWidget.Pivot.Center;

	// Token: 0x040073A6 RID: 29606
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static INGUIAtlas mapCursorAtlas;

	// Token: 0x040073A7 RID: 29607
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speed;

	// Token: 0x040073A8 RID: 29608
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mouseSpeed;

	// Token: 0x040073A9 RID: 29609
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float LastFrameTime;

	// Token: 0x040073AA RID: 29610
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool hidden;

	// Token: 0x040073AB RID: 29611
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool snapped;

	// Token: 0x040073AC RID: 29612
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float OffsetSnapBounds = 0.1f;

	// Token: 0x040073AD RID: 29613
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UIPanel cursorPanel;

	// Token: 0x040073AE RID: 29614
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector2 lastMousePosition = Vector2.zero;

	// Token: 0x040073AF RID: 29615
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float currentAcceleration;

	// Token: 0x040073B0 RID: 29616
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedMultiplier = 1f;

	// Token: 0x040073B1 RID: 29617
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool movingMouse;

	// Token: 0x040073B2 RID: 29618
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UISprite selectionBox;

	// Token: 0x040073B3 RID: 29619
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int selectionBoxMargin = 10;

	// Token: 0x040073B4 RID: 29620
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public PlayerInputManager.InputStyle m_lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x040073B5 RID: 29621
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_softcursorEnabled;

	// Token: 0x040073B6 RID: 29622
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool cursorModeActive;

	// Token: 0x040073B7 RID: 29623
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public CursorControllerAbs.ECursorType currentCursorType = CursorControllerAbs.ECursorType.Default;
}
