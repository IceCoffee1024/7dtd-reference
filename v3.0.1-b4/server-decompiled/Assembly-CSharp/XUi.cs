using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using Platform;
using UnityEngine;

// Token: 0x02001154 RID: 4436
public class XUi : MonoBehaviour
{
	// Token: 0x170010DC RID: 4316
	// (get) Token: 0x06008DCD RID: 36301 RVA: 0x00356088 File Offset: 0x00354288
	// (set) Token: 0x06008DCE RID: 36302 RVA: 0x00356090 File Offset: 0x00354290
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

	// Token: 0x06008DCF RID: 36303 RVA: 0x003560F1 File Offset: 0x003542F1
	public static bool IsGameRunning()
	{
		return GameManager.Instance != null && GameManager.Instance.World != null;
	}

	// Token: 0x170010DD RID: 4317
	// (get) Token: 0x06008DD0 RID: 36304 RVA: 0x0035610F File Offset: 0x0035430F
	// (set) Token: 0x06008DD1 RID: 36305 RVA: 0x00356117 File Offset: 0x00354317
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

	// Token: 0x170010DE RID: 4318
	// (get) Token: 0x06008DD2 RID: 36306 RVA: 0x00356120 File Offset: 0x00354320
	// (set) Token: 0x06008DD3 RID: 36307 RVA: 0x00356128 File Offset: 0x00354328
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

	// Token: 0x170010DF RID: 4319
	// (get) Token: 0x06008DD4 RID: 36308 RVA: 0x00356131 File Offset: 0x00354331
	// (set) Token: 0x06008DD5 RID: 36309 RVA: 0x00356139 File Offset: 0x00354339
	public XUiC_DragAndDropWindow DragAndDropWindow { get; set; }

	// Token: 0x170010E0 RID: 4320
	// (get) Token: 0x06008DD6 RID: 36310 RVA: 0x00356142 File Offset: 0x00354342
	// (set) Token: 0x06008DD7 RID: 36311 RVA: 0x0035614A File Offset: 0x0035434A
	public XUiC_OnScreenIcons OnScreenIconsWindow { get; set; }

	// Token: 0x170010E1 RID: 4321
	// (get) Token: 0x06008DD8 RID: 36312 RVA: 0x00356153 File Offset: 0x00354353
	// (set) Token: 0x06008DD9 RID: 36313 RVA: 0x0035615B File Offset: 0x0035435B
	public XUiC_ToolTip ToolTipWindow { get; set; }

	// Token: 0x170010E2 RID: 4322
	// (get) Token: 0x06008DDA RID: 36314 RVA: 0x00356164 File Offset: 0x00354364
	// (set) Token: 0x06008DDB RID: 36315 RVA: 0x0035616C File Offset: 0x0035436C
	public XUiC_PopupMenu PopupMenuWindow { get; set; }

	// Token: 0x170010E3 RID: 4323
	// (get) Token: 0x06008DDC RID: 36316 RVA: 0x00356175 File Offset: 0x00354375
	// (set) Token: 0x06008DDD RID: 36317 RVA: 0x0035617D File Offset: 0x0035437D
	public XUiC_SaveIndicator SaveIndicator { get; set; }

	// Token: 0x170010E4 RID: 4324
	// (get) Token: 0x06008DDE RID: 36318 RVA: 0x00356186 File Offset: 0x00354386
	// (set) Token: 0x06008DDF RID: 36319 RVA: 0x0035618E File Offset: 0x0035438E
	public XUiC_WorkstationToolGrid CurrentWorkstationToolGrid { get; set; }

	// Token: 0x170010E5 RID: 4325
	// (get) Token: 0x06008DE0 RID: 36320 RVA: 0x00356197 File Offset: 0x00354397
	// (set) Token: 0x06008DE1 RID: 36321 RVA: 0x0035619F File Offset: 0x0035439F
	public XUiC_WorkstationFuelGrid CurrentWorkstationFuelGrid { get; set; }

	// Token: 0x170010E6 RID: 4326
	// (get) Token: 0x06008DE2 RID: 36322 RVA: 0x003561A8 File Offset: 0x003543A8
	// (set) Token: 0x06008DE3 RID: 36323 RVA: 0x003561B0 File Offset: 0x003543B0
	public XUiC_WorkstationInputGrid CurrentWorkstationInputGrid { get; set; }

	// Token: 0x170010E7 RID: 4327
	// (get) Token: 0x06008DE4 RID: 36324 RVA: 0x003561B9 File Offset: 0x003543B9
	// (set) Token: 0x06008DE5 RID: 36325 RVA: 0x003561C1 File Offset: 0x003543C1
	public XUiC_WorkstationOutputGrid CurrentWorkstationOutputGrid { get; set; }

	// Token: 0x170010E8 RID: 4328
	// (get) Token: 0x06008DE6 RID: 36326 RVA: 0x003561CA File Offset: 0x003543CA
	// (set) Token: 0x06008DE7 RID: 36327 RVA: 0x003561D2 File Offset: 0x003543D2
	public XUiC_DewCollectorModGrid CurrentCollectorModGrid { get; set; }

	// Token: 0x170010E9 RID: 4329
	// (get) Token: 0x06008DE8 RID: 36328 RVA: 0x003561DB File Offset: 0x003543DB
	// (set) Token: 0x06008DE9 RID: 36329 RVA: 0x003561E3 File Offset: 0x003543E3
	public XUiC_CollectorFuelGrid CurrentCollectorFuelGrid { get; set; }

	// Token: 0x170010EA RID: 4330
	// (get) Token: 0x06008DEA RID: 36330 RVA: 0x003561EC File Offset: 0x003543EC
	// (set) Token: 0x06008DEB RID: 36331 RVA: 0x003561F4 File Offset: 0x003543F4
	public XUiC_CollectorFuelGrid currentCollectorCatalystGrid { get; set; }

	// Token: 0x170010EB RID: 4331
	// (get) Token: 0x06008DEC RID: 36332 RVA: 0x003561FD File Offset: 0x003543FD
	// (set) Token: 0x06008DED RID: 36333 RVA: 0x00356205 File Offset: 0x00354405
	public XUiC_CombineGrid CurrentCombineGrid { get; set; }

	// Token: 0x170010EC RID: 4332
	// (get) Token: 0x06008DEE RID: 36334 RVA: 0x0035620E File Offset: 0x0035440E
	// (set) Token: 0x06008DEF RID: 36335 RVA: 0x00356216 File Offset: 0x00354416
	public XUiC_PowerSourceSlots CurrentPowerSourceSlots { get; set; }

	// Token: 0x170010ED RID: 4333
	// (get) Token: 0x06008DF0 RID: 36336 RVA: 0x0035621F File Offset: 0x0035441F
	// (set) Token: 0x06008DF1 RID: 36337 RVA: 0x00356227 File Offset: 0x00354427
	public XUiC_PowerRangedAmmoSlots CurrentPowerAmmoSlots { get; set; }

	// Token: 0x170010EE RID: 4334
	// (get) Token: 0x06008DF2 RID: 36338 RVA: 0x00356230 File Offset: 0x00354430
	// (set) Token: 0x06008DF3 RID: 36339 RVA: 0x00356238 File Offset: 0x00354438
	public XUiC_SelectableEntry CurrentSelectedEntry { get; set; }

	// Token: 0x170010EF RID: 4335
	// (get) Token: 0x06008DF4 RID: 36340 RVA: 0x00356241 File Offset: 0x00354441
	// (set) Token: 0x06008DF5 RID: 36341 RVA: 0x00356249 File Offset: 0x00354449
	public ProgressionValue SelectedSkill { get; set; }

	// Token: 0x170010F0 RID: 4336
	// (get) Token: 0x06008DF6 RID: 36342 RVA: 0x00356252 File Offset: 0x00354452
	// (set) Token: 0x06008DF7 RID: 36343 RVA: 0x0035625A File Offset: 0x0035445A
	public ITileEntityLootable LootContainer { get; set; }

	// Token: 0x170010F1 RID: 4337
	// (get) Token: 0x06008DF8 RID: 36344 RVA: 0x00356263 File Offset: 0x00354463
	// (set) Token: 0x06008DF9 RID: 36345 RVA: 0x0035626B File Offset: 0x0035446B
	public bool IsUsingItemActionEntryUse { get; set; }

	// Token: 0x170010F2 RID: 4338
	// (get) Token: 0x06008DFA RID: 36346 RVA: 0x00356274 File Offset: 0x00354474
	// (set) Token: 0x06008DFB RID: 36347 RVA: 0x0035627C File Offset: 0x0035447C
	public bool IsUsingItemActionEntryPromptComplete { get; set; }

	// Token: 0x170010F3 RID: 4339
	// (get) Token: 0x06008DFC RID: 36348 RVA: 0x00356285 File Offset: 0x00354485
	// (set) Token: 0x06008DFD RID: 36349 RVA: 0x0035628D File Offset: 0x0035448D
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
	// (add) Token: 0x06008DFE RID: 36350 RVA: 0x003562B0 File Offset: 0x003544B0
	// (remove) Token: 0x06008DFF RID: 36351 RVA: 0x003562E8 File Offset: 0x003544E8
	public event Action OnShutdown;

	// Token: 0x140000EF RID: 239
	// (add) Token: 0x06008E00 RID: 36352 RVA: 0x00356320 File Offset: 0x00354520
	// (remove) Token: 0x06008E01 RID: 36353 RVA: 0x00356358 File Offset: 0x00354558
	public event Action OnBuilt;

	// Token: 0x170010F4 RID: 4340
	// (get) Token: 0x06008E02 RID: 36354 RVA: 0x0035638D File Offset: 0x0035458D
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

	// Token: 0x06008E03 RID: 36355 RVA: 0x003563B0 File Offset: 0x003545B0
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

	// Token: 0x06008E04 RID: 36356 RVA: 0x0035642D File Offset: 0x0035462D
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

	// Token: 0x06008E05 RID: 36357 RVA: 0x00356468 File Offset: 0x00354668
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

	// Token: 0x06008E06 RID: 36358 RVA: 0x00356554 File Offset: 0x00354754
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

	// Token: 0x06008E07 RID: 36359 RVA: 0x00356600 File Offset: 0x00354800
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

	// Token: 0x06008E08 RID: 36360 RVA: 0x0035660F File Offset: 0x0035480F
	public void Awake()
	{
		this.WindowGroups = new List<XUiWindowGroup>();
		this.windowsAndGroupsByControllerType = new Dictionary<Type, List<XUiController>>();
		this.initFonts();
	}

	// Token: 0x06008E09 RID: 36361 RVA: 0x00356630 File Offset: 0x00354830
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

	// Token: 0x06008E0A RID: 36362 RVA: 0x0035674C File Offset: 0x0035494C
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

	// Token: 0x06008E0B RID: 36363 RVA: 0x0035679B File Offset: 0x0035499B
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

	// Token: 0x06008E0C RID: 36364 RVA: 0x003567B4 File Offset: 0x003549B4
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

	// Token: 0x06008E0D RID: 36365 RVA: 0x00356874 File Offset: 0x00354A74
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.ResolutionChanged -= this.OnResolutionChanged;
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
		LocalPlayerManager.OnLocalPlayersChanged -= this.handleLocalPlayersChanged;
		this.Shutdown(false);
	}

	// Token: 0x06008E0E RID: 36366 RVA: 0x003568B0 File Offset: 0x00354AB0
	public void Shutdown(bool _destroyImmediate = false)
	{
		Action onShutdown = this.OnShutdown;
		if (onShutdown != null)
		{
			onShutdown();
		}
		this.Cleanup(_destroyImmediate);
	}

	// Token: 0x06008E0F RID: 36367 RVA: 0x003568CC File Offset: 0x00354ACC
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

	// Token: 0x06008E10 RID: 36368 RVA: 0x00356938 File Offset: 0x00354B38
	public void CancelLoading()
	{
		if (this.loadAsyncCoroutine != null)
		{
			ThreadManager.StopCoroutine(this.loadAsyncCoroutine);
			this.loadAsyncCoroutine = null;
		}
	}

	// Token: 0x06008E11 RID: 36369 RVA: 0x00356954 File Offset: 0x00354B54
	public void StopAllVideo()
	{
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			XUi.<StopAllVideo>g__StopVideo|145_0(this.WindowGroups[i].Controller);
		}
	}

	// Token: 0x06008E12 RID: 36370 RVA: 0x00356990 File Offset: 0x00354B90
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

	// Token: 0x06008E13 RID: 36371 RVA: 0x003569CE File Offset: 0x00354BCE
	public void AddWindow(XUiV_Window _window)
	{
		this.windows.Add(_window);
	}

	// Token: 0x06008E14 RID: 36372 RVA: 0x003569DC File Offset: 0x00354BDC
	public void RemoveWindow(XUiV_Window _window)
	{
		this.windows.Remove(_window);
	}

	// Token: 0x06008E15 RID: 36373 RVA: 0x003569EC File Offset: 0x00354BEC
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

	// Token: 0x06008E16 RID: 36374 RVA: 0x00356A4C File Offset: 0x00354C4C
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

	// Token: 0x06008E17 RID: 36375 RVA: 0x00356ACA File Offset: 0x00354CCA
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnResolutionChanged(int _arg1, int _arg2)
	{
		ThreadManager.StartCoroutine(this.delayedScaleUpdate());
	}

	// Token: 0x06008E18 RID: 36376 RVA: 0x00356AD8 File Offset: 0x00354CD8
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator delayedScaleUpdate()
	{
		yield return null;
		this.SetScale(-1f);
		yield break;
	}

	// Token: 0x06008E19 RID: 36377 RVA: 0x00356AE8 File Offset: 0x00354CE8
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

	// Token: 0x06008E1A RID: 36378 RVA: 0x00356B4E File Offset: 0x00354D4E
	public float GetScale()
	{
		return this.xuiGlobalScaling;
	}

	// Token: 0x06008E1B RID: 36379 RVA: 0x00356B56 File Offset: 0x00354D56
	public void LoadData<T>(string _path, Action<T> _callback) where T : global::UnityEngine.Object
	{
		LoadManager.LoadAsset<T>(_path, _callback, this.loadGroup, false, !this.asyncLoad, false);
	}

	// Token: 0x06008E1C RID: 36380 RVA: 0x00356B74 File Offset: 0x00354D74
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

	// Token: 0x06008E1D RID: 36381 RVA: 0x00356BF4 File Offset: 0x00354DF4
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

	// Token: 0x06008E1E RID: 36382 RVA: 0x00356D60 File Offset: 0x00354F60
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

	// Token: 0x06008E1F RID: 36383 RVA: 0x00356DBF File Offset: 0x00354FBF
	public Vector2i GetXUiScreenSize()
	{
		return new Vector2i(new Vector2((float)this.playerUI.camera.pixelWidth, (float)this.playerUI.camera.pixelHeight) * this.GetPixelRatioFactor());
	}

	// Token: 0x06008E20 RID: 36384 RVA: 0x00356DF8 File Offset: 0x00354FF8
	public Vector2i GetMouseXUiPosition()
	{
		Vector3 vector = this.playerUI.CursorController.GetLocalScreenPosition();
		return this.TranslateScreenVectorToXuiVector(vector);
	}

	// Token: 0x06008E21 RID: 36385 RVA: 0x00356E28 File Offset: 0x00355028
	public Vector2i TranslateScreenVectorToXuiVector(Vector2 _screenSpaceVector)
	{
		_screenSpaceVector.x -= (float)this.playerUI.camera.pixelWidth / 2f;
		_screenSpaceVector.y -= (float)this.playerUI.camera.pixelHeight / 2f;
		return new Vector2i(_screenSpaceVector * this.GetPixelRatioFactor());
	}

	// Token: 0x06008E22 RID: 36386 RVA: 0x00356E8C File Offset: 0x0035508C
	public Vector3 TranslateScreenVectorToXuiVector(Vector3 _screenSpaceVector)
	{
		_screenSpaceVector.x -= (float)this.playerUI.camera.pixelWidth / 2f;
		_screenSpaceVector.y -= (float)this.playerUI.camera.pixelHeight / 2f;
		return _screenSpaceVector * this.GetPixelRatioFactor();
	}

	// Token: 0x06008E23 RID: 36387 RVA: 0x00356EEC File Offset: 0x003550EC
	public Bounds GetXUiWindowScreenBounds(Transform _xuiElement, bool _includeInactive = false)
	{
		Bounds xuiWindowWorldBounds = XUi.getXUiWindowWorldBounds(_xuiElement, _includeInactive);
		Bounds bounds = new Bounds(this.playerUI.camera.WorldToScreenPoint(xuiWindowWorldBounds.min), Vector3.zero);
		bounds.Encapsulate(this.playerUI.camera.WorldToScreenPoint(xuiWindowWorldBounds.max));
		return bounds;
	}

	// Token: 0x06008E24 RID: 36388 RVA: 0x00356F44 File Offset: 0x00355144
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

	// Token: 0x170010F5 RID: 4341
	// (get) Token: 0x06008E25 RID: 36389 RVA: 0x00357014 File Offset: 0x00355214
	public Transform StackPanelTransform
	{
		get
		{
			return this.stackPanelRoot;
		}
	}

	// Token: 0x06008E26 RID: 36390 RVA: 0x0035701C File Offset: 0x0035521C
	[PublicizedFrom(EAccessModifier.Private)]
	public void initStackPanels()
	{
		this.stackPanelRoot = base.transform.Find("StackPanels").transform;
		this.StackPanels.Add("Left", new XUi.StackPanel("Left", XUi.EStackPanelPos.Left, base.transform.Find("StackPanels/Left").transform));
		this.StackPanels.Add("Center", new XUi.StackPanel("Center", XUi.EStackPanelPos.Center, base.transform.Find("StackPanels/Center").transform));
		this.StackPanels.Add("Right", new XUi.StackPanel("Right", XUi.EStackPanelPos.Right, base.transform.Find("StackPanels/Right").transform));
	}

	// Token: 0x06008E27 RID: 36391 RVA: 0x003570D4 File Offset: 0x003552D4
	public void SetStackPanelScale(float _scale)
	{
		this.defaultStackPanelScale = _scale;
		this.stackPanelRoot.localScale = Vector3.one * _scale;
		this.stackPanelRoot.localPosition = Vector3.zero;
	}

	// Token: 0x06008E28 RID: 36392 RVA: 0x00357103 File Offset: 0x00355303
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

	// Token: 0x06008E29 RID: 36393 RVA: 0x0035713E File Offset: 0x0035533E
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

	// Token: 0x06008E2A RID: 36394 RVA: 0x00357154 File Offset: 0x00355354
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

	// Token: 0x06008E2B RID: 36395 RVA: 0x003571B0 File Offset: 0x003553B0
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

	// Token: 0x06008E2C RID: 36396 RVA: 0x003573DC File Offset: 0x003555DC
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

	// Token: 0x06008E2D RID: 36397 RVA: 0x00357484 File Offset: 0x00355684
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

	// Token: 0x06008E2E RID: 36398 RVA: 0x00357504 File Offset: 0x00355704
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

	// Token: 0x06008E2F RID: 36399 RVA: 0x00357553 File Offset: 0x00355753
	public void PlayMenuScrollSound()
	{
		Manager.PlayXUiSound(this.uiScrollSound, this.uiScrollVolume);
	}

	// Token: 0x06008E30 RID: 36400 RVA: 0x00357566 File Offset: 0x00355766
	public void PlayMenuClickSound()
	{
		Manager.PlayXUiSound(this.uiClickSound, this.uiClickVolume);
	}

	// Token: 0x06008E31 RID: 36401 RVA: 0x00357579 File Offset: 0x00355779
	public void PlayMenuConfirmSound()
	{
		Manager.PlayXUiSound(this.uiConfirmSound, this.uiConfirmVolume);
	}

	// Token: 0x06008E32 RID: 36402 RVA: 0x0035758C File Offset: 0x0035578C
	public void PlayMenuBackSound()
	{
		Manager.PlayXUiSound(this.uiBackSound, this.uiBackVolume);
	}

	// Token: 0x06008E33 RID: 36403 RVA: 0x0035759F File Offset: 0x0035579F
	public void PlayMenuSliderSound()
	{
		Manager.PlayXUiSound(this.uiSliderSound, this.uiSliderVolume);
	}

	// Token: 0x06008E34 RID: 36404 RVA: 0x003575B4 File Offset: 0x003557B4
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

	// Token: 0x06008E35 RID: 36405 RVA: 0x00357600 File Offset: 0x00355800
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

	// Token: 0x06008E36 RID: 36406 RVA: 0x00357650 File Offset: 0x00355850
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

	// Token: 0x06008E37 RID: 36407 RVA: 0x00357694 File Offset: 0x00355894
	public List<XUiController> GetChildrenById(string _id)
	{
		List<XUiController> list = new List<XUiController>();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.GetChildrenById(_id, list);
		}
		return list;
	}

	// Token: 0x06008E38 RID: 36408 RVA: 0x003576D8 File Offset: 0x003558D8
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

	// Token: 0x06008E39 RID: 36409 RVA: 0x00357728 File Offset: 0x00355928
	public List<T> GetChildrenByType<T>() where T : XUiController
	{
		List<T> list = new List<T>();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.GetChildrenByType<T>(list);
		}
		return list;
	}

	// Token: 0x06008E3A RID: 36410 RVA: 0x0035776C File Offset: 0x0035596C
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

	// Token: 0x06008E3B RID: 36411 RVA: 0x003577DC File Offset: 0x003559DC
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

	// Token: 0x06008E3C RID: 36412 RVA: 0x00357858 File Offset: 0x00355A58
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

	// Token: 0x06008E3D RID: 36413 RVA: 0x003578BC File Offset: 0x00355ABC
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

	// Token: 0x06008E3E RID: 36414 RVA: 0x00357928 File Offset: 0x00355B28
	[PublicizedFrom(EAccessModifier.Private)]
	public void initAtlases()
	{
		MultiSourceAtlasManager[] array = Resources.FindObjectsOfTypeAll<MultiSourceAtlasManager>();
		for (int i = 0; i < array.Length; i++)
		{
			this.allMultiSourceAtlases.Add(array[i].name, array[i]);
		}
	}

	// Token: 0x06008E3F RID: 36415 RVA: 0x00357960 File Offset: 0x00355B60
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

	// Token: 0x06008E40 RID: 36416 RVA: 0x00357998 File Offset: 0x00355B98
	[PublicizedFrom(EAccessModifier.Private)]
	public void initFonts()
	{
		this.fontsByName = new CaseInsensitiveStringDictionary<NGUIFont>();
		foreach (NGUIFont nguifont in this.NGUIFonts)
		{
			this.fontsByName.Add(nguifont.name, nguifont);
		}
	}

	// Token: 0x06008E41 RID: 36417 RVA: 0x003579DC File Offset: 0x00355BDC
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

	// Token: 0x06008E42 RID: 36418 RVA: 0x00357A1C File Offset: 0x00355C1C
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

	// Token: 0x06008E43 RID: 36419 RVA: 0x00357A58 File Offset: 0x00355C58
	public UIAnchor GetAnchor(UIAnchor.Side _anchorSide)
	{
		return this.xuiAnchors[(int)_anchorSide];
	}

	// Token: 0x06008E44 RID: 36420 RVA: 0x00357A64 File Offset: 0x00355C64
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

	// Token: 0x06008E45 RID: 36421 RVA: 0x00357AE4 File Offset: 0x00355CE4
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

	// Token: 0x06008E46 RID: 36422 RVA: 0x00357BF4 File Offset: 0x00355DF4
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

	// Token: 0x06008E47 RID: 36423 RVA: 0x00357C38 File Offset: 0x00355E38
	public void CancelAllCrafting()
	{
		XUiC_RecipeStack[] recipesToCraft = this.FindWindowGroupByName("crafting").GetChildByType<XUiC_CraftingQueue>().GetRecipesToCraft();
		for (int i = 0; i < recipesToCraft.Length; i++)
		{
			recipesToCraft[i].ForceCancel();
		}
	}

	// Token: 0x06008E48 RID: 36424 RVA: 0x00357C74 File Offset: 0x00355E74
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

	// Token: 0x06008E49 RID: 36425 RVA: 0x00357D7C File Offset: 0x00355F7C
	public void SetCraftingData(CraftingData _cd)
	{
		base.StartCoroutine(this.setCraftingDataAsync(_cd));
	}

	// Token: 0x06008E4A RID: 36426 RVA: 0x00357D8C File Offset: 0x00355F8C
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

	// Token: 0x06008E4B RID: 36427 RVA: 0x00357DA4 File Offset: 0x00355FA4
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

	// Token: 0x06008E4C RID: 36428 RVA: 0x00357F3F File Offset: 0x0035613F
	public void UpdateWindowSoftCursorBounds(XUiV_Window _window)
	{
		this.playerUI.CursorController != null;
	}

	// Token: 0x06008E4D RID: 36429 RVA: 0x00357F54 File Offset: 0x00356154
	public void RemoveWindowFromSoftCursorBounds(XUiV_Window _window)
	{
		CursorControllerAbs cursorController = this.playerUI.CursorController;
		if (cursorController != null)
		{
			cursorController.RemoveBounds(_window.ID);
		}
	}

	// Token: 0x06008E4E RID: 36430 RVA: 0x00357F84 File Offset: 0x00356184
	public void ForceInputStyleChange()
	{
		this.GetOpenWindows(XUi.forceInputStyleChangedWindowsList);
		foreach (XUiV_Window xuiV_Window in XUi.forceInputStyleChangedWindowsList)
		{
			xuiV_Window.Controller.ForceInputStyleChange(PlatformManager.NativePlatform.Input.CurrentInputStyle, PlatformManager.NativePlatform.Input.CurrentInputStyle);
		}
		XUi.forceInputStyleChangedWindowsList.Clear();
	}

	// Token: 0x06008E51 RID: 36433 RVA: 0x003580C4 File Offset: 0x003562C4
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

	// Token: 0x06008E52 RID: 36434 RVA: 0x0035812C File Offset: 0x0035632C
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

	// Token: 0x040067BE RID: 26558
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lastScreenHeight;

	// Token: 0x040067BF RID: 26559
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float pixelRatioFactor;

	// Token: 0x040067C0 RID: 26560
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static UIRoot uiRoot;

	// Token: 0x040067C1 RID: 26561
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool inGameMenuOpen;

	// Token: 0x040067C2 RID: 26562
	public NGUIFont[] NGUIFonts;

	// Token: 0x040067C3 RID: 26563
	public List<XUiWindowGroup> WindowGroups;

	// Token: 0x040067C4 RID: 26564
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Type, List<XUiController>> windowsAndGroupsByControllerType;

	// Token: 0x040067C5 RID: 26565
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<XUiV_Window> windows;

	// Token: 0x040067C6 RID: 26566
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float backgroundGlobalOpacity = 1f;

	// Token: 0x040067C7 RID: 26567
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float foregroundGlobalOpacity = 1f;

	// Token: 0x040067C8 RID: 26568
	public XUiM_PlayerInventory PlayerInventory;

	// Token: 0x040067C9 RID: 26569
	public XUiM_PlayerEquipment PlayerEquipment;

	// Token: 0x040067CA RID: 26570
	public XUiM_Vehicle Vehicle;

	// Token: 0x040067CB RID: 26571
	public XUiM_AssembleItem AssembleItem;

	// Token: 0x040067CC RID: 26572
	public XUiM_Quest QuestTracker;

	// Token: 0x040067CD RID: 26573
	public XUiM_Recipes Recipes;

	// Token: 0x040067CE RID: 26574
	public XUiM_Trader Trader;

	// Token: 0x040067CF RID: 26575
	public XUiM_Dialog Dialog;

	// Token: 0x040067D0 RID: 26576
	public XUiC_BuffPopoutList BuffPopoutList;

	// Token: 0x040067D1 RID: 26577
	public XUiC_CollectedItemList CollectedItemList;

	// Token: 0x040067D2 RID: 26578
	public XUiC_Radial RadialWindow;

	// Token: 0x040067D3 RID: 26579
	public XUiC_GamepadCalloutWindow CalloutWindow;

	// Token: 0x040067E8 RID: 26600
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float xuiGlobalScaling = 1.255f;

	// Token: 0x040067E9 RID: 26601
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2i lastScreenSize = Vector2i.zero;

	// Token: 0x040067EA RID: 26602
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsReady;

	// Token: 0x040067EB RID: 26603
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool asyncLoad;

	// Token: 0x040067EC RID: 26604
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LoadManager.LoadGroup loadGroup;

	// Token: 0x040067EF RID: 26607
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI mPlayerUI;

	// Token: 0x040067F0 RID: 26608
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public long accumElapsedMilliseconds;

	// Token: 0x040067F1 RID: 26609
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Coroutine loadAsyncCoroutine;

	// Token: 0x040067F2 RID: 26610
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<UIBasicSprite> getXUiWindowWorldBoundsList = new List<UIBasicSprite>();

	// Token: 0x040067F3 RID: 26611
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float defaultStackPanelScale = 1f;

	// Token: 0x040067F4 RID: 26612
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform stackPanelRoot;

	// Token: 0x040067F5 RID: 26613
	public readonly DictionaryList<string, XUi.StackPanel> StackPanels = new DictionaryList<string, XUi.StackPanel>();

	// Token: 0x040067F6 RID: 26614
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiScrollSound;

	// Token: 0x040067F7 RID: 26615
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiScrollVolume = 0.5f;

	// Token: 0x040067F8 RID: 26616
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiClickSound;

	// Token: 0x040067F9 RID: 26617
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiClickVolume = 0.5f;

	// Token: 0x040067FA RID: 26618
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiConfirmSound;

	// Token: 0x040067FB RID: 26619
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiConfirmVolume = 0.5f;

	// Token: 0x040067FC RID: 26620
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiBackSound;

	// Token: 0x040067FD RID: 26621
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiBackVolume = 0.5f;

	// Token: 0x040067FE RID: 26622
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiSliderSound;

	// Token: 0x040067FF RID: 26623
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiSliderVolume = 0.25f;

	// Token: 0x04006800 RID: 26624
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, MultiSourceAtlasManager> allMultiSourceAtlases = new CaseInsensitiveStringDictionary<MultiSourceAtlasManager>();

	// Token: 0x04006801 RID: 26625
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CaseInsensitiveStringDictionary<NGUIFont> fontsByName;

	// Token: 0x04006802 RID: 26626
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform anchorRoot;

	// Token: 0x04006803 RID: 26627
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIAnchor[] xuiAnchors;

	// Token: 0x04006804 RID: 26628
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 previousPagingVector = Vector2.zero;

	// Token: 0x04006805 RID: 26629
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float pagingRepeatTimer;

	// Token: 0x04006806 RID: 26630
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool initialPagingInput;

	// Token: 0x04006807 RID: 26631
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<XUiV_Window> forceInputStyleChangedWindowsList = new List<XUiV_Window>();

	// Token: 0x02001155 RID: 4437
	public enum EStackPanelPos
	{
		// Token: 0x04006809 RID: 26633
		Left,
		// Token: 0x0400680A RID: 26634
		Center,
		// Token: 0x0400680B RID: 26635
		Right
	}

	// Token: 0x02001156 RID: 4438
	public class StackPanel
	{
		// Token: 0x06008E58 RID: 36440 RVA: 0x003582A5 File Offset: 0x003564A5
		public StackPanel(string _name, XUi.EStackPanelPos _position, Transform _transform)
		{
			this.Name = _name;
			this.Position = _position;
			this.Transform = _transform;
		}

		// Token: 0x0400680C RID: 26636
		public readonly string Name;

		// Token: 0x0400680D RID: 26637
		public readonly XUi.EStackPanelPos Position;

		// Token: 0x0400680E RID: 26638
		public readonly Transform Transform;

		// Token: 0x0400680F RID: 26639
		public int WindowCount;

		// Token: 0x04006810 RID: 26640
		public Vector2Int Size;
	}

	// Token: 0x02001157 RID: 4439
	public enum UISoundType
	{
		// Token: 0x04006812 RID: 26642
		ClickSound,
		// Token: 0x04006813 RID: 26643
		ScrollSound,
		// Token: 0x04006814 RID: 26644
		ConfirmSound,
		// Token: 0x04006815 RID: 26645
		BackSound,
		// Token: 0x04006816 RID: 26646
		SliderSound,
		// Token: 0x04006817 RID: 26647
		None
	}
}
