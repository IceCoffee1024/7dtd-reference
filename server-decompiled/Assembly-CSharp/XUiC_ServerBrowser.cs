using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DC4 RID: 3524
[Preserve]
public class XUiC_ServerBrowser : XUiC_PlayGameDialogBase
{
	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x06006BCE RID: 27598 RVA: 0x002A17E7 File Offset: 0x0029F9E7
	// (set) Token: 0x06006BCF RID: 27599 RVA: 0x002A17EF File Offset: 0x0029F9EF
	[XuiXmlBinding("filtersvisible")]
	public bool ShowingFilters
	{
		get
		{
			return this.showingFilters;
		}
		set
		{
			if (value != this.showingFilters)
			{
				this.showingFilters = value;
				if (!this.showingFilters)
				{
					this.filtersHiddenRefreshRequired = true;
				}
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x06006BD0 RID: 27600 RVA: 0x002A1817 File Offset: 0x0029FA17
	[XuiXmlBinding("filtersbuttonselected")]
	public bool HasActiveFilters
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anyFilterSet();
		}
	}

	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x06006BD1 RID: 27601 RVA: 0x002A181F File Offset: 0x0029FA1F
	[XuiXmlBinding("selectedlisttype")]
	public XUiC_ServersList.EnumServerLists SelectedListType
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ServersList xuiC_ServersList = this.serversList;
			if (xuiC_ServersList == null)
			{
				return XUiC_ServersList.EnumServerLists.Dedicated;
			}
			return xuiC_ServersList.CurrentServerTypeList;
		}
	}

	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x06006BD2 RID: 27602 RVA: 0x002A1832 File Offset: 0x0029FA32
	[XuiXmlBinding("typededicatedcount")]
	public int ServersCountDedicated
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ServersList xuiC_ServersList = this.serversList;
			if (xuiC_ServersList == null)
			{
				return 0;
			}
			return xuiC_ServersList.GetServerCount(XUiC_ServersList.EnumServerLists.Dedicated);
		}
	}

	// Token: 0x17000BD4 RID: 3028
	// (get) Token: 0x06006BD3 RID: 27603 RVA: 0x002A1846 File Offset: 0x0029FA46
	[XuiXmlBinding("typepeercount")]
	public int ServersCountPeer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ServersList xuiC_ServersList = this.serversList;
			if (xuiC_ServersList == null)
			{
				return 0;
			}
			return xuiC_ServersList.GetServerCount(XUiC_ServersList.EnumServerLists.Peer);
		}
	}

	// Token: 0x17000BD5 RID: 3029
	// (get) Token: 0x06006BD4 RID: 27604 RVA: 0x002A185A File Offset: 0x0029FA5A
	[XuiXmlBinding("typefriendscount")]
	public int ServersCountFriends
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ServersList xuiC_ServersList = this.serversList;
			if (xuiC_ServersList == null)
			{
				return 0;
			}
			return xuiC_ServersList.GetServerCount(XUiC_ServersList.EnumServerLists.Friends);
		}
	}

	// Token: 0x17000BD6 RID: 3030
	// (get) Token: 0x06006BD5 RID: 27605 RVA: 0x002A186E File Offset: 0x0029FA6E
	[XuiXmlBinding("typehistorycount")]
	public int ServersCountHistory
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ServersList xuiC_ServersList = this.serversList;
			if (xuiC_ServersList == null)
			{
				return 0;
			}
			return xuiC_ServersList.GetServerCount(XUiC_ServersList.EnumServerLists.History);
		}
	}

	// Token: 0x17000BD7 RID: 3031
	// (get) Token: 0x06006BD6 RID: 27606 RVA: 0x002A1882 File Offset: 0x0029FA82
	[XuiXmlBinding("typelancount")]
	public int ServersCountLan
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ServersList xuiC_ServersList = this.serversList;
			if (xuiC_ServersList == null)
			{
				return 0;
			}
			return xuiC_ServersList.GetServerCount(XUiC_ServersList.EnumServerLists.LAN);
		}
	}

	// Token: 0x17000BD8 RID: 3032
	// (get) Token: 0x06006BD7 RID: 27607 RVA: 0x002A1897 File Offset: 0x0029FA97
	[XuiXmlBinding("isprefilteredsearch")]
	public bool IsPrefilteredSearch
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ServerListManager.Instance.IsPrefilteredSearch;
		}
	}

	// Token: 0x17000BD9 RID: 3033
	// (get) Token: 0x06006BD8 RID: 27608 RVA: 0x002A18A3 File Offset: 0x0029FAA3
	[XuiXmlBinding("limitwarningvisible")]
	public bool SearchLimitReached
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.maxSearchResultsReached;
		}
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x06006BD9 RID: 27609 RVA: 0x002A18AB File Offset: 0x0029FAAB
	[XuiXmlBinding("resultlimit")]
	public int SearchLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.searchResultLimit;
		}
	}

	// Token: 0x06006BDA RID: 27610 RVA: 0x002A18B4 File Offset: 0x0029FAB4
	public override void Init()
	{
		base.Init();
		XUiC_ServerBrowser.ID = base.WindowGroup.Id;
		using (IEnumerator<XUiC_ServersList.EnumServerLists> enumerator = EnumUtils.Values<XUiC_ServersList.EnumServerLists>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XUiC_ServersList.EnumServerLists list = enumerator.Current;
				string text = "type" + list.ToStringCached<XUiC_ServersList.EnumServerLists>();
				XUiController xuiController;
				XUiC_Button xuiC_Button;
				if (base.TryGetChildController<XUiController>(text, out xuiController) && xuiController.TryGetChildController<XUiC_Button>("", out xuiC_Button))
				{
					xuiC_Button.OnPress += delegate(XUiController _, int _)
					{
						this.serversList.SetServerTypeFilter(list);
						this.IsDirty = true;
					};
				}
			}
		}
		this.serverFilters.SetServersList(this.serversList);
		this.setServerListType((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() ? XUiC_ServersList.EnumServerLists.Peer : XUiC_ServersList.EnumServerLists.Dedicated);
	}

	// Token: 0x06006BDB RID: 27611 RVA: 0x002A1988 File Offset: 0x0029FB88
	[PublicizedFrom(EAccessModifier.Private)]
	public void setServerListType(XUiC_ServersList.EnumServerLists _type)
	{
		this.serversList.SetServerTypeFilter(_type);
		this.serverInfo.InitializeForListFilter(_type);
		this.IsDirty = true;
	}

	// Token: 0x06006BDC RID: 27612 RVA: 0x002A19AC File Offset: 0x0029FBAC
	[XuiBindEvent("OnPress", "btnDirectConnect")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDirectConnect_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_ServerBrowserDirectConnect.Open(this.xui, delegate
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}, delegate
		{
			_sender.SelectCursorElement(true, false);
		});
	}

	// Token: 0x06006BDD RID: 27613 RVA: 0x002A19F0 File Offset: 0x0029FBF0
	[XuiBindEvent("OnPress", "btnConnect")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnConnect_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_ServerBrowser.<>c__DisplayClass37_0 CS$<>8__locals1 = new XUiC_ServerBrowser.<>c__DisplayClass37_0();
		CS$<>8__locals1.<>4__this = this;
		if (!this.btnConnect.ViewComponent.Enabled)
		{
			return;
		}
		XUiC_ServerBrowser.<>c__DisplayClass37_0 CS$<>8__locals2 = CS$<>8__locals1;
		XUiC_ServersList xuiC_ServersList = this.serversList;
		GameServerInfo gameServerInfo;
		if (xuiC_ServersList == null)
		{
			gameServerInfo = null;
		}
		else
		{
			XUiC_ServersList.ListEntry selectedEntryData = xuiC_ServersList.SelectedEntryData;
			gameServerInfo = ((selectedEntryData != null) ? selectedEntryData.GameServerInfo : null);
		}
		CS$<>8__locals2.gsi = gameServerInfo;
		if (CS$<>8__locals1.gsi == null)
		{
			return;
		}
		XUiC_MultiplayerPrivilegeNotification.ResolvePrivilegesWithDialog(CS$<>8__locals1.gsi.AllowsCrossplay ? (EUserPerms.Multiplayer | EUserPerms.Crossplay) : EUserPerms.Multiplayer, delegate(bool _result)
		{
			if (!_result)
			{
				return;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo = CS$<>8__locals1.gsi;
			IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
			if ((antiCheatClient == null || !antiCheatClient.ClientAntiCheatEnabled()) && CS$<>8__locals1.gsi.EACEnabled)
			{
				XUi xui = CS$<>8__locals1.<>4__this.xui;
				string text = Localization.Get("xuiServerEacNeeded", false, null);
				string text2 = Localization.Get("xuiServerEacNeededText", false, null);
				string text3 = "";
				Action action;
				if ((action = CS$<>8__locals1.<>9__1) == null)
				{
					action = (CS$<>8__locals1.<>9__1 = delegate
					{
						CS$<>8__locals1.<>4__this.btnConnect.SelectCursorElement(true, false);
					});
				}
				XUiC_MessageBoxWindowGroup.ShowOk(xui, text, text2, text3, action, false, false, true);
				return;
			}
			if (CS$<>8__locals1.gsi.GetValue(GameInfoBool.IsPasswordProtected))
			{
				XUiC_ServerPasswordWindow.OpenPasswordWindow(CS$<>8__locals1.<>4__this.xui, false, ServerInfoCache.Instance.GetPassword(CS$<>8__locals1.gsi), false, new Action<string>(CS$<>8__locals1.<>4__this.connectToServer), delegate
				{
				});
				return;
			}
			CS$<>8__locals1.<>4__this.connectToServer("");
		}, -1f, false, null);
	}

	// Token: 0x06006BDE RID: 27614 RVA: 0x002A1A74 File Offset: 0x0029FC74
	[PublicizedFrom(EAccessModifier.Private)]
	public void connectToServer(string _password)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo != null)
		{
			ServerInfoCache.Instance.SavePassword(SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo, _password);
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		SingletonMonoBehaviour<ConnectionManager>.Instance.LastJoinSource = "Server Browser";
		SingletonMonoBehaviour<ConnectionManager>.Instance.Connect(SingletonMonoBehaviour<ConnectionManager>.Instance.LastGameServerInfo);
	}

	// Token: 0x06006BDF RID: 27615 RVA: 0x002A1AE1 File Offset: 0x0029FCE1
	[XuiBindEvent("OnFilterChanged", "serverFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnFilterValueChanged(IServerBrowserFilterControl _sender)
	{
		this.serversList.SetFilter(_sender.GetFilter());
		this.IsDirty = true;
	}

	// Token: 0x06006BE0 RID: 27616 RVA: 0x002A1AFB File Offset: 0x0029FCFB
	[PublicizedFrom(EAccessModifier.Private)]
	public bool anyFilterSet()
	{
		return this.serversList != null && this.serversList.GetActiveFilterCount() > 0;
	}

	// Token: 0x06006BE1 RID: 27617 RVA: 0x002A1B15 File Offset: 0x0029FD15
	[XuiBindEvent("OnPress", "btnShowFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showFilters();
	}

	// Token: 0x06006BE2 RID: 27618 RVA: 0x002A1B20 File Offset: 0x0029FD20
	[PublicizedFrom(EAccessModifier.Private)]
	public void showFilters()
	{
		if (ServerListManager.Instance.IsPrefilteredSearch)
		{
			ServerListManager.Instance.StopSearch();
		}
		this.ShowingFilters = true;
		this.serverFilters.GetChildByType<XUiC_TabSelector>().SelectedTabIndex = 0;
		if (!ServerListManager.Instance.IsPrefilteredSearch)
		{
			this.xui.playerUI.CursorController.SetNavigationLockView(this.serverFilters.ViewComponent, this.serverFilters.GetInitialSelectedElement());
		}
	}

	// Token: 0x06006BE3 RID: 27619 RVA: 0x002A1B94 File Offset: 0x0029FD94
	public override void OnOpen()
	{
		this.IsDirty = true;
		base.OnOpen();
		this.windowGroup.isEscClosable = false;
		ServerListManager.Instance.RegisterGameServerFoundCallback(new GameServerFoundCallback(this.onGameServerFoundCallback), new MaxResultsReachedCallback(this.maxResultsCallback), new ServerSearchErrorCallback(this.serverSearchErrorCallback));
		this.btnConnect.ViewComponent.Enabled = false;
		if (ServerListManager.Instance.IsPrefilteredSearch)
		{
			this.showFilters();
			return;
		}
		ServerListManager.Instance.StartSearch(null);
		this.ShowingFilters = false;
		base.GetChildById("btnBack").SelectCursorElement(false, false);
	}

	// Token: 0x06006BE4 RID: 27620 RVA: 0x002A1C31 File Offset: 0x0029FE31
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MultiplayerPrivilegeNotification.Close();
		ServerListManager.Instance.Disconnect();
	}

	// Token: 0x06006BE5 RID: 27621 RVA: 0x002A1C48 File Offset: 0x0029FE48
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!PermissionsManager.IsMultiplayerAllowed() && this.xui.playerUI.windowManager.IsWindowOpen(this.windowGroup.Id))
		{
			this.CloseBrowser();
		}
		if (this.xui.playerUI.playerInput == null)
		{
			return;
		}
		if (XUiUtils.HotkeysAllowedFor(this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.GUIActions.Apply.WasPressed && this.btnConnect.ViewComponent.Enabled)
		{
			this.BtnConnect_OnPressed(null, 0);
		}
	}

	// Token: 0x06006BE6 RID: 27622 RVA: 0x002A1CF2 File Offset: 0x0029FEF2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void close()
	{
		this.handleBackOrEscape();
	}

	// Token: 0x06006BE7 RID: 27623 RVA: 0x002A1CFC File Offset: 0x0029FEFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void onDirtyUpdate()
	{
		base.onDirtyUpdate();
		if (!this.filtersHiddenRefreshRequired)
		{
			return;
		}
		this.filtersHiddenRefreshRequired = false;
		if (ServerListManager.Instance.IsPrefilteredSearch)
		{
			base.GetChildById("btnBack").SelectCursorElement(true, false);
			return;
		}
		base.GetChildById("btnShowFilters").SelectCursorElement(true, false);
	}

	// Token: 0x06006BE8 RID: 27624 RVA: 0x002A1D52 File Offset: 0x0029FF52
	[PublicizedFrom(EAccessModifier.Private)]
	public void onGameServerDetailsCallback(bool _success, string _message, GameServerInfo _info)
	{
		if (!_success)
		{
			return;
		}
		this.serversList.SetAllChildrenDirty();
		XUiC_ServersList.ListEntry selectedEntryData = this.serversList.SelectedEntryData;
		if (_info.Equals((selectedEntryData != null) ? selectedEntryData.GameServerInfo : null))
		{
			this.serverInfo.SetServerInfo(_info);
		}
	}

	// Token: 0x06006BE9 RID: 27625 RVA: 0x002A1D90 File Offset: 0x0029FF90
	[PublicizedFrom(EAccessModifier.Private)]
	public void onGameServerFoundCallback(IPlatform _owner, GameServerInfo _gameServerInfo, EServerRelationType _source)
	{
		if (!(_gameServerInfo.AllowsCrossplay ? PermissionsManager.IsCrossplayAllowed() : _gameServerInfo.PlayGroup.IsCurrent()))
		{
			return;
		}
		if (Submission.Enabled && !_gameServerInfo.IsCompatibleVersion)
		{
			return;
		}
		if (_gameServerInfo.IsDedicated)
		{
			this.serversList.AddGameServer(_gameServerInfo, _source);
			this.IsDirty = true;
			return;
		}
		string value = _gameServerInfo.GetValue(GameInfoString.CombinedPrimaryId);
		string value2 = _gameServerInfo.GetValue(GameInfoString.CombinedNativeId);
		if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value2))
		{
			this.serversList.AddGameServer(_gameServerInfo, _source);
			this.IsDirty = true;
			return;
		}
		PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromCombinedString(value, true);
		PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromCombinedString(value2, true);
		ThreadManager.StartCoroutine(this.resolveBlocksAndAddServerCoroutine(platformUserIdentifierAbs, platformUserIdentifierAbs2, _gameServerInfo, _source));
	}

	// Token: 0x06006BEA RID: 27626 RVA: 0x002A1E40 File Offset: 0x002A0040
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator resolveBlocksAndAddServerCoroutine(PlatformUserIdentifierAbs _hostPrimaryId, PlatformUserIdentifierAbs _hostNativeId, GameServerInfo _gameServerInfo, EServerRelationType _source)
	{
		IPlatformUserData userData = PlatformUserManager.GetOrCreate(_hostPrimaryId);
		userData.NativeId = _hostNativeId;
		userData.MarkBlockedStateChanged();
		yield return PlatformUserManager.ResolveUserBlockedCoroutine(userData);
		if (userData.Blocked[EBlockType.Play].IsBlocked())
		{
			yield break;
		}
		this.serversList.AddGameServer(_gameServerInfo, _source);
		this.IsDirty = true;
		yield break;
	}

	// Token: 0x06006BEB RID: 27627 RVA: 0x002A1E6C File Offset: 0x002A006C
	[PublicizedFrom(EAccessModifier.Private)]
	public void maxResultsCallback(IPlatform _sourcePlatform, bool _maxReached, int _limit)
	{
		this.maxSearchResultsReached = _maxReached;
		this.searchResultLimit = _limit;
		this.IsDirty = true;
	}

	// Token: 0x06006BEC RID: 27628 RVA: 0x002A1E83 File Offset: 0x002A0083
	[PublicizedFrom(EAccessModifier.Private)]
	public void serverSearchErrorCallback(string _message)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiServerBrowserSearchErrorTitle", false, null), _message, "", null, false, false, true);
	}

	// Token: 0x06006BED RID: 27629 RVA: 0x002A1EA6 File Offset: 0x002A00A6
	[XuiBindEvent("ListEntryDoubleClicked", "serversList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnEntryDoubleClicked(XUiC_List<XUiC_ServersList.ListEntry> _list, XUiC_ServersList.ListEntry _entry)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && this.serversList.SelectedEntryIndex >= 0)
		{
			this.BtnConnect_OnPressed(this, -1);
		}
	}

	// Token: 0x06006BEE RID: 27630 RVA: 0x002A1ED0 File Offset: 0x002A00D0
	[XuiBindEvent("SelectionChanged", "serversList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnSelectionChanged(XUiC_List<XUiC_ServersList.ListEntry> _list, XUiC_ServersList.ListEntry _previousEntry, XUiC_ServersList.ListEntry _newEntry)
	{
		bool flag = _newEntry != null;
		GameServerInfo gameServerInfo = (flag ? _newEntry.GameServerInfo : null);
		this.btnConnect.ViewComponent.Enabled = flag && gameServerInfo.IsCompatibleVersion;
		this.serverInfo.SetServerInfo(gameServerInfo);
		if (flag)
		{
			ServerInformationTcpClient.RequestRules(gameServerInfo, false, new ServerInformationTcpClient.RulesRequestDone(this.onGameServerDetailsCallback));
		}
	}

	// Token: 0x06006BEF RID: 27631 RVA: 0x0008A715 File Offset: 0x00088915
	[XuiBindEvent("CountsChanged", "serversList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnCountsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06006BF0 RID: 27632 RVA: 0x002A1F30 File Offset: 0x002A0130
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleBackOrEscape()
	{
		if (ServerListManager.Instance.IsPrefilteredSearch)
		{
			if (this.ShowingFilters)
			{
				this.CloseBrowser();
				return;
			}
			this.maxSearchResultsReached = false;
			this.serversList.RebuildList(false);
			this.showFilters();
			this.btnConnect.ViewComponent.Enabled = false;
			return;
		}
		else
		{
			if (this.ShowingFilters)
			{
				this.serverFilters.CloseFilters();
				base.GetChildById("btnShowFilters").SelectCursorElement(true, false);
				return;
			}
			this.CloseBrowser();
			return;
		}
	}

	// Token: 0x06006BF1 RID: 27633 RVA: 0x0008A567 File Offset: 0x00088767
	public void CloseBrowser()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x04005255 RID: 21077
	public static string ID = "";

	// Token: 0x04005256 RID: 21078
	[XuiBindComponent("btnConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConnect;

	// Token: 0x04005257 RID: 21079
	[XuiBindComponent("btnDirectConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button[] btnDirectConnect;

	// Token: 0x04005258 RID: 21080
	[XuiBindComponent("btnShowFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnShowFilters;

	// Token: 0x04005259 RID: 21081
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServersList serversList;

	// Token: 0x0400525A RID: 21082
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerFilters serverFilters;

	// Token: 0x0400525B RID: 21083
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerInfo serverInfo;

	// Token: 0x0400525C RID: 21084
	[PublicizedFrom(EAccessModifier.Private)]
	public bool maxSearchResultsReached;

	// Token: 0x0400525D RID: 21085
	[PublicizedFrom(EAccessModifier.Private)]
	public int searchResultLimit;

	// Token: 0x0400525E RID: 21086
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingFilters;

	// Token: 0x0400525F RID: 21087
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filtersHiddenRefreshRequired;
}
