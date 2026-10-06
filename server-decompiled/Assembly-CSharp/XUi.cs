using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Audio;
using Platform;
using UnityEngine;

// Token: 0x02001185 RID: 4485
public class XUi : MonoBehaviour
{
	// Token: 0x17001159 RID: 4441
	// (get) Token: 0x06008F4A RID: 36682 RVA: 0x0035F080 File Offset: 0x0035D280
	// (set) Token: 0x06008F4B RID: 36683 RVA: 0x0035F088 File Offset: 0x0035D288
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

	// Token: 0x06008F4C RID: 36684 RVA: 0x0035F0E9 File Offset: 0x0035D2E9
	public static bool IsGameRunning()
	{
		return GameManager.Instance != null && GameManager.Instance.World != null;
	}

	// Token: 0x1700115A RID: 4442
	// (get) Token: 0x06008F4D RID: 36685 RVA: 0x0035F107 File Offset: 0x0035D307
	// (set) Token: 0x06008F4E RID: 36686 RVA: 0x0035F10F File Offset: 0x0035D30F
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

	// Token: 0x1700115B RID: 4443
	// (get) Token: 0x06008F4F RID: 36687 RVA: 0x0035F118 File Offset: 0x0035D318
	// (set) Token: 0x06008F50 RID: 36688 RVA: 0x0035F120 File Offset: 0x0035D320
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

	// Token: 0x1700115C RID: 4444
	// (get) Token: 0x06008F51 RID: 36689 RVA: 0x0035F129 File Offset: 0x0035D329
	// (set) Token: 0x06008F52 RID: 36690 RVA: 0x0035F131 File Offset: 0x0035D331
	public XUiC_DragAndDropWindow DragAndDropWindow { get; set; }

	// Token: 0x1700115D RID: 4445
	// (get) Token: 0x06008F53 RID: 36691 RVA: 0x0035F13A File Offset: 0x0035D33A
	// (set) Token: 0x06008F54 RID: 36692 RVA: 0x0035F142 File Offset: 0x0035D342
	public XUiC_OnScreenIcons OnScreenIconsWindow { get; set; }

	// Token: 0x1700115E RID: 4446
	// (get) Token: 0x06008F55 RID: 36693 RVA: 0x0035F14B File Offset: 0x0035D34B
	// (set) Token: 0x06008F56 RID: 36694 RVA: 0x0035F153 File Offset: 0x0035D353
	public XUiC_ToolTip ToolTipWindow { get; set; }

	// Token: 0x1700115F RID: 4447
	// (get) Token: 0x06008F57 RID: 36695 RVA: 0x0035F15C File Offset: 0x0035D35C
	// (set) Token: 0x06008F58 RID: 36696 RVA: 0x0035F164 File Offset: 0x0035D364
	public XUiC_PopupMenu PopupMenuWindow { get; set; }

	// Token: 0x17001160 RID: 4448
	// (get) Token: 0x06008F59 RID: 36697 RVA: 0x0035F16D File Offset: 0x0035D36D
	// (set) Token: 0x06008F5A RID: 36698 RVA: 0x0035F175 File Offset: 0x0035D375
	public XUiC_SaveIndicator SaveIndicator { get; set; }

	// Token: 0x17001161 RID: 4449
	// (get) Token: 0x06008F5B RID: 36699 RVA: 0x0035F17E File Offset: 0x0035D37E
	// (set) Token: 0x06008F5C RID: 36700 RVA: 0x0035F186 File Offset: 0x0035D386
	public XUiC_WorkstationToolGrid CurrentWorkstationToolGrid { get; set; }

	// Token: 0x17001162 RID: 4450
	// (get) Token: 0x06008F5D RID: 36701 RVA: 0x0035F18F File Offset: 0x0035D38F
	// (set) Token: 0x06008F5E RID: 36702 RVA: 0x0035F197 File Offset: 0x0035D397
	public XUiC_WorkstationFuelGrid CurrentWorkstationFuelGrid { get; set; }

	// Token: 0x17001163 RID: 4451
	// (get) Token: 0x06008F5F RID: 36703 RVA: 0x0035F1A0 File Offset: 0x0035D3A0
	// (set) Token: 0x06008F60 RID: 36704 RVA: 0x0035F1A8 File Offset: 0x0035D3A8
	public XUiC_WorkstationInputGrid CurrentWorkstationInputGrid { get; set; }

	// Token: 0x17001164 RID: 4452
	// (get) Token: 0x06008F61 RID: 36705 RVA: 0x0035F1B1 File Offset: 0x0035D3B1
	// (set) Token: 0x06008F62 RID: 36706 RVA: 0x0035F1B9 File Offset: 0x0035D3B9
	public XUiC_WorkstationOutputGrid CurrentWorkstationOutputGrid { get; set; }

	// Token: 0x17001165 RID: 4453
	// (get) Token: 0x06008F63 RID: 36707 RVA: 0x0035F1C2 File Offset: 0x0035D3C2
	// (set) Token: 0x06008F64 RID: 36708 RVA: 0x0035F1CA File Offset: 0x0035D3CA
	public XUiC_DewCollectorModGrid CurrentCollectorModGrid { get; set; }

	// Token: 0x17001166 RID: 4454
	// (get) Token: 0x06008F65 RID: 36709 RVA: 0x0035F1D3 File Offset: 0x0035D3D3
	// (set) Token: 0x06008F66 RID: 36710 RVA: 0x0035F1DB File Offset: 0x0035D3DB
	public XUiC_CollectorFuelGrid CurrentCollectorFuelGrid { get; set; }

	// Token: 0x17001167 RID: 4455
	// (get) Token: 0x06008F67 RID: 36711 RVA: 0x0035F1E4 File Offset: 0x0035D3E4
	// (set) Token: 0x06008F68 RID: 36712 RVA: 0x0035F1EC File Offset: 0x0035D3EC
	public XUiC_CollectorFuelGrid currentCollectorCatalystGrid { get; set; }

	// Token: 0x17001168 RID: 4456
	// (get) Token: 0x06008F69 RID: 36713 RVA: 0x0035F1F5 File Offset: 0x0035D3F5
	// (set) Token: 0x06008F6A RID: 36714 RVA: 0x0035F1FD File Offset: 0x0035D3FD
	public XUiC_CombineGrid CurrentCombineGrid { get; set; }

	// Token: 0x17001169 RID: 4457
	// (get) Token: 0x06008F6B RID: 36715 RVA: 0x0035F206 File Offset: 0x0035D406
	// (set) Token: 0x06008F6C RID: 36716 RVA: 0x0035F20E File Offset: 0x0035D40E
	public XUiC_PowerSourceSlots CurrentPowerSourceSlots { get; set; }

	// Token: 0x1700116A RID: 4458
	// (get) Token: 0x06008F6D RID: 36717 RVA: 0x0035F217 File Offset: 0x0035D417
	// (set) Token: 0x06008F6E RID: 36718 RVA: 0x0035F21F File Offset: 0x0035D41F
	public XUiC_PowerRangedAmmoSlots CurrentPowerAmmoSlots { get; set; }

	// Token: 0x1700116B RID: 4459
	// (get) Token: 0x06008F6F RID: 36719 RVA: 0x0035F228 File Offset: 0x0035D428
	// (set) Token: 0x06008F70 RID: 36720 RVA: 0x0035F230 File Offset: 0x0035D430
	public XUiC_SelectableEntry CurrentSelectedEntry { get; set; }

	// Token: 0x1700116C RID: 4460
	// (get) Token: 0x06008F71 RID: 36721 RVA: 0x0035F239 File Offset: 0x0035D439
	// (set) Token: 0x06008F72 RID: 36722 RVA: 0x0035F241 File Offset: 0x0035D441
	public ProgressionValue SelectedSkill { get; set; }

	// Token: 0x1700116D RID: 4461
	// (get) Token: 0x06008F73 RID: 36723 RVA: 0x0035F24A File Offset: 0x0035D44A
	// (set) Token: 0x06008F74 RID: 36724 RVA: 0x0035F252 File Offset: 0x0035D452
	public TEFeatureStorage LootContainer { get; set; }

	// Token: 0x1700116E RID: 4462
	// (get) Token: 0x06008F75 RID: 36725 RVA: 0x0035F25B File Offset: 0x0035D45B
	// (set) Token: 0x06008F76 RID: 36726 RVA: 0x0035F263 File Offset: 0x0035D463
	public bool IsUsingItemActionEntryUse { get; set; }

	// Token: 0x1700116F RID: 4463
	// (get) Token: 0x06008F77 RID: 36727 RVA: 0x0035F26C File Offset: 0x0035D46C
	// (set) Token: 0x06008F78 RID: 36728 RVA: 0x0035F274 File Offset: 0x0035D474
	public bool IsUsingItemActionEntryPromptComplete { get; set; }

	// Token: 0x17001170 RID: 4464
	// (get) Token: 0x06008F79 RID: 36729 RVA: 0x0035F27D File Offset: 0x0035D47D
	// (set) Token: 0x06008F7A RID: 36730 RVA: 0x0035F285 File Offset: 0x0035D485
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
	// (add) Token: 0x06008F7B RID: 36731 RVA: 0x0035F2A8 File Offset: 0x0035D4A8
	// (remove) Token: 0x06008F7C RID: 36732 RVA: 0x0035F2E0 File Offset: 0x0035D4E0
	public event Action OnShutdown;

	// Token: 0x140000EF RID: 239
	// (add) Token: 0x06008F7D RID: 36733 RVA: 0x0035F318 File Offset: 0x0035D518
	// (remove) Token: 0x06008F7E RID: 36734 RVA: 0x0035F350 File Offset: 0x0035D550
	public event Action OnBuilt;

	// Token: 0x17001171 RID: 4465
	// (get) Token: 0x06008F7F RID: 36735 RVA: 0x0035F385 File Offset: 0x0035D585
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

	// Token: 0x06008F80 RID: 36736 RVA: 0x0035F3A8 File Offset: 0x0035D5A8
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

	// Token: 0x06008F81 RID: 36737 RVA: 0x0035F425 File Offset: 0x0035D625
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

	// Token: 0x06008F82 RID: 36738 RVA: 0x0035F460 File Offset: 0x0035D660
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

	// Token: 0x06008F83 RID: 36739 RVA: 0x0035F54C File Offset: 0x0035D74C
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

	// Token: 0x06008F84 RID: 36740 RVA: 0x0035F5F8 File Offset: 0x0035D7F8
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

	// Token: 0x06008F85 RID: 36741 RVA: 0x0035F607 File Offset: 0x0035D807
	public void Awake()
	{
		this.WindowGroups = new List<XUiWindowGroup>();
		this.windowsAndGroupsByControllerType = new Dictionary<Type, List<XUiController>>();
		this.initFonts();
	}

	// Token: 0x06008F86 RID: 36742 RVA: 0x0035F628 File Offset: 0x0035D828
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

	// Token: 0x06008F87 RID: 36743 RVA: 0x0035F744 File Offset: 0x0035D944
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

	// Token: 0x06008F88 RID: 36744 RVA: 0x0035F793 File Offset: 0x0035D993
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

	// Token: 0x06008F89 RID: 36745 RVA: 0x0035F7AC File Offset: 0x0035D9AC
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

	// Token: 0x06008F8A RID: 36746 RVA: 0x0035F86C File Offset: 0x0035DA6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		GameOptionsManager.ResolutionChanged -= this.OnResolutionChanged;
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
		LocalPlayerManager.OnLocalPlayersChanged -= this.handleLocalPlayersChanged;
		this.Shutdown(false);
	}

	// Token: 0x06008F8B RID: 36747 RVA: 0x0035F8A8 File Offset: 0x0035DAA8
	public void Shutdown(bool _destroyImmediate = false)
	{
		Action onShutdown = this.OnShutdown;
		if (onShutdown != null)
		{
			onShutdown();
		}
		this.Cleanup(_destroyImmediate);
	}

	// Token: 0x06008F8C RID: 36748 RVA: 0x0035F8C4 File Offset: 0x0035DAC4
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

	// Token: 0x06008F8D RID: 36749 RVA: 0x0035F930 File Offset: 0x0035DB30
	public void CancelLoading()
	{
		if (this.loadAsyncCoroutine != null)
		{
			ThreadManager.StopCoroutine(this.loadAsyncCoroutine);
			this.loadAsyncCoroutine = null;
		}
	}

	// Token: 0x06008F8E RID: 36750 RVA: 0x0035F94C File Offset: 0x0035DB4C
	public void StopAllVideo()
	{
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			XUi.<StopAllVideo>g__StopVideo|145_0(this.WindowGroups[i].Controller);
		}
	}

	// Token: 0x06008F8F RID: 36751 RVA: 0x0035F988 File Offset: 0x0035DB88
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

	// Token: 0x06008F90 RID: 36752 RVA: 0x0035F9C6 File Offset: 0x0035DBC6
	public void AddWindow(XUiV_Window _window)
	{
		this.windows.Add(_window);
	}

	// Token: 0x06008F91 RID: 36753 RVA: 0x0035F9D4 File Offset: 0x0035DBD4
	public void RemoveWindow(XUiV_Window _window)
	{
		this.windows.Remove(_window);
	}

	// Token: 0x06008F92 RID: 36754 RVA: 0x0035F9E4 File Offset: 0x0035DBE4
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

	// Token: 0x06008F93 RID: 36755 RVA: 0x0035FA44 File Offset: 0x0035DC44
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

	// Token: 0x06008F94 RID: 36756 RVA: 0x0035FAC2 File Offset: 0x0035DCC2
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnResolutionChanged(int _arg1, int _arg2)
	{
		ThreadManager.StartCoroutine(this.delayedScaleUpdate());
	}

	// Token: 0x06008F95 RID: 36757 RVA: 0x0035FAD0 File Offset: 0x0035DCD0
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator delayedScaleUpdate()
	{
		yield return null;
		this.SetScale(-1f);
		yield break;
	}

	// Token: 0x06008F96 RID: 36758 RVA: 0x0035FAE0 File Offset: 0x0035DCE0
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

	// Token: 0x06008F97 RID: 36759 RVA: 0x0035FB46 File Offset: 0x0035DD46
	public float GetScale()
	{
		return this.xuiGlobalScaling;
	}

	// Token: 0x06008F98 RID: 36760 RVA: 0x0035FB4E File Offset: 0x0035DD4E
	public void LoadData<T>(string _path, Action<T> _callback) where T : global::UnityEngine.Object
	{
		LoadManager.LoadAsset<T>(_path, _callback, this.loadGroup, false, !this.asyncLoad, false);
	}

	// Token: 0x06008F99 RID: 36761 RVA: 0x0035FB6C File Offset: 0x0035DD6C
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

	// Token: 0x06008F9A RID: 36762 RVA: 0x0035FBEC File Offset: 0x0035DDEC
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

	// Token: 0x06008F9B RID: 36763 RVA: 0x0035FD58 File Offset: 0x0035DF58
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

	// Token: 0x06008F9C RID: 36764 RVA: 0x0035FDB7 File Offset: 0x0035DFB7
	public Vector2i GetXUiScreenSize()
	{
		return new Vector2i(new Vector2((float)this.playerUI.camera.pixelWidth, (float)this.playerUI.camera.pixelHeight) * this.GetPixelRatioFactor());
	}

	// Token: 0x06008F9D RID: 36765 RVA: 0x0035FDF0 File Offset: 0x0035DFF0
	public Vector2i GetMouseXUiPosition()
	{
		Vector3 vector = this.playerUI.CursorController.GetLocalScreenPosition();
		return this.TranslateScreenVectorToXuiVector(vector);
	}

	// Token: 0x06008F9E RID: 36766 RVA: 0x0035FE20 File Offset: 0x0035E020
	public Vector2i TranslateScreenVectorToXuiVector(Vector2 _screenSpaceVector)
	{
		_screenSpaceVector.x -= (float)this.playerUI.camera.pixelWidth / 2f;
		_screenSpaceVector.y -= (float)this.playerUI.camera.pixelHeight / 2f;
		return new Vector2i(_screenSpaceVector * this.GetPixelRatioFactor());
	}

	// Token: 0x06008F9F RID: 36767 RVA: 0x0035FE84 File Offset: 0x0035E084
	public Vector3 TranslateScreenVectorToXuiVector(Vector3 _screenSpaceVector)
	{
		_screenSpaceVector.x -= (float)this.playerUI.camera.pixelWidth / 2f;
		_screenSpaceVector.y -= (float)this.playerUI.camera.pixelHeight / 2f;
		return _screenSpaceVector * this.GetPixelRatioFactor();
	}

	// Token: 0x06008FA0 RID: 36768 RVA: 0x0035FEE4 File Offset: 0x0035E0E4
	public Bounds GetXUiWindowScreenBounds(Transform _xuiElement, bool _includeInactive = false)
	{
		Bounds xuiWindowWorldBounds = XUi.getXUiWindowWorldBounds(_xuiElement, _includeInactive);
		Bounds bounds = new Bounds(this.playerUI.camera.WorldToScreenPoint(xuiWindowWorldBounds.min), Vector3.zero);
		bounds.Encapsulate(this.playerUI.camera.WorldToScreenPoint(xuiWindowWorldBounds.max));
		return bounds;
	}

	// Token: 0x06008FA1 RID: 36769 RVA: 0x0035FF3C File Offset: 0x0035E13C
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

	// Token: 0x17001172 RID: 4466
	// (get) Token: 0x06008FA2 RID: 36770 RVA: 0x0036000C File Offset: 0x0035E20C
	public Transform StackPanelTransform
	{
		get
		{
			return this.stackPanelRoot;
		}
	}

	// Token: 0x06008FA3 RID: 36771 RVA: 0x00360014 File Offset: 0x0035E214
	[PublicizedFrom(EAccessModifier.Private)]
	public void initStackPanels()
	{
		this.stackPanelRoot = base.transform.Find("StackPanels").transform;
		this.StackPanels.Add("Left", new XUi.StackPanel("Left", XUi.EStackPanelPos.Left, base.transform.Find("StackPanels/Left").transform));
		this.StackPanels.Add("Center", new XUi.StackPanel("Center", XUi.EStackPanelPos.Center, base.transform.Find("StackPanels/Center").transform));
		this.StackPanels.Add("Right", new XUi.StackPanel("Right", XUi.EStackPanelPos.Right, base.transform.Find("StackPanels/Right").transform));
	}

	// Token: 0x06008FA4 RID: 36772 RVA: 0x003600CC File Offset: 0x0035E2CC
	public void SetStackPanelScale(float _scale)
	{
		this.defaultStackPanelScale = _scale;
		this.stackPanelRoot.localScale = Vector3.one * _scale;
		this.stackPanelRoot.localPosition = Vector3.zero;
	}

	// Token: 0x06008FA5 RID: 36773 RVA: 0x003600FB File Offset: 0x0035E2FB
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

	// Token: 0x06008FA6 RID: 36774 RVA: 0x00360136 File Offset: 0x0035E336
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

	// Token: 0x06008FA7 RID: 36775 RVA: 0x0036014C File Offset: 0x0035E34C
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

	// Token: 0x06008FA8 RID: 36776 RVA: 0x003601A8 File Offset: 0x0035E3A8
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

	// Token: 0x06008FA9 RID: 36777 RVA: 0x003603D4 File Offset: 0x0035E5D4
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

	// Token: 0x06008FAA RID: 36778 RVA: 0x0036047C File Offset: 0x0035E67C
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

	// Token: 0x06008FAB RID: 36779 RVA: 0x003604FC File Offset: 0x0035E6FC
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

	// Token: 0x06008FAC RID: 36780 RVA: 0x0036054B File Offset: 0x0035E74B
	public void PlayMenuScrollSound()
	{
		Manager.PlayXUiSound(this.uiScrollSound, this.uiScrollVolume);
	}

	// Token: 0x06008FAD RID: 36781 RVA: 0x0036055E File Offset: 0x0035E75E
	public void PlayMenuClickSound()
	{
		Manager.PlayXUiSound(this.uiClickSound, this.uiClickVolume);
	}

	// Token: 0x06008FAE RID: 36782 RVA: 0x00360571 File Offset: 0x0035E771
	public void PlayMenuConfirmSound()
	{
		Manager.PlayXUiSound(this.uiConfirmSound, this.uiConfirmVolume);
	}

	// Token: 0x06008FAF RID: 36783 RVA: 0x00360584 File Offset: 0x0035E784
	public void PlayMenuBackSound()
	{
		Manager.PlayXUiSound(this.uiBackSound, this.uiBackVolume);
	}

	// Token: 0x06008FB0 RID: 36784 RVA: 0x00360597 File Offset: 0x0035E797
	public void PlayMenuSliderSound()
	{
		Manager.PlayXUiSound(this.uiSliderSound, this.uiSliderVolume);
	}

	// Token: 0x06008FB1 RID: 36785 RVA: 0x003605AC File Offset: 0x0035E7AC
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

	// Token: 0x06008FB2 RID: 36786 RVA: 0x003605F8 File Offset: 0x0035E7F8
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

	// Token: 0x06008FB3 RID: 36787 RVA: 0x00360648 File Offset: 0x0035E848
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

	// Token: 0x06008FB4 RID: 36788 RVA: 0x0036068C File Offset: 0x0035E88C
	public List<XUiController> GetChildrenById(string _id)
	{
		List<XUiController> list = new List<XUiController>();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.GetChildrenById(_id, list);
		}
		return list;
	}

	// Token: 0x06008FB5 RID: 36789 RVA: 0x003606D0 File Offset: 0x0035E8D0
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

	// Token: 0x06008FB6 RID: 36790 RVA: 0x00360720 File Offset: 0x0035E920
	public List<T> GetChildrenByType<T>() where T : XUiController
	{
		List<T> list = new List<T>();
		for (int i = 0; i < this.WindowGroups.Count; i++)
		{
			this.WindowGroups[i].Controller.GetChildrenByType<T>(list);
		}
		return list;
	}

	// Token: 0x06008FB7 RID: 36791 RVA: 0x00360764 File Offset: 0x0035E964
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

	// Token: 0x06008FB8 RID: 36792 RVA: 0x003607D4 File Offset: 0x0035E9D4
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

	// Token: 0x06008FB9 RID: 36793 RVA: 0x00360850 File Offset: 0x0035EA50
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

	// Token: 0x06008FBA RID: 36794 RVA: 0x003608B4 File Offset: 0x0035EAB4
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

	// Token: 0x06008FBB RID: 36795 RVA: 0x00360920 File Offset: 0x0035EB20
	[PublicizedFrom(EAccessModifier.Private)]
	public void initAtlases()
	{
		MultiSourceAtlasManager[] array = Resources.FindObjectsOfTypeAll<MultiSourceAtlasManager>();
		for (int i = 0; i < array.Length; i++)
		{
			this.allMultiSourceAtlases.Add(array[i].name, array[i]);
		}
	}

	// Token: 0x06008FBC RID: 36796 RVA: 0x00360958 File Offset: 0x0035EB58
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

	// Token: 0x06008FBD RID: 36797 RVA: 0x00360990 File Offset: 0x0035EB90
	[PublicizedFrom(EAccessModifier.Private)]
	public void initFonts()
	{
		this.fontsByName = new CaseInsensitiveStringDictionary<NGUIFont>();
		foreach (NGUIFont nguifont in this.NGUIFonts)
		{
			this.fontsByName.Add(nguifont.name, nguifont);
		}
	}

	// Token: 0x06008FBE RID: 36798 RVA: 0x003609D4 File Offset: 0x0035EBD4
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

	// Token: 0x06008FBF RID: 36799 RVA: 0x00360A14 File Offset: 0x0035EC14
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

	// Token: 0x06008FC0 RID: 36800 RVA: 0x00360A50 File Offset: 0x0035EC50
	public UIAnchor GetAnchor(UIAnchor.Side _anchorSide)
	{
		return this.xuiAnchors[(int)_anchorSide];
	}

	// Token: 0x06008FC1 RID: 36801 RVA: 0x00360A5C File Offset: 0x0035EC5C
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

	// Token: 0x06008FC2 RID: 36802 RVA: 0x00360ADC File Offset: 0x0035ECDC
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

	// Token: 0x06008FC3 RID: 36803 RVA: 0x00360BEC File Offset: 0x0035EDEC
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

	// Token: 0x06008FC4 RID: 36804 RVA: 0x00360C30 File Offset: 0x0035EE30
	public void CancelAllCrafting()
	{
		XUiC_RecipeStack[] recipesToCraft = this.FindWindowGroupByName("crafting").GetChildByType<XUiC_CraftingQueue>().GetRecipesToCraft();
		for (int i = 0; i < recipesToCraft.Length; i++)
		{
			recipesToCraft[i].ForceCancel();
		}
	}

	// Token: 0x06008FC5 RID: 36805 RVA: 0x00360C6C File Offset: 0x0035EE6C
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

	// Token: 0x06008FC6 RID: 36806 RVA: 0x00360D74 File Offset: 0x0035EF74
	public void SetCraftingData(CraftingData _cd)
	{
		base.StartCoroutine(this.setCraftingDataAsync(_cd));
	}

	// Token: 0x06008FC7 RID: 36807 RVA: 0x00360D84 File Offset: 0x0035EF84
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

	// Token: 0x06008FC8 RID: 36808 RVA: 0x00360D9C File Offset: 0x0035EF9C
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

	// Token: 0x06008FC9 RID: 36809 RVA: 0x00360F37 File Offset: 0x0035F137
	public void UpdateWindowSoftCursorBounds(XUiV_Window _window)
	{
		this.playerUI.CursorController != null;
	}

	// Token: 0x06008FCA RID: 36810 RVA: 0x00360F4C File Offset: 0x0035F14C
	public void RemoveWindowFromSoftCursorBounds(XUiV_Window _window)
	{
		CursorControllerAbs cursorController = this.playerUI.CursorController;
		if (cursorController != null)
		{
			cursorController.RemoveBounds(_window.ID);
		}
	}

	// Token: 0x06008FCB RID: 36811 RVA: 0x00360F7C File Offset: 0x0035F17C
	public void ForceInputStyleChange()
	{
		this.GetOpenWindows(XUi.forceInputStyleChangedWindowsList);
		foreach (XUiV_Window xuiV_Window in XUi.forceInputStyleChangedWindowsList)
		{
			xuiV_Window.Controller.ForceInputStyleChange(PlatformManager.NativePlatform.Input.CurrentInputStyle, PlatformManager.NativePlatform.Input.CurrentInputStyle);
		}
		XUi.forceInputStyleChangedWindowsList.Clear();
	}

	// Token: 0x06008FCE RID: 36814 RVA: 0x003610BC File Offset: 0x0035F2BC
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

	// Token: 0x06008FCF RID: 36815 RVA: 0x00361124 File Offset: 0x0035F324
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

	// Token: 0x040068EE RID: 26862
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static int lastScreenHeight;

	// Token: 0x040068EF RID: 26863
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float pixelRatioFactor;

	// Token: 0x040068F0 RID: 26864
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static UIRoot uiRoot;

	// Token: 0x040068F1 RID: 26865
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool inGameMenuOpen;

	// Token: 0x040068F2 RID: 26866
	public NGUIFont[] NGUIFonts;

	// Token: 0x040068F3 RID: 26867
	public List<XUiWindowGroup> WindowGroups;

	// Token: 0x040068F4 RID: 26868
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Type, List<XUiController>> windowsAndGroupsByControllerType;

	// Token: 0x040068F5 RID: 26869
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<XUiV_Window> windows;

	// Token: 0x040068F6 RID: 26870
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float backgroundGlobalOpacity = 1f;

	// Token: 0x040068F7 RID: 26871
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float foregroundGlobalOpacity = 1f;

	// Token: 0x040068F8 RID: 26872
	public XUiM_PlayerInventory PlayerInventory;

	// Token: 0x040068F9 RID: 26873
	public XUiM_PlayerEquipment PlayerEquipment;

	// Token: 0x040068FA RID: 26874
	public XUiM_Vehicle Vehicle;

	// Token: 0x040068FB RID: 26875
	public XUiM_AssembleItem AssembleItem;

	// Token: 0x040068FC RID: 26876
	public XUiM_Quest QuestTracker;

	// Token: 0x040068FD RID: 26877
	public XUiM_Recipes Recipes;

	// Token: 0x040068FE RID: 26878
	public XUiM_Trader Trader;

	// Token: 0x040068FF RID: 26879
	public XUiM_Dialog Dialog;

	// Token: 0x04006900 RID: 26880
	public XUiC_BuffPopoutList BuffPopoutList;

	// Token: 0x04006901 RID: 26881
	public XUiC_CollectedItemList CollectedItemList;

	// Token: 0x04006902 RID: 26882
	public XUiC_Radial RadialWindow;

	// Token: 0x04006903 RID: 26883
	public XUiC_GamepadCalloutWindow CalloutWindow;

	// Token: 0x04006918 RID: 26904
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float xuiGlobalScaling = 1.255f;

	// Token: 0x04006919 RID: 26905
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2i lastScreenSize = Vector2i.zero;

	// Token: 0x0400691A RID: 26906
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool mIsReady;

	// Token: 0x0400691B RID: 26907
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool asyncLoad;

	// Token: 0x0400691C RID: 26908
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LoadManager.LoadGroup loadGroup;

	// Token: 0x0400691F RID: 26911
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI mPlayerUI;

	// Token: 0x04006920 RID: 26912
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public long accumElapsedMilliseconds;

	// Token: 0x04006921 RID: 26913
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Coroutine loadAsyncCoroutine;

	// Token: 0x04006922 RID: 26914
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<UIBasicSprite> getXUiWindowWorldBoundsList = new List<UIBasicSprite>();

	// Token: 0x04006923 RID: 26915
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float defaultStackPanelScale = 1f;

	// Token: 0x04006924 RID: 26916
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform stackPanelRoot;

	// Token: 0x04006925 RID: 26917
	public readonly DictionaryList<string, XUi.StackPanel> StackPanels = new DictionaryList<string, XUi.StackPanel>();

	// Token: 0x04006926 RID: 26918
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiScrollSound;

	// Token: 0x04006927 RID: 26919
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiScrollVolume = 0.5f;

	// Token: 0x04006928 RID: 26920
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiClickSound;

	// Token: 0x04006929 RID: 26921
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiClickVolume = 0.5f;

	// Token: 0x0400692A RID: 26922
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiConfirmSound;

	// Token: 0x0400692B RID: 26923
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiConfirmVolume = 0.5f;

	// Token: 0x0400692C RID: 26924
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiBackSound;

	// Token: 0x0400692D RID: 26925
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiBackVolume = 0.5f;

	// Token: 0x0400692E RID: 26926
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioClip uiSliderSound;

	// Token: 0x0400692F RID: 26927
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float uiSliderVolume = 0.25f;

	// Token: 0x04006930 RID: 26928
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, MultiSourceAtlasManager> allMultiSourceAtlases = new CaseInsensitiveStringDictionary<MultiSourceAtlasManager>();

	// Token: 0x04006931 RID: 26929
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public CaseInsensitiveStringDictionary<NGUIFont> fontsByName;

	// Token: 0x04006932 RID: 26930
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform anchorRoot;

	// Token: 0x04006933 RID: 26931
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIAnchor[] xuiAnchors;

	// Token: 0x04006934 RID: 26932
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 previousPagingVector = Vector2.zero;

	// Token: 0x04006935 RID: 26933
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float pagingRepeatTimer;

	// Token: 0x04006936 RID: 26934
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static bool initialPagingInput;

	// Token: 0x04006937 RID: 26935
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<XUiV_Window> forceInputStyleChangedWindowsList = new List<XUiV_Window>();

	// Token: 0x02001186 RID: 4486
	public enum EStackPanelPos
	{
		// Token: 0x04006939 RID: 26937
		Left,
		// Token: 0x0400693A RID: 26938
		Center,
		// Token: 0x0400693B RID: 26939
		Right
	}

	// Token: 0x02001187 RID: 4487
	public class StackPanel
	{
		// Token: 0x06008FD5 RID: 36821 RVA: 0x0036129D File Offset: 0x0035F49D
		public StackPanel(string _name, XUi.EStackPanelPos _position, Transform _transform)
		{
			this.Name = _name;
			this.Position = _position;
			this.Transform = _transform;
		}

		// Token: 0x0400693C RID: 26940
		public readonly string Name;

		// Token: 0x0400693D RID: 26941
		public readonly XUi.EStackPanelPos Position;

		// Token: 0x0400693E RID: 26942
		public readonly Transform Transform;

		// Token: 0x0400693F RID: 26943
		public int WindowCount;

		// Token: 0x04006940 RID: 26944
		public Vector2Int Size;
	}

	// Token: 0x02001188 RID: 4488
	public enum UISoundType
	{
		// Token: 0x04006942 RID: 26946
		ClickSound,
		// Token: 0x04006943 RID: 26947
		ScrollSound,
		// Token: 0x04006944 RID: 26948
		ConfirmSound,
		// Token: 0x04006945 RID: 26949
		BackSound,
		// Token: 0x04006946 RID: 26950
		SliderSound,
		// Token: 0x04006947 RID: 26951
		None
	}
}
