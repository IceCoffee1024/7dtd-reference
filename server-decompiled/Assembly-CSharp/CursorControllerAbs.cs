using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x020012EC RID: 4844
public abstract class CursorControllerAbs : MonoBehaviour, IGamePrefsChangedListener
{
	// Token: 0x17001250 RID: 4688
	// (get) Token: 0x0600992B RID: 39211 RVA: 0x003A3283 File Offset: 0x003A1483
	public static bool PrefabReady
	{
		get
		{
			return CursorControllerAbs.softCursorPrefab != null;
		}
	}

	// Token: 0x17001251 RID: 4689
	// (get) Token: 0x0600992C RID: 39212 RVA: 0x003A3290 File Offset: 0x003A1490
	// (set) Token: 0x0600992D RID: 39213 RVA: 0x003A3298 File Offset: 0x003A1498
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

	// Token: 0x17001252 RID: 4690
	// (get) Token: 0x0600992E RID: 39214 RVA: 0x003A32AB File Offset: 0x003A14AB
	// (set) Token: 0x0600992F RID: 39215 RVA: 0x003A32B3 File Offset: 0x003A14B3
	public XUiView navigationTarget
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17001253 RID: 4691
	// (get) Token: 0x06009930 RID: 39216 RVA: 0x003A32BC File Offset: 0x003A14BC
	// (set) Token: 0x06009931 RID: 39217 RVA: 0x003A32C4 File Offset: 0x003A14C4
	public XUiView navigationTargetLater
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17001254 RID: 4692
	// (get) Token: 0x06009932 RID: 39218 RVA: 0x003A32CD File Offset: 0x003A14CD
	// (set) Token: 0x06009933 RID: 39219 RVA: 0x003A32D5 File Offset: 0x003A14D5
	public XUiView lockNavigationToView
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17001255 RID: 4693
	// (get) Token: 0x06009934 RID: 39220 RVA: 0x003A32DE File Offset: 0x003A14DE
	// (set) Token: 0x06009935 RID: 39221 RVA: 0x003A32E6 File Offset: 0x003A14E6
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

	// Token: 0x17001256 RID: 4694
	// (get) Token: 0x06009936 RID: 39222 RVA: 0x003A32EF File Offset: 0x003A14EF
	// (set) Token: 0x06009937 RID: 39223 RVA: 0x003A32F7 File Offset: 0x003A14F7
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

	// Token: 0x17001257 RID: 4695
	// (get) Token: 0x06009938 RID: 39224 RVA: 0x003A330F File Offset: 0x003A150F
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

	// Token: 0x17001258 RID: 4696
	// (get) Token: 0x06009939 RID: 39225 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CursorModeActive
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600993A RID: 39226
	public abstract Vector2 GetScreenPosition();

	// Token: 0x0600993B RID: 39227
	public abstract Vector2 GetLocalScreenPosition();

	// Token: 0x0600993C RID: 39228
	public abstract void SetScreenPosition(Vector2 _newPosition);

	// Token: 0x0600993D RID: 39229
	public abstract void SetScreenPosition(float _x, float _y);

	// Token: 0x0600993E RID: 39230
	public abstract void SetNavigationTarget(XUiView _view);

	// Token: 0x0600993F RID: 39231
	public abstract void SetNavigationTargetLater(XUiView _view);

	// Token: 0x06009940 RID: 39232
	public abstract void ResetNavigationTarget();

	// Token: 0x06009941 RID: 39233
	public abstract void ResetToCenter();

	// Token: 0x06009942 RID: 39234
	public abstract void SetNavigationLockView(XUiView _view, XUiView _viewToSelect = null);

	// Token: 0x06009943 RID: 39235
	public abstract bool IsWithinNavigationLockView(XUiView _view);

	// Token: 0x06009944 RID: 39236
	public abstract void RefreshSelection();

	// Token: 0x06009945 RID: 39237
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void OnVirtualCursorVisibleChanged();

	// Token: 0x06009946 RID: 39238 RVA: 0x003A3326 File Offset: 0x003A1526
	public void SetGUIActions(PlayerActionsGUI _guiActions)
	{
		this.guiActions = _guiActions;
	}

	// Token: 0x06009947 RID: 39239 RVA: 0x003A332F File Offset: 0x003A152F
	public void SetWindowManager(GUIWindowManager _windowManager)
	{
		this.windowManager = _windowManager;
	}

	// Token: 0x06009948 RID: 39240 RVA: 0x003A3338 File Offset: 0x003A1538
	public static void UpdateGamePrefs()
	{
		CursorControllerAbs.bSnapCursor = GamePrefs.GetBool(EnumGamePrefs.OptionsControllerCursorSnap);
		CursorControllerAbs.regularSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		CursorControllerAbs.hoverSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsControllerCursorHoverSensitivity);
	}

	// Token: 0x06009949 RID: 39241 RVA: 0x003A3367 File Offset: 0x003A1567
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AwakeBase()
	{
		GamePrefs.AddChangeListener(this);
		GameOptionsManager.ResolutionChanged += this.OnResolutionChanged;
		CursorControllerAbs.UpdateGamePrefs();
		CursorControllerAbs.softCursors.Add(this);
	}

	// Token: 0x0600994A RID: 39242 RVA: 0x003A3390 File Offset: 0x003A1590
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DestroyBase()
	{
		GamePrefs.RemoveChangeListener(this);
		GameOptionsManager.ResolutionChanged -= this.OnResolutionChanged;
		CursorControllerAbs.softCursors.Remove(this);
	}

	// Token: 0x0600994B RID: 39243 RVA: 0x003A33B8 File Offset: 0x003A15B8
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

	// Token: 0x0600994C RID: 39244
	public abstract void UpdateMoveSpeed();

	// Token: 0x0600994D RID: 39245 RVA: 0x003A3462 File Offset: 0x003A1662
	public void UpdateBounds(string _boundsName, Bounds _bounds)
	{
		_bounds.Expand(this.cursorBuffer);
		this.activeBounds[_boundsName] = _bounds;
		this.RefreshBounds();
	}

	// Token: 0x0600994E RID: 39246 RVA: 0x003A3484 File Offset: 0x003A1684
	public void RemoveBounds(string _boundsName)
	{
		this.activeBounds.Remove(_boundsName);
		this.RefreshBounds();
	}

	// Token: 0x0600994F RID: 39247 RVA: 0x003A349C File Offset: 0x003A169C
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

	// Token: 0x06009950 RID: 39248 RVA: 0x003A3588 File Offset: 0x003A1788
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnResolutionChanged(int _width, int _height)
	{
		base.StartCoroutine(this.RefreshBoundsNextFrame());
	}

	// Token: 0x06009951 RID: 39249 RVA: 0x003A3597 File Offset: 0x003A1797
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

	// Token: 0x06009952 RID: 39250 RVA: 0x003A35D0 File Offset: 0x003A17D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator RefreshBoundsNextFrame()
	{
		yield return null;
		this.RefreshBounds();
		yield break;
	}

	// Token: 0x06009953 RID: 39251
	public abstract void SetCursorHidden(bool _hidden);

	// Token: 0x06009954 RID: 39252
	public abstract bool GetCursorHidden();

	// Token: 0x06009955 RID: 39253 RVA: 0x003A35E0 File Offset: 0x003A17E0
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

	// Token: 0x06009956 RID: 39254 RVA: 0x003A3670 File Offset: 0x003A1870
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

	// Token: 0x06009957 RID: 39255 RVA: 0x003A3700 File Offset: 0x003A1900
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

	// Token: 0x06009958 RID: 39256 RVA: 0x003A3788 File Offset: 0x003A1988
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

	// Token: 0x06009959 RID: 39257 RVA: 0x003A383B File Offset: 0x003A1A3B
	public static void SetCursor(CursorControllerAbs.ECursorType _cursorType)
	{
		SoftCursor.SetCursor(_cursorType);
	}

	// Token: 0x0600995A RID: 39258 RVA: 0x003A3843 File Offset: 0x003A1A43
	public static void LoadStaticData(LoadManager.LoadGroup _loadGroup)
	{
		LoadManager.LoadAssetFromResources<GameObject>(CursorControllerAbs.softCursorPrefabPath, delegate(GameObject _asset)
		{
			CursorControllerAbs.softCursorPrefab = _asset;
		}, null, false, true);
	}

	// Token: 0x0600995B RID: 39259 RVA: 0x003A3874 File Offset: 0x003A1A74
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

	// Token: 0x0600995C RID: 39260 RVA: 0x003A3907 File Offset: 0x003A1B07
	public void PlayPagingSound()
	{
		Manager.PlayXUiSound(this.pagingSound, 1f);
	}

	// Token: 0x0600995D RID: 39261 RVA: 0x003A3919 File Offset: 0x003A1B19
	[PublicizedFrom(EAccessModifier.Protected)]
	public CursorControllerAbs()
	{
	}

	// Token: 0x0400732D RID: 29485
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string softCursorPrefabPath = "Prefabs/SoftCursor";

	// Token: 0x0400732E RID: 29486
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject softCursorPrefab;

	// Token: 0x0400732F RID: 29487
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<CursorControllerAbs> softCursors = new List<CursorControllerAbs>();

	// Token: 0x04007330 RID: 29488
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UICamera uiCamera;

	// Token: 0x04007331 RID: 29489
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public PlayerActionsGUI guiActions;

	// Token: 0x04007332 RID: 29490
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIWindowManager windowManager;

	// Token: 0x04007333 RID: 29491
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UISprite cursor;

	// Token: 0x04007334 RID: 29492
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Bounds cursorWorldBounds;

	// Token: 0x04007335 RID: 29493
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 cursorBuffer;

	// Token: 0x04007336 RID: 29494
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, Bounds> activeBounds = new Dictionary<string, Bounds>();

	// Token: 0x04007337 RID: 29495
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Bounds currentBounds;

	// Token: 0x04007338 RID: 29496
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip cursorSelectSound;

	// Token: 0x04007339 RID: 29497
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip pagingSound;

	// Token: 0x0400733A RID: 29498
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public XUiView hoverTarget;

	// Token: 0x0400733B RID: 29499
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bHasHoverTarget;

	// Token: 0x0400733C RID: 29500
	public static bool bSnapCursor;

	// Token: 0x0400733D RID: 29501
	public static float regularSpeed = 1f;

	// Token: 0x0400733E RID: 29502
	public static float hoverSpeed = 1f;

	// Token: 0x04007342 RID: 29506
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public AnimationCurve accelerationCurve;

	// Token: 0x04007343 RID: 29507
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool _locked;

	// Token: 0x04007344 RID: 29508
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool _virtualCursorHidden;

	// Token: 0x04007345 RID: 29509
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static bool FreeCursorEnabled = true;

	// Token: 0x020012ED RID: 4845
	public enum InputType
	{
		// Token: 0x04007347 RID: 29511
		Controller,
		// Token: 0x04007348 RID: 29512
		Mouse,
		// Token: 0x04007349 RID: 29513
		Both
	}

	// Token: 0x020012EE RID: 4846
	public enum ECursorType
	{
		// Token: 0x0400734B RID: 29515
		None,
		// Token: 0x0400734C RID: 29516
		Default,
		// Token: 0x0400734D RID: 29517
		Map,
		// Token: 0x0400734E RID: 29518
		Count
	}
}
