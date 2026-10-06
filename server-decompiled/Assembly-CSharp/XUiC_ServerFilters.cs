using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DF0 RID: 3568
[Preserve]
public class XUiC_ServerFilters : XUiController
{
	// Token: 0x14000098 RID: 152
	// (add) Token: 0x06006CA5 RID: 27813 RVA: 0x002A88CC File Offset: 0x002A6ACC
	// (remove) Token: 0x06006CA6 RID: 27814 RVA: 0x002A8904 File Offset: 0x002A6B04
	public event Action<IServerBrowserFilterControl> OnFilterChanged;

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x06006CA7 RID: 27815 RVA: 0x002A8939 File Offset: 0x002A6B39
	// (set) Token: 0x06006CA8 RID: 27816 RVA: 0x002A8941 File Offset: 0x002A6B41
	[XuiXmlBinding("results")]
	public int ResultCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06006CA9 RID: 27817 RVA: 0x002A894A File Offset: 0x002A6B4A
	[XuiXmlBinding("crossplaytooltip")]
	public string CrossplayTooltip
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.All) ?? Localization.Get("xuiOptionsGeneralCrossplayTooltip", false, null);
		}
	}

	// Token: 0x06006CAA RID: 27818 RVA: 0x002A8964 File Offset: 0x002A6B64
	public override void Init()
	{
		base.Init();
		foreach (XUiC_ServerBrowserGamePrefSelectorCombo xuiC_ServerBrowserGamePrefSelectorCombo in base.GetChildrenByType<XUiC_ServerBrowserGamePrefSelectorCombo>(null))
		{
			GameInfoInt gameInfoInt = xuiC_ServerBrowserGamePrefSelectorCombo.GameInfoInt;
			if (gameInfoInt != GameInfoInt.CurrentServerTime)
			{
				if (gameInfoInt != GameInfoInt.AirDropFrequency)
				{
					if (gameInfoInt == GameInfoInt.WorldSize)
					{
						if (PlatformOptimizations.EnforceMaxWorldSizeClient)
						{
							xuiC_ServerBrowserGamePrefSelectorCombo.ValueRangeMin = new int?(0);
							xuiC_ServerBrowserGamePrefSelectorCombo.ValueRangeMax = new int?(PlatformOptimizations.MaxWorldSizeClient);
						}
					}
				}
				else
				{
					xuiC_ServerBrowserGamePrefSelectorCombo.ValuePreDisplayModifierFunc = (int _n) => _n;
				}
			}
			else
			{
				xuiC_ServerBrowserGamePrefSelectorCombo.CustomValuePreFilterModifierFunc = (int _n) => (_n - 1) * 24000;
			}
			if (xuiC_ServerBrowserGamePrefSelectorCombo.GameInfoString == GameInfoString.Region)
			{
				this.regionFilter = xuiC_ServerBrowserGamePrefSelectorCombo;
			}
			GameInfoBool gameInfoBool = xuiC_ServerBrowserGamePrefSelectorCombo.GameInfoBool;
			switch (gameInfoBool)
			{
			case GameInfoBool.EACEnabled:
				this.eacFilter = xuiC_ServerBrowserGamePrefSelectorCombo;
				break;
			case GameInfoBool.SanctionsIgnored:
				this.ignoreSanctionsFilter = xuiC_ServerBrowserGamePrefSelectorCombo;
				break;
			case GameInfoBool.Architecture64:
			case GameInfoBool.StockSettings:
			case GameInfoBool.StockFiles:
				break;
			case GameInfoBool.ModdedConfig:
				this.moddedConfigsFilter = xuiC_ServerBrowserGamePrefSelectorCombo;
				break;
			case GameInfoBool.RequiresMod:
				this.requiresModsFilter = xuiC_ServerBrowserGamePrefSelectorCombo;
				break;
			default:
				if (gameInfoBool == GameInfoBool.AllowCrossplay)
				{
					this.crossplayFilter = xuiC_ServerBrowserGamePrefSelectorCombo;
				}
				break;
			}
			XUiC_ServerBrowserGamePrefSelectorCombo xuiC_ServerBrowserGamePrefSelectorCombo2 = xuiC_ServerBrowserGamePrefSelectorCombo;
			xuiC_ServerBrowserGamePrefSelectorCombo2.OnValueChanged = (Action<IServerBrowserFilterControl>)Delegate.Combine(xuiC_ServerBrowserGamePrefSelectorCombo2.OnValueChanged, new Action<IServerBrowserFilterControl>(this.OnFilterValueChanged));
			this.allFilterControls.Add(xuiC_ServerBrowserGamePrefSelectorCombo);
		}
		foreach (XUiC_ServerBrowserGamePrefString xuiC_ServerBrowserGamePrefString in base.GetChildrenByType<XUiC_ServerBrowserGamePrefString>(null))
		{
			if (xuiC_ServerBrowserGamePrefString.GameInfoString == GameInfoString.Language)
			{
				this.languageFilter = xuiC_ServerBrowserGamePrefString;
			}
			XUiC_ServerBrowserGamePrefString xuiC_ServerBrowserGamePrefString2 = xuiC_ServerBrowserGamePrefString;
			xuiC_ServerBrowserGamePrefString2.OnValueChanged = (Action<IServerBrowserFilterControl>)Delegate.Combine(xuiC_ServerBrowserGamePrefString2.OnValueChanged, new Action<IServerBrowserFilterControl>(this.OnFilterValueChanged));
			this.allFilterControls.Add(xuiC_ServerBrowserGamePrefString);
		}
		base.OnVisiblity += this.OnVisibilityChanged;
	}

	// Token: 0x06006CAB RID: 27819 RVA: 0x002A8B32 File Offset: 0x002A6D32
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x06006CAC RID: 27820 RVA: 0x002A8B4C File Offset: 0x002A6D4C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (XUiUtils.HotkeysAllowedFor(base.ViewComponent, false))
		{
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
			{
				ThreadManager.RunTaskAfterFrames(new Action(this.startShortcutPressed), 1);
			}
			if (this.xui.playerUI.playerInput.GUIActions.Cancel.WasReleased)
			{
				ThreadManager.RunTaskAfterFrames(new Action(this.backShortcutPressed), 1);
			}
		}
	}

	// Token: 0x06006CAD RID: 27821 RVA: 0x002A8BDB File Offset: 0x002A6DDB
	[XuiBindEvent("OnPress", "fullScreenCollider")]
	[XuiBindEvent("OnPress", "btnCloseFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloseFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.CloseFilters();
	}

	// Token: 0x06006CAE RID: 27822 RVA: 0x002A8BE3 File Offset: 0x002A6DE3
	[XuiBindEvent("OnPress", "btnResetFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.resetFilters();
	}

	// Token: 0x06006CAF RID: 27823 RVA: 0x002A8BEB File Offset: 0x002A6DEB
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BackButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.parentBrowser.CloseBrowser();
	}

	// Token: 0x06006CB0 RID: 27824 RVA: 0x002A8BF8 File Offset: 0x002A6DF8
	[XuiBindEvent("OnPress", "btnStartSearch")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void StartSearchButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.CloseFilters();
		List<IServerListInterface.ServerFilter> list = new List<IServerListInterface.ServerFilter>();
		foreach (IServerBrowserFilterControl serverBrowserFilterControl in this.allFilterControls)
		{
			XUiC_ServersList.UiServerFilter filter = serverBrowserFilterControl.GetFilter();
			if (filter.Type != IServerListInterface.ServerFilter.EServerFilterType.Any)
			{
				list.Add(filter);
			}
		}
		ServerListManager.Instance.StartSearch(list);
	}

	// Token: 0x06006CB1 RID: 27825 RVA: 0x002A8C70 File Offset: 0x002A6E70
	[PublicizedFrom(EAccessModifier.Private)]
	public void backShortcutPressed()
	{
		if (this.btnBack.ViewComponent.IsActiveInHierarchy)
		{
			this.BackButton_OnPress(null, 0);
			return;
		}
		this.CloseFiltersButton_OnPress(null, 0);
	}

	// Token: 0x06006CB2 RID: 27826 RVA: 0x002A8C95 File Offset: 0x002A6E95
	[PublicizedFrom(EAccessModifier.Private)]
	public void startShortcutPressed()
	{
		if (this.btnStartSearch.ViewComponent.Enabled)
		{
			this.StartSearchButton_OnPress(null, 0);
			return;
		}
		this.CloseFiltersButton_OnPress(null, 0);
	}

	// Token: 0x06006CB3 RID: 27827 RVA: 0x002A8CBC File Offset: 0x002A6EBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetFilters()
	{
		foreach (IServerBrowserFilterControl serverBrowserFilterControl in this.allFilterControls)
		{
			serverBrowserFilterControl.Reset();
		}
		if (this.regionFilter != null)
		{
			string text = GamePrefs.GetString(EnumGamePrefs.Region) ?? "";
			this.regionFilter.SelectEntry(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(text, null));
		}
		if (this.languageFilter != null)
		{
			string text2 = GamePrefs.GetString(EnumGamePrefs.LanguageBrowser) ?? "";
			this.languageFilter.SetValue(text2);
		}
		this.applyForcedSettings();
	}

	// Token: 0x06006CB4 RID: 27828 RVA: 0x002A8D68 File Offset: 0x002A6F68
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyForcedSettings()
	{
		bool flag = PermissionsManager.IsCrossplayAllowed();
		this.crossplayFilter.Enabled = flag;
		if (!flag)
		{
			this.crossplayFilter.SelectEntry(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(0, null));
		}
		if (!LaunchPrefs.AllowJoinConfigModded.Value)
		{
			this.moddedConfigsFilter.SelectEntry(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(0, null));
			this.moddedConfigsFilter.ViewComponent.IsVisible = false;
		}
		if (!(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
		{
			return;
		}
		this.requiresModsFilter.SelectEntry(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(0, null));
		this.requiresModsFilter.ViewComponent.IsVisible = false;
		if (!Submission.Enabled)
		{
			return;
		}
		this.eacFilter.SelectEntry(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(1, null));
		this.eacFilter.ViewComponent.IsVisible = false;
		this.ignoreSanctionsFilter.SelectEntry(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(0, null));
		this.ignoreSanctionsFilter.ViewComponent.IsVisible = false;
	}

	// Token: 0x06006CB5 RID: 27829 RVA: 0x002A8E46 File Offset: 0x002A7046
	public void CloseFilters()
	{
		this.windowGroup.Controller.GetChildByType<XUiC_ServerBrowser>().ShowingFilters = false;
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
	}

	// Token: 0x06006CB6 RID: 27830 RVA: 0x002A8E75 File Offset: 0x002A7075
	public void SetServersList(XUiC_ServersList _serversList)
	{
		_serversList.OnFilterResultsChanged += this.ServersList_OnFilterResultsChanged;
	}

	// Token: 0x06006CB7 RID: 27831 RVA: 0x002A8E89 File Offset: 0x002A7089
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnFilterResultsChanged(int _count)
	{
		this.ResultCount = _count;
		this.IsDirty = true;
	}

	// Token: 0x06006CB8 RID: 27832 RVA: 0x002A8E99 File Offset: 0x002A7099
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnFilterValueChanged(IServerBrowserFilterControl _sender)
	{
		Action<IServerBrowserFilterControl> onFilterChanged = this.OnFilterChanged;
		if (onFilterChanged != null)
		{
			onFilterChanged(_sender);
		}
		this.IsDirty = true;
	}

	// Token: 0x06006CB9 RID: 27833 RVA: 0x002A8EB4 File Offset: 0x002A70B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			this.selectInitialElement();
		}
	}

	// Token: 0x06006CBA RID: 27834 RVA: 0x002A8EBF File Offset: 0x002A70BF
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.openedBefore)
		{
			this.openedBefore = true;
			this.resetFilters();
		}
		else
		{
			this.applyForcedSettings();
		}
		base.RefreshBindings();
	}

	// Token: 0x06006CBB RID: 27835 RVA: 0x002A8EEA File Offset: 0x002A70EA
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectInitialElement()
	{
		this.GetInitialSelectedElement().Controller.SelectCursorElement(true, false);
	}

	// Token: 0x06006CBC RID: 27836 RVA: 0x002A8EFF File Offset: 0x002A70FF
	public XUiView GetInitialSelectedElement()
	{
		return base.GetChildById(ServerListManager.Instance.IsPrefilteredSearch ? "btnStartSearch" : "btnResetFilters").ViewComponent;
	}

	// Token: 0x06006CBD RID: 27837 RVA: 0x002A8F24 File Offset: 0x002A7124
	public override void OnClose()
	{
		base.OnClose();
		if (this.regionFilter != null)
		{
			XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue selection = this.regionFilter.GetSelection();
			if (selection.Type == XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.String)
			{
				GamePrefs.Set(EnumGamePrefs.Region, selection.StringValue);
			}
		}
		if (this.languageFilter != null)
		{
			string value = this.languageFilter.GetValue();
			GamePrefs.Set(EnumGamePrefs.LanguageBrowser, value);
		}
		GamePrefs.Set(EnumGamePrefs.IgnoreEOSSanctions, false);
	}

	// Token: 0x0400531B RID: 21275
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IServerBrowserFilterControl> allFilterControls = new List<IServerBrowserFilterControl>();

	// Token: 0x0400531D RID: 21277
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo regionFilter;

	// Token: 0x0400531E RID: 21278
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefString languageFilter;

	// Token: 0x0400531F RID: 21279
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo crossplayFilter;

	// Token: 0x04005320 RID: 21280
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo moddedConfigsFilter;

	// Token: 0x04005321 RID: 21281
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo requiresModsFilter;

	// Token: 0x04005322 RID: 21282
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo eacFilter;

	// Token: 0x04005323 RID: 21283
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo ignoreSanctionsFilter;

	// Token: 0x04005324 RID: 21284
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x04005325 RID: 21285
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerBrowser parentBrowser;

	// Token: 0x04005326 RID: 21286
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_FullScreenCollider fullScreenCollider;

	// Token: 0x04005327 RID: 21287
	[XuiBindComponent("btnStartSearch", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStartSearch;

	// Token: 0x04005328 RID: 21288
	[XuiBindComponent("btnResetFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnResetFilters;

	// Token: 0x04005329 RID: 21289
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x0400532A RID: 21290
	[XuiBindComponent("btnCloseFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCloseFilters;
}
