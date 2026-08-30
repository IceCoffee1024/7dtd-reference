using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using Platform;
using UnityEngine;

// Token: 0x0200116D RID: 4461
public class XUi : MonoBehaviour
{
	// Token: 0x17001115 RID: 4373
	// (get) Token: 0x06008EBF RID: 36543 RVA: 0x0035A8C4 File Offset: 0x00358AC4
	// (set) Token: 0x06008EC0 RID: 36544 RVA: 0x0035A8CC File Offset: 0x00358ACC
	public static bool InGameMenuOpen
	{
		get
		{
			return XUi.inGameMenuOpen;
		}
		set
		{
			XUi.inGameMenuOpen = value;
			World world = GameManager.Instance.World;
			if (world == null)
			{
				return;
			}
			EntityPlayerLocal primaryPlayer = world.GetPrimaryPlayer();
			if (primaryPlayer == null)
			{
				return;
			}
			LocalPlayerUI playerUI = primaryPlayer.PlayerUI;
			if (playerUI == null)
			{
				return;
			}
			NGUIWindowManager nguiWindowManager = playerUI.nguiWindowManager;
			if (nguiWindowManager == null)
			{
				return;
			}
			GameManager.Instance.SetToolTipPause(nguiWindowManager, value);
		}
	}

	// Token: 0x06008EC1 RID: 36545 RVA: 0x0035A92D File Offset: 0x00358B2D
	public static bool IsGameRunning()
	{
		return GameManager.Instance != null && GameManager.Instance.World != null;
	}

	// Token: 0x17001116 RID: 4374
	// (get) Token: 0x06008EC2 RID: 36546 RVA: 0x0035A94B File Offset: 0x00358B4B
	// (set) Token: 0x06008EC3 RID: 36547 RVA: 0x0035A953 File Offset: 0x00358B53
	public float BackgroundGlobalOpacity
	{
		get
		{
			return this.backgroundGlobalOpacity;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.backgroundGlobalOpacity = value;
		}
	}

	// Token: 0x17001117 RID: 4375
	// (get) Token: 0x06008EC4 RID: 36548 RVA: 0x0035A95C File Offset: 0x00358B5C
	// (set) Token: 0x06008EC5 RID: 36549 RVA: 0x0035A964 File Offset: 0x00358B64
	public float ForegroundGlobalOpacity
	{
		get
		{
			return this.foregroundGlobalOpacity;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.foregroundGlobalOpacity = value;
		}
	}

	// Token: 0x17001118 RID: 4376
	// (get) Token: 0x06008EC6 RID: 36550 RVA: 0x0035A96D File Offset: 0x00358B6D
	// (set) Token: 0x06008EC7 RID: 36551 RVA: 0x0035A975 File Offset: 0x00358B75
	public XUiC_DragAndDropWindow DragAndDropWindow { get; set; }

	// Token: 0x17001119 RID: 4377
	// (get) Token: 0x06008EC8 RID: 36552 RVA: 0x0035A97E File Offset: 0x00358B7E
	// (set) Token: 0x06008EC9 RID: 36553 RVA: 0x0035A986 File Offset: 0x00358B86
	public XUiC_OnScreenIcons OnScreenIconsWindow { get; set; }

	// Token: 0x1700111A RID: 4378
	// (get) Token: 0x06008ECA RID: 36554 RVA: 0x0035A98F File Offset: 0x00358B8F
	// (set) Token: 0x06008ECB RID: 36555 RVA: 0x0035A997 File Offset: 0x00358B97
	public XUiC_ToolTip ToolTipWindow { get; set; }

	// Token: 0x1700111B RID: 4379
	// (get) Token: 0x06008ECC RID: 36556 RVA: 0x0035A9A0 File Offset: 0x00358BA0
	// (set) Token: 0x06008ECD RID: 36557 RVA: 0x0035A9A8 File Offset: 0x00358BA8
	public XUiC_PopupMenu PopupMenuWindow { get; set; }

	// Token: 0x1700111C RID: 4380
	// (get) Token: 0x06008ECE RID: 36558 RVA: 0x0035A9B1 File Offset: 0x00358BB1
	// (set) Token: 0x06008ECF RID: 36559 RVA: 0x0035A9B9 File Offset: 0x00358BB9
	public XUiC_SaveIndicator SaveIndicator { get; set; }

	// Token: 0x1700111D RID: 4381
	// (get) Token: 0x06008ED0 RID: 36560 RVA: 0x0035A9C2 File Offset: 0x00358BC2
	// (set) Token: 0x06008ED1 RID: 36561 RVA: 0x0035A9CA File Offset: 0x00358BCA
	public XUiC_WorkstationToolGrid CurrentWorkstationToolGrid { get; set; }

	// Token: 0x1700111E RID: 4382
	// (get) Token: 0x06008ED2 RID: 36562 RVA: 0x0035A9D3 File Offset: 0x00358BD3
	// (set) Token: 0x06008ED3 RID: 36563 RVA: 0x0035A9DB File Offset: 0x00358BDB
	public XUiC_WorkstationFuelGrid CurrentWorkstationFuelGrid { get; set; }

	// Token: 0x1700111F RID: 4383
	// (get) Token: 0x06008ED4 RID: 36564 RVA: 0x0035A9E4 File Offset: 0x00358BE4
	// (set) Token: 0x06008ED5 RID: 36565 RVA: 0x0035A9EC File Offset: 0x00358BEC
	public XUiC_WorkstationInputGrid CurrentWorkstationInputGrid { get; set; }

	// Token: 0x17001120 RID: 4384
	// (get) Token: 0x06008ED6 RID: 36566 RVA: 0x0035A9F5 File Offset: 0x00358BF5
	// (set) Token: 0x06008ED7 RID: 36567 RVA: 0x0035A9FD File Offset: 0x00358BFD
	public XUiC_WorkstationOutputGrid CurrentWorkstationOutputGrid { get; set; }

	// Token: 0x17001121 RID: 4385
	// (get) Token: 0x06008ED8 RID: 36568 RVA: 0x0035AA06 File Offset: 0x00358C06
	// (set) Token: 0x06008ED9 RID: 36569 RVA: 0x0035AA0E File Offset: 0x00358C0E
	public XUiC_DewCollectorModGrid CurrentCollectorModGrid { get; set; }

	// Token: 0x17001122 RID: 4386
	// (get) Token: 0x06008EDA RID: 36570 RVA: 0x0035AA17 File Offset: 0x00358C17
	// (set) Token: 0x06008EDB RID: 36571 RVA: 0x0035AA1F File Offset: 0x00358C1F
	public XUiC_CollectorFuelGrid CurrentCollectorFuelGrid { get; set; }

	// Token: 0x17001123 RID: 4387
	// (get) Token: 0x06008EDC RID: 36572 RVA: 0x0035AA28 File Offset: 0x00358C28
	// (set) Token: 0x06008EDD RID: 36573 RVA: 0x0035AA30 File Offset: 0x00358C30
	public XUiC_CollectorFuelGrid currentCollectorCatalystGrid { get; set; }

	// Token: 0x17001124 RID: 4388
	// (get) Token: 0x06008EDE RID: 36574 RVA: 0x0035AA39 File Offset: 0x00358C39
	// (set) Token: 0x06008EDF RID: 36575 RVA: 0x0035AA41 File Offset: 0x00358C41
	public XUiC_CombineGrid CurrentCombineGrid { get; set; }

	// Token: 0x17001125 RID: 4389
	// (get) Token: 0x06008EE0 RID: 36576 RVA: 0x0035AA4A File Offset: 0x00358C4A
	// (set) Token: 0x06008EE1 RID: 36577 RVA: 0x0035AA52 File Offset: 0x00358C52
	public XUiC_PowerSourceSlots CurrentPowerSourceSlots { get; set; }

	// Token: 0x17001126 RID: 4390
	// (get) Token: 0x06008EE2 RID: 36578 RVA: 0x0035AA5B File Offset: 0x00358C5B
	// (set) Token: 0x06008EE3 RID: 36579 RVA: 0x0035AA63 File Offset: 0x00358C63
	public XUiC_PowerRangedAmmoSlots CurrentPowerAmmoSlots { get; set; }

	// Token: 0x17001127 RID: 4391
	// (get) Token: 0x06008EE4 RID: 36580 RVA: 0x0035AA6C File Offset: 0x00358C6C
	// (set) Token: 0x06008EE5 RID: 36581 RVA: 0x0035AA74 File Offset: 0x00358C74
	public XUiC_SelectableEntry CurrentSelectedEntry { get; set; }

	// Token: 0x17001128 RID: 4392
	// (get) Token: 0x06008EE6 RID: 36582 RVA: 0x0035AA7D File Offset: 0x00358C7D
	// (set) Token: 0x06008EE7 RID: 36583 RVA: 0x0035AA85 File Offset: 0x00358C85
	public ProgressionValue SelectedSkill { get; set; }

	// Token: 0x17001129 RID: 4393
	// (get) Token: 0x06008EE8 RID: 36584 RVA: 0x0035AA8E File Offset: 0x00358C8E
	// (set) Token: 0x06008EE9 RID: 36585 RVA: 0x0035AA96 File Offset: 0x00358C96
	public ITileEntityLootable LootContainer { get; set; }

	// Token: 0x1700112A RID: 4394
	// (get) Token: 0x06008EEA RID: 36586 RVA: 0x0035AA9F File Offset: 0x00358C9F
	// (set) Token: 0x06008EEB RID: 36587 RVA: 0x0035AAA7 File Offset: 0x00358CA7
	public bool IsUsingItemActionEntryUse { get; set; }

	// Token: 0x1700112B RID: 4395
	// (get) Token: 0x06008EEC RID: 36588 RVA: 0x0035AAB0 File Offset: 0x00358CB0
	// (set) Token: 0x06008EED RID: 36589 RVA: 0x0035AAB8 File Offset: 0x00358CB8
	public bool IsUsingItemActionEntryPromptComplete { get; set; }

	// Token: 0x1700112C RID: 4396
	// (get) Token: 0x06008EEE RID: 36590 RVA: 0x0035AAC1 File Offset: 0x00358CC1
	// (set) Token: 0x06008EEF RID: 36591 RVA: 0x0035AAC9 File Offset: 0x00358CC9
	public bool IsReady
	{
		get
		{
			return this.mIsReady;
		}
		set
		{
			this.mIsReady = value;
			if (this.mIsReady)
			{
				Action onBuilt = this.OnBuilt;
				if (onBuilt == null)
				{
					return;
				}
				onBuilt();
			}
		}
	}

	// Token: 0x140000EF RID: 239
	// (add) Token: 0x06008EF0 RID: 36592 RVA: 0x0035AAEC File Offset: 0x00358CEC
	// (remove) Token: 0x06008EF1 RID: 36593 RVA: 0x0035AB24 File Offset: 0x00358D24
	public event Action OnShutdown;

	// Token: 0x140000F0 RID: 240
	// (add) Token: 0x06008EF2 RID: 36594 RVA: 0x0035AB5C File Offset: 0x00358D5C
	// (remove) Token: 0x06008EF3 RID: 36595 RVA: 0x0035AB94 File Offset: 0x00358D94
	public event Action OnBuilt;

	// Token: 0x1700112D RID: 4397
	// (get) Token: 0x06008EF4 RID: 36596 RVA: 0x0035ABC9 File Offset: 0x00358DC9
	public LocalPlayerUI playerUI
	{
		get
		{
			if (this.mPlayerUI == null)
			{
				this.mPlayerUI = base.GetComponentInParent<LocalPlayerUI>();
			}
			return this.mPlayerUI;
		}
	}

	// Token: 0x06008EF5 RID: 36597 RVA: 0x0035ABEC File Offset: 0x00358DEC
	public static XUi Instantiate(LocalPlayerUI _playerUI)
	{
		if (GameManager.IsDedicatedServer)
		{
			return null;
		}
		Log.Out("[XUi] Instantiating XUi from default prefab.");
		Transform transform = global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/XUi")).transform;
		transform.name = "XUi";
		transform.parent = _playerUI.transform;
		XUi.uiRoot = global::UnityEngine.Object.FindAnyObjectByType<UIRoot>();
		XUi component = transform.GetComponent<XUi>();
		component.SetScale(-1f);
		component.gameObject.SetActive(true);
		component.Init(_playerUI.isPrimaryUI);
		return component;
	}

	// Token: 0x06008EF6 RID: 36598 RVA: 0x0035AC69 File Offset: 0x00358E69
	public static void Reload(LocalPlayerUI _playerUI)
	{
		if (_playerUI.xui != null)
		{
			_playerUI.xui.Shutdown(true);
		}
		XUi.SetXmlsForUi(_playerUI.isPrimaryUI);
		XUi xui = XUi.Instantiate(_playerUI);
		xui.Load(null, false);
		xui.SetDataConnections();
	}

	// Token: 0x06008EF7 RID: 36599 RVA: 0x0035ACA4 File Offset: 0x00358EA4
	public static bool ReloadWindow(LocalPlayerUI _playerUI, string _windowGroupName)
	{
		if (_playerUI.xui == null)
		{
			Log.Error("Can not reload single window, XUi not instantiated");
			return false;
		}
		for (int i = 0; i < _playerUI.xui.WindowGroups.Count; i++)
		{
			XUiWindowGroup xuiWindowGroup = _playerUI.xui.WindowGroups[i];
			if (xuiWindowGroup.Id.EqualsCaseInsensitive(_windowGroupName))
			{
				_playerUI.windowManager.Remove(xuiWindowGroup);
				_playerUI.xui.WindowGroups.RemoveAt(i);
				xuiWindowGroup.Cleanup();
				break;
			}
		}
		XUi.SetXmlsForUi(_playerUI.isPrimaryUI);
		_playerUI.xui.Load(new List<string> { _windowGroupName }, false);
		_playerUI.xui.IsReady = true;
		return _playerUI.xui.WindowGroups.Find((XUiWindowGroup _group) => _group.Id.EqualsCaseInsensitive(_windowGroupName)) != null;
	}

	// Token: 0x06008EF8 RID: 36600 RVA: 0x0035AD90 File Offset: 0x00358F90
	public static void SetXmlsForUi(bool _menuUi)
	{
		XUiFromXml.ClearData();
		ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_Common/styles"));
		ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_Common/templates"));
		if (!_menuUi)
		{
			ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_InGame/styles"));
			ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_InGame/templates"));
			ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_InGame/windows"));
			ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_InGame/xui"));
			return;
		}
		ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_Menu/styles"));
		ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_Menu/templates"));
		ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_Menu/windows"));
		ThreadManager.RunCoroutineSync(XUi.PatchAndLoadXuiXml("XUi_Menu/xui"));
	}

	// Token: 0x06008EF9 RID: 36601 RVA: 0x0035AE3C File Offset: 0x0035903C
	public static IEnumerator PatchAndLoadXuiXml(string _relPathXuiFile)
	{
		MicroStopwatch timer = null;
		bool coroutineHadException = false;
		XmlFile xmlFile = null;
		yield return XmlPatcher.LoadAndPatchConfig(_relPathXuiFile, delegate(XmlFile _file)
		{
			xmlFile = _file;
		});
		yield return XmlPatcher.ApplyConditionalXmlBlocks(_relPathXuiFile, xmlFile, timer, XmlPatcher.EEvaluator.Host, delegate
		{
			coroutineHadException = true;
		});
		if (coroutineHadException)
		{
			yield break;
		}
		yield return XmlPatcher.ApplyConditionalXmlBlocks(_relPathXuiFile, xmlFile, timer, XmlPatcher.EEvaluator.Client, delegate
		{
			coroutineHadException = true;
		});
		if (coroutineHadException)
		{
			yield break;
		}
		yield return XUiFromXml.Load(xmlFile);
		yield break;
	}

	// Token: 0x06008EFA RID: 36602 RVA: 0x0035AE4B File Offset: 0x0035904B
	public void Awake()
	{
		this.WindowGroups = new List<XUiWindowGroup>();
		this.windowsAndGroupsByControllerType = new Dictionary<Type, List<XUiController>>();
		this.initFonts();
	}

	// Token: 0x06008EFB RID: 36603 RVA: 0x0035AE6C File Offset: 0x0035906C
	public void Init(bool _menuUi)
	{
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
		GameOptionsManager.ResolutionChanged += this.OnResolutionChanged;
		this.loadGroup = LoadManager.CreateGroup();
		this.windows = new List<XUiV_Window>();
		this.lastScreenSize = new Vector2i(this.playerUI.camera.pixelWidth, this.playerUI.camera.pixelHeight);
		if (!_menuUi)
		{
			base.gameObject.GetOrAddComponent<XUi_FallThrough>().SetXUi(this);
		}
		this.initStackPanels();
		this.initAtlases();
		if (Application.isPlaying)
		{
			this.initSounds();
		}
		this.initAnchors();
		this.updateAnchors();
		this.BackgroundGlobalOpacity = GamePrefs.GetFloat(EnumGamePrefs.OptionsBackgroundGlobalOpacity);
		this.ForegroundGlobalOpacity = GamePrefs.GetFloat(EnumGamePrefs.OptionsForegroundGlobalOpacity);
		LocalPlayerManager.OnLocalPlayersChanged += this.handleLocalPlayersChanged;
		this.Vehicle = new XUiM_Vehicle();
		this.AssembleItem = new XUiM_AssembleItem();
		this.QuestTracker = new XUiM_Quest();
		this.Recipes = new XUiM_Recipes();
		this.Trader = new XUiM_Trader();
		this.Dialog = new XUiM_Dialog();
	}

	// Token: 0x06008EFC RID: 36604 RVA: 0x0035AF88 File Offset: 0x00359188
	public void Load(List<string> _windowGroupSubset = null, bool _async = false)
	{
		if (_async)
		{
			this.asyncLoad = true;
			this.loadAsyncCoroutine = ThreadManager.StartCoroutine(this.loadAsync(_windowGroupSubset));
			return;
		}
		this.asyncLoad = false;
		if (!XUiFromXml.HasData())
		{
			Log.Error("Loading XUi synchronously failed: XMLs not set.");
			return;
		}
		ThreadManager.RunCoroutineSync(this.loadAsync(_windowGroupSubset));
	}

	// Token: 0x06008EFD RID: 36605 RVA: 0x0035AFD7 File Offset: 0x003591D7
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator loadAsync(List<string> _windowGroupSubset = null)
	{
		yield return null;
		while (!XUiFromXml.HasData())
		{
			yield return null;
		}
		MicroStopwatch msw = new MicroStopwatch();
		Log.Out("[XUi] Loading XUi " + (this.asyncLoad ? "asynchronously" : "synchronously"));
		List<string> asyncWindowGroupList = ((_windowGroupSubset != null) ? new List<string>(_windowGroupSubset) : new List<string>());
		if (_windowGroupSubset == null)
		{
			XUiFromXml.GetWindowGroupNames(out asyncWindowGroupList);
		}
		if (msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
		{
			yield return null;
			msw.ResetAndRestart();
		}
		this.accumElapsedMilliseconds = 0L;
		MicroStopwatch ms = new MicroStopwatch(true);
		foreach (string text in asyncWindowGroupList)
		{
			ms.Reset();
			ms.Start();
			XUiFromXml.LoadXui(this, text);
			this.accumElapsedMilliseconds += ms.ElapsedMilliseconds;
			if (XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
			{
				Log.Out("[XUi] Parsing window group, {0}, completed in {1} ms.", new object[] { text, ms.ElapsedMilliseconds });
			}
			if (msw.ElapsedMilliseconds > 20L)
			{
				yield return null;
				msw.ResetAndRestart();
			}
		}
		List<string>.Enumerator enumerator = default(List<string>.Enumerator);
		XUiFromXml.LoadDone(_windowGroupSubset == null);
		Log.Out("[XUi] Parsing all window groups completed in {0} ms total.", new object[] { this.accumElapsedMilliseconds });
		if (msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
		{
			yield return null;
			msw.ResetAndRestart();
		}
		this.accumElapsedMilliseconds = 0L;
		int num;
		for (int i = 0; i < this.WindowGroups.Count; i = num + 1)
		{
			XUiWindowGroup xuiWindowGroup = this.WindowGroups[i];
			ms.Reset();
			ms.Start();
			try
			{
				xuiWindowGroup.Init();
			}
			catch (Exception ex)
			{
				Log.Error("[XUi] Failed initializing window group " + xuiWindowGroup.Id);
				Log.Exception(ex);
			}
			this.accumElapsedMilliseconds += ms.ElapsedMilliseconds;
			if (XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
			{
				Log.Out("[XUi] Initialize window group, {0}, completed in {1} ms.", new object[] { xuiWindowGroup.Id, ms.ElapsedMilliseconds });
			}
			if (msw.ElapsedMilliseconds > 20L)
			{
				yield return null;
				msw.ResetAndRestart();
			}
			num = i;
		}
		Log.Out("[XUi] Initialized all window groups completed in {0} ms total.", new object[] { this.accumElapsedMilliseconds });
		while (this.loadGroup.Pending)
		{
			yield return null;
		}
		this.postLoadInit();
		this.IsReady = _windowGroupSubset == null;
		this.loadAsyncCoroutine = null;
		XUiUpdater.Add(this);
		yield break;
		yield break;
	}

	// Token: 0x06008EFE RID: 36606 RVA: 0x0035AFF0 File Offset: 0x003591F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void postLoadInit()
	{
		this.windowsAndGroupsByControllerType.Clear();
		foreach (XUiWindowGroup xuiWindowGroup in this.WindowGroups)
		{
			if (xuiWindowGroup.Initialized)
			{
				this.addWindowOrGroupControllerTypeEntry(xuiWindowGroup.Controller);
				foreach (XUiController xuiController in xuiWindowGroup.Controller.Children)
				{
					this.addWindowOrGroupControllerTypeEntry(xuiController);
				}
			}
		}
		if (WorldStaticData.LoadAllXmlsCoComplete)
		{
			XUiFromXml.ClearLoadingData();
		}
	}

	// Token: 0x06008EFF RID: 36607 RVA: 0x0035B0B0 File Offset: 0x003592B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.ResolutionChanged -= this.OnResolutionChanged;
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
		LocalPlayerManager.OnLocalPlayersChanged -= this.handleLocalPlayersChanged;
		this.Shutdown(false);
	}

	// Token: 0x06008F00 RID: 36608 RVA: 0x0035B0EC File Offset: 0x003592EC
	public void Shutdown(bool _destroyImmediate = false)
	{
		Action onShutdown = this.OnShutdown;
		if (onShutdown != null)
		{
			onShutdown();
		}
		this.Cleanup(_destroyImmediate);
	}

	// Token: 0x06008F01 RID: 36609 RVA: 0x0035B108 File Offset: 0x00359308
	public void Cleanup(bool _destroyImmediate = false)
	{
		this.CancelLoading();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			XUiWindowGroup xuiWindowGroup = this.WindowGroups[i];
			this.playerUI.windowManager.Remove(xuiWindowGroup);
			xuiWindowGroup.Cleanup();
		}
		this.WindowGroups.Clear();
		XUiUpdater.Remove(this);
		if (_destroyImmediate)
		{
			global::UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	// Token: 0x06008F02 RID: 36610 RVA: 0x0035B174 File Offset: 0x00359374
	public void CancelLoading()
	{
		if (this.loadAsyncCoroutine != null)
		{
			ThreadManager.StopCoroutine(this.loadAsyncCoroutine);
			this.loadAsyncCoroutine = null;
		}
	}

	// Token: 0x06008F03 RID: 36611 RVA: 0x0035B190 File Offset: 0x00359390
	public void StopAllVideo()
	{
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			XUi.<StopAllVideo>g__StopVideo|145_0(this.WindowGroups[i].Controller);
		}
	}

	// Token: 0x06008F04 RID: 36612 RVA: 0x0035B1CC File Offset: 0x003593CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void addWindowOrGroupControllerTypeEntry(XUiController _controller)
	{
		Type type = _controller.GetType();
		List<XUiController> list;
		if (!this.windowsAndGroupsByControllerType.TryGetValue(type, out list))
		{
			list = new List<XUiController>();
			this.windowsAndGroupsByControllerType.Add(type, list);
		}
		list.Add(_controller);
	}

	// Token: 0x06008F05 RID: 36613 RVA: 0x0035B20A File Offset: 0x0035940A
	public void AddWindow(XUiV_Window _window)
	{
		this.windows.Add(_window);
	}

	// Token: 0x06008F06 RID: 36614 RVA: 0x0035B218 File Offset: 0x00359418
	public void RemoveWindow(XUiV_Window _window)
	{
		this.windows.Remove(_window);
	}

	// Token: 0x06008F07 RID: 36615 RVA: 0x0035B228 File Offset: 0x00359428
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetDataConnections()
	{
		if (this.playerUI.entityPlayer == null)
		{
			return;
		}
		if (this.PlayerInventory == null)
		{
			this.PlayerInventory = new XUiM_PlayerInventory(this, this.playerUI.entityPlayer);
		}
		if (this.PlayerEquipment == null)
		{
			this.PlayerEquipment = new XUiM_PlayerEquipment(this, this.playerUI.entityPlayer);
		}
	}

	// Token: 0x06008F08 RID: 36616 RVA: 0x0035B288 File Offset: 0x00359488
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _obj)
	{
		if (_obj <= EnumGamePrefs.OptionsHudSize)
		{
			if (_obj == EnumGamePrefs.OptionsScreenBoundsValue)
			{
				this.SetScale(-1f);
				this.updateAnchors();
				this.RecenterWindowGroup(null, true);
				return;
			}
			if (_obj != EnumGamePrefs.OptionsHudSize)
			{
				return;
			}
			this.SetScale(-1f);
			return;
		}
		else
		{
			if (_obj == EnumGamePrefs.OptionsBackgroundGlobalOpacity)
			{
				this.BackgroundGlobalOpacity = GamePrefs.GetFloat(EnumGamePrefs.OptionsBackgroundGlobalOpacity);
				return;
			}
			if (_obj != EnumGamePrefs.OptionsForegroundGlobalOpacity)
			{
				return;
			}
			this.ForegroundGlobalOpacity = GamePrefs.GetFloat(EnumGamePrefs.OptionsForegroundGlobalOpacity);
			return;
		}
	}

	// Token: 0x06008F09 RID: 36617 RVA: 0x0035B306 File Offset: 0x00359506
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnResolutionChanged(int _arg1, int _arg2)
	{
		ThreadManager.StartCoroutine(this.delayedScaleUpdate());
	}

	// Token: 0x06008F0A RID: 36618 RVA: 0x0035B314 File Offset: 0x00359514
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator delayedScaleUpdate()
	{
		yield return null;
		this.SetScale(-1f);
		yield break;
	}

	// Token: 0x06008F0B RID: 36619 RVA: 0x0035B324 File Offset: 0x00359524
	public void SetScale(float _scale = -1f)
	{
		if (_scale > 0f)
		{
			this.xuiGlobalScaling = _scale;
		}
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsScreenBoundsValue);
		float activeUiScale = GameOptionsManager.GetActiveUiScale();
		base.transform.localScale = this.xuiGlobalScaling * @float * activeUiScale * Vector3.one;
		base.transform.localPosition = Vector3.zero;
		XUi.pixelRatioFactor = 0f;
	}

	// Token: 0x06008F0C RID: 36620 RVA: 0x0035B38A File Offset: 0x0035958A
	public float GetScale()
	{
		return this.xuiGlobalScaling;
	}

	// Token: 0x06008F0D RID: 36621 RVA: 0x0035B392 File Offset: 0x00359592
	public void LoadData<T>(string _path, Action<T> _callback) where T : global::UnityEngine.Object
	{
		LoadManager.LoadAsset<T>(_path, _callback, this.loadGroup, false, !this.asyncLoad, false);
	}

	// Token: 0x06008F0E RID: 36622 RVA: 0x0035B3B0 File Offset: 0x003595B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleLocalPlayersChanged()
	{
		this.lastScreenSize = new Vector2i(this.playerUI.camera.pixelWidth, this.playerUI.camera.pixelHeight);
		this.updateAnchors();
		this.RecenterWindowGroup(null, true);
		for (int i = 0; i < this.windows.Count; i++)
		{
			XUiV_Window xuiV_Window = this.windows[i];
			if (xuiV_Window.IsCursorArea && xuiV_Window.IsOpen)
			{
				this.UpdateWindowSoftCursorBounds(xuiV_Window);
			}
		}
	}

	// Token: 0x06008F0F RID: 36623 RVA: 0x0035B430 File Offset: 0x00359630
	public void OnUpdateDeltaTime(float _updateDeltaTime)
	{
		if (this.playerUI.entityPlayer != null)
		{
			this.SetDataConnections();
		}
		if (this.WindowGroups == null)
		{
			return;
		}
		if (this.ToolTipWindow != null)
		{
			this.playerUI.windowManager.Open(this.ToolTipWindow.WindowGroup, false);
		}
		if (this.SaveIndicator != null)
		{
			this.playerUI.windowManager.Open(this.SaveIndicator.WindowGroup, false);
		}
		if (this.lastScreenSize.x != this.playerUI.camera.pixelWidth || this.lastScreenSize.y != this.playerUI.camera.pixelHeight)
		{
			this.lastScreenSize = new Vector2i(this.playerUI.camera.pixelWidth, this.playerUI.camera.pixelHeight);
			this.updateAnchors();
			this.RecenterWindowGroup(null, true);
		}
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			XUiWindowGroup xuiWindowGroup = this.WindowGroups[i];
			if (xuiWindowGroup.Initialized && (xuiWindowGroup.isShowing || xuiWindowGroup.Controller.AlwaysUpdate))
			{
				try
				{
					xuiWindowGroup.Controller.Update(_updateDeltaTime);
				}
				catch (Exception ex)
				{
					Log.Error("[XUi] Error while updating window group '" + xuiWindowGroup.Id + "':");
					Log.Exception(ex);
				}
			}
		}
	}

	// Token: 0x06008F10 RID: 36624 RVA: 0x0035B59C File Offset: 0x0035979C
	public float GetPixelRatioFactor()
	{
		if (XUi.pixelRatioFactor == 0f || Screen.height != XUi.lastScreenHeight)
		{
			float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsScreenBoundsValue);
			float activeUiScale = GameOptionsManager.GetActiveUiScale();
			XUi.pixelRatioFactor = XUi.uiRoot.pixelSizeAdjustment / this.xuiGlobalScaling / @float / activeUiScale;
			XUi.lastScreenHeight = Screen.height;
		}
		return XUi.pixelRatioFactor;
	}

	// Token: 0x06008F11 RID: 36625 RVA: 0x0035B5FB File Offset: 0x003597FB
	public Vector2i GetXUiScreenSize()
	{
		return new Vector2i(new Vector2((float)this.playerUI.camera.pixelWidth, (float)this.playerUI.camera.pixelHeight) * this.GetPixelRatioFactor());
	}

	// Token: 0x06008F12 RID: 36626 RVA: 0x0035B634 File Offset: 0x00359834
	public Vector2i GetMouseXUiPosition()
	{
		Vector3 vector = this.playerUI.CursorController.GetLocalScreenPosition();
		return this.TranslateScreenVectorToXuiVector(vector);
	}

	// Token: 0x06008F13 RID: 36627 RVA: 0x0035B664 File Offset: 0x00359864
	public Vector2i TranslateScreenVectorToXuiVector(Vector2 _screenSpaceVector)
	{
		_screenSpaceVector.x -= (float)this.playerUI.camera.pixelWidth / 2f;
		_screenSpaceVector.y -= (float)this.playerUI.camera.pixelHeight / 2f;
		return new Vector2i(_screenSpaceVector * this.GetPixelRatioFactor());
	}

	// Token: 0x06008F14 RID: 36628 RVA: 0x0035B6C8 File Offset: 0x003598C8
	public Vector3 TranslateScreenVectorToXuiVector(Vector3 _screenSpaceVector)
	{
		_screenSpaceVector.x -= (float)this.playerUI.camera.pixelWidth / 2f;
		_screenSpaceVector.y -= (float)this.playerUI.camera.pixelHeight / 2f;
		return _screenSpaceVector * this.GetPixelRatioFactor();
	}

	// Token: 0x06008F15 RID: 36629 RVA: 0x0035B728 File Offset: 0x00359928
	public Bounds GetXUiWindowScreenBounds(Transform _xuiElement, bool _includeInactive = false)
	{
		Bounds xuiWindowWorldBounds = XUi.getXUiWindowWorldBounds(_xuiElement, _includeInactive);
		Bounds bounds = new Bounds(this.playerUI.camera.WorldToScreenPoint(xuiWindowWorldBounds.min), Vector3.zero);
		bounds.Encapsulate(this.playerUI.camera.WorldToScreenPoint(xuiWindowWorldBounds.max));
		return bounds;
	}

	// Token: 0x06008F16 RID: 36630 RVA: 0x0035B780 File Offset: 0x00359980
	[PublicizedFrom(EAccessModifier.Private)]
	public static Bounds getXUiWindowWorldBounds(Transform _xuiElement, bool _includeInactive = false)
	{
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
		bool flag = false;
		_xuiElement.GetComponentsInChildren<UIBasicSprite>(_includeInactive, XUi.getXUiWindowWorldBoundsList);
		List<UIBasicSprite> list = XUi.getXUiWindowWorldBoundsList;
		for (int i = 0; i < list.Count; i++)
		{
			UIBasicSprite uibasicSprite = list[i];
			Transform parent = uibasicSprite.transform.parent;
			if (!(parent != null) || !parent.name.Equals("MapSpriteEntity(Clone)"))
			{
				Vector3[] worldCorners = uibasicSprite.worldCorners;
				for (int j = 0; j < worldCorners.Length; j++)
				{
					if (!flag)
					{
						bounds = new Bounds(worldCorners[j], Vector3.zero);
						flag = true;
					}
					else
					{
						bounds.Encapsulate(worldCorners[j]);
					}
				}
			}
		}
		XUi.getXUiWindowWorldBoundsList.Clear();
		return bounds;
	}

	// Token: 0x1700112E RID: 4398
	// (get) Token: 0x06008F17 RID: 36631 RVA: 0x0035B850 File Offset: 0x00359A50
	public Transform StackPanelTransform
	{
		get
		{
			return this.stackPanelRoot;
		}
	}

	// Token: 0x06008F18 RID: 36632 RVA: 0x0035B858 File Offset: 0x00359A58
	[PublicizedFrom(EAccessModifier.Private)]
	public void initStackPanels()
	{
		this.stackPanelRoot = base.transform.Find("StackPanels").transform;
		this.StackPanels.Add("Left", new XUi.StackPanel("Left", XUi.EStackPanelPos.Left, base.transform.Find("StackPanels/Left").transform));
		this.StackPanels.Add("Center", new XUi.StackPanel("Center", XUi.EStackPanelPos.Center, base.transform.Find("StackPanels/Center").transform));
		this.StackPanels.Add("Right", new XUi.StackPanel("Right", XUi.EStackPanelPos.Right, base.transform.Find("StackPanels/Right").transform));
	}

	// Token: 0x06008F19 RID: 36633 RVA: 0x0035B910 File Offset: 0x00359B10
	public void SetStackPanelScale(float _scale)
	{
		this.defaultStackPanelScale = _scale;
		this.stackPanelRoot.localScale = Vector3.one * _scale;
		this.stackPanelRoot.localPosition = Vector3.zero;
	}

	// Token: 0x06008F1A RID: 36634 RVA: 0x0035B93F File Offset: 0x00359B3F
	public void RecenterWindowGroup(XUiWindowGroup _wg, bool _forceImmediate = false)
	{
		if (!_forceImmediate && GameStats.GetInt(EnumGameStats.GameState) != 2)
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.recenterLater(_wg));
			}
			return;
		}
		if (_wg == null)
		{
			this.calculateWindowGroupLayouts();
			return;
		}
		this.calculateWindowGroupLayout(_wg);
	}

	// Token: 0x06008F1B RID: 36635 RVA: 0x0035B97A File Offset: 0x00359B7A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator recenterLater(XUiWindowGroup _wg)
	{
		yield return null;
		if (_wg != null)
		{
			this.calculateWindowGroupLayout(_wg);
		}
		else
		{
			this.calculateWindowGroupLayouts();
		}
		yield return null;
		this.playerUI.CursorController.ResetNavigationTarget();
		yield break;
	}

	// Token: 0x06008F1C RID: 36636 RVA: 0x0035B990 File Offset: 0x00359B90
	[PublicizedFrom(EAccessModifier.Private)]
	public void calculateWindowGroupLayouts()
	{
		foreach (XUiWindowGroup xuiWindowGroup in this.WindowGroups)
		{
			if (xuiWindowGroup.isShowing)
			{
				this.calculateWindowGroupLayout(xuiWindowGroup);
			}
		}
	}

	// Token: 0x06008F1D RID: 36637 RVA: 0x0035B9EC File Offset: 0x00359BEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void calculateWindowGroupLayout(XUiWindowGroup _wg)
	{
		if (_wg == null || _wg.Id == "backpack")
		{
			return;
		}
		XUiController controller = _wg.Controller;
		if (((controller != null) ? controller.Children : null) == null)
		{
			return;
		}
		if (!_wg.HasStackPanelWindows())
		{
			return;
		}
		bool flag = false;
		int num = 0;
		int num2 = 0;
		foreach (XUi.StackPanel stackPanel in this.StackPanels.list)
		{
			bool flag2 = this.layoutWindowsInPanel(_wg, stackPanel);
			if (flag2 && num > 0)
			{
				num += _wg.StackPanelPadding;
			}
			stackPanel.Transform.localPosition = new Vector3((float)num, 0f, 0f);
			num += stackPanel.Size.x;
			num2 = Math.Max(num2, stackPanel.Size.y);
			flag = flag || flag2;
		}
		if (flag)
		{
			this.stackPanelRoot.localPosition = new Vector3(-(this.defaultStackPanelScale * (float)num / 2f), (float)_wg.StackPanelYOffset, 0f);
			this.stackPanelRoot.localScale = Vector3.one * this.defaultStackPanelScale;
		}
		if (flag && (!_wg.LeftPanelVAlignTop || !_wg.RightPanelVAlignTop))
		{
			if (!_wg.LeftPanelVAlignTop)
			{
				this.StackPanels.list[0].Transform.localPosition = new Vector3((float)((int)this.StackPanels.list[0].Transform.position.x), (float)(-(float)(num2 - this.StackPanels.list[0].Size.y)), 0f);
			}
			if (!_wg.RightPanelVAlignTop)
			{
				this.StackPanels.list[2].Transform.localPosition = new Vector3((float)((int)this.StackPanels.list[2].Transform.position.x), (float)(-(float)(num2 - this.StackPanels.list[2].Size.y)), 0f);
			}
		}
	}

	// Token: 0x06008F1E RID: 36638 RVA: 0x0035BC18 File Offset: 0x00359E18
	[PublicizedFrom(EAccessModifier.Private)]
	public bool layoutWindowsInPanel(XUiWindowGroup _windowGroup, XUi.StackPanel _panel)
	{
		XUi.<>c__DisplayClass179_0 CS$<>8__locals1;
		CS$<>8__locals1._panel = _panel;
		CS$<>8__locals1.windowCount = 0;
		CS$<>8__locals1.yPos = 0;
		CS$<>8__locals1.maxWidth = 0;
		XUi.<layoutWindowsInPanel>g__LayoutWindowGroupWindows|179_0(_windowGroup, ref CS$<>8__locals1);
		if (this.playerUI.windowManager.IsWindowOpen("backpack"))
		{
			XUi.<layoutWindowsInPanel>g__LayoutWindowGroupWindows|179_0(this.GetWindowGroupById("backpack"), ref CS$<>8__locals1);
		}
		CS$<>8__locals1._panel.Size.x = CS$<>8__locals1.maxWidth;
		CS$<>8__locals1._panel.Size.y = -CS$<>8__locals1.yPos;
		CS$<>8__locals1._panel.WindowCount = CS$<>8__locals1.windowCount;
		return CS$<>8__locals1.windowCount != 0;
	}

	// Token: 0x06008F1F RID: 36639 RVA: 0x0035BCC0 File Offset: 0x00359EC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void initSounds()
	{
		this.LoadData<AudioClip>("@:Sounds/UI/ui_menu_cycle.wav", delegate(AudioClip _o)
		{
			this.uiScrollSound = _o;
		});
		this.LoadData<AudioClip>("@:Sounds/UI/ui_menu_click.wav", delegate(AudioClip _o)
		{
			this.uiClickSound = _o;
		});
		this.LoadData<AudioClip>("@:Sounds/UI/ui_menu_start.wav", delegate(AudioClip _o)
		{
			this.uiConfirmSound = _o;
		});
		this.LoadData<AudioClip>("@:Sounds/UI/ui_menu_back.wav", delegate(AudioClip _o)
		{
			this.uiBackSound = _o;
		});
		this.LoadData<AudioClip>("@:Sounds/UI/ui_hover.wav", delegate(AudioClip _o)
		{
			this.uiSliderSound = _o;
		});
	}

	// Token: 0x06008F20 RID: 36640 RVA: 0x0035BD40 File Offset: 0x00359F40
	public void PlayMenuSound(XUi.UISoundType _soundType)
	{
		switch (_soundType)
		{
		case XUi.UISoundType.ClickSound:
			this.PlayMenuClickSound();
			return;
		case XUi.UISoundType.ScrollSound:
			this.PlayMenuScrollSound();
			return;
		case XUi.UISoundType.ConfirmSound:
			this.PlayMenuConfirmSound();
			return;
		case XUi.UISoundType.BackSound:
			this.PlayMenuBackSound();
			return;
		case XUi.UISoundType.SliderSound:
			this.PlayMenuSliderSound();
			return;
		case XUi.UISoundType.None:
			return;
		default:
			return;
		}
	}

	// Token: 0x06008F21 RID: 36641 RVA: 0x0035BD8F File Offset: 0x00359F8F
	public void PlayMenuScrollSound()
	{
		Manager.PlayXUiSound(this.uiScrollSound, this.uiScrollVolume);
	}

	// Token: 0x06008F22 RID: 36642 RVA: 0x0035BDA2 File Offset: 0x00359FA2
	public void PlayMenuClickSound()
	{
		Manager.PlayXUiSound(this.uiClickSound, this.uiClickVolume);
	}

	// Token: 0x06008F23 RID: 36643 RVA: 0x0035BDB5 File Offset: 0x00359FB5
	public void PlayMenuConfirmSound()
	{
		Manager.PlayXUiSound(this.uiConfirmSound, this.uiConfirmVolume);
	}

	// Token: 0x06008F24 RID: 36644 RVA: 0x0035BDC8 File Offset: 0x00359FC8
	public void PlayMenuBackSound()
	{
		Manager.PlayXUiSound(this.uiBackSound, this.uiBackVolume);
	}

	// Token: 0x06008F25 RID: 36645 RVA: 0x0035BDDB File Offset: 0x00359FDB
	public void PlayMenuSliderSound()
	{
		Manager.PlayXUiSound(this.uiSliderSound, this.uiSliderVolume);
	}

	// Token: 0x06008F26 RID: 36646 RVA: 0x0035BDF0 File Offset: 0x00359FF0
	public XUiV_Window GetWindow(string _name)
	{
		for (int i = 0; i < this.windows.Count; i++)
		{
			if (this.windows[i].ID == _name)
			{
				return this.windows[i];
			}
		}
		return null;
	}

	// Token: 0x06008F27 RID: 36647 RVA: 0x0035BE3C File Offset: 0x0035A03C
	public XUiController FindWindowGroupByName(string _name)
	{
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			if (this.WindowGroups[i].Id.EqualsCaseInsensitive(_name))
			{
				return this.WindowGroups[i].Controller;
			}
		}
		return null;
	}

	// Token: 0x06008F28 RID: 36648 RVA: 0x0035BE8C File Offset: 0x0035A08C
	public XUiController GetChildById(string _id)
	{
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			XUiController childById = this.WindowGroups[i].Controller.GetChildById(_id);
			if (childById != null)
			{
				return childById;
			}
		}
		return null;
	}

	// Token: 0x06008F29 RID: 36649 RVA: 0x0035BED0 File Offset: 0x0035A0D0
	public List<XUiController> GetChildrenById(string _id)
	{
		List<XUiController> list = new List<XUiController>();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.GetChildrenById(_id, list);
		}
		return list;
	}

	// Token: 0x06008F2A RID: 36650 RVA: 0x0035BF14 File Offset: 0x0035A114
	public T GetChildByType<T>() where T : XUiController
	{
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			T childByType = this.WindowGroups[i].Controller.GetChildByType<T>();
			if (childByType != null)
			{
				return childByType;
			}
		}
		return default(T);
	}

	// Token: 0x06008F2B RID: 36651 RVA: 0x0035BF64 File Offset: 0x0035A164
	public List<T> GetChildrenByType<T>() where T : XUiController
	{
		List<T> list = new List<T>();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.GetChildrenByType<T>(list);
		}
		return list;
	}

	// Token: 0x06008F2C RID: 36652 RVA: 0x0035BFA8 File Offset: 0x0035A1A8
	public T GetWindowByType<T>() where T : XUiController
	{
		Type typeFromHandle = typeof(T);
		List<XUiController> list;
		this.windowsAndGroupsByControllerType.TryGetValue(typeFromHandle, out list);
		if (list == null || list.Count == 0)
		{
			return default(T);
		}
		if (list.Count > 1)
		{
			Log.Warning("Multiple controllers of type " + typeof(T).FullName);
		}
		return (T)((object)list[0]);
	}

	// Token: 0x06008F2D RID: 36653 RVA: 0x0035C018 File Offset: 0x0035A218
	public List<T> GetWindowsByType<T>() where T : XUiController
	{
		Type typeFromHandle = typeof(T);
		List<XUiController> list;
		this.windowsAndGroupsByControllerType.TryGetValue(typeFromHandle, out list);
		List<T> list2 = new List<T>();
		if (list == null)
		{
			return list2;
		}
		foreach (XUiController xuiController in list)
		{
			list2.Add((T)((object)xuiController));
		}
		return list2;
	}

	// Token: 0x06008F2E RID: 36654 RVA: 0x0035C094 File Offset: 0x0035A294
	public XUiWindowGroup GetWindowGroupById(string _id)
	{
		foreach (XUiWindowGroup xuiWindowGroup in this.WindowGroups)
		{
			if (xuiWindowGroup.Id == _id)
			{
				return xuiWindowGroup;
			}
		}
		return null;
	}

	// Token: 0x06008F2F RID: 36655 RVA: 0x0035C0F8 File Offset: 0x0035A2F8
	public void GetOpenWindows(List<XUiV_Window> _list)
	{
		_list.Clear();
		foreach (XUiV_Window xuiV_Window in this.windows)
		{
			if (xuiV_Window.IsOpen && xuiV_Window.IsVisible)
			{
				_list.Add(xuiV_Window);
			}
		}
	}

	// Token: 0x06008F30 RID: 36656 RVA: 0x0035C164 File Offset: 0x0035A364
	[PublicizedFrom(EAccessModifier.Private)]
	public void initAtlases()
	{
		MultiSourceAtlasManager[] array = Resources.FindObjectsOfTypeAll<MultiSourceAtlasManager>();
		for (int i = 0; i < array.Length; i++)
		{
			this.allMultiSourceAtlases.Add(array[i].name, array[i]);
		}
	}

	// Token: 0x06008F31 RID: 36657 RVA: 0x0035C19C File Offset: 0x0035A39C
	public INGUIAtlas GetAtlasByName(string _atlasName, string _spriteName)
	{
		if (string.IsNullOrEmpty(_atlasName))
		{
			return null;
		}
		MultiSourceAtlasManager multiSourceAtlasManager;
		if (!string.IsNullOrEmpty(_spriteName) && this.allMultiSourceAtlases.TryGetValue(_atlasName, out multiSourceAtlasManager))
		{
			return multiSourceAtlasManager.GetAtlasForSprite(_spriteName);
		}
		return null;
	}

	// Token: 0x06008F32 RID: 36658 RVA: 0x0035C1D4 File Offset: 0x0035A3D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void initFonts()
	{
		this.fontsByName = new CaseInsensitiveStringDictionary<NGUIFont>();
		foreach (NGUIFont nguifont in this.NGUIFonts)
		{
			this.fontsByName.Add(nguifont.name, nguifont);
		}
	}

	// Token: 0x06008F33 RID: 36659 RVA: 0x0035C218 File Offset: 0x0035A418
	public NGUIFont GetUIFontByName(string _name, bool _showWarning = true)
	{
		NGUIFont nguifont;
		if (this.fontsByName.TryGetValue(_name, out nguifont))
		{
			return nguifont;
		}
		if (_showWarning)
		{
			Log.Warning("XUi font not found: " + _name + ", from: " + StackTraceUtility.ExtractStackTrace());
		}
		return null;
	}

	// Token: 0x06008F34 RID: 36660 RVA: 0x0035C258 File Offset: 0x0035A458
	public Transform GetAnchor(string _anchorName)
	{
		if (string.IsNullOrEmpty(_anchorName))
		{
			return null;
		}
		UIAnchor.Side side;
		if (EnumUtils.TryParse<UIAnchor.Side>(_anchorName, out side, true))
		{
			return this.xuiAnchors[(int)side].transform;
		}
		return this.anchorRoot.Find(_anchorName);
	}

	// Token: 0x06008F35 RID: 36661 RVA: 0x0035C294 File Offset: 0x0035A494
	public UIAnchor GetAnchor(UIAnchor.Side _anchorSide)
	{
		return this.xuiAnchors[(int)_anchorSide];
	}

	// Token: 0x06008F36 RID: 36662 RVA: 0x0035C2A0 File Offset: 0x0035A4A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void initAnchors()
	{
		this.anchorRoot = base.transform.Find("Anchors").transform;
		UIAnchor[] componentsInChildren = this.anchorRoot.parent.GetComponentsInChildren<UIAnchor>();
		this.xuiAnchors = new UIAnchor[9];
		foreach (UIAnchor uianchor in componentsInChildren)
		{
			uianchor.runOnlyOnce = false;
			uianchor.uiCamera = this.playerUI.camera;
			this.xuiAnchors[(int)uianchor.side] = uianchor;
		}
	}

	// Token: 0x06008F37 RID: 36663 RVA: 0x0035C320 File Offset: 0x0035A520
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateAnchors()
	{
		float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsScreenBoundsValue);
		float num = (1f - @float) / 2f;
		foreach (UIAnchor uianchor in this.xuiAnchors)
		{
			if (!(uianchor == null))
			{
				UIAnchor.Side side = uianchor.side;
				if (side == UIAnchor.Side.Right || side == UIAnchor.Side.TopRight || side == UIAnchor.Side.BottomRight)
				{
					uianchor.relativeOffset.x = -num;
				}
				side = uianchor.side;
				if (side == UIAnchor.Side.Left || side == UIAnchor.Side.TopLeft || side == UIAnchor.Side.BottomLeft)
				{
					uianchor.relativeOffset.x = num;
				}
				side = uianchor.side;
				if (side == UIAnchor.Side.Top || side == UIAnchor.Side.TopRight || side == UIAnchor.Side.TopLeft)
				{
					uianchor.relativeOffset.y = -num;
				}
				side = uianchor.side;
				if (side == UIAnchor.Side.Bottom || side == UIAnchor.Side.BottomRight || side == UIAnchor.Side.BottomLeft)
				{
					uianchor.relativeOffset.y = num;
				}
				side = uianchor.side;
				if (side == UIAnchor.Side.Bottom || side == UIAnchor.Side.Top || side == UIAnchor.Side.Center)
				{
					uianchor.relativeOffset.x = 0f;
				}
			}
		}
	}

	// Token: 0x06008F38 RID: 36664 RVA: 0x0035C430 File Offset: 0x0035A630
	public void RefreshAllWindows()
	{
		if (this.WindowGroups == null)
		{
			return;
		}
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.SetAllChildrenDirty();
		}
	}

	// Token: 0x06008F39 RID: 36665 RVA: 0x0035C474 File Offset: 0x0035A674
	public void CancelAllCrafting()
	{
		XUiC_RecipeStack[] recipesToCraft = this.FindWindowGroupByName("crafting").GetChildByType<XUiC_CraftingQueue>().GetRecipesToCraft();
		for (int i = 0; i < recipesToCraft.Length; i++)
		{
			recipesToCraft[i].ForceCancel();
		}
	}

	// Token: 0x06008F3A RID: 36666 RVA: 0x0035C4B0 File Offset: 0x0035A6B0
	public CraftingData GetCraftingData()
	{
		CraftingData craftingData = new CraftingData();
		XUiController xuiController = this.FindWindowGroupByName("crafting");
		if (xuiController == null)
		{
			return craftingData;
		}
		XUiC_CraftingQueue childByType = xuiController.GetChildByType<XUiC_CraftingQueue>();
		if (childByType == null)
		{
			return craftingData;
		}
		XUiC_RecipeStack[] recipesToCraft = childByType.GetRecipesToCraft();
		if (recipesToCraft == null)
		{
			return craftingData;
		}
		craftingData.RecipeQueueItems = new RecipeQueueItem[recipesToCraft.Length];
		for (int i = 0; i < recipesToCraft.Length; i++)
		{
			RecipeQueueItem recipeQueueItem = new RecipeQueueItem();
			recipeQueueItem.Recipe = recipesToCraft[i].GetRecipe();
			recipeQueueItem.Multiplier = (short)recipesToCraft[i].GetRecipeCount();
			recipeQueueItem.CraftingTimeLeft = recipesToCraft[i].GetRecipeCraftingTimeLeft();
			recipeQueueItem.IsCrafting = recipesToCraft[i].IsCrafting;
			recipeQueueItem.Quality = (byte)recipesToCraft[i].OutputQuality;
			recipeQueueItem.StartingEntityId = recipesToCraft[i].StartingEntityId;
			recipeQueueItem.RepairItem = recipesToCraft[i].OriginalItem;
			recipeQueueItem.AmountToRepair = (ushort)recipesToCraft[i].AmountToRepair;
			recipeQueueItem.OneItemCraftTime = recipesToCraft[i].GetOneItemCraftTime();
			craftingData.RecipeQueueItems[i] = recipeQueueItem;
		}
		return craftingData;
	}

	// Token: 0x06008F3B RID: 36667 RVA: 0x0035C5B8 File Offset: 0x0035A7B8
	public void SetCraftingData(CraftingData _cd)
	{
		base.StartCoroutine(this.setCraftingDataAsync(_cd));
	}

	// Token: 0x06008F3C RID: 36668 RVA: 0x0035C5C8 File Offset: 0x0035A7C8
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator setCraftingDataAsync(CraftingData _cd)
	{
		while (!this.IsReady)
		{
			yield return null;
		}
		XUiC_CraftingQueue childByType = this.GetChildByType<XUiC_CraftingQueue>();
		if (childByType == null)
		{
			yield break;
		}
		childByType.ClearQueue();
		for (int i = 0; i < _cd.RecipeQueueItems.Length; i++)
		{
			RecipeQueueItem recipeQueueItem = _cd.RecipeQueueItems[i];
			if (recipeQueueItem != null)
			{
				if (recipeQueueItem.RepairItem != null && recipeQueueItem.RepairItem.type != 0)
				{
					childByType.AddItemToRepairAtIndex(i, recipeQueueItem.CraftingTimeLeft, recipeQueueItem.RepairItem, (int)recipeQueueItem.AmountToRepair, recipeQueueItem.IsCrafting, recipeQueueItem.StartingEntityId);
				}
				else
				{
					childByType.AddRecipeToCraftAtIndex(i, recipeQueueItem.Recipe, (int)recipeQueueItem.Multiplier, recipeQueueItem.CraftingTimeLeft, recipeQueueItem.IsCrafting, false, (int)recipeQueueItem.Quality, recipeQueueItem.StartingEntityId, recipeQueueItem.OneItemCraftTime);
				}
			}
			else
			{
				childByType.AddRecipeToCraftAtIndex(i, null, 0, -1f, false, false, -1, -1, -1f);
			}
		}
		childByType.IsDirty = true;
		yield break;
	}

	// Token: 0x06008F3D RID: 36669 RVA: 0x0035C5E0 File Offset: 0x0035A7E0
	public static void HandlePaging(XUi _xui, Func<bool> _onPageUp, Func<bool> _onPageDown, bool _useVerticalAxis = false)
	{
		if (null != _xui.playerUI)
		{
			PlayerActionsLocal playerInput = _xui.playerUI.playerInput;
			if (((playerInput != null) ? playerInput.GUIActions : null) != null && _xui.playerUI.windowManager.IsKeyShortcutsAllowed())
			{
				Vector2 vector = _xui.playerUI.playerInput.GUIActions.Camera.Vector;
				if (vector == Vector2.zero)
				{
					XUi.pagingRepeatTimer = 0f;
					XUi.previousPagingVector = Vector2.zero;
					return;
				}
				if (XUi.previousPagingVector != Vector2.zero)
				{
					XUi.pagingRepeatTimer -= Time.unscaledDeltaTime;
					if (XUi.pagingRepeatTimer > 0f)
					{
						return;
					}
				}
				else
				{
					XUi.initialPagingInput = true;
				}
				XUi.previousPagingVector = vector;
				XUi.pagingRepeatTimer = (XUi.initialPagingInput ? 0.35f : 0.1f);
				XUi.initialPagingInput = false;
				bool flag = false;
				float num = Mathf.Abs(vector.x);
				float num2 = Mathf.Abs(vector.y);
				if (_useVerticalAxis)
				{
					if ((double)num2 < (double)num * 1.8)
					{
						return;
					}
					if ((vector.y > 0f && _onPageUp()) || (vector.y < 0f && _onPageDown()))
					{
						flag = true;
					}
				}
				else
				{
					if ((double)num < (double)num2 * 1.8)
					{
						return;
					}
					if ((vector.x > 0f && _onPageUp()) || (vector.x < 0f && _onPageDown()))
					{
						flag = true;
					}
				}
				if (flag)
				{
					_xui.playerUI.CursorController.PlayPagingSound();
					return;
				}
				return;
			}
		}
		XUi.previousPagingVector = Vector2.zero;
	}

	// Token: 0x06008F3E RID: 36670 RVA: 0x0035C77B File Offset: 0x0035A97B
	public void UpdateWindowSoftCursorBounds(XUiV_Window _window)
	{
		this.playerUI.CursorController != null;
	}

	// Token: 0x06008F3F RID: 36671 RVA: 0x0035C790 File Offset: 0x0035A990
	public void RemoveWindowFromSoftCursorBounds(XUiV_Window _window)
	{
		CursorControllerAbs cursorController = this.playerUI.CursorController;
		if (cursorController != null)
		{
			cursorController.RemoveBounds(_window.ID);
		}
	}

	// Token: 0x06008F40 RID: 36672 RVA: 0x0035C7C0 File Offset: 0x0035A9C0
	public void ForceInputStyleChange()
	{
		this.GetOpenWindows(XUi.forceInputStyleChangedWindowsList);
		foreach (XUiV_Window xuiV_Window in XUi.forceInputStyleChangedWindowsList)
		{
			xuiV_Window.Controller.ForceInputStyleChange(PlatformManager.NativePlatform.Input.CurrentInputStyle, PlatformManager.NativePlatform.Input.CurrentInputStyle);
		}
		XUi.forceInputStyleChangedWindowsList.Clear();
	}

	// Token: 0x06008F43 RID: 36675 RVA: 0x0035C900 File Offset: 0x0035AB00
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <StopAllVideo>g__StopVideo|145_0(XUiController controller)
	{
		XUiV_Video xuiV_Video = controller.ViewComponent as XUiV_Video;
		if (xuiV_Video != null)
		{
			xuiV_Video.Playing = false;
		}
		foreach (XUiController xuiController in controller.Children)
		{
			XUi.<StopAllVideo>g__StopVideo|145_0(xuiController);
		}
	}

	// Token: 0x06008F44 RID: 36676 RVA: 0x0035C968 File Offset: 0x0035AB68
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <layoutWindowsInPanel>g__LayoutWindowGroupWindows|179_0(XUiWindowGroup _wg, ref XUi.<>c__DisplayClass179_0 A_1)
	{
		foreach (XUiController xuiController in _wg.Controller.Children)
		{
			XUiV_Window xuiV_Window = (XUiV_Window)xuiController.ViewComponent;
			if (xuiV_Window != null && xuiV_Window.IsActiveInHierarchy && !(xuiController.ViewComponent.UiTransform.parent.name != A_1._panel.Name) && xuiV_Window.Size.y > 0)
			{
				if (A_1.yPos < 0)
				{
					A_1.yPos -= _wg.StackPanelPadding;
				}
				xuiV_Window.Position = new Vector2i(0, A_1.yPos);
				xuiV_Window.UiTransform.localPosition = new Vector3(0f, (float)A_1.yPos);
				A_1.yPos -= xuiV_Window.Size.y;
				A_1.maxWidth = Math.Max(A_1.maxWidth, xuiV_Window.Size.x);
				int windowCount = A_1.windowCount;
				A_1.windowCount = windowCount + 1;
			}
		}
	}

	// Token: 0x04006888 RID: 26760
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lastScreenHeight;

	// Token: 0x04006889 RID: 26761
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float pixelRatioFactor;

	// Token: 0x0400688A RID: 26762
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static UIRoot uiRoot;

	// Token: 0x0400688B RID: 26763
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool inGameMenuOpen;

	// Token: 0x0400688C RID: 26764
	public NGUIFont[] NGUIFonts;

	// Token: 0x0400688D RID: 26765
	public List<XUiWindowGroup> WindowGroups;

	// Token: 0x0400688E RID: 26766
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Type, List<XUiController>> windowsAndGroupsByControllerType;

	// Token: 0x0400688F RID: 26767
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<XUiV_Window> windows;

	// Token: 0x04006890 RID: 26768
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float backgroundGlobalOpacity = 1f;

	// Token: 0x04006891 RID: 26769
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float foregroundGlobalOpacity = 1f;

	// Token: 0x04006892 RID: 26770
	public XUiM_PlayerInventory PlayerInventory;

	// Token: 0x04006893 RID: 26771
	public XUiM_PlayerEquipment PlayerEquipment;

	// Token: 0x04006894 RID: 26772
	public XUiM_Vehicle Vehicle;

	// Token: 0x04006895 RID: 26773
	public XUiM_AssembleItem AssembleItem;

	// Token: 0x04006896 RID: 26774
	public XUiM_Quest QuestTracker;

	// Token: 0x04006897 RID: 26775
	public XUiM_Recipes Recipes;

	// Token: 0x04006898 RID: 26776
	public XUiM_Trader Trader;

	// Token: 0x04006899 RID: 26777
	public XUiM_Dialog Dialog;

	// Token: 0x0400689A RID: 26778
	public XUiC_BuffPopoutList BuffPopoutList;

	// Token: 0x0400689B RID: 26779
	public XUiC_CollectedItemList CollectedItemList;

	// Token: 0x0400689C RID: 26780
	public XUiC_Radial RadialWindow;

	// Token: 0x0400689D RID: 26781
	public XUiC_GamepadCalloutWindow CalloutWindow;

	// Token: 0x040068B2 RID: 26802
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float xuiGlobalScaling = 1.255f;

	// Token: 0x040068B3 RID: 26803
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2i lastScreenSize = Vector2i.zero;

	// Token: 0x040068B4 RID: 26804
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsReady;

	// Token: 0x040068B5 RID: 26805
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool asyncLoad;

	// Token: 0x040068B6 RID: 26806
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LoadManager.LoadGroup loadGroup;

	// Token: 0x040068B9 RID: 26809
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI mPlayerUI;

	// Token: 0x040068BA RID: 26810
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public long accumElapsedMilliseconds;

	// Token: 0x040068BB RID: 26811
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Coroutine loadAsyncCoroutine;

	// Token: 0x040068BC RID: 26812
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<UIBasicSprite> getXUiWindowWorldBoundsList = new List<UIBasicSprite>();

	// Token: 0x040068BD RID: 26813
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float defaultStackPanelScale = 1f;

	// Token: 0x040068BE RID: 26814
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform stackPanelRoot;

	// Token: 0x040068BF RID: 26815
	public readonly DictionaryList<string, XUi.StackPanel> StackPanels = new DictionaryList<string, XUi.StackPanel>();

	// Token: 0x040068C0 RID: 26816
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiScrollSound;

	// Token: 0x040068C1 RID: 26817
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiScrollVolume = 0.5f;

	// Token: 0x040068C2 RID: 26818
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiClickSound;

	// Token: 0x040068C3 RID: 26819
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiClickVolume = 0.5f;

	// Token: 0x040068C4 RID: 26820
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiConfirmSound;

	// Token: 0x040068C5 RID: 26821
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiConfirmVolume = 0.5f;

	// Token: 0x040068C6 RID: 26822
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiBackSound;

	// Token: 0x040068C7 RID: 26823
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiBackVolume = 0.5f;

	// Token: 0x040068C8 RID: 26824
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiSliderSound;

	// Token: 0x040068C9 RID: 26825
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiSliderVolume = 0.25f;

	// Token: 0x040068CA RID: 26826
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, MultiSourceAtlasManager> allMultiSourceAtlases = new CaseInsensitiveStringDictionary<MultiSourceAtlasManager>();

	// Token: 0x040068CB RID: 26827
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CaseInsensitiveStringDictionary<NGUIFont> fontsByName;

	// Token: 0x040068CC RID: 26828
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform anchorRoot;

	// Token: 0x040068CD RID: 26829
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIAnchor[] xuiAnchors;

	// Token: 0x040068CE RID: 26830
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 previousPagingVector = Vector2.zero;

	// Token: 0x040068CF RID: 26831
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float pagingRepeatTimer;

	// Token: 0x040068D0 RID: 26832
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool initialPagingInput;

	// Token: 0x040068D1 RID: 26833
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<XUiV_Window> forceInputStyleChangedWindowsList = new List<XUiV_Window>();

	// Token: 0x0200116E RID: 4462
	public enum EStackPanelPos
	{
		// Token: 0x040068D3 RID: 26835
		Left,
		// Token: 0x040068D4 RID: 26836
		Center,
		// Token: 0x040068D5 RID: 26837
		Right
	}

	// Token: 0x0200116F RID: 4463
	public class StackPanel
	{
		// Token: 0x06008F4A RID: 36682 RVA: 0x0035CAE1 File Offset: 0x0035ACE1
		public StackPanel(string _name, XUi.EStackPanelPos _position, Transform _transform)
		{
			this.Name = _name;
			this.Position = _position;
			this.Transform = _transform;
		}

		// Token: 0x040068D6 RID: 26838
		public readonly string Name;

		// Token: 0x040068D7 RID: 26839
		public readonly XUi.EStackPanelPos Position;

		// Token: 0x040068D8 RID: 26840
		public readonly Transform Transform;

		// Token: 0x040068D9 RID: 26841
		public int WindowCount;

		// Token: 0x040068DA RID: 26842
		public Vector2Int Size;
	}

	// Token: 0x02001170 RID: 4464
	public enum UISoundType
	{
		// Token: 0x040068DC RID: 26844
		ClickSound,
		// Token: 0x040068DD RID: 26845
		ScrollSound,
		// Token: 0x040068DE RID: 26846
		ConfirmSound,
		// Token: 0x040068DF RID: 26847
		BackSound,
		// Token: 0x040068E0 RID: 26848
		SliderSound,
		// Token: 0x040068E1 RID: 26849
		None
	}
}
