using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DE1 RID: 3553
[Preserve]
public class XUiC_ServerFilters : XUiController
{
	// Token: 0x14000099 RID: 153
	// (add) Token: 0x06006CAD RID: 27821 RVA: 0x002A553C File Offset: 0x002A373C
	// (remove) Token: 0x06006CAE RID: 27822 RVA: 0x002A5574 File Offset: 0x002A3774
	public event Action<IServerBrowserFilterControl> OnFilterChanged;

	// Token: 0x17000BFF RID: 3071
	// (get) Token: 0x06006CAF RID: 27823 RVA: 0x002A55A9 File Offset: 0x002A37A9
	// (set) Token: 0x06006CB0 RID: 27824 RVA: 0x002A55B1 File Offset: 0x002A37B1
	[XuiXmlBinding("results")]
	public int ResultCount
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000C00 RID: 3072
	// (get) Token: 0x06006CB1 RID: 27825 RVA: 0x002A55BA File Offset: 0x002A37BA
	[XuiXmlBinding("crossplaytooltip")]
	public string CrossplayTooltip
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.All) ?? Localization.Get("xuiOptionsGeneralCrossplayTooltip", false, null);
		}
	}

	// Token: 0x06006CB2 RID: 27826 RVA: 0x002A55D4 File Offset: 0x002A37D4
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

	// Token: 0x06006CB3 RID: 27827 RVA: 0x002A57A2 File Offset: 0x002A39A2
	public override void Cleanup()
	{
		base.Cleanup();
		base.OnVisiblity -= this.OnVisibilityChanged;
	}

	// Token: 0x06006CB4 RID: 27828 RVA: 0x002A57BC File Offset: 0x002A39BC
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

	// Token: 0x06006CB5 RID: 27829 RVA: 0x002A584B File Offset: 0x002A3A4B
	[XuiBindEvent("OnPress", "fullScreenCollider")]
	[XuiBindEvent("OnPress", "btnCloseFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void CloseFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.CloseFilters();
	}

	// Token: 0x06006CB6 RID: 27830 RVA: 0x002A5853 File Offset: 0x002A3A53
	[XuiBindEvent("OnPress", "btnResetFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.resetFilters();
	}

	// Token: 0x06006CB7 RID: 27831 RVA: 0x002A585B File Offset: 0x002A3A5B
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BackButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.parentBrowser.CloseBrowser();
	}

	// Token: 0x06006CB8 RID: 27832 RVA: 0x002A5868 File Offset: 0x002A3A68
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

	// Token: 0x06006CB9 RID: 27833 RVA: 0x002A58E0 File Offset: 0x002A3AE0
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

	// Token: 0x06006CBA RID: 27834 RVA: 0x002A5905 File Offset: 0x002A3B05
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

	// Token: 0x06006CBB RID: 27835 RVA: 0x002A592C File Offset: 0x002A3B2C
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

	// Token: 0x06006CBC RID: 27836 RVA: 0x002A59D8 File Offset: 0x002A3BD8
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

	// Token: 0x06006CBD RID: 27837 RVA: 0x002A5AB6 File Offset: 0x002A3CB6
	public void CloseFilters()
	{
		this.windowGroup.Controller.GetChildByType<XUiC_ServerBrowser>().ShowingFilters = false;
		this.xui.playerUI.CursorController.SetNavigationLockView(null, null);
	}

	// Token: 0x06006CBE RID: 27838 RVA: 0x002A5AE5 File Offset: 0x002A3CE5
	public void SetServersList(XUiC_ServersList _serversList)
	{
		_serversList.OnFilterResultsChanged += this.ServersList_OnFilterResultsChanged;
	}

	// Token: 0x06006CBF RID: 27839 RVA: 0x002A5AF9 File Offset: 0x002A3CF9
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnFilterResultsChanged(int _count)
	{
		this.ResultCount = _count;
		this.IsDirty = true;
	}

	// Token: 0x06006CC0 RID: 27840 RVA: 0x002A5B09 File Offset: 0x002A3D09
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

	// Token: 0x06006CC1 RID: 27841 RVA: 0x002A5B24 File Offset: 0x002A3D24
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnVisibilityChanged(XUiController _sender, bool _visibleSelf, bool _visibleInScene)
	{
		if (_visibleInScene)
		{
			this.selectInitialElement();
		}
	}

	// Token: 0x06006CC2 RID: 27842 RVA: 0x002A5B2F File Offset: 0x002A3D2F
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

	// Token: 0x06006CC3 RID: 27843 RVA: 0x002A5B5A File Offset: 0x002A3D5A
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectInitialElement()
	{
		this.GetInitialSelectedElement().Controller.SelectCursorElement(true, false);
	}

	// Token: 0x06006CC4 RID: 27844 RVA: 0x002A5B6F File Offset: 0x002A3D6F
	public XUiView GetInitialSelectedElement()
	{
		return base.GetChildById(ServerListManager.Instance.IsPrefilteredSearch ? "btnStartSearch" : "btnResetFilters").ViewComponent;
	}

	// Token: 0x06006CC5 RID: 27845 RVA: 0x002A5B94 File Offset: 0x002A3D94
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

	// Token: 0x040052E4 RID: 21220
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IServerBrowserFilterControl> allFilterControls = new List<IServerBrowserFilterControl>();

	// Token: 0x040052E6 RID: 21222
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo regionFilter;

	// Token: 0x040052E7 RID: 21223
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefString languageFilter;

	// Token: 0x040052E8 RID: 21224
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo crossplayFilter;

	// Token: 0x040052E9 RID: 21225
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo moddedConfigsFilter;

	// Token: 0x040052EA RID: 21226
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo requiresModsFilter;

	// Token: 0x040052EB RID: 21227
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo eacFilter;

	// Token: 0x040052EC RID: 21228
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ServerBrowserGamePrefSelectorCombo ignoreSanctionsFilter;

	// Token: 0x040052ED RID: 21229
	[PublicizedFrom(EAccessModifier.Private)]
	public bool openedBefore;

	// Token: 0x040052EE RID: 21230
	[XuiBindParent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerBrowser parentBrowser;

	// Token: 0x040052EF RID: 21231
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_FullScreenCollider fullScreenCollider;

	// Token: 0x040052F0 RID: 21232
	[XuiBindComponent("btnStartSearch", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStartSearch;

	// Token: 0x040052F1 RID: 21233
	[XuiBindComponent("btnResetFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnResetFilters;

	// Token: 0x040052F2 RID: 21234
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040052F3 RID: 21235
	[XuiBindComponent("btnCloseFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCloseFilters;
}
