using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using Platform;
using UnityEngine;

// Token: 0x02001195 RID: 4501
public class XUi : MonoBehaviour
{
	// Token: 0x17001164 RID: 4452
	// (get) Token: 0x06008F9E RID: 36766 RVA: 0x00361170 File Offset: 0x0035F370
	// (set) Token: 0x06008F9F RID: 36767 RVA: 0x00361178 File Offset: 0x0035F378
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

	// Token: 0x06008FA0 RID: 36768 RVA: 0x003611D9 File Offset: 0x0035F3D9
	public static bool IsGameRunning()
	{
		return GameManager.Instance != null && GameManager.Instance.World != null;
	}

	// Token: 0x17001165 RID: 4453
	// (get) Token: 0x06008FA1 RID: 36769 RVA: 0x003611F7 File Offset: 0x0035F3F7
	// (set) Token: 0x06008FA2 RID: 36770 RVA: 0x003611FF File Offset: 0x0035F3FF
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

	// Token: 0x17001166 RID: 4454
	// (get) Token: 0x06008FA3 RID: 36771 RVA: 0x00361208 File Offset: 0x0035F408
	// (set) Token: 0x06008FA4 RID: 36772 RVA: 0x00361210 File Offset: 0x0035F410
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

	// Token: 0x17001167 RID: 4455
	// (get) Token: 0x06008FA5 RID: 36773 RVA: 0x00361219 File Offset: 0x0035F419
	// (set) Token: 0x06008FA6 RID: 36774 RVA: 0x00361221 File Offset: 0x0035F421
	public XUiC_DragAndDropWindow DragAndDropWindow { get; set; }

	// Token: 0x17001168 RID: 4456
	// (get) Token: 0x06008FA7 RID: 36775 RVA: 0x0036122A File Offset: 0x0035F42A
	// (set) Token: 0x06008FA8 RID: 36776 RVA: 0x00361232 File Offset: 0x0035F432
	public XUiC_OnScreenIcons OnScreenIconsWindow { get; set; }

	// Token: 0x17001169 RID: 4457
	// (get) Token: 0x06008FA9 RID: 36777 RVA: 0x0036123B File Offset: 0x0035F43B
	// (set) Token: 0x06008FAA RID: 36778 RVA: 0x00361243 File Offset: 0x0035F443
	public XUiC_ToolTip ToolTipWindow { get; set; }

	// Token: 0x1700116A RID: 4458
	// (get) Token: 0x06008FAB RID: 36779 RVA: 0x0036124C File Offset: 0x0035F44C
	// (set) Token: 0x06008FAC RID: 36780 RVA: 0x00361254 File Offset: 0x0035F454
	public XUiC_PopupMenu PopupMenuWindow { get; set; }

	// Token: 0x1700116B RID: 4459
	// (get) Token: 0x06008FAD RID: 36781 RVA: 0x0036125D File Offset: 0x0035F45D
	// (set) Token: 0x06008FAE RID: 36782 RVA: 0x00361265 File Offset: 0x0035F465
	public XUiC_SaveIndicator SaveIndicator { get; set; }

	// Token: 0x1700116C RID: 4460
	// (get) Token: 0x06008FAF RID: 36783 RVA: 0x0036126E File Offset: 0x0035F46E
	// (set) Token: 0x06008FB0 RID: 36784 RVA: 0x00361276 File Offset: 0x0035F476
	public XUiC_WorkstationToolGrid CurrentWorkstationToolGrid { get; set; }

	// Token: 0x1700116D RID: 4461
	// (get) Token: 0x06008FB1 RID: 36785 RVA: 0x0036127F File Offset: 0x0035F47F
	// (set) Token: 0x06008FB2 RID: 36786 RVA: 0x00361287 File Offset: 0x0035F487
	public XUiC_WorkstationFuelGrid CurrentWorkstationFuelGrid { get; set; }

	// Token: 0x1700116E RID: 4462
	// (get) Token: 0x06008FB3 RID: 36787 RVA: 0x00361290 File Offset: 0x0035F490
	// (set) Token: 0x06008FB4 RID: 36788 RVA: 0x00361298 File Offset: 0x0035F498
	public XUiC_WorkstationInputGrid CurrentWorkstationInputGrid { get; set; }

	// Token: 0x1700116F RID: 4463
	// (get) Token: 0x06008FB5 RID: 36789 RVA: 0x003612A1 File Offset: 0x0035F4A1
	// (set) Token: 0x06008FB6 RID: 36790 RVA: 0x003612A9 File Offset: 0x0035F4A9
	public XUiC_WorkstationOutputGrid CurrentWorkstationOutputGrid { get; set; }

	// Token: 0x17001170 RID: 4464
	// (get) Token: 0x06008FB7 RID: 36791 RVA: 0x003612B2 File Offset: 0x0035F4B2
	// (set) Token: 0x06008FB8 RID: 36792 RVA: 0x003612BA File Offset: 0x0035F4BA
	public XUiC_DewCollectorModGrid CurrentCollectorModGrid { get; set; }

	// Token: 0x17001171 RID: 4465
	// (get) Token: 0x06008FB9 RID: 36793 RVA: 0x003612C3 File Offset: 0x0035F4C3
	// (set) Token: 0x06008FBA RID: 36794 RVA: 0x003612CB File Offset: 0x0035F4CB
	public XUiC_CollectorFuelGrid CurrentCollectorFuelGrid { get; set; }

	// Token: 0x17001172 RID: 4466
	// (get) Token: 0x06008FBB RID: 36795 RVA: 0x003612D4 File Offset: 0x0035F4D4
	// (set) Token: 0x06008FBC RID: 36796 RVA: 0x003612DC File Offset: 0x0035F4DC
	public XUiC_CollectorFuelGrid currentCollectorCatalystGrid { get; set; }

	// Token: 0x17001173 RID: 4467
	// (get) Token: 0x06008FBD RID: 36797 RVA: 0x003612E5 File Offset: 0x0035F4E5
	// (set) Token: 0x06008FBE RID: 36798 RVA: 0x003612ED File Offset: 0x0035F4ED
	public XUiC_CombineGrid CurrentCombineGrid { get; set; }

	// Token: 0x17001174 RID: 4468
	// (get) Token: 0x06008FBF RID: 36799 RVA: 0x003612F6 File Offset: 0x0035F4F6
	// (set) Token: 0x06008FC0 RID: 36800 RVA: 0x003612FE File Offset: 0x0035F4FE
	public XUiC_PowerSourceSlots CurrentPowerSourceSlots { get; set; }

	// Token: 0x17001175 RID: 4469
	// (get) Token: 0x06008FC1 RID: 36801 RVA: 0x00361307 File Offset: 0x0035F507
	// (set) Token: 0x06008FC2 RID: 36802 RVA: 0x0036130F File Offset: 0x0035F50F
	public XUiC_PowerRangedAmmoSlots CurrentPowerAmmoSlots { get; set; }

	// Token: 0x17001176 RID: 4470
	// (get) Token: 0x06008FC3 RID: 36803 RVA: 0x00361318 File Offset: 0x0035F518
	// (set) Token: 0x06008FC4 RID: 36804 RVA: 0x00361320 File Offset: 0x0035F520
	public XUiC_SelectableEntry CurrentSelectedEntry { get; set; }

	// Token: 0x17001177 RID: 4471
	// (get) Token: 0x06008FC5 RID: 36805 RVA: 0x00361329 File Offset: 0x0035F529
	// (set) Token: 0x06008FC6 RID: 36806 RVA: 0x00361331 File Offset: 0x0035F531
	public ProgressionValue SelectedSkill { get; set; }

	// Token: 0x17001178 RID: 4472
	// (get) Token: 0x06008FC7 RID: 36807 RVA: 0x0036133A File Offset: 0x0035F53A
	// (set) Token: 0x06008FC8 RID: 36808 RVA: 0x00361342 File Offset: 0x0035F542
	public TEFeatureStorage LootContainer { get; set; }

	// Token: 0x17001179 RID: 4473
	// (get) Token: 0x06008FC9 RID: 36809 RVA: 0x0036134B File Offset: 0x0035F54B
	// (set) Token: 0x06008FCA RID: 36810 RVA: 0x00361353 File Offset: 0x0035F553
	public bool IsUsingItemActionEntryUse { get; set; }

	// Token: 0x1700117A RID: 4474
	// (get) Token: 0x06008FCB RID: 36811 RVA: 0x0036135C File Offset: 0x0035F55C
	// (set) Token: 0x06008FCC RID: 36812 RVA: 0x00361364 File Offset: 0x0035F564
	public bool IsUsingItemActionEntryPromptComplete { get; set; }

	// Token: 0x1700117B RID: 4475
	// (get) Token: 0x06008FCD RID: 36813 RVA: 0x0036136D File Offset: 0x0035F56D
	// (set) Token: 0x06008FCE RID: 36814 RVA: 0x00361375 File Offset: 0x0035F575
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

	// Token: 0x140000EE RID: 238
	// (add) Token: 0x06008FCF RID: 36815 RVA: 0x00361398 File Offset: 0x0035F598
	// (remove) Token: 0x06008FD0 RID: 36816 RVA: 0x003613D0 File Offset: 0x0035F5D0
	public event Action OnShutdown;

	// Token: 0x140000EF RID: 239
	// (add) Token: 0x06008FD1 RID: 36817 RVA: 0x00361408 File Offset: 0x0035F608
	// (remove) Token: 0x06008FD2 RID: 36818 RVA: 0x00361440 File Offset: 0x0035F640
	public event Action OnBuilt;

	// Token: 0x1700117C RID: 4476
	// (get) Token: 0x06008FD3 RID: 36819 RVA: 0x00361475 File Offset: 0x0035F675
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

	// Token: 0x06008FD4 RID: 36820 RVA: 0x00361498 File Offset: 0x0035F698
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

	// Token: 0x06008FD5 RID: 36821 RVA: 0x00361515 File Offset: 0x0035F715
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

	// Token: 0x06008FD6 RID: 36822 RVA: 0x00361550 File Offset: 0x0035F750
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

	// Token: 0x06008FD7 RID: 36823 RVA: 0x0036163C File Offset: 0x0035F83C
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

	// Token: 0x06008FD8 RID: 36824 RVA: 0x003616E8 File Offset: 0x0035F8E8
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

	// Token: 0x06008FD9 RID: 36825 RVA: 0x003616F7 File Offset: 0x0035F8F7
	public void Awake()
	{
		this.WindowGroups = new List<XUiWindowGroup>();
		this.windowsAndGroupsByControllerType = new Dictionary<Type, List<XUiController>>();
		this.initFonts();
	}

	// Token: 0x06008FDA RID: 36826 RVA: 0x00361718 File Offset: 0x0035F918
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

	// Token: 0x06008FDB RID: 36827 RVA: 0x00361834 File Offset: 0x0035FA34
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

	// Token: 0x06008FDC RID: 36828 RVA: 0x00361883 File Offset: 0x0035FA83
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

	// Token: 0x06008FDD RID: 36829 RVA: 0x0036189C File Offset: 0x0035FA9C
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

	// Token: 0x06008FDE RID: 36830 RVA: 0x0036195C File Offset: 0x0035FB5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.ResolutionChanged -= this.OnResolutionChanged;
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
		LocalPlayerManager.OnLocalPlayersChanged -= this.handleLocalPlayersChanged;
		this.Shutdown(false);
	}

	// Token: 0x06008FDF RID: 36831 RVA: 0x00361998 File Offset: 0x0035FB98
	public void Shutdown(bool _destroyImmediate = false)
	{
		Action onShutdown = this.OnShutdown;
		if (onShutdown != null)
		{
			onShutdown();
		}
		this.Cleanup(_destroyImmediate);
	}

	// Token: 0x06008FE0 RID: 36832 RVA: 0x003619B4 File Offset: 0x0035FBB4
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

	// Token: 0x06008FE1 RID: 36833 RVA: 0x00361A20 File Offset: 0x0035FC20
	public void CancelLoading()
	{
		if (this.loadAsyncCoroutine != null)
		{
			ThreadManager.StopCoroutine(this.loadAsyncCoroutine);
			this.loadAsyncCoroutine = null;
		}
	}

	// Token: 0x06008FE2 RID: 36834 RVA: 0x00361A3C File Offset: 0x0035FC3C
	public void StopAllVideo()
	{
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			XUi.<StopAllVideo>g__StopVideo|145_0(this.WindowGroups[i].Controller);
		}
	}

	// Token: 0x06008FE3 RID: 36835 RVA: 0x00361A78 File Offset: 0x0035FC78
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

	// Token: 0x06008FE4 RID: 36836 RVA: 0x00361AB6 File Offset: 0x0035FCB6
	public void AddWindow(XUiV_Window _window)
	{
		this.windows.Add(_window);
	}

	// Token: 0x06008FE5 RID: 36837 RVA: 0x00361AC4 File Offset: 0x0035FCC4
	public void RemoveWindow(XUiV_Window _window)
	{
		this.windows.Remove(_window);
	}

	// Token: 0x06008FE6 RID: 36838 RVA: 0x00361AD4 File Offset: 0x0035FCD4
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

	// Token: 0x06008FE7 RID: 36839 RVA: 0x00361B34 File Offset: 0x0035FD34
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

	// Token: 0x06008FE8 RID: 36840 RVA: 0x00361BB2 File Offset: 0x0035FDB2
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnResolutionChanged(int _arg1, int _arg2)
	{
		ThreadManager.StartCoroutine(this.delayedScaleUpdate());
	}

	// Token: 0x06008FE9 RID: 36841 RVA: 0x00361BC0 File Offset: 0x0035FDC0
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator delayedScaleUpdate()
	{
		yield return null;
		this.SetScale(-1f);
		yield break;
	}

	// Token: 0x06008FEA RID: 36842 RVA: 0x00361BD0 File Offset: 0x0035FDD0
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

	// Token: 0x06008FEB RID: 36843 RVA: 0x00361C36 File Offset: 0x0035FE36
	public float GetScale()
	{
		return this.xuiGlobalScaling;
	}

	// Token: 0x06008FEC RID: 36844 RVA: 0x00361C3E File Offset: 0x0035FE3E
	public void LoadData<T>(string _path, Action<T> _callback) where T : global::UnityEngine.Object
	{
		LoadManager.LoadAsset<T>(_path, _callback, this.loadGroup, false, !this.asyncLoad, false);
	}

	// Token: 0x06008FED RID: 36845 RVA: 0x00361C5C File Offset: 0x0035FE5C
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

	// Token: 0x06008FEE RID: 36846 RVA: 0x00361CDC File Offset: 0x0035FEDC
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

	// Token: 0x06008FEF RID: 36847 RVA: 0x00361E48 File Offset: 0x00360048
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

	// Token: 0x06008FF0 RID: 36848 RVA: 0x00361EA7 File Offset: 0x003600A7
	public Vector2i GetXUiScreenSize()
	{
		return new Vector2i(new Vector2((float)this.playerUI.camera.pixelWidth, (float)this.playerUI.camera.pixelHeight) * this.GetPixelRatioFactor());
	}

	// Token: 0x06008FF1 RID: 36849 RVA: 0x00361EE0 File Offset: 0x003600E0
	public Vector2i GetMouseXUiPosition()
	{
		Vector3 vector = this.playerUI.CursorController.GetLocalScreenPosition();
		return this.TranslateScreenVectorToXuiVector(vector);
	}

	// Token: 0x06008FF2 RID: 36850 RVA: 0x00361F10 File Offset: 0x00360110
	public Vector2i TranslateScreenVectorToXuiVector(Vector2 _screenSpaceVector)
	{
		_screenSpaceVector.x -= (float)this.playerUI.camera.pixelWidth / 2f;
		_screenSpaceVector.y -= (float)this.playerUI.camera.pixelHeight / 2f;
		return new Vector2i(_screenSpaceVector * this.GetPixelRatioFactor());
	}

	// Token: 0x06008FF3 RID: 36851 RVA: 0x00361F74 File Offset: 0x00360174
	public Vector3 TranslateScreenVectorToXuiVector(Vector3 _screenSpaceVector)
	{
		_screenSpaceVector.x -= (float)this.playerUI.camera.pixelWidth / 2f;
		_screenSpaceVector.y -= (float)this.playerUI.camera.pixelHeight / 2f;
		return _screenSpaceVector * this.GetPixelRatioFactor();
	}

	// Token: 0x06008FF4 RID: 36852 RVA: 0x00361FD4 File Offset: 0x003601D4
	public Bounds GetXUiWindowScreenBounds(Transform _xuiElement, bool _includeInactive = false)
	{
		Bounds xuiWindowWorldBounds = XUi.getXUiWindowWorldBounds(_xuiElement, _includeInactive);
		Bounds bounds = new Bounds(this.playerUI.camera.WorldToScreenPoint(xuiWindowWorldBounds.min), Vector3.zero);
		bounds.Encapsulate(this.playerUI.camera.WorldToScreenPoint(xuiWindowWorldBounds.max));
		return bounds;
	}

	// Token: 0x06008FF5 RID: 36853 RVA: 0x0036202C File Offset: 0x0036022C
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

	// Token: 0x1700117D RID: 4477
	// (get) Token: 0x06008FF6 RID: 36854 RVA: 0x003620FC File Offset: 0x003602FC
	public Transform StackPanelTransform
	{
		get
		{
			return this.stackPanelRoot;
		}
	}

	// Token: 0x06008FF7 RID: 36855 RVA: 0x00362104 File Offset: 0x00360304
	[PublicizedFrom(EAccessModifier.Private)]
	public void initStackPanels()
	{
		this.stackPanelRoot = base.transform.Find("StackPanels").transform;
		this.StackPanels.Add("Left", new XUi.StackPanel("Left", XUi.EStackPanelPos.Left, base.transform.Find("StackPanels/Left").transform));
		this.StackPanels.Add("Center", new XUi.StackPanel("Center", XUi.EStackPanelPos.Center, base.transform.Find("StackPanels/Center").transform));
		this.StackPanels.Add("Right", new XUi.StackPanel("Right", XUi.EStackPanelPos.Right, base.transform.Find("StackPanels/Right").transform));
	}

	// Token: 0x06008FF8 RID: 36856 RVA: 0x003621BC File Offset: 0x003603BC
	public void SetStackPanelScale(float _scale)
	{
		this.defaultStackPanelScale = _scale;
		this.stackPanelRoot.localScale = Vector3.one * _scale;
		this.stackPanelRoot.localPosition = Vector3.zero;
	}

	// Token: 0x06008FF9 RID: 36857 RVA: 0x003621EB File Offset: 0x003603EB
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

	// Token: 0x06008FFA RID: 36858 RVA: 0x00362226 File Offset: 0x00360426
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

	// Token: 0x06008FFB RID: 36859 RVA: 0x0036223C File Offset: 0x0036043C
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

	// Token: 0x06008FFC RID: 36860 RVA: 0x00362298 File Offset: 0x00360498
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

	// Token: 0x06008FFD RID: 36861 RVA: 0x003624C4 File Offset: 0x003606C4
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

	// Token: 0x06008FFE RID: 36862 RVA: 0x0036256C File Offset: 0x0036076C
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

	// Token: 0x06008FFF RID: 36863 RVA: 0x003625EC File Offset: 0x003607EC
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

	// Token: 0x06009000 RID: 36864 RVA: 0x0036263B File Offset: 0x0036083B
	public void PlayMenuScrollSound()
	{
		Manager.PlayXUiSound(this.uiScrollSound, this.uiScrollVolume);
	}

	// Token: 0x06009001 RID: 36865 RVA: 0x0036264E File Offset: 0x0036084E
	public void PlayMenuClickSound()
	{
		Manager.PlayXUiSound(this.uiClickSound, this.uiClickVolume);
	}

	// Token: 0x06009002 RID: 36866 RVA: 0x00362661 File Offset: 0x00360861
	public void PlayMenuConfirmSound()
	{
		Manager.PlayXUiSound(this.uiConfirmSound, this.uiConfirmVolume);
	}

	// Token: 0x06009003 RID: 36867 RVA: 0x00362674 File Offset: 0x00360874
	public void PlayMenuBackSound()
	{
		Manager.PlayXUiSound(this.uiBackSound, this.uiBackVolume);
	}

	// Token: 0x06009004 RID: 36868 RVA: 0x00362687 File Offset: 0x00360887
	public void PlayMenuSliderSound()
	{
		Manager.PlayXUiSound(this.uiSliderSound, this.uiSliderVolume);
	}

	// Token: 0x06009005 RID: 36869 RVA: 0x0036269C File Offset: 0x0036089C
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

	// Token: 0x06009006 RID: 36870 RVA: 0x003626E8 File Offset: 0x003608E8
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

	// Token: 0x06009007 RID: 36871 RVA: 0x00362738 File Offset: 0x00360938
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

	// Token: 0x06009008 RID: 36872 RVA: 0x0036277C File Offset: 0x0036097C
	public List<XUiController> GetChildrenById(string _id)
	{
		List<XUiController> list = new List<XUiController>();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.GetChildrenById(_id, list);
		}
		return list;
	}

	// Token: 0x06009009 RID: 36873 RVA: 0x003627C0 File Offset: 0x003609C0
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

	// Token: 0x0600900A RID: 36874 RVA: 0x00362810 File Offset: 0x00360A10
	public List<T> GetChildrenByType<T>() where T : XUiController
	{
		List<T> list = new List<T>();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.GetChildrenByType<T>(list);
		}
		return list;
	}

	// Token: 0x0600900B RID: 36875 RVA: 0x00362854 File Offset: 0x00360A54
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

	// Token: 0x0600900C RID: 36876 RVA: 0x003628C4 File Offset: 0x00360AC4
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

	// Token: 0x0600900D RID: 36877 RVA: 0x00362940 File Offset: 0x00360B40
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

	// Token: 0x0600900E RID: 36878 RVA: 0x003629A4 File Offset: 0x00360BA4
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

	// Token: 0x0600900F RID: 36879 RVA: 0x00362A10 File Offset: 0x00360C10
	[PublicizedFrom(EAccessModifier.Private)]
	public void initAtlases()
	{
		MultiSourceAtlasManager[] array = Resources.FindObjectsOfTypeAll<MultiSourceAtlasManager>();
		for (int i = 0; i < array.Length; i++)
		{
			this.allMultiSourceAtlases.Add(array[i].name, array[i]);
		}
	}

	// Token: 0x06009010 RID: 36880 RVA: 0x00362A48 File Offset: 0x00360C48
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

	// Token: 0x06009011 RID: 36881 RVA: 0x00362A80 File Offset: 0x00360C80
	[PublicizedFrom(EAccessModifier.Private)]
	public void initFonts()
	{
		this.fontsByName = new CaseInsensitiveStringDictionary<NGUIFont>();
		foreach (NGUIFont nguifont in this.NGUIFonts)
		{
			this.fontsByName.Add(nguifont.name, nguifont);
		}
	}

	// Token: 0x06009012 RID: 36882 RVA: 0x00362AC4 File Offset: 0x00360CC4
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

	// Token: 0x06009013 RID: 36883 RVA: 0x00362B04 File Offset: 0x00360D04
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

	// Token: 0x06009014 RID: 36884 RVA: 0x00362B40 File Offset: 0x00360D40
	public UIAnchor GetAnchor(UIAnchor.Side _anchorSide)
	{
		return this.xuiAnchors[(int)_anchorSide];
	}

	// Token: 0x06009015 RID: 36885 RVA: 0x00362B4C File Offset: 0x00360D4C
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

	// Token: 0x06009016 RID: 36886 RVA: 0x00362BCC File Offset: 0x00360DCC
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

	// Token: 0x06009017 RID: 36887 RVA: 0x00362CDC File Offset: 0x00360EDC
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

	// Token: 0x06009018 RID: 36888 RVA: 0x00362D20 File Offset: 0x00360F20
	public void CancelAllCrafting()
	{
		XUiC_RecipeStack[] recipesToCraft = this.FindWindowGroupByName("crafting").GetChildByType<XUiC_CraftingQueue>().GetRecipesToCraft();
		for (int i = 0; i < recipesToCraft.Length; i++)
		{
			recipesToCraft[i].ForceCancel();
		}
	}

	// Token: 0x06009019 RID: 36889 RVA: 0x00362D5C File Offset: 0x00360F5C
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

	// Token: 0x0600901A RID: 36890 RVA: 0x00362E64 File Offset: 0x00361064
	public void SetCraftingData(CraftingData _cd)
	{
		base.StartCoroutine(this.setCraftingDataAsync(_cd));
	}

	// Token: 0x0600901B RID: 36891 RVA: 0x00362E74 File Offset: 0x00361074
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

	// Token: 0x0600901C RID: 36892 RVA: 0x00362E8C File Offset: 0x0036108C
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

	// Token: 0x0600901D RID: 36893 RVA: 0x00363027 File Offset: 0x00361227
	public void UpdateWindowSoftCursorBounds(XUiV_Window _window)
	{
		this.playerUI.CursorController != null;
	}

	// Token: 0x0600901E RID: 36894 RVA: 0x0036303C File Offset: 0x0036123C
	public void RemoveWindowFromSoftCursorBounds(XUiV_Window _window)
	{
		CursorControllerAbs cursorController = this.playerUI.CursorController;
		if (cursorController != null)
		{
			cursorController.RemoveBounds(_window.ID);
		}
	}

	// Token: 0x0600901F RID: 36895 RVA: 0x0036306C File Offset: 0x0036126C
	public void ForceInputStyleChange()
	{
		this.GetOpenWindows(XUi.forceInputStyleChangedWindowsList);
		foreach (XUiV_Window xuiV_Window in XUi.forceInputStyleChangedWindowsList)
		{
			xuiV_Window.Controller.ForceInputStyleChange(PlatformManager.NativePlatform.Input.CurrentInputStyle, PlatformManager.NativePlatform.Input.CurrentInputStyle);
		}
		XUi.forceInputStyleChangedWindowsList.Clear();
	}

	// Token: 0x06009022 RID: 36898 RVA: 0x003631AC File Offset: 0x003613AC
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

	// Token: 0x06009023 RID: 36899 RVA: 0x00363214 File Offset: 0x00361414
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

	// Token: 0x0400692E RID: 26926
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lastScreenHeight;

	// Token: 0x0400692F RID: 26927
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float pixelRatioFactor;

	// Token: 0x04006930 RID: 26928
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static UIRoot uiRoot;

	// Token: 0x04006931 RID: 26929
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool inGameMenuOpen;

	// Token: 0x04006932 RID: 26930
	public NGUIFont[] NGUIFonts;

	// Token: 0x04006933 RID: 26931
	public List<XUiWindowGroup> WindowGroups;

	// Token: 0x04006934 RID: 26932
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Type, List<XUiController>> windowsAndGroupsByControllerType;

	// Token: 0x04006935 RID: 26933
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<XUiV_Window> windows;

	// Token: 0x04006936 RID: 26934
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float backgroundGlobalOpacity = 1f;

	// Token: 0x04006937 RID: 26935
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float foregroundGlobalOpacity = 1f;

	// Token: 0x04006938 RID: 26936
	public XUiM_PlayerInventory PlayerInventory;

	// Token: 0x04006939 RID: 26937
	public XUiM_PlayerEquipment PlayerEquipment;

	// Token: 0x0400693A RID: 26938
	public XUiM_Vehicle Vehicle;

	// Token: 0x0400693B RID: 26939
	public XUiM_AssembleItem AssembleItem;

	// Token: 0x0400693C RID: 26940
	public XUiM_Quest QuestTracker;

	// Token: 0x0400693D RID: 26941
	public XUiM_Recipes Recipes;

	// Token: 0x0400693E RID: 26942
	public XUiM_Trader Trader;

	// Token: 0x0400693F RID: 26943
	public XUiM_Dialog Dialog;

	// Token: 0x04006940 RID: 26944
	public XUiC_BuffPopoutList BuffPopoutList;

	// Token: 0x04006941 RID: 26945
	public XUiC_CollectedItemList CollectedItemList;

	// Token: 0x04006942 RID: 26946
	public XUiC_Radial RadialWindow;

	// Token: 0x04006943 RID: 26947
	public XUiC_GamepadCalloutWindow CalloutWindow;

	// Token: 0x04006958 RID: 26968
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float xuiGlobalScaling = 1.255f;

	// Token: 0x04006959 RID: 26969
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2i lastScreenSize = Vector2i.zero;

	// Token: 0x0400695A RID: 26970
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsReady;

	// Token: 0x0400695B RID: 26971
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool asyncLoad;

	// Token: 0x0400695C RID: 26972
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LoadManager.LoadGroup loadGroup;

	// Token: 0x0400695F RID: 26975
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI mPlayerUI;

	// Token: 0x04006960 RID: 26976
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public long accumElapsedMilliseconds;

	// Token: 0x04006961 RID: 26977
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Coroutine loadAsyncCoroutine;

	// Token: 0x04006962 RID: 26978
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<UIBasicSprite> getXUiWindowWorldBoundsList = new List<UIBasicSprite>();

	// Token: 0x04006963 RID: 26979
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float defaultStackPanelScale = 1f;

	// Token: 0x04006964 RID: 26980
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform stackPanelRoot;

	// Token: 0x04006965 RID: 26981
	public readonly DictionaryList<string, XUi.StackPanel> StackPanels = new DictionaryList<string, XUi.StackPanel>();

	// Token: 0x04006966 RID: 26982
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiScrollSound;

	// Token: 0x04006967 RID: 26983
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiScrollVolume = 0.5f;

	// Token: 0x04006968 RID: 26984
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiClickSound;

	// Token: 0x04006969 RID: 26985
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiClickVolume = 0.5f;

	// Token: 0x0400696A RID: 26986
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiConfirmSound;

	// Token: 0x0400696B RID: 26987
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiConfirmVolume = 0.5f;

	// Token: 0x0400696C RID: 26988
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiBackSound;

	// Token: 0x0400696D RID: 26989
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiBackVolume = 0.5f;

	// Token: 0x0400696E RID: 26990
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiSliderSound;

	// Token: 0x0400696F RID: 26991
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiSliderVolume = 0.25f;

	// Token: 0x04006970 RID: 26992
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, MultiSourceAtlasManager> allMultiSourceAtlases = new CaseInsensitiveStringDictionary<MultiSourceAtlasManager>();

	// Token: 0x04006971 RID: 26993
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CaseInsensitiveStringDictionary<NGUIFont> fontsByName;

	// Token: 0x04006972 RID: 26994
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform anchorRoot;

	// Token: 0x04006973 RID: 26995
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIAnchor[] xuiAnchors;

	// Token: 0x04006974 RID: 26996
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 previousPagingVector = Vector2.zero;

	// Token: 0x04006975 RID: 26997
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float pagingRepeatTimer;

	// Token: 0x04006976 RID: 26998
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool initialPagingInput;

	// Token: 0x04006977 RID: 26999
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<XUiV_Window> forceInputStyleChangedWindowsList = new List<XUiV_Window>();

	// Token: 0x02001196 RID: 4502
	public enum EStackPanelPos
	{
		// Token: 0x04006979 RID: 27001
		Left,
		// Token: 0x0400697A RID: 27002
		Center,
		// Token: 0x0400697B RID: 27003
		Right
	}

	// Token: 0x02001197 RID: 4503
	public class StackPanel
	{
		// Token: 0x06009029 RID: 36905 RVA: 0x0036338D File Offset: 0x0036158D
		public StackPanel(string _name, XUi.EStackPanelPos _position, Transform _transform)
		{
			this.Name = _name;
			this.Position = _position;
			this.Transform = _transform;
		}

		// Token: 0x0400697C RID: 27004
		public readonly string Name;

		// Token: 0x0400697D RID: 27005
		public readonly XUi.EStackPanelPos Position;

		// Token: 0x0400697E RID: 27006
		public readonly Transform Transform;

		// Token: 0x0400697F RID: 27007
		public int WindowCount;

		// Token: 0x04006980 RID: 27008
		public Vector2Int Size;
	}

	// Token: 0x02001198 RID: 4504
	public enum UISoundType
	{
		// Token: 0x04006982 RID: 27010
		ClickSound,
		// Token: 0x04006983 RID: 27011
		ScrollSound,
		// Token: 0x04006984 RID: 27012
		ConfirmSound,
		// Token: 0x04006985 RID: 27013
		BackSound,
		// Token: 0x04006986 RID: 27014
		SliderSound,
		// Token: 0x04006987 RID: 27015
		None
	}
}
