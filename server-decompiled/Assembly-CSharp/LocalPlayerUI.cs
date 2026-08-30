using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform;
using UnityEngine;

// Token: 0x02000617 RID: 1559
public class LocalPlayerUI : MonoBehaviour
{
	// Token: 0x1400003B RID: 59
	// (add) Token: 0x060032B8 RID: 12984 RVA: 0x00149584 File Offset: 0x00147784
	// (remove) Token: 0x060032B9 RID: 12985 RVA: 0x001495BC File Offset: 0x001477BC
	public event Action<EntityPlayerLocal> OnEntityPlayerLocalAssigned;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x060032BA RID: 12986 RVA: 0x001495F4 File Offset: 0x001477F4
	// (remove) Token: 0x060032BB RID: 12987 RVA: 0x0014962C File Offset: 0x0014782C
	public event Action OnUIShutdown;

	// Token: 0x060032BC RID: 12988 RVA: 0x00149661 File Offset: 0x00147861
	public static void QueueUIForNewPlayerEntity(LocalPlayerUI _playerUI)
	{
		if (!LocalPlayerUI.playerUIQueueForPendingEntity.Contains(_playerUI))
		{
			LocalPlayerUI.playerUIQueueForPendingEntity.Enqueue(_playerUI);
		}
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x0014967B File Offset: 0x0014787B
	public static LocalPlayerUI DispatchNewPlayerForUI(EntityPlayerLocal _entityPlayer)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_entityPlayer);
		uiforPlayer.entityPlayer = _entityPlayer;
		return uiforPlayer;
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x0014968C File Offset: 0x0014788C
	public static LocalPlayerUI GetUIForPrimaryPlayer()
	{
		for (int i = 0; i < LocalPlayerUI.playerUIs.Count; i++)
		{
			LocalPlayerUI localPlayerUI = LocalPlayerUI.playerUIs[i];
			if (!localPlayerUI.isPrimaryUI)
			{
				return localPlayerUI;
			}
		}
		return null;
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x001496C8 File Offset: 0x001478C8
	public static LocalPlayerUI GetUIForPlayer(EntityPlayerLocal _entityPlayer)
	{
		if (_entityPlayer == null)
		{
			return null;
		}
		LocalPlayerUI localPlayerUI = null;
		for (int i = 0; i < LocalPlayerUI.playerUIs.Count; i++)
		{
			LocalPlayerUI localPlayerUI2 = LocalPlayerUI.playerUIs[i];
			if (localPlayerUI2.entityPlayer == _entityPlayer)
			{
				return localPlayerUI2;
			}
			if (!localPlayerUI2.isPrimaryUI && localPlayerUI2.entityPlayer == null)
			{
				localPlayerUI = localPlayerUI2;
			}
		}
		while (LocalPlayerUI.playerUIQueueForPendingEntity.Count > 0)
		{
			LocalPlayerUI localPlayerUI3 = LocalPlayerUI.playerUIQueueForPendingEntity.Dequeue();
			if (!(localPlayerUI3 == null))
			{
				localPlayerUI3.mEntityPlayer = _entityPlayer;
				return localPlayerUI3;
			}
		}
		return localPlayerUI;
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x00149758 File Offset: 0x00147958
	public static LocalPlayerUI CreateUIForNewLocalPlayer()
	{
		if (!LocalPlayerUI.primaryUI)
		{
			throw new Exception("Can't create UI for new local player, primary UI not set.");
		}
		GameObject gameObject = LocalPlayerUI.mCleanCopy.gameObject;
		Transform transform = LocalPlayerUI.primaryUI.transform;
		gameObject.SetActive(false);
		GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject, LocalPlayerUI.primaryUI.transform.parent, true);
		gameObject2.transform.position = Vector3.back * (float)LocalPlayerUI.localPlayerCount * 4f;
		gameObject2.transform.localRotation = transform.localRotation;
		gameObject2.transform.localScale = transform.localScale;
		gameObject2.gameObject.name = "GUI(Player" + LocalPlayerUI.playerUIs.Count.ToString() + ")";
		MainMenuMono component = gameObject2.GetComponent<MainMenuMono>();
		if (component != null)
		{
			global::UnityEngine.Object.Destroy(component);
		}
		gameObject2.SetActive(true);
		LocalPlayerUI component2 = gameObject2.GetComponent<LocalPlayerUI>();
		component2.nguiWindowManager.Show(EnumNGUIWindow.Version, false);
		GameManager.Instance.AddWindows(component2.windowManager);
		List<string> list = null;
		bool flag = true;
		if (LocalPlayerUI.localPlayerCount > 1)
		{
			list = new List<string>(new string[] { "secondaryPlayerJoin", "CalloutGroup", "popupGroup" });
			flag = false;
		}
		XUi.Instantiate(component2).Load(list, flag);
		return component2;
	}

	// Token: 0x060032C1 RID: 12993 RVA: 0x001498A8 File Offset: 0x00147AA8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PrepareUIForSplitScreen(Transform _uiTransform)
	{
		UIRect.AnchorPoint[] array = new UIRect.AnchorPoint[4];
		UICamera componentInParent = _uiTransform.GetComponentInParent<UICamera>();
		foreach (UIRect uirect in _uiTransform.GetComponentsInChildren<UIRect>(true))
		{
			if (uirect.isAnchored)
			{
				array[0] = uirect.topAnchor;
				array[1] = uirect.leftAnchor;
				array[2] = uirect.rightAnchor;
				array[3] = uirect.bottomAnchor;
				foreach (UIRect.AnchorPoint anchorPoint in array)
				{
					if (anchorPoint != null && anchorPoint.target != null)
					{
						UIPanel component = anchorPoint.target.gameObject.GetComponent<UIPanel>();
						if (component != null && component.clipping == UIDrawCall.Clipping.None)
						{
							anchorPoint.target = componentInParent.transform;
						}
					}
				}
				uirect.UpdateAnchors();
			}
		}
	}

	// Token: 0x060032C2 RID: 12994 RVA: 0x00149980 File Offset: 0x00147B80
	public static bool AnyModalWindowOpen()
	{
		for (int i = 0; i < LocalPlayerUI.playerUIs.Count; i++)
		{
			if (LocalPlayerUI.playerUIs[i].windowManager.IsModalWindowOpen())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060032C3 RID: 12995 RVA: 0x001499BC File Offset: 0x00147BBC
	public void UpdateChildCameraIndices()
	{
		int num = 0;
		foreach (LocalPlayerCamera localPlayerCamera in base.GetComponentsInChildren<LocalPlayerCamera>())
		{
			localPlayerCamera.uiChildIndex = num++;
			localPlayerCamera.SetCameraDepth();
		}
	}

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x060032C4 RID: 12996 RVA: 0x001499F3 File Offset: 0x00147BF3
	public static LocalPlayerUI primaryUI
	{
		get
		{
			return LocalPlayerUI.mPrimaryUI;
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x060032C5 RID: 12997 RVA: 0x001499FA File Offset: 0x00147BFA
	// (set) Token: 0x060032C6 RID: 12998 RVA: 0x00149A02 File Offset: 0x00147C02
	public int playerIndex
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x060032C7 RID: 12999 RVA: 0x00149A0B File Offset: 0x00147C0B
	public static int localPlayerCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return LocalPlayerUI.playerUIs.Count - 1;
		}
	}

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x060032C8 RID: 13000 RVA: 0x00149A19 File Offset: 0x00147C19
	public PlayerActionsLocal playerInput
	{
		get
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (nativePlatform == null)
			{
				return null;
			}
			PlayerInputManager input = nativePlatform.Input;
			if (input == null)
			{
				return null;
			}
			return input.PrimaryPlayer;
		}
	}

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x060032C9 RID: 13001 RVA: 0x00149A36 File Offset: 0x00147C36
	public ActionSetManager ActionSetManager
	{
		get
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (nativePlatform == null)
			{
				return null;
			}
			PlayerInputManager input = nativePlatform.Input;
			if (input == null)
			{
				return null;
			}
			return input.ActionSetManager;
		}
	}

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x060032CA RID: 13002 RVA: 0x00149A53 File Offset: 0x00147C53
	// (set) Token: 0x060032CB RID: 13003 RVA: 0x00149A5B File Offset: 0x00147C5B
	public EntityPlayerLocal entityPlayer
	{
		get
		{
			return this.mEntityPlayer;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.mEntityPlayer = value;
			Action<EntityPlayerLocal> onEntityPlayerLocalAssigned = this.OnEntityPlayerLocalAssigned;
			if (onEntityPlayerLocalAssigned == null)
			{
				return;
			}
			onEntityPlayerLocalAssigned(this.mEntityPlayer);
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x060032CC RID: 13004 RVA: 0x00149A7A File Offset: 0x00147C7A
	public LocalPlayer localPlayer
	{
		get
		{
			if (this.entityPlayer != null)
			{
				return this.entityPlayer.GetComponent<LocalPlayer>();
			}
			return null;
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x060032CD RID: 13005 RVA: 0x00149A97 File Offset: 0x00147C97
	public bool isPrimaryUI
	{
		get
		{
			return this.mIsPrimaryUI;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x060032CE RID: 13006 RVA: 0x00149A9F File Offset: 0x00147C9F
	// (set) Token: 0x060032CF RID: 13007 RVA: 0x00149AA7 File Offset: 0x00147CA7
	public bool IsCleanCopy
	{
		get
		{
			return this.mIsCleanCopy;
		}
		set
		{
			this.mIsCleanCopy = value;
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x00149AB0 File Offset: 0x00147CB0
	public GUIWindowManager windowManager
	{
		get
		{
			if (!(this.mWindowManager != null))
			{
				return this.mWindowManager = base.GetComponent<GUIWindowManager>();
			}
			return this.mWindowManager;
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x060032D1 RID: 13009 RVA: 0x00149AE4 File Offset: 0x00147CE4
	public NGUIWindowManager nguiWindowManager
	{
		get
		{
			if (!(this.mNGUIWindowManager != null))
			{
				return this.mNGUIWindowManager = base.GetComponent<NGUIWindowManager>();
			}
			return this.mNGUIWindowManager;
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x060032D2 RID: 13010 RVA: 0x00149B18 File Offset: 0x00147D18
	public UICamera uiCamera
	{
		get
		{
			if (!(this.mUICamera != null))
			{
				return this.mUICamera = base.GetComponentInParent<UICamera>();
			}
			return this.mUICamera;
		}
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x060032D3 RID: 13011 RVA: 0x00149B4C File Offset: 0x00147D4C
	public Camera camera
	{
		get
		{
			if (!(this.mCamera != null))
			{
				return this.mCamera = base.GetComponentInParent<Camera>();
			}
			return this.mCamera;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x060032D4 RID: 13012 RVA: 0x00149B80 File Offset: 0x00147D80
	public CursorControllerAbs CursorController
	{
		get
		{
			if (this.mCursorController != null)
			{
				return this.mCursorController;
			}
			UICamera uiCamera = this.uiCamera;
			if (uiCamera == null)
			{
				return null;
			}
			return this.mCursorController = uiCamera.GetComponentInChildren<CursorControllerAbs>();
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x060032D5 RID: 13013 RVA: 0x00149BC3 File Offset: 0x00147DC3
	public XUi xui
	{
		get
		{
			if (this.mXUi == null)
			{
				this.mXUi = base.GetComponentInChildren<XUi>();
			}
			return this.mXUi;
		}
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x00149BE8 File Offset: 0x00147DE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		LocalPlayerUI.playerUIs.Add(this);
		this.mIsPrimaryUI = LocalPlayerUI.playerUIs.Count == 1;
		if (this.mIsPrimaryUI)
		{
			LocalPlayerUI.mPrimaryUI = this;
			base.gameObject.name = "GUI(Menu)";
			LocalPlayerUI.PrepareUIForSplitScreen(this.uiCamera.transform);
			LocalPlayerUI.CreatingCleanCopy = true;
			LocalPlayerUI.mCleanCopy = global::UnityEngine.Object.Instantiate<LocalPlayerUI>(this, LocalPlayerUI.primaryUI.transform.parent, true);
			LocalPlayerUI.mCleanCopy.name = "GUI(CleanCopy)";
			LocalPlayerUI.mCleanCopy.gameObject.SetActive(false);
			LocalPlayerUI.mCleanCopy.IsCleanCopy = true;
			GameManager.Instance.AddWindows(LocalPlayerUI.mCleanCopy.windowManager);
			LocalPlayerUI.mCleanCopy.nguiWindowManager.ShowAll(false);
			MainMenuMono component = LocalPlayerUI.mCleanCopy.GetComponent<MainMenuMono>();
			if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
			LocalPlayerUI.playerUIs.Remove(LocalPlayerUI.mCleanCopy);
			LocalPlayerUI.CreatingCleanCopy = false;
			return;
		}
		Camera[] componentsInParent = base.GetComponentsInParent<Camera>();
		for (int i = 0; i < componentsInParent.Length; i++)
		{
			LocalPlayerCamera.AddToCamera(componentsInParent[i], LocalPlayerCamera.CameraType.UI);
		}
	}

	// Token: 0x060032D7 RID: 13015 RVA: 0x00149D03 File Offset: 0x00147F03
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.DispatchLocalPlayersChanged();
		if (!this.mIsPrimaryUI || GameManager.IsDedicatedServer)
		{
			return;
		}
		XUi.SetXmlsForUi(true);
		XUi.Instantiate(this).Load(null, true);
		this.SetupMenuSoftCursor();
	}

	// Token: 0x060032D8 RID: 13016 RVA: 0x00149D34 File Offset: 0x00147F34
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupMenuSoftCursor()
	{
		CursorControllerAbs.AddSoftCursor(this.uiCamera, PlatformManager.NativePlatform.Input.PrimaryPlayer.GUIActions, this.windowManager);
		this.CursorController.RefreshBounds();
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x00149D67 File Offset: 0x00147F67
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		LocalPlayerManager.OnLocalPlayersChanged += this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x00149D7A File Offset: 0x00147F7A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x00149D90 File Offset: 0x00147F90
	[PublicizedFrom(EAccessModifier.Private)]
	public void DispatchLocalPlayersChanged()
	{
		int num = 0;
		for (int i = 0; i < LocalPlayerUI.playerUIs.Count; i++)
		{
			LocalPlayerUI localPlayerUI = LocalPlayerUI.playerUIs[i];
			if (!localPlayerUI.isPrimaryUI)
			{
				localPlayerUI.playerIndex = num++;
			}
		}
		LocalPlayerManager.LocalPlayersChanged();
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x00149DD8 File Offset: 0x00147FD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleLocalPlayersChanged()
	{
		if (this.isPrimaryUI)
		{
			AudioListener componentInParent = base.GetComponentInParent<AudioListener>();
			if (componentInParent != null)
			{
				componentInParent.enabled = !(GameManager.Instance != null) || GameManager.Instance.World == null || !(GameManager.Instance.World.GetPrimaryPlayer() != null);
			}
		}
	}

	// Token: 0x060032DD RID: 13021 RVA: 0x00149E38 File Offset: 0x00148038
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (this.isPrimaryUI && GameManager.Instance.triggerEffectManager != null)
		{
			bool flag = this.windowManager.IsModalWindowOpen();
			if (!flag)
			{
				using (IEnumerator<LocalPlayerUI> enumerator = LocalPlayerUI.PlayerUIs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.windowManager.IsModalWindowOpen())
						{
							flag = true;
							break;
						}
					}
				}
			}
			GameManager.Instance.triggerEffectManager.inUI = flag;
		}
		if (this.isPrimaryUI && !this.windowManager.IsModalWindowOpen() && this.xui.CalloutWindow != null)
		{
			this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		}
		if (GUIWindowConsole.IsOpen())
		{
			return;
		}
		if (!this.isPrimaryUI && LocalPlayerUI.primaryUI.windowManager.IsModalWindowOpen())
		{
			return;
		}
		PlayerActionsLocal playerInput = this.playerInput;
		if (playerInput == null)
		{
			return;
		}
		if (playerInput.GUIActions.Inspect.IsPressed)
		{
			return;
		}
		if (this.xui.playerUI.CursorController.Locked)
		{
			return;
		}
		if (this.xui.playerUI.CursorController.lockNavigationToView != null && this.xui.playerUI.CursorController.lockNavigationToView.xui.playerUI != this)
		{
			return;
		}
		if (this.playerInput.GUIActions.BackButton.WasPressed && this.TryItemStackGridNavigation())
		{
			return;
		}
		Vector2 vector = playerInput.GUIActions.Nav.Vector;
		if (vector == Vector2.zero)
		{
			this.inputRepeatTimer = 0f;
			this.previousInputVector = Vector2.zero;
			return;
		}
		if (this.previousInputVector != Vector2.zero)
		{
			this.inputRepeatTimer -= Time.unscaledDeltaTime;
			if (this.inputRepeatTimer > 0f)
			{
				return;
			}
		}
		else
		{
			this.initialRepeat = true;
		}
		XUiView navigationTarget = this.CursorController.navigationTarget;
		this.previousInputVector = vector;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		if (Mathf.Abs(vector.y) > Mathf.Abs(vector.x))
		{
			flag2 = vector.y > 0f;
			flag3 = vector.y < 0f;
		}
		else
		{
			flag5 = vector.x > 0f;
			flag4 = vector.x < 0f;
		}
		LocalPlayerUI.openWindows.Clear();
		LocalPlayerUI.navViews.Clear();
		if (this.CursorController.lockNavigationToView != null)
		{
			this.CursorController.lockNavigationToView.Controller.FindNavigatableChildren(LocalPlayerUI.navViews);
		}
		else
		{
			this.xui.GetOpenWindows(LocalPlayerUI.openWindows);
			foreach (XUiV_Window xuiV_Window in LocalPlayerUI.openWindows)
			{
				xuiV_Window.Controller.FindNavigatableChildren(LocalPlayerUI.navViews);
			}
		}
		if (LocalPlayerUI.navViews.Count == 0)
		{
			return;
		}
		float num = float.MaxValue;
		XUiView xuiView = null;
		Vector2 flatPosition = ((SoftCursor)this.CursorController).GetFlatPosition();
		if (navigationTarget != null && !navigationTarget.IsNavigatable)
		{
			this.CursorController.SetNavigationTarget(null);
		}
		bool flag6 = false;
		if (navigationTarget == null)
		{
			foreach (XUiView xuiView2 in LocalPlayerUI.navViews)
			{
				float sqrMagnitude = (flatPosition - xuiView2.GetClosestPoint(flatPosition)).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					xuiView = xuiView2;
				}
			}
			if (num < 3.4028235E+38f)
			{
				this.CursorController.SetNavigationTarget(xuiView);
				flag6 = true;
			}
		}
		if (!flag6)
		{
			if (flag2 && navigationTarget.NavUpTarget != null)
			{
				navigationTarget.NavUpTarget.Controller.SelectCursorElement(false, false);
				flag6 = true;
			}
			else if (flag3 && navigationTarget.NavDownTarget != null)
			{
				navigationTarget.NavDownTarget.Controller.SelectCursorElement(false, false);
				flag6 = true;
			}
			else if (flag4 && navigationTarget.NavLeftTarget != null)
			{
				navigationTarget.NavLeftTarget.Controller.SelectCursorElement(false, false);
				flag6 = true;
			}
			else if (flag5 && navigationTarget.NavRightTarget != null)
			{
				navigationTarget.NavRightTarget.Controller.SelectCursorElement(false, false);
				flag6 = true;
			}
		}
		LocalPlayerUI.navigationCandidates.Clear();
		LocalPlayerUI.navigationPrimeCandidates.Clear();
		LocalPlayerUI.navigationWrapAroundCandidates.Clear();
		LocalPlayerUI.navigationWrapAroundPrimeCandidates.Clear();
		if (!flag6)
		{
			Vector2 vector2;
			if (flag2)
			{
				vector2 = new Vector2(navigationTarget.ColliderCenter.x, (float)(-(float)Screen.height));
			}
			else if (flag3)
			{
				vector2 = new Vector2(navigationTarget.ColliderCenter.x, (float)Screen.height);
			}
			else if (flag4)
			{
				vector2 = new Vector2((float)Screen.width, navigationTarget.ColliderCenter.y);
			}
			else
			{
				vector2 = new Vector2((float)(-(float)Screen.width), navigationTarget.ColliderCenter.y);
			}
			foreach (XUiView xuiView3 in LocalPlayerUI.navViews)
			{
				if (flag2)
				{
					if (xuiView3.ColliderCenter.y > navigationTarget.ColliderCenter.y)
					{
						if (Vector3.Angle(navigationTarget.UiTransform.up, (xuiView3.ColliderCenter - navigationTarget.ColliderCenter).normalized) <= 78f)
						{
							if (Mathf.Abs(navigationTarget.ColliderCenter.x - xuiView3.ColliderCenter.x) <= Mathf.Max(navigationTarget.ColliderWidthExtent, xuiView3.ColliderWidthExtent))
							{
								LocalPlayerUI.navigationPrimeCandidates.Add(xuiView3);
							}
							else
							{
								LocalPlayerUI.navigationCandidates.Add(xuiView3);
							}
						}
					}
					else if (Mathf.Abs(navigationTarget.ColliderCenter.x - xuiView3.ColliderCenter.x) <= Mathf.Max(navigationTarget.ColliderWidthExtent, xuiView3.ColliderWidthExtent))
					{
						LocalPlayerUI.navigationWrapAroundPrimeCandidates.Add(xuiView3);
					}
					else
					{
						LocalPlayerUI.navigationWrapAroundCandidates.Add(xuiView3);
					}
				}
				else if (flag3)
				{
					if (xuiView3.ColliderCenter.y < navigationTarget.ColliderCenter.y)
					{
						if (Vector3.Angle(-navigationTarget.UiTransform.up, (xuiView3.ColliderCenter - navigationTarget.ColliderCenter).normalized) <= 78f)
						{
							if (Mathf.Abs(navigationTarget.ColliderCenter.x - xuiView3.ColliderCenter.x) <= Mathf.Max(navigationTarget.ColliderWidthExtent, xuiView3.ColliderWidthExtent))
							{
								LocalPlayerUI.navigationPrimeCandidates.Add(xuiView3);
							}
							else
							{
								LocalPlayerUI.navigationCandidates.Add(xuiView3);
							}
						}
					}
					else if (Mathf.Abs(navigationTarget.ColliderCenter.x - xuiView3.ColliderCenter.x) <= Mathf.Max(navigationTarget.ColliderWidthExtent, xuiView3.ColliderWidthExtent))
					{
						LocalPlayerUI.navigationWrapAroundPrimeCandidates.Add(xuiView3);
					}
					else
					{
						LocalPlayerUI.navigationWrapAroundCandidates.Add(xuiView3);
					}
				}
				else if (flag4)
				{
					if (xuiView3.ColliderCenter.x < navigationTarget.ColliderCenter.x)
					{
						if (Vector3.Angle(-navigationTarget.UiTransform.right, (xuiView3.ColliderCenter - navigationTarget.ColliderCenter).normalized) <= 78f)
						{
							if (Mathf.Abs(navigationTarget.ColliderCenter.y - xuiView3.ColliderCenter.y) <= Mathf.Max(navigationTarget.ColliderHeightExtent, xuiView3.ColliderHeightExtent))
							{
								LocalPlayerUI.navigationPrimeCandidates.Add(xuiView3);
							}
							else
							{
								LocalPlayerUI.navigationCandidates.Add(xuiView3);
							}
						}
					}
					else if (Mathf.Abs(navigationTarget.ColliderCenter.y - xuiView3.ColliderCenter.y) <= Mathf.Max(navigationTarget.ColliderHeightExtent, xuiView3.ColliderHeightExtent))
					{
						LocalPlayerUI.navigationWrapAroundPrimeCandidates.Add(xuiView3);
					}
					else
					{
						LocalPlayerUI.navigationWrapAroundCandidates.Add(xuiView3);
					}
				}
				else if (flag5)
				{
					if (xuiView3.ColliderCenter.x > navigationTarget.ColliderCenter.x)
					{
						if (Vector3.Angle(navigationTarget.UiTransform.right, (xuiView3.ColliderCenter - navigationTarget.ColliderCenter).normalized) <= 78f)
						{
							if (Mathf.Abs(navigationTarget.ColliderCenter.y - xuiView3.ColliderCenter.y) <= Mathf.Max(navigationTarget.ColliderHeightExtent, xuiView3.ColliderHeightExtent))
							{
								LocalPlayerUI.navigationPrimeCandidates.Add(xuiView3);
							}
							else
							{
								LocalPlayerUI.navigationCandidates.Add(xuiView3);
							}
						}
					}
					else if (Mathf.Abs(navigationTarget.ColliderCenter.y - xuiView3.ColliderCenter.y) <= Mathf.Max(navigationTarget.ColliderHeightExtent, xuiView3.ColliderHeightExtent))
					{
						LocalPlayerUI.navigationWrapAroundPrimeCandidates.Add(xuiView3);
					}
					else
					{
						LocalPlayerUI.navigationWrapAroundCandidates.Add(xuiView3);
					}
				}
			}
			if (!flag6 && LocalPlayerUI.navigationPrimeCandidates.Count > 0)
			{
				using (List<XUiView>.Enumerator enumerator3 = LocalPlayerUI.navigationPrimeCandidates.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						XUiView xuiView4 = enumerator3.Current;
						float num2;
						if (flag4 || flag5)
						{
							num2 = Mathf.Abs(navigationTarget.ColliderCenter.x - xuiView4.ColliderCenter.x);
						}
						else
						{
							num2 = Mathf.Abs(navigationTarget.ColliderCenter.y - xuiView4.ColliderCenter.y);
						}
						if (num2 < num)
						{
							num = num2;
							xuiView = xuiView4;
						}
					}
					goto IL_0B1B;
				}
			}
			if (!flag6 && LocalPlayerUI.navigationCandidates.Count > 0)
			{
				using (List<XUiView>.Enumerator enumerator3 = LocalPlayerUI.navigationCandidates.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						XUiView xuiView5 = enumerator3.Current;
						float sqrMagnitude2 = (navigationTarget.GetClosestPoint(xuiView5.ColliderCenter) - xuiView5.GetClosestPoint(navigationTarget.ColliderCenter)).sqrMagnitude;
						if (sqrMagnitude2 < num)
						{
							num = sqrMagnitude2;
							xuiView = xuiView5;
						}
					}
					goto IL_0B1B;
				}
			}
			if (!flag6 && LocalPlayerUI.navigationWrapAroundPrimeCandidates.Count > 0)
			{
				num = float.MaxValue;
				using (List<XUiView>.Enumerator enumerator3 = LocalPlayerUI.navigationWrapAroundPrimeCandidates.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						XUiView xuiView6 = enumerator3.Current;
						float num3;
						if (flag4 || flag5)
						{
							num3 = Mathf.Abs(vector2.x - xuiView6.ColliderCenter.x);
						}
						else
						{
							num3 = Mathf.Abs(vector2.y - xuiView6.ColliderCenter.y);
						}
						if (num3 < num)
						{
							num = num3;
							xuiView = xuiView6;
						}
					}
					goto IL_0B1B;
				}
			}
			if (!flag6 && LocalPlayerUI.navigationWrapAroundCandidates.Count > 0)
			{
				num = float.MaxValue;
				foreach (XUiView xuiView7 in LocalPlayerUI.navigationWrapAroundCandidates)
				{
					float sqrMagnitude3 = (vector2 - xuiView7.GetClosestPoint(vector2)).sqrMagnitude;
					if (sqrMagnitude3 < num)
					{
						num = sqrMagnitude3;
						xuiView = xuiView7;
					}
				}
			}
		}
		IL_0B1B:
		if (!flag6 && num < 3.4028235E+38f)
		{
			this.CursorController.SetNavigationTarget(xuiView);
		}
		this.inputRepeatTimer = (this.initialRepeat ? 0.35f : 0.1f);
		this.initialRepeat = false;
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x0014AA60 File Offset: 0x00148C60
	public void RefreshNavigationTarget()
	{
		LocalPlayerUI.openWindows.Clear();
		LocalPlayerUI.navViews.Clear();
		if (this.CursorController.lockNavigationToView != null)
		{
			this.CursorController.lockNavigationToView.Controller.FindNavigatableChildren(LocalPlayerUI.navViews);
		}
		else
		{
			this.xui.GetOpenWindows(LocalPlayerUI.openWindows);
			foreach (XUiV_Window xuiV_Window in LocalPlayerUI.openWindows)
			{
				xuiV_Window.Controller.FindNavigatableChildren(LocalPlayerUI.navViews);
			}
		}
		if (LocalPlayerUI.navViews.Count == 0)
		{
			return;
		}
		XUiView navigationTarget = this.CursorController.navigationTarget;
		float num = float.MaxValue;
		XUiView xuiView = null;
		Vector2 flatPosition = ((SoftCursor)this.CursorController).GetFlatPosition();
		foreach (XUiView xuiView2 in LocalPlayerUI.navViews)
		{
			float sqrMagnitude = (flatPosition - xuiView2.GetClosestPoint(flatPosition)).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				xuiView = xuiView2;
			}
		}
		if (num < 3.4028235E+38f)
		{
			this.CursorController.SetNavigationTarget(xuiView);
		}
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x0014ABB4 File Offset: 0x00148DB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		Action onUIShutdown = this.OnUIShutdown;
		if (onUIShutdown != null)
		{
			onUIShutdown();
		}
		LocalPlayerUI.playerUIs.Remove(this);
		LocalPlayerUI.navViews.Clear();
		LocalPlayerUI.openWindows.Clear();
		this.DispatchLocalPlayersChanged();
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x0014ABED File Offset: 0x00148DED
	public void RegisterItemStackGrid(XUiC_ItemStackGrid _grid)
	{
		if (this.activeItemStackGrids.Contains(_grid))
		{
			return;
		}
		this.activeItemStackGrids.Add(_grid);
		this.SortItemStackGrids();
	}

	// Token: 0x060032E1 RID: 13025 RVA: 0x0014AC10 File Offset: 0x00148E10
	public void UnregisterItemStackGrid(XUiC_ItemStackGrid _grid)
	{
		this.activeItemStackGrids.Remove(_grid);
		this.SortItemStackGrids();
	}

	// Token: 0x060032E2 RID: 13026 RVA: 0x0014AC25 File Offset: 0x00148E25
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortItemStackGrids()
	{
		this.activeItemStackGrids.Sort(delegate(XUiC_ItemStackGrid x, XUiC_ItemStackGrid y)
		{
			if (x.ViewComponent.ColliderCenter.y + x.ViewComponent.ColliderHeightExtent <= y.ViewComponent.ColliderCenter.y + y.ViewComponent.ColliderHeightExtent)
			{
				return -1;
			}
			return 1;
		});
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x0014AC54 File Offset: 0x00148E54
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryItemStackGridNavigation()
	{
		if (this.activeItemStackGrids.Count == 0)
		{
			return false;
		}
		XUiC_ItemStackGrid xuiC_ItemStackGrid = null;
		if (this.CursorController.navigationTarget != null)
		{
			foreach (XUiC_ItemStackGrid xuiC_ItemStackGrid2 in this.activeItemStackGrids)
			{
				if (this.CursorController.navigationTarget.Controller.IsChildOf(xuiC_ItemStackGrid2))
				{
					xuiC_ItemStackGrid = xuiC_ItemStackGrid2;
					break;
				}
			}
		}
		int num = 0;
		if (xuiC_ItemStackGrid != null)
		{
			num = this.activeItemStackGrids.IndexOf(xuiC_ItemStackGrid);
		}
		int num2 = num;
		if (this.activeItemStackGrids.Count == 1)
		{
			this.activeItemStackGrids[num].SelectCursorElement(true, false);
			return true;
		}
		num++;
		if (num >= this.activeItemStackGrids.Count)
		{
			num = 0;
		}
		while (num != num2)
		{
			XUiView xuiView;
			if (this.activeItemStackGrids[num].TryFindFirstNavigableView(out xuiView))
			{
				if (!this.xui.DragAndDropWindow.CurrentStack.Equals(ItemStack.Empty))
				{
					int num3 = this.activeItemStackGrids[num].FindFirstEmptySlot();
					if (num3 >= 0)
					{
						this.activeItemStackGrids[num].GetItemStackControllers()[num3].SelectCursorElement(true, true);
						return true;
					}
				}
				this.activeItemStackGrids[num].SelectCursorElement(true, true);
				return true;
			}
			num++;
			if (num >= this.activeItemStackGrids.Count)
			{
				num = 0;
			}
		}
		return false;
	}

	// Token: 0x0400283A RID: 10298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static LocalPlayerUI mPrimaryUI;

	// Token: 0x0400283B RID: 10299
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static LocalPlayerUI mCleanCopy;

	// Token: 0x0400283D RID: 10301
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal mEntityPlayer;

	// Token: 0x0400283E RID: 10302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsPrimaryUI;

	// Token: 0x0400283F RID: 10303
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsCleanCopy;

	// Token: 0x04002840 RID: 10304
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager mWindowManager;

	// Token: 0x04002841 RID: 10305
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager mNGUIWindowManager;

	// Token: 0x04002842 RID: 10306
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UICamera mUICamera;

	// Token: 0x04002843 RID: 10307
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mCamera;

	// Token: 0x04002844 RID: 10308
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CursorControllerAbs mCursorController;

	// Token: 0x04002845 RID: 10309
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public XUi mXUi;

	// Token: 0x04002846 RID: 10310
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<LocalPlayerUI> playerUIs = new List<LocalPlayerUI>();

	// Token: 0x04002847 RID: 10311
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Queue<LocalPlayerUI> playerUIQueueForPendingEntity = new Queue<LocalPlayerUI>();

	// Token: 0x04002848 RID: 10312
	public static readonly ReadOnlyCollection<LocalPlayerUI> PlayerUIs = new ReadOnlyCollection<LocalPlayerUI>(LocalPlayerUI.playerUIs);

	// Token: 0x04002849 RID: 10313
	public static bool CreatingCleanCopy;

	// Token: 0x0400284A RID: 10314
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navigationCandidates = new List<XUiView>();

	// Token: 0x0400284B RID: 10315
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navigationPrimeCandidates = new List<XUiView>();

	// Token: 0x0400284C RID: 10316
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navigationWrapAroundCandidates = new List<XUiView>();

	// Token: 0x0400284D RID: 10317
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navigationWrapAroundPrimeCandidates = new List<XUiView>();

	// Token: 0x0400284E RID: 10318
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiV_Window> openWindows = new List<XUiV_Window>();

	// Token: 0x0400284F RID: 10319
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navViews = new List<XUiView>();

	// Token: 0x04002850 RID: 10320
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float navigationAngleLimit = 78f;

	// Token: 0x04002851 RID: 10321
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 previousInputVector = Vector2.zero;

	// Token: 0x04002852 RID: 10322
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float inputRepeatTimer;

	// Token: 0x04002853 RID: 10323
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initialRepeat;

	// Token: 0x04002854 RID: 10324
	public List<XUiC_ItemStackGrid> activeItemStackGrids = new List<XUiC_ItemStackGrid>();
}
