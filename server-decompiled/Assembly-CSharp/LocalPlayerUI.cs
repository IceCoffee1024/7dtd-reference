using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Platform;
using UnityEngine;

// Token: 0x02000619 RID: 1561
public class LocalPlayerUI : MonoBehaviour
{
	// Token: 0x1400003C RID: 60
	// (add) Token: 0x060032FB RID: 13051 RVA: 0x00149F28 File Offset: 0x00148128
	// (remove) Token: 0x060032FC RID: 13052 RVA: 0x00149F60 File Offset: 0x00148160
	public event Action<EntityPlayerLocal> OnEntityPlayerLocalAssigned;

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x060032FD RID: 13053 RVA: 0x00149F98 File Offset: 0x00148198
	// (remove) Token: 0x060032FE RID: 13054 RVA: 0x00149FD0 File Offset: 0x001481D0
	public event Action OnUIShutdown;

	// Token: 0x060032FF RID: 13055 RVA: 0x0014A005 File Offset: 0x00148205
	public static void QueueUIForNewPlayerEntity(LocalPlayerUI _playerUI)
	{
		if (!LocalPlayerUI.playerUIQueueForPendingEntity.Contains(_playerUI))
		{
			LocalPlayerUI.playerUIQueueForPendingEntity.Enqueue(_playerUI);
		}
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x0014A01F File Offset: 0x0014821F
	public static LocalPlayerUI DispatchNewPlayerForUI(EntityPlayerLocal _entityPlayer)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_entityPlayer);
		uiforPlayer.entityPlayer = _entityPlayer;
		return uiforPlayer;
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x0014A030 File Offset: 0x00148230
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

	// Token: 0x06003302 RID: 13058 RVA: 0x0014A06C File Offset: 0x0014826C
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

	// Token: 0x06003303 RID: 13059 RVA: 0x0014A0FC File Offset: 0x001482FC
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

	// Token: 0x06003304 RID: 13060 RVA: 0x0014A24C File Offset: 0x0014844C
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

	// Token: 0x06003305 RID: 13061 RVA: 0x0014A324 File Offset: 0x00148524
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

	// Token: 0x06003306 RID: 13062 RVA: 0x0014A360 File Offset: 0x00148560
	public void UpdateChildCameraIndices()
	{
		int num = 0;
		foreach (LocalPlayerCamera localPlayerCamera in base.GetComponentsInChildren<LocalPlayerCamera>())
		{
			localPlayerCamera.uiChildIndex = num++;
			localPlayerCamera.SetCameraDepth();
		}
	}

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x06003307 RID: 13063 RVA: 0x0014A397 File Offset: 0x00148597
	public static LocalPlayerUI primaryUI
	{
		get
		{
			return LocalPlayerUI.mPrimaryUI;
		}
	}

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06003308 RID: 13064 RVA: 0x0014A39E File Offset: 0x0014859E
	// (set) Token: 0x06003309 RID: 13065 RVA: 0x0014A3A6 File Offset: 0x001485A6
	public int playerIndex
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x0600330A RID: 13066 RVA: 0x0014A3AF File Offset: 0x001485AF
	public static int localPlayerCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return LocalPlayerUI.playerUIs.Count - 1;
		}
	}

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x0600330B RID: 13067 RVA: 0x0014A3BD File Offset: 0x001485BD
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

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x0600330C RID: 13068 RVA: 0x0014A3DA File Offset: 0x001485DA
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

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x0600330D RID: 13069 RVA: 0x0014A3F7 File Offset: 0x001485F7
	// (set) Token: 0x0600330E RID: 13070 RVA: 0x0014A3FF File Offset: 0x001485FF
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

	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x0600330F RID: 13071 RVA: 0x0014A41E File Offset: 0x0014861E
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

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06003310 RID: 13072 RVA: 0x0014A43B File Offset: 0x0014863B
	public bool isPrimaryUI
	{
		get
		{
			return this.mIsPrimaryUI;
		}
	}

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06003311 RID: 13073 RVA: 0x0014A443 File Offset: 0x00148643
	// (set) Token: 0x06003312 RID: 13074 RVA: 0x0014A44B File Offset: 0x0014864B
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

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06003313 RID: 13075 RVA: 0x0014A454 File Offset: 0x00148654
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

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06003314 RID: 13076 RVA: 0x0014A488 File Offset: 0x00148688
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

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06003315 RID: 13077 RVA: 0x0014A4BC File Offset: 0x001486BC
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

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06003316 RID: 13078 RVA: 0x0014A4F0 File Offset: 0x001486F0
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

	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06003317 RID: 13079 RVA: 0x0014A524 File Offset: 0x00148724
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

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06003318 RID: 13080 RVA: 0x0014A567 File Offset: 0x00148767
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

	// Token: 0x06003319 RID: 13081 RVA: 0x0014A58C File Offset: 0x0014878C
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

	// Token: 0x0600331A RID: 13082 RVA: 0x0014A6A7 File Offset: 0x001488A7
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

	// Token: 0x0600331B RID: 13083 RVA: 0x0014A6D8 File Offset: 0x001488D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupMenuSoftCursor()
	{
		CursorControllerAbs.AddSoftCursor(this.uiCamera, PlatformManager.NativePlatform.Input.PrimaryPlayer.GUIActions, this.windowManager);
		this.CursorController.RefreshBounds();
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x0014A70B File Offset: 0x0014890B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		LocalPlayerManager.OnLocalPlayersChanged += this.HandleLocalPlayersChanged;
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x0014A71E File Offset: 0x0014891E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		LocalPlayerManager.OnLocalPlayersChanged -= this.HandleLocalPlayersChanged;
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x0014A734 File Offset: 0x00148934
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

	// Token: 0x0600331F RID: 13087 RVA: 0x0014A77C File Offset: 0x0014897C
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

	// Token: 0x06003320 RID: 13088 RVA: 0x0014A7DC File Offset: 0x001489DC
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

	// Token: 0x06003321 RID: 13089 RVA: 0x0014B404 File Offset: 0x00149604
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

	// Token: 0x06003322 RID: 13090 RVA: 0x0014B558 File Offset: 0x00149758
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

	// Token: 0x06003323 RID: 13091 RVA: 0x0014B591 File Offset: 0x00149791
	public void RegisterItemStackGrid(XUiC_ItemStackGrid _grid)
	{
		if (this.activeItemStackGrids.Contains(_grid))
		{
			return;
		}
		this.activeItemStackGrids.Add(_grid);
		this.SortItemStackGrids();
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x0014B5B4 File Offset: 0x001497B4
	public void UnregisterItemStackGrid(XUiC_ItemStackGrid _grid)
	{
		this.activeItemStackGrids.Remove(_grid);
		this.SortItemStackGrids();
	}

	// Token: 0x06003325 RID: 13093 RVA: 0x0014B5C9 File Offset: 0x001497C9
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

	// Token: 0x06003326 RID: 13094 RVA: 0x0014B5F8 File Offset: 0x001497F8
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

	// Token: 0x04002833 RID: 10291
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static LocalPlayerUI mPrimaryUI;

	// Token: 0x04002834 RID: 10292
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static LocalPlayerUI mCleanCopy;

	// Token: 0x04002836 RID: 10294
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal mEntityPlayer;

	// Token: 0x04002837 RID: 10295
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsPrimaryUI;

	// Token: 0x04002838 RID: 10296
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsCleanCopy;

	// Token: 0x04002839 RID: 10297
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public GUIWindowManager mWindowManager;

	// Token: 0x0400283A RID: 10298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public NGUIWindowManager mNGUIWindowManager;

	// Token: 0x0400283B RID: 10299
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UICamera mUICamera;

	// Token: 0x0400283C RID: 10300
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Camera mCamera;

	// Token: 0x0400283D RID: 10301
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CursorControllerAbs mCursorController;

	// Token: 0x0400283E RID: 10302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public XUi mXUi;

	// Token: 0x0400283F RID: 10303
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<LocalPlayerUI> playerUIs = new List<LocalPlayerUI>();

	// Token: 0x04002840 RID: 10304
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Queue<LocalPlayerUI> playerUIQueueForPendingEntity = new Queue<LocalPlayerUI>();

	// Token: 0x04002841 RID: 10305
	public static readonly ReadOnlyCollection<LocalPlayerUI> PlayerUIs = new ReadOnlyCollection<LocalPlayerUI>(LocalPlayerUI.playerUIs);

	// Token: 0x04002842 RID: 10306
	public static bool CreatingCleanCopy;

	// Token: 0x04002843 RID: 10307
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navigationCandidates = new List<XUiView>();

	// Token: 0x04002844 RID: 10308
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navigationPrimeCandidates = new List<XUiView>();

	// Token: 0x04002845 RID: 10309
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navigationWrapAroundCandidates = new List<XUiView>();

	// Token: 0x04002846 RID: 10310
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navigationWrapAroundPrimeCandidates = new List<XUiView>();

	// Token: 0x04002847 RID: 10311
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiV_Window> openWindows = new List<XUiV_Window>();

	// Token: 0x04002848 RID: 10312
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static List<XUiView> navViews = new List<XUiView>();

	// Token: 0x04002849 RID: 10313
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float navigationAngleLimit = 78f;

	// Token: 0x0400284A RID: 10314
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 previousInputVector = Vector2.zero;

	// Token: 0x0400284B RID: 10315
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float inputRepeatTimer;

	// Token: 0x0400284C RID: 10316
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool initialRepeat;

	// Token: 0x0400284D RID: 10317
	public List<XUiC_ItemStackGrid> activeItemStackGrids = new List<XUiC_ItemStackGrid>();
}
