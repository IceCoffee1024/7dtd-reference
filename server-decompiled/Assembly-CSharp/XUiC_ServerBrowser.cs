using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000DDE RID: 3550
[Preserve]
public class XUiC_ServerBrowser : XUiC_PlayGameDialogBase
{
	// Token: 0x17000BF1 RID: 3057
	// (get) Token: 0x06006BEB RID: 27627 RVA: 0x002A609F File Offset: 0x002A429F
	// (set) Token: 0x06006BEC RID: 27628 RVA: 0x002A60A7 File Offset: 0x002A42A7
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

	// Token: 0x17000BF2 RID: 3058
	// (get) Token: 0x06006BED RID: 27629 RVA: 0x002A60CF File Offset: 0x002A42CF
	[XuiXmlBinding("filtersbuttonselected")]
	public bool HasActiveFilters
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anyFilterSet();
		}
	}

	// Token: 0x17000BF3 RID: 3059
	// (get) Token: 0x06006BEE RID: 27630 RVA: 0x002A60D7 File Offset: 0x002A42D7
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

	// Token: 0x17000BF4 RID: 3060
	// (get) Token: 0x06006BEF RID: 27631 RVA: 0x002A60EA File Offset: 0x002A42EA
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

	// Token: 0x17000BF5 RID: 3061
	// (get) Token: 0x06006BF0 RID: 27632 RVA: 0x002A60FE File Offset: 0x002A42FE
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

	// Token: 0x17000BF6 RID: 3062
	// (get) Token: 0x06006BF1 RID: 27633 RVA: 0x002A6112 File Offset: 0x002A4312
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

	// Token: 0x17000BF7 RID: 3063
	// (get) Token: 0x06006BF2 RID: 27634 RVA: 0x002A6126 File Offset: 0x002A4326
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

	// Token: 0x17000BF8 RID: 3064
	// (get) Token: 0x06006BF3 RID: 27635 RVA: 0x002A613A File Offset: 0x002A433A
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

	// Token: 0x17000BF9 RID: 3065
	// (get) Token: 0x06006BF4 RID: 27636 RVA: 0x002A614F File Offset: 0x002A434F
	[XuiXmlBinding("isprefilteredsearch")]
	public bool IsPrefilteredSearch
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return ServerListManager.Instance.IsPrefilteredSearch;
		}
	}

	// Token: 0x17000BFA RID: 3066
	// (get) Token: 0x06006BF5 RID: 27637 RVA: 0x002A615B File Offset: 0x002A435B
	[XuiXmlBinding("limitwarningvisible")]
	public bool SearchLimitReached
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.maxSearchResultsReached;
		}
	}

	// Token: 0x17000BFB RID: 3067
	// (get) Token: 0x06006BF6 RID: 27638 RVA: 0x002A6163 File Offset: 0x002A4363
	[XuiXmlBinding("resultlimit")]
	public int SearchLimit
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.searchResultLimit;
		}
	}

	// Token: 0x06006BF7 RID: 27639 RVA: 0x002A616C File Offset: 0x002A436C
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

	// Token: 0x06006BF8 RID: 27640 RVA: 0x002A6240 File Offset: 0x002A4440
	[PublicizedFrom(EAccessModifier.Private)]
	public void setServerListType(XUiC_ServersList.EnumServerLists _type)
	{
		this.serversList.SetServerTypeFilter(_type);
		this.serverInfo.InitializeForListFilter(_type);
		this.IsDirty = true;
	}

	// Token: 0x06006BF9 RID: 27641 RVA: 0x002A6264 File Offset: 0x002A4464
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

	// Token: 0x06006BFA RID: 27642 RVA: 0x002A62A8 File Offset: 0x002A44A8
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

	// Token: 0x06006BFB RID: 27643 RVA: 0x002A632C File Offset: 0x002A452C
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

	// Token: 0x06006BFC RID: 27644 RVA: 0x002A6399 File Offset: 0x002A4599
	[XuiBindEvent("OnFilterChanged", "serverFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnFilterValueChanged(IServerBrowserFilterControl _sender)
	{
		this.serversList.SetFilter(_sender.GetFilter());
		this.IsDirty = true;
	}

	// Token: 0x06006BFD RID: 27645 RVA: 0x002A63B3 File Offset: 0x002A45B3
	[PublicizedFrom(EAccessModifier.Private)]
	public bool anyFilterSet()
	{
		return this.serversList != null && this.serversList.GetActiveFilterCount() > 0;
	}

	// Token: 0x06006BFE RID: 27646 RVA: 0x002A63CD File Offset: 0x002A45CD
	[XuiBindEvent("OnPress", "btnShowFilters")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ShowFiltersButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.showFilters();
	}

	// Token: 0x06006BFF RID: 27647 RVA: 0x002A63D8 File Offset: 0x002A45D8
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

	// Token: 0x06006C00 RID: 27648 RVA: 0x002A644C File Offset: 0x002A464C
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

	// Token: 0x06006C01 RID: 27649 RVA: 0x002A64E9 File Offset: 0x002A46E9
	public override void OnClose()
	{
		base.OnClose();
		XUiC_MultiplayerPrivilegeNotification.Close();
		ServerListManager.Instance.Disconnect();
	}

	// Token: 0x06006C02 RID: 27650 RVA: 0x002A6500 File Offset: 0x002A4700
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

	// Token: 0x06006C03 RID: 27651 RVA: 0x002A65AA File Offset: 0x002A47AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void close()
	{
		this.handleBackOrEscape();
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x002A65B4 File Offset: 0x002A47B4
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

	// Token: 0x06006C05 RID: 27653 RVA: 0x002A660A File Offset: 0x002A480A
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

	// Token: 0x06006C06 RID: 27654 RVA: 0x002A6648 File Offset: 0x002A4848
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

	// Token: 0x06006C07 RID: 27655 RVA: 0x002A66F8 File Offset: 0x002A48F8
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

	// Token: 0x06006C08 RID: 27656 RVA: 0x002A6724 File Offset: 0x002A4924
	[PublicizedFrom(EAccessModifier.Private)]
	public void maxResultsCallback(IPlatform _sourcePlatform, bool _maxReached, int _limit)
	{
		this.maxSearchResultsReached = _maxReached;
		this.searchResultLimit = _limit;
		this.IsDirty = true;
	}

	// Token: 0x06006C09 RID: 27657 RVA: 0x002A673B File Offset: 0x002A493B
	[PublicizedFrom(EAccessModifier.Private)]
	public void serverSearchErrorCallback(string _message)
	{
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiServerBrowserSearchErrorTitle", false, null), _message, "", null, false, false, true);
	}

	// Token: 0x06006C0A RID: 27658 RVA: 0x002A675E File Offset: 0x002A495E
	[XuiBindEvent("ListEntryDoubleClicked", "serversList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnEntryDoubleClicked(XUiC_List<XUiC_ServersList.ListEntry> _list, XUiC_ServersList.ListEntry _entry)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard && this.serversList.SelectedEntryIndex >= 0)
		{
			this.BtnConnect_OnPressed(this, -1);
		}
	}

	// Token: 0x06006C0B RID: 27659 RVA: 0x002A6788 File Offset: 0x002A4988
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

	// Token: 0x06006C0C RID: 27660 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[XuiBindEvent("CountsChanged", "serversList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void ServersList_OnCountsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06006C0D RID: 27661 RVA: 0x002A67E8 File Offset: 0x002A49E8
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

	// Token: 0x06006C0E RID: 27662 RVA: 0x0008AA1B File Offset: 0x00088C1B
	public void CloseBrowser()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
	}

	// Token: 0x040052B1 RID: 21169
	public static string ID = "";

	// Token: 0x040052B2 RID: 21170
	[XuiBindComponent("btnConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnConnect;

	// Token: 0x040052B3 RID: 21171
	[XuiBindComponent("btnDirectConnect", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button[] btnDirectConnect;

	// Token: 0x040052B4 RID: 21172
	[XuiBindComponent("btnShowFilters", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnShowFilters;

	// Token: 0x040052B5 RID: 21173
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServersList serversList;

	// Token: 0x040052B6 RID: 21174
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerFilters serverFilters;

	// Token: 0x040052B7 RID: 21175
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ServerInfo serverInfo;

	// Token: 0x040052B8 RID: 21176
	[PublicizedFrom(EAccessModifier.Private)]
	public bool maxSearchResultsReached;

	// Token: 0x040052B9 RID: 21177
	[PublicizedFrom(EAccessModifier.Private)]
	public int searchResultLimit;

	// Token: 0x040052BA RID: 21178
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showingFilters;

	// Token: 0x040052BB RID: 21179
	[PublicizedFrom(EAccessModifier.Private)]
	public bool filtersHiddenRefreshRequired;
}
