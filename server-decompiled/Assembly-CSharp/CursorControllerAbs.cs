using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x020012FE RID: 4862
public abstract class CursorControllerAbs : MonoBehaviour, IGamePrefsChangedListener
{
	// Token: 0x1700125C RID: 4700
	// (get) Token: 0x06009983 RID: 39299 RVA: 0x003A5687 File Offset: 0x003A3887
	public static bool PrefabReady
	{
		get
		{
			return CursorControllerAbs.softCursorPrefab != null;
		}
	}

	// Token: 0x1700125D RID: 4701
	// (get) Token: 0x06009984 RID: 39300 RVA: 0x003A5694 File Offset: 0x003A3894
	// (set) Token: 0x06009985 RID: 39301 RVA: 0x003A569C File Offset: 0x003A389C
	public virtual XUiView HoverTarget
	{
		get
		{
			return this.hoverTarget;
		}
		set
		{
			this.hoverTarget = value;
			this.bHasHoverTarget = value != null;
		}
	}

	// Token: 0x1700125E RID: 4702
	// (get) Token: 0x06009986 RID: 39302 RVA: 0x003A56AF File Offset: 0x003A38AF
	// (set) Token: 0x06009987 RID: 39303 RVA: 0x003A56B7 File Offset: 0x003A38B7
	public XUiView navigationTarget
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x1700125F RID: 4703
	// (get) Token: 0x06009988 RID: 39304 RVA: 0x003A56C0 File Offset: 0x003A38C0
	// (set) Token: 0x06009989 RID: 39305 RVA: 0x003A56C8 File Offset: 0x003A38C8
	public XUiView navigationTargetLater
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17001260 RID: 4704
	// (get) Token: 0x0600998A RID: 39306 RVA: 0x003A56D1 File Offset: 0x003A38D1
	// (set) Token: 0x0600998B RID: 39307 RVA: 0x003A56D9 File Offset: 0x003A38D9
	public XUiView lockNavigationToView
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17001261 RID: 4705
	// (get) Token: 0x0600998C RID: 39308 RVA: 0x003A56E2 File Offset: 0x003A38E2
	// (set) Token: 0x0600998D RID: 39309 RVA: 0x003A56EA File Offset: 0x003A38EA
	public bool Locked
	{
		get
		{
			return this._locked;
		}
		set
		{
			this._locked = value;
		}
	}

	// Token: 0x17001262 RID: 4706
	// (get) Token: 0x0600998E RID: 39310 RVA: 0x003A56F3 File Offset: 0x003A38F3
	// (set) Token: 0x0600998F RID: 39311 RVA: 0x003A56FB File Offset: 0x003A38FB
	public bool VirtualCursorHidden
	{
		get
		{
			return this._virtualCursorHidden;
		}
		set
		{
			if (value != this._virtualCursorHidden)
			{
				this._virtualCursorHidden = value;
				this.OnVirtualCursorVisibleChanged();
			}
		}
	}

	// Token: 0x17001263 RID: 4707
	// (get) Token: 0x06009990 RID: 39312 RVA: 0x003A5713 File Offset: 0x003A3913
	public XUiView CurrentTarget
	{
		get
		{
			if (this.CursorModeActive)
			{
				return this.hoverTarget;
			}
			return this.navigationTarget;
		}
	}

	// Token: 0x17001264 RID: 4708
	// (get) Token: 0x06009991 RID: 39313 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CursorModeActive
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06009992 RID: 39314
	public abstract Vector2 GetScreenPosition();

	// Token: 0x06009993 RID: 39315
	public abstract Vector2 GetLocalScreenPosition();

	// Token: 0x06009994 RID: 39316
	public abstract void SetScreenPosition(Vector2 _newPosition);

	// Token: 0x06009995 RID: 39317
	public abstract void SetScreenPosition(float _x, float _y);

	// Token: 0x06009996 RID: 39318
	public abstract void SetNavigationTarget(XUiView _view);

	// Token: 0x06009997 RID: 39319
	public abstract void SetNavigationTargetLater(XUiView _view);

	// Token: 0x06009998 RID: 39320
	public abstract void ResetNavigationTarget();

	// Token: 0x06009999 RID: 39321
	public abstract void ResetToCenter();

	// Token: 0x0600999A RID: 39322
	public abstract void SetNavigationLockView(XUiView _view, XUiView _viewToSelect = null);

	// Token: 0x0600999B RID: 39323
	public abstract bool IsWithinNavigationLockView(XUiView _view);

	// Token: 0x0600999C RID: 39324
	public abstract void RefreshSelection();

	// Token: 0x0600999D RID: 39325
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void OnVirtualCursorVisibleChanged();

	// Token: 0x0600999E RID: 39326 RVA: 0x003A572A File Offset: 0x003A392A
	public void SetGUIActions(PlayerActionsGUI _guiActions)
	{
		this.guiActions = _guiActions;
	}

	// Token: 0x0600999F RID: 39327 RVA: 0x003A5733 File Offset: 0x003A3933
	public void SetWindowManager(GUIWindowManager _windowManager)
	{
		this.windowManager = _windowManager;
	}

	// Token: 0x060099A0 RID: 39328 RVA: 0x003A573C File Offset: 0x003A393C
	public static void UpdateGamePrefs()
	{
		CursorControllerAbs.bSnapCursor = GamePrefs.GetBool(EnumGamePrefs.OptionsControllerCursorSnap);
		CursorControllerAbs.regularSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		CursorControllerAbs.hoverSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsControllerCursorHoverSensitivity);
	}

	// Token: 0x060099A1 RID: 39329 RVA: 0x003A576B File Offset: 0x003A396B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AwakeBase()
	{
		GamePrefs.AddChangeListener(this);
		GameOptionsManager.ResolutionChanged += this.OnResolutionChanged;
		CursorControllerAbs.UpdateGamePrefs();
		CursorControllerAbs.softCursors.Add(this);
	}

	// Token: 0x060099A2 RID: 39330 RVA: 0x003A5794 File Offset: 0x003A3994
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DestroyBase()
	{
		GamePrefs.RemoveChangeListener(this);
		GameOptionsManager.ResolutionChanged -= this.OnResolutionChanged;
		CursorControllerAbs.softCursors.Remove(this);
	}

	// Token: 0x060099A3 RID: 39331 RVA: 0x003A57BC File Offset: 0x003A39BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void InitCursorBounds()
	{
		this.cursorWorldBounds = new Bounds(this.cursor.worldCenter, Vector3.zero);
		Vector3[] worldCorners = this.cursor.worldCorners;
		for (int i = 0; i < worldCorners.Length; i++)
		{
			this.cursorWorldBounds.Encapsulate(worldCorners[i]);
		}
		Bounds bounds = new Bounds(this.uiCamera.cachedCamera.WorldToScreenPoint(this.cursorWorldBounds.min), Vector3.zero);
		bounds.Encapsulate(this.uiCamera.cachedCamera.WorldToScreenPoint(this.cursorWorldBounds.max));
		this.cursorBuffer = bounds.extents;
	}

	// Token: 0x060099A4 RID: 39332
	public abstract void UpdateMoveSpeed();

	// Token: 0x060099A5 RID: 39333 RVA: 0x003A5866 File Offset: 0x003A3A66
	public void UpdateBounds(string _boundsName, Bounds _bounds)
	{
		_bounds.Expand(this.cursorBuffer);
		this.activeBounds[_boundsName] = _bounds;
		this.RefreshBounds();
	}

	// Token: 0x060099A6 RID: 39334 RVA: 0x003A5888 File Offset: 0x003A3A88
	public void RemoveBounds(string _boundsName)
	{
		this.activeBounds.Remove(_boundsName);
		this.RefreshBounds();
	}

	// Token: 0x060099A7 RID: 39335 RVA: 0x003A58A0 File Offset: 0x003A3AA0
	public void RefreshBounds()
	{
		this.currentBounds.size = Vector3.zero;
		if (this.activeBounds.Count > 0)
		{
			bool flag = true;
			using (Dictionary<string, Bounds>.Enumerator enumerator = this.activeBounds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, Bounds> keyValuePair = enumerator.Current;
					if (flag)
					{
						this.currentBounds.center = keyValuePair.Value.center;
						flag = false;
					}
					this.currentBounds.Encapsulate(keyValuePair.Value);
				}
				return;
			}
		}
		this.currentBounds.center = new Vector3(0f, 0f);
		this.currentBounds.Encapsulate(new Vector3((float)this.uiCamera.cachedCamera.pixelWidth, (float)this.uiCamera.cachedCamera.pixelHeight));
	}

	// Token: 0x060099A8 RID: 39336 RVA: 0x003A598C File Offset: 0x003A3B8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnResolutionChanged(int _width, int _height)
	{
		base.StartCoroutine(this.RefreshBoundsNextFrame());
	}

	// Token: 0x060099A9 RID: 39337 RVA: 0x003A599B File Offset: 0x003A3B9B
	public void OnGamePrefChanged(EnumGamePrefs _enum)
	{
		if (_enum == EnumGamePrefs.OptionsInterfaceSensitivity)
		{
			this.UpdateMoveSpeed();
			return;
		}
		if (_enum == EnumGamePrefs.OptionsControllerCursorSnap)
		{
			CursorControllerAbs.bSnapCursor = GamePrefs.GetBool(_enum);
			return;
		}
		if (_enum != EnumGamePrefs.OptionsControllerCursorHoverSensitivity)
		{
			return;
		}
		CursorControllerAbs.hoverSpeed = GamePrefs.GetFloat(_enum);
	}

	// Token: 0x060099AA RID: 39338 RVA: 0x003A59D4 File Offset: 0x003A3BD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator RefreshBoundsNextFrame()
	{
		yield return null;
		this.RefreshBounds();
		yield break;
	}

	// Token: 0x060099AB RID: 39339
	public abstract void SetCursorHidden(bool _hidden);

	// Token: 0x060099AC RID: 39340
	public abstract bool GetCursorHidden();

	// Token: 0x060099AD RID: 39341 RVA: 0x003A59E4 File Offset: 0x003A3BE4
	public bool GetMouseButtonDown(UICamera.MouseButton _mouseButton)
	{
		if (this.guiActions == null)
		{
			return false;
		}
		if (GUIWindowConsole.IsOpen())
		{
			return false;
		}
		switch (_mouseButton)
		{
		case UICamera.MouseButton.LeftButton:
			return this.guiActions.Submit.WasPressed || this.guiActions.LeftClick.WasPressed;
		case UICamera.MouseButton.RightButton:
			return this.guiActions.Inspect.WasPressed || this.guiActions.RightClick.WasPressed;
		case UICamera.MouseButton.MiddleButton:
			return this.guiActions.MiddleClick.WasPressed;
		default:
			return false;
		}
	}

	// Token: 0x060099AE RID: 39342 RVA: 0x003A5A74 File Offset: 0x003A3C74
	public bool GetMouseButton(UICamera.MouseButton _mouseButton)
	{
		if (this.guiActions == null)
		{
			return false;
		}
		if (GUIWindowConsole.IsOpen())
		{
			return false;
		}
		switch (_mouseButton)
		{
		case UICamera.MouseButton.LeftButton:
			return this.guiActions.Submit.IsPressed || this.guiActions.LeftClick.IsPressed;
		case UICamera.MouseButton.RightButton:
			return this.guiActions.Inspect.IsPressed || this.guiActions.RightClick.IsPressed;
		case UICamera.MouseButton.MiddleButton:
			return this.guiActions.MiddleClick.IsPressed;
		default:
			return false;
		}
	}

	// Token: 0x060099AF RID: 39343 RVA: 0x003A5B04 File Offset: 0x003A3D04
	public bool GetMouseButtonUp(UICamera.MouseButton _mouseButton)
	{
		if (this.guiActions == null)
		{
			return false;
		}
		switch (_mouseButton)
		{
		case UICamera.MouseButton.LeftButton:
			return this.guiActions.Submit.WasReleased || this.guiActions.LeftClick.WasReleased;
		case UICamera.MouseButton.RightButton:
			return this.guiActions.Inspect.WasReleased || this.guiActions.RightClick.WasReleased;
		case UICamera.MouseButton.MiddleButton:
			return this.guiActions.MiddleClick.WasReleased;
		default:
			return false;
		}
	}

	// Token: 0x060099B0 RID: 39344 RVA: 0x003A5B8C File Offset: 0x003A3D8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DebugDrawBound(Bounds _bound)
	{
		Vector3 vector = _bound.max;
		Vector3 vector2 = _bound.min;
		Vector3 vector3 = new Vector3(vector2.x, vector.y, vector.z);
		Vector3 vector4 = new Vector3(vector.x, vector2.y, vector2.z);
		vector = this.uiCamera.cachedCamera.ScreenToWorldPoint(vector);
		vector3 = this.uiCamera.cachedCamera.ScreenToWorldPoint(vector3);
		vector4 = this.uiCamera.cachedCamera.ScreenToWorldPoint(vector4);
		vector2 = this.uiCamera.cachedCamera.ScreenToWorldPoint(vector2);
		Debug.DrawLine(vector, vector3);
		Debug.DrawLine(vector4, vector2);
		Debug.DrawLine(vector, vector4);
		Debug.DrawLine(vector3, vector2);
	}

	// Token: 0x060099B1 RID: 39345 RVA: 0x003A5C3F File Offset: 0x003A3E3F
	public static void SetCursor(CursorControllerAbs.ECursorType _cursorType)
	{
		SoftCursor.SetCursor(_cursorType);
	}

	// Token: 0x060099B2 RID: 39346 RVA: 0x003A5C47 File Offset: 0x003A3E47
	public static void LoadStaticData(LoadManager.LoadGroup _loadGroup)
	{
		LoadManager.LoadAssetFromResources<GameObject>(CursorControllerAbs.softCursorPrefabPath, delegate(GameObject _asset)
		{
			CursorControllerAbs.softCursorPrefab = _asset;
		}, null, false, true);
	}

	// Token: 0x060099B3 RID: 39347 RVA: 0x003A5C78 File Offset: 0x003A3E78
	public static CursorControllerAbs AddSoftCursor(UICamera _camera, PlayerActionsGUI _guiActions, GUIWindowManager _windowManager)
	{
		GameObject gameObject = _camera.gameObject.AddChild(CursorControllerAbs.softCursorPrefab);
		SoftCursor component = gameObject.GetComponent<SoftCursor>();
		component.SetGUIActions(_guiActions);
		component.SetWindowManager(_windowManager);
		_camera.cancelKey0 = KeyCode.None;
		_camera.submitKey1 = KeyCode.None;
		_camera.cancelKey1 = KeyCode.None;
		UICamera.GetMousePosition = new UICamera.GetMousePositionFunc(component.GetScreenPosition);
		UICamera.GetMouseButton = new UICamera.GetMouseButtonFunc(component.GetMouseButton);
		UICamera.GetMouseButtonDown = new UICamera.GetMouseButtonFunc(component.GetMouseButtonDown);
		UICamera.GetMouseButtonUp = new UICamera.GetMouseButtonFunc(component.GetMouseButtonUp);
		gameObject.SetActive(true);
		return component;
	}

	// Token: 0x060099B4 RID: 39348 RVA: 0x003A5D0B File Offset: 0x003A3F0B
	public void PlayPagingSound()
	{
		Manager.PlayXUiSound(this.pagingSound, 1f);
	}

	// Token: 0x060099B5 RID: 39349 RVA: 0x003A5D1D File Offset: 0x003A3F1D
	[PublicizedFrom(EAccessModifier.Protected)]
	public CursorControllerAbs()
	{
	}

	// Token: 0x0400736E RID: 29550
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string softCursorPrefabPath = "Prefabs/SoftCursor";

	// Token: 0x0400736F RID: 29551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject softCursorPrefab;

	// Token: 0x04007370 RID: 29552
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<CursorControllerAbs> softCursors = new List<CursorControllerAbs>();

	// Token: 0x04007371 RID: 29553
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UICamera uiCamera;

	// Token: 0x04007372 RID: 29554
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public PlayerActionsGUI guiActions;

	// Token: 0x04007373 RID: 29555
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIWindowManager windowManager;

	// Token: 0x04007374 RID: 29556
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UISprite cursor;

	// Token: 0x04007375 RID: 29557
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Bounds cursorWorldBounds;

	// Token: 0x04007376 RID: 29558
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 cursorBuffer;

	// Token: 0x04007377 RID: 29559
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, Bounds> activeBounds = new Dictionary<string, Bounds>();

	// Token: 0x04007378 RID: 29560
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Bounds currentBounds;

	// Token: 0x04007379 RID: 29561
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip cursorSelectSound;

	// Token: 0x0400737A RID: 29562
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip pagingSound;

	// Token: 0x0400737B RID: 29563
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public XUiView hoverTarget;

	// Token: 0x0400737C RID: 29564
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bHasHoverTarget;

	// Token: 0x0400737D RID: 29565
	public static bool bSnapCursor;

	// Token: 0x0400737E RID: 29566
	public static float regularSpeed = 1f;

	// Token: 0x0400737F RID: 29567
	public static float hoverSpeed = 1f;

	// Token: 0x04007383 RID: 29571
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public AnimationCurve accelerationCurve;

	// Token: 0x04007384 RID: 29572
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool _locked;

	// Token: 0x04007385 RID: 29573
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool _virtualCursorHidden;

	// Token: 0x04007386 RID: 29574
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static bool FreeCursorEnabled = true;

	// Token: 0x020012FF RID: 4863
	public enum InputType
	{
		// Token: 0x04007388 RID: 29576
		Controller,
		// Token: 0x04007389 RID: 29577
		Mouse,
		// Token: 0x0400738A RID: 29578
		Both
	}

	// Token: 0x02001300 RID: 4864
	public enum ECursorType
	{
		// Token: 0x0400738C RID: 29580
		None,
		// Token: 0x0400738D RID: 29581
		Default,
		// Token: 0x0400738E RID: 29582
		Map,
		// Token: 0x0400738F RID: 29583
		Count
	}
}
