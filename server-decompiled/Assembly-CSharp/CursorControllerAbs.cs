using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x020012B2 RID: 4786
public abstract class CursorControllerAbs : MonoBehaviour, IGamePrefsChangedListener
{
	// Token: 0x170011D2 RID: 4562
	// (get) Token: 0x06009784 RID: 38788 RVA: 0x0039948F File Offset: 0x0039768F
	public static bool PrefabReady
	{
		get
		{
			return CursorControllerAbs.softCursorPrefab != null;
		}
	}

	// Token: 0x170011D3 RID: 4563
	// (get) Token: 0x06009785 RID: 38789 RVA: 0x0039949C File Offset: 0x0039769C
	// (set) Token: 0x06009786 RID: 38790 RVA: 0x003994A4 File Offset: 0x003976A4
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

	// Token: 0x170011D4 RID: 4564
	// (get) Token: 0x06009787 RID: 38791 RVA: 0x003994B7 File Offset: 0x003976B7
	// (set) Token: 0x06009788 RID: 38792 RVA: 0x003994BF File Offset: 0x003976BF
	public XUiView navigationTarget
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x170011D5 RID: 4565
	// (get) Token: 0x06009789 RID: 38793 RVA: 0x003994C8 File Offset: 0x003976C8
	// (set) Token: 0x0600978A RID: 38794 RVA: 0x003994D0 File Offset: 0x003976D0
	public XUiView navigationTargetLater
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x170011D6 RID: 4566
	// (get) Token: 0x0600978B RID: 38795 RVA: 0x003994D9 File Offset: 0x003976D9
	// (set) Token: 0x0600978C RID: 38796 RVA: 0x003994E1 File Offset: 0x003976E1
	public XUiView lockNavigationToView
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x170011D7 RID: 4567
	// (get) Token: 0x0600978D RID: 38797 RVA: 0x003994EA File Offset: 0x003976EA
	// (set) Token: 0x0600978E RID: 38798 RVA: 0x003994F2 File Offset: 0x003976F2
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

	// Token: 0x170011D8 RID: 4568
	// (get) Token: 0x0600978F RID: 38799 RVA: 0x003994FB File Offset: 0x003976FB
	// (set) Token: 0x06009790 RID: 38800 RVA: 0x00399503 File Offset: 0x00397703
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

	// Token: 0x170011D9 RID: 4569
	// (get) Token: 0x06009791 RID: 38801 RVA: 0x0039951B File Offset: 0x0039771B
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

	// Token: 0x170011DA RID: 4570
	// (get) Token: 0x06009792 RID: 38802 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CursorModeActive
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06009793 RID: 38803
	public abstract Vector2 GetScreenPosition();

	// Token: 0x06009794 RID: 38804
	public abstract Vector2 GetLocalScreenPosition();

	// Token: 0x06009795 RID: 38805
	public abstract void SetScreenPosition(Vector2 _newPosition);

	// Token: 0x06009796 RID: 38806
	public abstract void SetScreenPosition(float _x, float _y);

	// Token: 0x06009797 RID: 38807
	public abstract void SetNavigationTarget(XUiView _view);

	// Token: 0x06009798 RID: 38808
	public abstract void SetNavigationTargetLater(XUiView _view);

	// Token: 0x06009799 RID: 38809
	public abstract void ResetNavigationTarget();

	// Token: 0x0600979A RID: 38810
	public abstract void ResetToCenter();

	// Token: 0x0600979B RID: 38811
	public abstract void SetNavigationLockView(XUiView _view, XUiView _viewToSelect = null);

	// Token: 0x0600979C RID: 38812
	public abstract bool IsWithinNavigationLockView(XUiView _view);

	// Token: 0x0600979D RID: 38813
	public abstract void RefreshSelection();

	// Token: 0x0600979E RID: 38814
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void OnVirtualCursorVisibleChanged();

	// Token: 0x0600979F RID: 38815 RVA: 0x00399532 File Offset: 0x00397732
	public void SetGUIActions(PlayerActionsGUI _guiActions)
	{
		this.guiActions = _guiActions;
	}

	// Token: 0x060097A0 RID: 38816 RVA: 0x0039953B File Offset: 0x0039773B
	public void SetWindowManager(GUIWindowManager _windowManager)
	{
		this.windowManager = _windowManager;
	}

	// Token: 0x060097A1 RID: 38817 RVA: 0x00399544 File Offset: 0x00397744
	public static void UpdateGamePrefs()
	{
		CursorControllerAbs.bSnapCursor = GamePrefs.GetBool(EnumGamePrefs.OptionsControllerCursorSnap);
		CursorControllerAbs.regularSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		CursorControllerAbs.hoverSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsControllerCursorHoverSensitivity);
	}

	// Token: 0x060097A2 RID: 38818 RVA: 0x00399573 File Offset: 0x00397773
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AwakeBase()
	{
		GamePrefs.AddChangeListener(this);
		GameOptionsManager.ResolutionChanged += this.OnResolutionChanged;
		CursorControllerAbs.UpdateGamePrefs();
		CursorControllerAbs.softCursors.Add(this);
	}

	// Token: 0x060097A3 RID: 38819 RVA: 0x0039959C File Offset: 0x0039779C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DestroyBase()
	{
		GamePrefs.RemoveChangeListener(this);
		GameOptionsManager.ResolutionChanged -= this.OnResolutionChanged;
		CursorControllerAbs.softCursors.Remove(this);
	}

	// Token: 0x060097A4 RID: 38820 RVA: 0x003995C4 File Offset: 0x003977C4
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

	// Token: 0x060097A5 RID: 38821
	public abstract void UpdateMoveSpeed();

	// Token: 0x060097A6 RID: 38822 RVA: 0x0039966E File Offset: 0x0039786E
	public void UpdateBounds(string _boundsName, Bounds _bounds)
	{
		_bounds.Expand(this.cursorBuffer);
		this.activeBounds[_boundsName] = _bounds;
		this.RefreshBounds();
	}

	// Token: 0x060097A7 RID: 38823 RVA: 0x00399690 File Offset: 0x00397890
	public void RemoveBounds(string _boundsName)
	{
		this.activeBounds.Remove(_boundsName);
		this.RefreshBounds();
	}

	// Token: 0x060097A8 RID: 38824 RVA: 0x003996A8 File Offset: 0x003978A8
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

	// Token: 0x060097A9 RID: 38825 RVA: 0x00399794 File Offset: 0x00397994
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnResolutionChanged(int _width, int _height)
	{
		base.StartCoroutine(this.RefreshBoundsNextFrame());
	}

	// Token: 0x060097AA RID: 38826 RVA: 0x003997A3 File Offset: 0x003979A3
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

	// Token: 0x060097AB RID: 38827 RVA: 0x003997DC File Offset: 0x003979DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator RefreshBoundsNextFrame()
	{
		yield return null;
		this.RefreshBounds();
		yield break;
	}

	// Token: 0x060097AC RID: 38828
	public abstract void SetCursorHidden(bool _hidden);

	// Token: 0x060097AD RID: 38829
	public abstract bool GetCursorHidden();

	// Token: 0x060097AE RID: 38830 RVA: 0x003997EC File Offset: 0x003979EC
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

	// Token: 0x060097AF RID: 38831 RVA: 0x0039987C File Offset: 0x00397A7C
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

	// Token: 0x060097B0 RID: 38832 RVA: 0x0039990C File Offset: 0x00397B0C
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

	// Token: 0x060097B1 RID: 38833 RVA: 0x00399994 File Offset: 0x00397B94
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

	// Token: 0x060097B2 RID: 38834 RVA: 0x00399A47 File Offset: 0x00397C47
	public static void SetCursor(CursorControllerAbs.ECursorType _cursorType)
	{
		SoftCursor.SetCursor(_cursorType);
	}

	// Token: 0x060097B3 RID: 38835 RVA: 0x00399A4F File Offset: 0x00397C4F
	public static void LoadStaticData(LoadManager.LoadGroup _loadGroup)
	{
		LoadManager.LoadAssetFromResources<GameObject>(CursorControllerAbs.softCursorPrefabPath, delegate(GameObject _asset)
		{
			CursorControllerAbs.softCursorPrefab = _asset;
		}, null, false, true);
	}

	// Token: 0x060097B4 RID: 38836 RVA: 0x00399A80 File Offset: 0x00397C80
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

	// Token: 0x060097B5 RID: 38837 RVA: 0x00399B13 File Offset: 0x00397D13
	public void PlayPagingSound()
	{
		Manager.PlayXUiSound(this.pagingSound, 1f);
	}

	// Token: 0x060097B6 RID: 38838 RVA: 0x00399B25 File Offset: 0x00397D25
	[PublicizedFrom(EAccessModifier.Protected)]
	public CursorControllerAbs()
	{
	}

	// Token: 0x040071DB RID: 29147
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string softCursorPrefabPath = "Prefabs/SoftCursor";

	// Token: 0x040071DC RID: 29148
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static GameObject softCursorPrefab;

	// Token: 0x040071DD RID: 29149
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static List<CursorControllerAbs> softCursors = new List<CursorControllerAbs>();

	// Token: 0x040071DE RID: 29150
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UICamera uiCamera;

	// Token: 0x040071DF RID: 29151
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public PlayerActionsGUI guiActions;

	// Token: 0x040071E0 RID: 29152
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GUIWindowManager windowManager;

	// Token: 0x040071E1 RID: 29153
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public UISprite cursor;

	// Token: 0x040071E2 RID: 29154
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Bounds cursorWorldBounds;

	// Token: 0x040071E3 RID: 29155
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 cursorBuffer;

	// Token: 0x040071E4 RID: 29156
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<string, Bounds> activeBounds = new Dictionary<string, Bounds>();

	// Token: 0x040071E5 RID: 29157
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Bounds currentBounds;

	// Token: 0x040071E6 RID: 29158
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip cursorSelectSound;

	// Token: 0x040071E7 RID: 29159
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioClip pagingSound;

	// Token: 0x040071E8 RID: 29160
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public XUiView hoverTarget;

	// Token: 0x040071E9 RID: 29161
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bHasHoverTarget;

	// Token: 0x040071EA RID: 29162
	public static bool bSnapCursor;

	// Token: 0x040071EB RID: 29163
	public static float regularSpeed = 1f;

	// Token: 0x040071EC RID: 29164
	public static float hoverSpeed = 1f;

	// Token: 0x040071F0 RID: 29168
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Protected)]
	public AnimationCurve accelerationCurve;

	// Token: 0x040071F1 RID: 29169
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool _locked;

	// Token: 0x040071F2 RID: 29170
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool _virtualCursorHidden;

	// Token: 0x040071F3 RID: 29171
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static bool FreeCursorEnabled = true;

	// Token: 0x020012B3 RID: 4787
	public enum InputType
	{
		// Token: 0x040071F5 RID: 29173
		Controller,
		// Token: 0x040071F6 RID: 29174
		Mouse,
		// Token: 0x040071F7 RID: 29175
		Both
	}

	// Token: 0x020012B4 RID: 4788
	public enum ECursorType
	{
		// Token: 0x040071F9 RID: 29177
		None,
		// Token: 0x040071FA RID: 29178
		Default,
		// Token: 0x040071FB RID: 29179
		Map,
		// Token: 0x040071FC RID: 29180
		Count
	}
}
