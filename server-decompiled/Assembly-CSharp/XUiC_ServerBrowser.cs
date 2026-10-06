using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DD3 RID: 3539
[Preserve]
public class XUiC_ServerBrowser : XUiC_PlayGameDialogBase
{
	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x06006BC6 RID: 27590 RVA: 0x002A4B77 File Offset: 0x002A2D77
	// (set) Token: 0x06006BC7 RID: 27591 RVA: 0x002A4B7F File Offset: 0x002A2D7F
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

	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x06006BC8 RID: 27592 RVA: 0x002A4BA7 File Offset: 0x002A2DA7
	[XuiXmlBinding("filtersbuttonselected")]
	public bool HasActiveFilters
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anyFilterSet();
		}
	}

	// Token: 0x17000BED RID: 3053
	// (get) Token: 0x06006BC9 RID: 27593 RVA: 0x002A4BAF File Offset: 0x002A2DAF
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

	// Token: 0x17000BEE RID: 3054
	// (get) Token: 0x06006BCA RID: 27594 RVA: 0x002A4BC2 File Offset: 0x002A2DC2
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

	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x06006BCB RID: 27595 RVA: 0x002A4BD6 File Offset: 0x002A2DD6
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

	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x06006BCC RID: 27596 RVA: 0x002A4BEA File Offset: 0x002A2DEA
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

	// Token: 0x17000BF1 RID: 3057
	// (get) Token: 0x06006BCD RID: 27597 RVA: 0x002A4BFE File Offset: 0x002A2DFE
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

	// Token: 0x17000BF2 RID: 3058
	// (get) Token: 0x06006BCE RID: 27598 RVA: 0x002A4C12 File Offset: 0x002A2E12
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

	// Token: 0x17000BF3 RID: 3059
	// (get) Token: 0x06006BCF RID: 27599 RVA: 0x002A4C27 File Offset: 0x002A2E27
	[XuiXmlBinding("isprefilteredsearch")]
	public bool IsPrefilteredSearch
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ServerListManager.Instance.IsPrefilteredSearch;
		}
	}

	// Token: 0x17000BF4 RID: 3060
	// (get) Token: 0x06006BD0 RID: 27600 RVA: 0x002A4C33 File Offset: 0x002A2E33
	[XuiXmlBinding("limitwarningvisible")]
	public bool SearchLimitReached
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.maxSearchResultsReached;
		}
	}

	// Token: 0x17000BF5 RID: 3061
	// (get) Token: 0x06006BD1 RID: 27601 RVA: 0x002A4C3B File Offset: 0x002A2E3B
	[XuiXmlBinding("resultlimit")]
	public int SearchLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.searchResultLimit;
		}
	}

	// Token: 0x06006BD2 RID: 27602 RVA: 0x002A4C44 File Offset: 0x002A2E44
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

	// Token: 0x06006BD3 RID: 27603 RVA: 0x002A4D18 File Offset: 0x002A2F18
	[PublicizedFrom(EAccessModifier.Private)]
	public void setServerListType(XUiC_ServersList.EnumServerLists _type)
	{
		this.serversList.SetServerTypeFilter(_type);
		this.serverInfo.InitializeForListFilter(_type);
		this.IsDirty = true;
	}

	// Token: 0x06006BD4 RID: 27604 RVA: 0x002A4D3C File Offset: 0x002A2F3C
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

	// Token: 0x06006BD5 RID: 27605 RVA: 0x002A4D80 File Offset: 0x002A2F80
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

	// Token: 0x06006BD6 RID: 27606 RVA: 0x002A4E04 File Offset: 0x002A3004
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

	// Token: 0x06006BD7 RID: 27607 RVA: 0x002A4E71 File Offset: 0x002A3071
	[XuiBindEvent("OnFilterChanged", "serverFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnFilterValueChanged(IServerBrowserFilterControl _sender)
	{
		this.serversList.SetFilter(_sender.GetFilter());
		this.IsDirty = true;
	}

	// Token: 0x06006BD8 RID: 27608 RVA: 0x002A4E8B File Offset: 0x002A308B
	[PublicizedFrom(EAccessModifier.Private)]
	public bool anyFilterSet()
	{
		return this.serversList != null && this.serversList.GetActiveFilterCount() > 0;
	}

	// Token: 0x06006BD9 RID: 27609 RVA: 0x002A4EA5 File Offset: 0x002A30A5
	[XuiBindEvent("OnPress", "btnShowFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showFilters();
	}

	// Token: 0x06006BDA RID: 27610 RVA: 0x002A4EB0 File Offset: 0x002A30B0
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

	// Token: 0x06006BDB RID: 27611 RVA: 0x002A4F24 File Offset: 0x002A3124
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

	// Token: 0x06006BDC RID: 27612 RVA: 0x002A4FC1 File Offset: 0x002A31C1
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MultiplayerPrivilegeNotification.Close();
		ServerListManager.Instance.Disconnect();
	}

	// Token: 0x06006BDD RID: 27613 RVA: 0x002A4FD8 File Offset: 0x002A31D8
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

	// Token: 0x06006BDE RID: 27614 RVA: 0x002A5082 File Offset: 0x002A3282
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void close()
	{
		this.handleBackOrEscape();
	}

	// Token: 0x06006BDF RID: 27615 RVA: 0x002A508C File Offset: 0x002A328C
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

	// Token: 0x06006BE0 RID: 27616 RVA: 0x002A50E2 File Offset: 0x002A32E2
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

	// Token: 0x06006BE1 RID: 27617 RVA: 0x002A5120 File Offset: 0x002A3320
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

	// Token: 0x06006BE2 RID: 27618 RVA: 0x002A51D0 File Offset: 0x002A33D0
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

	// Token: 0x06006BE3 RID: 27619 RVA: 0x002A51FC File Offset: 0x002A33FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void maxResultsCallback(IPlatform _sourcePlatform, bool _maxReached, int _limit)
	{
		this.maxSearchResultsReached = _maxReached;
		this.searchResultLimit = _limit;
		this.IsDirty = true;
	}

	// Token: 0x06006BE4 RID: 27620 RVA: 0x002A5213 File Offset: 0x002A3413
	[PublicizedFrom(EAccessModifier.Private)]
	public void serverSearchErrorCallback(string _message)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiServerBrowserSearchErrorTitle", false, null), _message, "", null, false, false, true);
	}

	// Token: 0x06006BE5 RID: 27621 RVA: 0x002A5236 File Offset: 0x002A3436
	[XuiBindEvent("ListEntryDoubleClicked", "serversList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnEntryDoubleClicked(XUiC_List<XUiC_ServersList.ListEntry> _list, XUiC_ServersList.ListEntry _entry)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && this.serversList.SelectedEntryIndex >= 0)
		{
			this.BtnConnect_OnPressed(this, -1);
		}
	}

	// Token: 0x06006BE6 RID: 27622 RVA: 0x002A5260 File Offset: 0x002A3460
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

	// Token: 0x06006BE7 RID: 27623 RVA: 0x0008A995 File Offset: 0x00088B95
	[XuiBindEvent("CountsChanged", "serversList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnCountsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06006BE8 RID: 27624 RVA: 0x002A52C0 File Offset: 0x002A34C0
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

	// Token: 0x06006BE9 RID: 27625 RVA: 0x0008A7E7 File Offset: 0x000889E7
	public void CloseBrowser()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x0400528C RID: 21132
	public static string ID = "";

	// Token: 0x0400528D RID: 21133
	[XuiBindComponent("btnConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConnect;

	// Token: 0x0400528E RID: 21134
	[XuiBindComponent("btnDirectConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button[] btnDirectConnect;

	// Token: 0x0400528F RID: 21135
	[XuiBindComponent("btnShowFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnShowFilters;

	// Token: 0x04005290 RID: 21136
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServersList serversList;

	// Token: 0x04005291 RID: 21137
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerFilters serverFilters;

	// Token: 0x04005292 RID: 21138
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerInfo serverInfo;

	// Token: 0x04005293 RID: 21139
	[PublicizedFrom(EAccessModifier.Private)]
	public bool maxSearchResultsReached;

	// Token: 0x04005294 RID: 21140
	[PublicizedFrom(EAccessModifier.Private)]
	public int searchResultLimit;

	// Token: 0x04005295 RID: 21141
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingFilters;

	// Token: 0x04005296 RID: 21142
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filtersHiddenRefreshRequired;
}
