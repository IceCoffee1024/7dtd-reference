using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DFB RID: 3579
[Preserve]
public class XUiC_ServerFilters : XUiController
{
	// Token: 0x14000098 RID: 152
	// (add) Token: 0x06006CCA RID: 27850 RVA: 0x002A9DF4 File Offset: 0x002A7FF4
	// (remove) Token: 0x06006CCB RID: 27851 RVA: 0x002A9E2C File Offset: 0x002A802C
	public event Action<IServerBrowserFilterControl> OnFilterChanged;

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x06006CCC RID: 27852 RVA: 0x002A9E61 File Offset: 0x002A8061
	// (set) Token: 0x06006CCD RID: 27853 RVA: 0x002A9E69 File Offset: 0x002A8069
	[XuiXmlBinding("results")]
	public int ResultCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x06006CCE RID: 27854 RVA: 0x002A9E72 File Offset: 0x002A8072
	[XuiXmlBinding("crossplaytooltip")]
	public string CrossplayTooltip
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.All) ?? Localization.Get("xuiOptionsGeneralCrossplayTooltip", false, null);
		}
	}

	// Token: 0x06006CCF RID: 27855 RVA: 0x002A9E8C File Offset: 0x002A808C
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

	// Token: 0x06006CD0 RID: 27856 RVA: 0x002AA05A File Offset: 0x002A825A
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x06006CD1 RID: 27857 RVA: 0x002AA074 File Offset: 0x002A8274
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

	// Token: 0x06006CD2 RID: 27858 RVA: 0x002AA103 File Offset: 0x002A8303
	[XuiBindEvent("OnPress", "fullScreenCollider")]
	[XuiBindEvent("OnPress", "btnCloseFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloseFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.CloseFilters();
	}

	// Token: 0x06006CD3 RID: 27859 RVA: 0x002AA10B File Offset: 0x002A830B
	[XuiBindEvent("OnPress", "btnResetFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.resetFilters();
	}

	// Token: 0x06006CD4 RID: 27860 RVA: 0x002AA113 File Offset: 0x002A8313
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BackButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.parentBrowser.CloseBrowser();
	}

	// Token: 0x06006CD5 RID: 27861 RVA: 0x002AA120 File Offset: 0x002A8320
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

	// Token: 0x06006CD6 RID: 27862 RVA: 0x002AA198 File Offset: 0x002A8398
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

	// Token: 0x06006CD7 RID: 27863 RVA: 0x002AA1BD File Offset: 0x002A83BD
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

	// Token: 0x06006CD8 RID: 27864 RVA: 0x002AA1E4 File Offset: 0x002A83E4
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

	// Token: 0x06006CD9 RID: 27865 RVA: 0x002AA290 File Offset: 0x002A8490
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

	// Token: 0x06006CDA RID: 27866 RVA: 0x002AA36E File Offset: 0x002A856E
	public void CloseFilters()
	{
		this.windowGroup.Controller.GetChildByType<XUiC_ServerBrowser>().ShowingFilters = false;
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
	}

	// Token: 0x06006CDB RID: 27867 RVA: 0x002AA39D File Offset: 0x002A859D
	public void SetServersList(XUiC_ServersList _serversList)
	{
		_serversList.OnFilterResultsChanged += this.ServersList_OnFilterResultsChanged;
	}

	// Token: 0x06006CDC RID: 27868 RVA: 0x002AA3B1 File Offset: 0x002A85B1
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnFilterResultsChanged(int _count)
	{
		this.ResultCount = _count;
		this.IsDirty = true;
	}

	// Token: 0x06006CDD RID: 27869 RVA: 0x002AA3C1 File Offset: 0x002A85C1
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

	// Token: 0x06006CDE RID: 27870 RVA: 0x002AA3DC File Offset: 0x002A85DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			this.selectInitialElement();
		}
	}

	// Token: 0x06006CDF RID: 27871 RVA: 0x002AA3E7 File Offset: 0x002A85E7
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

	// Token: 0x06006CE0 RID: 27872 RVA: 0x002AA412 File Offset: 0x002A8612
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectInitialElement()
	{
		this.GetInitialSelectedElement().Controller.SelectCursorElement(true, false);
	}

	// Token: 0x06006CE1 RID: 27873 RVA: 0x002AA427 File Offset: 0x002A8627
	public XUiView GetInitialSelectedElement()
	{
		return base.GetChildById(ServerListManager.Instance.IsPrefilteredSearch ? "btnStartSearch" : "btnResetFilters").ViewComponent;
	}

	// Token: 0x06006CE2 RID: 27874 RVA: 0x002AA44C File Offset: 0x002A864C
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

	// Token: 0x04005340 RID: 21312
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IServerBrowserFilterControl> allFilterControls = new List<IServerBrowserFilterControl>();

	// Token: 0x04005342 RID: 21314
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo regionFilter;

	// Token: 0x04005343 RID: 21315
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefString languageFilter;

	// Token: 0x04005344 RID: 21316
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo crossplayFilter;

	// Token: 0x04005345 RID: 21317
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo moddedConfigsFilter;

	// Token: 0x04005346 RID: 21318
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo requiresModsFilter;

	// Token: 0x04005347 RID: 21319
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo eacFilter;

	// Token: 0x04005348 RID: 21320
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo ignoreSanctionsFilter;

	// Token: 0x04005349 RID: 21321
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x0400534A RID: 21322
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerBrowser parentBrowser;

	// Token: 0x0400534B RID: 21323
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_FullScreenCollider fullScreenCollider;

	// Token: 0x0400534C RID: 21324
	[XuiBindComponent("btnStartSearch", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStartSearch;

	// Token: 0x0400534D RID: 21325
	[XuiBindComponent("btnResetFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnResetFilters;

	// Token: 0x0400534E RID: 21326
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x0400534F RID: 21327
	[XuiBindComponent("btnCloseFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCloseFilters;
}
