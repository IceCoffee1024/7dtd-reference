using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FB7 RID: 4023
[Preserve]
public class XUiC_PlayersListEntry : XUiController
{
	// Token: 0x17000EE8 RID: 3816
	// (set) Token: 0x06007EA4 RID: 32420 RVA: 0x00303BBD File Offset: 0x00301DBD
	public bool ShowOnMapEnabled
	{
		set
		{
			this.buttonShowOnMap.Enabled = value;
			this.buttonShowOnMap.IsVisible = value;
			this.labelShowOnMap.IsVisible = !value;
		}
	}

	// Token: 0x17000EE9 RID: 3817
	// (set) Token: 0x06007EA5 RID: 32421 RVA: 0x00303BE8 File Offset: 0x00301DE8
	public bool IsOffline
	{
		set
		{
			this.isOffline = value;
			Color color = (this.isOffline ? this.disabledColor : this.enabledColor);
			if (this.PlayerName.Color != color)
			{
				this.PlayerName.Color = color;
				this.LevelText.Color = color;
				this.GamestageText.Color = color;
				this.labelPartyIcon.Color = color;
				this.labelAllyIcon.Color = color;
				this.labelShowOnMap.Color = color;
				this.buttonAllyIcon.CurrentColor = color;
				this.DistanceToFriend.Color = color;
				this.ZombieKillsText.Color = color;
				this.PlayerKillsText.Color = color;
				this.DeathsText.Color = color;
				this.PingText.Color = color;
			}
		}
	}

	// Token: 0x17000EEA RID: 3818
	// (get) Token: 0x06007EA6 RID: 32422 RVA: 0x00303CB9 File Offset: 0x00301EB9
	// (set) Token: 0x06007EA7 RID: 32423 RVA: 0x00303CC1 File Offset: 0x00301EC1
	public bool IsLocalPlayer
	{
		get
		{
			return this.m_isLocalPlayer;
		}
		set
		{
			this.m_isLocalPlayer = value;
			this.updateInviteStatus();
		}
	}

	// Token: 0x17000EEB RID: 3819
	// (get) Token: 0x06007EA8 RID: 32424 RVA: 0x00303CD0 File Offset: 0x00301ED0
	public AllyStore.AllyStatus AllyStatus
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.PlayerData != null)
			{
				return this.PlayersList.AllyCache.GetStatus(PlatformManager.InternalLocalUserIdentifier, this.PlayerData.PrimaryId);
			}
			return AllyStore.AllyStatus.NotAllied;
		}
	}

	// Token: 0x17000EEC RID: 3820
	// (get) Token: 0x06007EA9 RID: 32425 RVA: 0x00303CFC File Offset: 0x00301EFC
	// (set) Token: 0x06007EAA RID: 32426 RVA: 0x00303D04 File Offset: 0x00301F04
	public XUiC_PlayersListEntry.EnumPartyStatus PartyStatus
	{
		get
		{
			return this.m_partyStatus;
		}
		set
		{
			this.m_partyStatus = value;
			this.updatePartyStatus();
		}
	}

	// Token: 0x17000EED RID: 3821
	// (set) Token: 0x06007EAB RID: 32427 RVA: 0x00303D13 File Offset: 0x00301F13
	public bool IsAlternating
	{
		set
		{
			if (value)
			{
				this.rowBG.Color = this.alternatingColor;
			}
		}
	}

	// Token: 0x17000EEE RID: 3822
	// (get) Token: 0x06007EAC RID: 32428 RVA: 0x00303D29 File Offset: 0x00301F29
	public static string SentKeyword
	{
		get
		{
			if (XUiC_PlayersListEntry.sentKeyword == "")
			{
				XUiC_PlayersListEntry.sentKeyword = Localization.Get("xuiSent", false, null);
			}
			return XUiC_PlayersListEntry.sentKeyword;
		}
	}

	// Token: 0x17000EEF RID: 3823
	// (get) Token: 0x06007EAD RID: 32429 RVA: 0x00303D52 File Offset: 0x00301F52
	public static string ReceivedKeyword
	{
		get
		{
			if (XUiC_PlayersListEntry.receivedKeyword == "")
			{
				XUiC_PlayersListEntry.receivedKeyword = Localization.Get("xuiReceived", false, null);
			}
			return XUiC_PlayersListEntry.receivedKeyword;
		}
	}

	// Token: 0x17000EF0 RID: 3824
	// (get) Token: 0x06007EAE RID: 32430 RVA: 0x00303D7B File Offset: 0x00301F7B
	public static string NAKeyword
	{
		get
		{
			if (XUiC_PlayersListEntry.naKeyword == "")
			{
				XUiC_PlayersListEntry.naKeyword = Localization.Get("xuiNA", false, null);
			}
			return XUiC_PlayersListEntry.naKeyword;
		}
	}

	// Token: 0x06007EAF RID: 32431 RVA: 0x00303DA4 File Offset: 0x00301FA4
	public override void Init()
	{
		base.Init();
		this.PlayerName = (XUiC_PlayerName)base.GetChildById("playerName");
		this.AdminSprite = (XUiV_Sprite)base.GetChildById("admin").ViewComponent;
		this.TwitchSprite = (XUiV_Sprite)base.GetChildById("twitch").ViewComponent;
		this.TwitchDisabledSprite = (XUiV_Sprite)base.GetChildById("twitchDisabled").ViewComponent;
		this.ZombieKillsText = (XUiV_Label)base.GetChildById("zombieKillsText").ViewComponent;
		this.PlayerKillsText = (XUiV_Label)base.GetChildById("playerKillsText").ViewComponent;
		this.DeathsText = (XUiV_Label)base.GetChildById("deathsText").ViewComponent;
		this.LevelText = (XUiV_Label)base.GetChildById("levelText").ViewComponent;
		this.PingText = (XUiV_Label)base.GetChildById("pingText").ViewComponent;
		this.GamestageText = (XUiV_Label)base.GetChildById("gamestageText").ViewComponent;
		this.Voice = (XUiV_Button)base.GetChildById("iconVoice").ViewComponent;
		this.Voice.Controller.OnPress += this.voiceChatButtonOnPress;
		this.Chat = (XUiV_Button)base.GetChildById("iconChat").ViewComponent;
		this.Chat.Controller.OnPress += this.textChatButtonOnPress;
		this.xui.OnShutdown += this.Shutdown;
		this.buttonShowOnMap = (XUiV_Button)base.GetChildById("iconShowOnMap").ViewComponent;
		this.DistanceToFriend = (XUiV_Label)base.GetChildById("labelDistanceWalked").ViewComponent;
		this.rowBG = (XUiV_Sprite)base.GetChildById("background").ViewComponent;
		this.buttonAllyIcon = (XUiV_Button)base.GetChildById("iconAllyIcon").ViewComponent;
		this.buttonAllyIcon.Controller.OnPress += this.oniconAllyIconPress;
		this.buttonPartyIcon = (XUiV_Button)base.GetChildById("iconPartyIcon").ViewComponent;
		this.buttonPartyIcon.Controller.OnPress += this.oniconPartyIconPress;
		this.buttonReportPlayer = (XUiV_Button)base.GetChildById("btnReportPlayer").ViewComponent;
		this.buttonReportPlayer.Controller.OnPress += this.onReportPlayerPress;
		this.buttonShowOnMap.Controller.OnPress += this.onShowOnMapPress;
		this.enabledColor = this.PingText.Color;
		this.labelPartyIcon = (XUiV_Label)base.GetChildById("labelPartyIcon").ViewComponent;
		this.labelAllyIcon = (XUiV_Label)base.GetChildById("labelAllyIcon").ViewComponent;
		this.labelShowOnMap = (XUiV_Label)base.GetChildById("labelShowOnMap").ViewComponent;
		PlatformUserManager.BlockedStateChanged += this.playerBlockStateChanged;
	}

	// Token: 0x06007EB0 RID: 32432 RVA: 0x003040CA File Offset: 0x003022CA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Shutdown()
	{
		this.xui.OnShutdown -= this.Shutdown;
		PlatformUserManager.BlockedStateChanged -= this.playerBlockStateChanged;
	}

	// Token: 0x06007EB1 RID: 32433 RVA: 0x003040F4 File Offset: 0x003022F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void oniconAllyIconPress(XUiController _sender, int _mouseButton)
	{
		if (this.PlayerData == null || this.m_isLocalPlayer)
		{
			return;
		}
		switch (this.AllyStatus)
		{
		case AllyStore.AllyStatus.NotAllied:
		case AllyStore.AllyStatus.IncomingInvite:
			this.PlayersList.AddInvitePress(this.EntityId);
			return;
		case AllyStore.AllyStatus.Allies:
		case AllyStore.AllyStatus.OutgoingInvite:
			this.xui.PopupMenuWindow.Setup(new Vector2i(0, -26), Localization.Get("xuiPlayerAllyActions", false, null)).AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("lblRemove", false, null), "ui_game_symbol_x", true, Array.Empty<object>(), new XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate(this.RemoveAlly_ItemClicked))).Show();
			return;
		default:
			return;
		}
	}

	// Token: 0x06007EB2 RID: 32434 RVA: 0x00304199 File Offset: 0x00302399
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveAlly_ItemClicked(XUiC_PopupMenuItem.Entry entry)
	{
		if (this.PlayerData != null)
		{
			this.PlayersList.RemoveInvitePress(this.PlayerData);
		}
	}

	// Token: 0x06007EB3 RID: 32435 RVA: 0x003041B4 File Offset: 0x003023B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void oniconPartyIconPress(XUiController _sender, int _mouseButton)
	{
		if (GameStats.GetBool(EnumGameStats.AutoParty))
		{
			return;
		}
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		switch (this.m_partyStatus)
		{
		case XUiC_PlayersListEntry.EnumPartyStatus.LocalPlayer_InParty:
			this.xui.PopupMenuWindow.Setup(new Vector2i(0, -26), Localization.Get("xuiPlayerPartyActions", false, null)).AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("lblLeave", false, null), "ui_game_symbol_x", true, Array.Empty<object>(), new XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate(this.LeaveParty_ItemClicked))).Show();
			return;
		case XUiC_PlayersListEntry.EnumPartyStatus.LocalPlayer_InPartyAsLead:
			this.xui.PopupMenuWindow.Setup(new Vector2i(0, -26), Localization.Get("xuiPlayerPartyActions", false, null)).AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("lblLeave", false, null), "ui_game_symbol_x", true, Array.Empty<object>(), new XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate(this.LeaveParty_ItemClicked))).Show();
			return;
		case XUiC_PlayersListEntry.EnumPartyStatus.LocalPlayer_NoParty:
		case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_NoPartyAsLead:
		{
			EntityPlayer entityPlayer2 = GameManager.Instance.World.GetEntity(this.EntityId) as EntityPlayer;
			if (Time.time <= this.lastTime)
			{
				GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttPartyInviteWait", false, null), entityPlayer2.PlayerDisplayName), false, false, 0f);
				return;
			}
			this.lastTime = Time.time + 5f;
			if (!entityPlayer2.partyInvites.Contains(entityPlayer))
			{
				entityPlayer2.AddPartyInvite(entityPlayer.entityId);
			}
			GameManager.ShowTooltip(entityPlayer, string.Format(Localization.Get("ttPartyInviteSent", false, null), entityPlayer2.PlayerDisplayName), false, false, 0f);
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.SendInvite, entityPlayer.entityId, this.EntityId, null, null), false);
				return;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.SendInvite, entityPlayer.entityId, this.EntityId, null, null), false, -1, -1, -1, null, 192, false);
			return;
		}
		case XUiC_PlayersListEntry.EnumPartyStatus.LocalPlayer_Received:
		{
			EntityPlayer entityPlayer3 = GameManager.Instance.World.GetEntity(this.EntityId) as EntityPlayer;
			Manager.PlayInsidePlayerHead("party_join", -1, 0f, false, false);
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.AcceptInvite, this.EntityId, entityPlayer.entityId, null, null), false);
				return;
			}
			if (entityPlayer.Party == null)
			{
				Party.ServerHandleAcceptInvite(entityPlayer3, entityPlayer);
			}
			break;
		}
		case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_InParty:
		case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_InPartyIsLead:
		case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_NoParty:
			break;
		case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_InPartyAsLead:
			this.xui.PopupMenuWindow.Setup(new Vector2i(0, -26), Localization.Get("xuiPlayerPartyActions", false, null)).AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("lblKick", false, null), "ui_game_symbol_x", true, Array.Empty<object>(), new XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate(this.KickParty_ItemClicked))).AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("lblMakeLeader", false, null), "server_favorite", true, Array.Empty<object>(), new XUiC_PopupMenuItem.Entry.MenuItemClickedDelegate(this.MakeLeader_ItemClicked)))
				.Show();
			return;
		default:
			return;
		}
	}

	// Token: 0x06007EB4 RID: 32436 RVA: 0x003044B8 File Offset: 0x003026B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void LeaveParty_ItemClicked(XUiC_PopupMenuItem.Entry entry)
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		Manager.PlayInsidePlayerHead("party_invite_leave", -1, 0f, false, false);
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.LeaveParty, entityPlayer.entityId, this.EntityId, null, null), false);
			return;
		}
		Party.ServerHandleLeaveParty(entityPlayer, this.EntityId);
	}

	// Token: 0x06007EB5 RID: 32437 RVA: 0x00304528 File Offset: 0x00302728
	[PublicizedFrom(EAccessModifier.Private)]
	public void MakeLeader_ItemClicked(XUiC_PopupMenuItem.Entry entry)
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.ChangeLead, entityPlayer.entityId, this.EntityId, null, null), false);
			return;
		}
		Party.ServerHandleChangeLead(GameManager.Instance.World.GetEntity(this.EntityId) as EntityPlayer);
	}

	// Token: 0x06007EB6 RID: 32438 RVA: 0x00304598 File Offset: 0x00302798
	[PublicizedFrom(EAccessModifier.Private)]
	public void KickParty_ItemClicked(XUiC_PopupMenuItem.Entry entry)
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackagePartyActions>().Setup(NetPackagePartyActions.PartyActions.KickFromParty, entityPlayer.entityId, this.EntityId, null, null), false);
			return;
		}
		Party.ServerHandleKickParty(this.EntityId);
	}

	// Token: 0x06007EB7 RID: 32439 RVA: 0x003045F2 File Offset: 0x003027F2
	[PublicizedFrom(EAccessModifier.Private)]
	public void onReportPlayerPress(XUiController _sender, int _mouseButton)
	{
		if (PlatformManager.MultiPlatform.PlayerReporting != null && this.PlayerData != null)
		{
			XUiC_ReportPlayer.Open(this.PlayerData.PlayerData, EnumReportCategory.None, "");
		}
	}

	// Token: 0x06007EB8 RID: 32440 RVA: 0x00304620 File Offset: 0x00302820
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateInviteStatus()
	{
		if (this.PlayerData == null)
		{
			this.buttonAllyIcon.IsVisible = false;
			this.labelAllyIcon.IsVisible = true;
			return;
		}
		if (this.m_isLocalPlayer)
		{
			this.labelAllyIcon.IsVisible = true;
			this.buttonAllyIcon.IsVisible = false;
			return;
		}
		this.labelAllyIcon.IsVisible = false;
		this.buttonAllyIcon.IsVisible = true;
		this.buttonAllyIcon.Enabled = true;
		switch (this.AllyStatus)
		{
		case AllyStore.AllyStatus.NotAllied:
			this.buttonAllyIcon.DefaultSpriteName = "ui_game_symbol_add";
			return;
		case AllyStore.AllyStatus.Allies:
			this.buttonAllyIcon.DefaultSpriteName = "ui_game_symbol_allies";
			return;
		case AllyStore.AllyStatus.OutgoingInvite:
			this.buttonAllyIcon.DefaultSpriteName = "ui_game_symbol_invite";
			return;
		case AllyStore.AllyStatus.IncomingInvite:
			this.buttonAllyIcon.DefaultSpriteName = "ui_game_symbol_invite";
			return;
		default:
			return;
		}
	}

	// Token: 0x06007EB9 RID: 32441 RVA: 0x003046F4 File Offset: 0x003028F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePartyStatus()
	{
		if (GameStats.GetBool(EnumGameStats.AutoParty))
		{
			if (this.m_partyStatus == XUiC_PlayersListEntry.EnumPartyStatus.Offline)
			{
				this.buttonPartyIcon.IsVisible = false;
				this.buttonPartyIcon.DefaultSpriteName = "";
				this.buttonPartyIcon.Enabled = false;
				this.labelPartyIcon.IsVisible = true;
				return;
			}
			this.buttonPartyIcon.IsVisible = true;
			this.buttonPartyIcon.DefaultSpriteName = "ui_game_symbol_players";
			this.buttonPartyIcon.Enabled = false;
			this.labelPartyIcon.IsVisible = false;
			return;
		}
		else
		{
			switch (this.m_partyStatus)
			{
			case XUiC_PlayersListEntry.EnumPartyStatus.LocalPlayer_InParty:
				this.buttonPartyIcon.IsVisible = true;
				this.buttonPartyIcon.DefaultSpriteName = "ui_game_symbol_players";
				this.buttonPartyIcon.Enabled = true;
				this.labelPartyIcon.IsVisible = false;
				return;
			case XUiC_PlayersListEntry.EnumPartyStatus.LocalPlayer_InPartyAsLead:
				this.buttonPartyIcon.IsVisible = true;
				this.buttonPartyIcon.DefaultSpriteName = "server_favorite";
				this.buttonPartyIcon.Enabled = true;
				this.labelPartyIcon.IsVisible = false;
				return;
			case XUiC_PlayersListEntry.EnumPartyStatus.LocalPlayer_NoParty:
			case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_NoParty:
			case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_PartyFullAsLead:
			case XUiC_PlayersListEntry.EnumPartyStatus.Offline:
				this.buttonPartyIcon.IsVisible = false;
				this.buttonPartyIcon.DefaultSpriteName = "";
				this.buttonPartyIcon.Enabled = false;
				this.labelPartyIcon.IsVisible = true;
				return;
			case XUiC_PlayersListEntry.EnumPartyStatus.LocalPlayer_Received:
				this.buttonPartyIcon.IsVisible = true;
				this.buttonPartyIcon.DefaultSpriteName = "ui_game_symbol_invite";
				this.buttonPartyIcon.Enabled = true;
				this.labelPartyIcon.IsVisible = false;
				return;
			case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_InParty:
				this.buttonPartyIcon.IsVisible = true;
				this.buttonPartyIcon.DefaultSpriteName = "ui_game_symbol_players";
				this.buttonPartyIcon.Enabled = true;
				this.labelPartyIcon.IsVisible = false;
				return;
			case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_InPartyIsLead:
				this.buttonPartyIcon.IsVisible = true;
				this.buttonPartyIcon.DefaultSpriteName = "server_favorite";
				this.buttonPartyIcon.Enabled = false;
				this.labelPartyIcon.IsVisible = false;
				return;
			case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_InPartyAsLead:
				this.buttonPartyIcon.IsVisible = true;
				this.buttonPartyIcon.DefaultSpriteName = "ui_game_symbol_players";
				this.buttonPartyIcon.Enabled = true;
				this.labelPartyIcon.IsVisible = false;
				return;
			case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_NoPartyAsLead:
				this.buttonPartyIcon.IsVisible = true;
				this.buttonPartyIcon.DefaultSpriteName = "ui_game_symbol_add";
				this.buttonPartyIcon.Enabled = true;
				this.labelPartyIcon.IsVisible = false;
				return;
			case XUiC_PlayersListEntry.EnumPartyStatus.OtherPlayer_Sent:
				this.buttonPartyIcon.IsVisible = true;
				this.buttonPartyIcon.DefaultSpriteName = "ui_game_symbol_invite";
				this.buttonPartyIcon.Enabled = false;
				this.labelPartyIcon.IsVisible = false;
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x06007EBA RID: 32442 RVA: 0x00304998 File Offset: 0x00302B98
	[PublicizedFrom(EAccessModifier.Private)]
	public void onShowOnMapPress(XUiController _sender, int _mouseButton)
	{
		this.PlayersList.ShowOnMap(this.EntityId);
	}

	// Token: 0x06007EBB RID: 32443 RVA: 0x003049AC File Offset: 0x00302BAC
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "disabled_color")
		{
			this.disabledColor = StringParsers.ParseColor32(value);
			return true;
		}
		if (!(name == "alternating_color"))
		{
			return base.ParseAttribute(name, value);
		}
		this.alternatingColor = StringParsers.ParseColor32(value);
		return true;
	}

	// Token: 0x06007EBC RID: 32444 RVA: 0x003049F9 File Offset: 0x00302BF9
	[PublicizedFrom(EAccessModifier.Private)]
	public void textChatButtonOnPress(XUiController _sender, int _mouseButton)
	{
		this.blockButtonPressed(EBlockType.TextChat);
	}

	// Token: 0x06007EBD RID: 32445 RVA: 0x00304A02 File Offset: 0x00302C02
	[PublicizedFrom(EAccessModifier.Private)]
	public void voiceChatButtonOnPress(XUiController _sender, int _mouseButton)
	{
		this.blockButtonPressed(EBlockType.VoiceChat);
	}

	// Token: 0x06007EBE RID: 32446 RVA: 0x00304A0C File Offset: 0x00302C0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void blockButtonPressed(EBlockType _blockType)
	{
		IPlatformUserBlockedData platformUserBlockedData = this.PlayerData.PlatformData.Blocked[_blockType];
		if (platformUserBlockedData.State == EUserBlockState.ByPlatform)
		{
			return;
		}
		platformUserBlockedData.Locally = !platformUserBlockedData.Locally;
	}

	// Token: 0x06007EBF RID: 32447 RVA: 0x00304A4C File Offset: 0x00302C4C
	public void playerBlockStateChanged(IPlatformUserData _pud, EBlockType _blockType, EUserBlockState _blockState)
	{
		if (this.PlayerData == null || !object.Equals(_pud.PrimaryId, this.PlayerData.PrimaryId))
		{
			return;
		}
		switch (_blockType)
		{
		case EBlockType.TextChat:
			this.updateBlockButton(_blockState, this.Chat, "xuiBlockChat");
			return;
		case EBlockType.VoiceChat:
			this.updateBlockButton(_blockState, this.Voice, "xuiBlockVoice");
			return;
		case EBlockType.Play:
			return;
		default:
			throw new ArgumentOutOfRangeException("_blockType", _blockType, string.Format("{0}.{1} missing implementation for {2}.{3}.", new object[] { "XUiC_PlayersListEntry", "playerBlockStateChanged", "EBlockType", _blockType }));
		}
	}

	// Token: 0x06007EC0 RID: 32448 RVA: 0x00304AF8 File Offset: 0x00302CF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateBlockButton(EUserBlockState _blockState, XUiV_Button _button, string _typeLocalizationKey)
	{
		_button.ManualColors = true;
		Color color;
		switch (_blockState)
		{
		case EUserBlockState.NotBlocked:
			color = Color.white;
			break;
		case EUserBlockState.InGame:
			color = new Color(0.8f, 0.4f, 0f);
			break;
		case EUserBlockState.ByPlatform:
			color = new Color(0.8f, 0f, 0f);
			break;
		default:
			throw new ArgumentOutOfRangeException("_blockState", _blockState, null);
		}
		_button.CurrentColor = color;
		_button.Enabled = _blockState != EUserBlockState.ByPlatform;
		string text = Localization.Get(_typeLocalizationKey, false, null);
		string text2;
		switch (_blockState)
		{
		case EUserBlockState.NotBlocked:
			text2 = "xuiChatNotBlocked";
			break;
		case EUserBlockState.InGame:
			text2 = "xuiChatBlockedInGame";
			break;
		case EUserBlockState.ByPlatform:
			text2 = "xuiChatBlockedByPlatform";
			break;
		default:
			throw new ArgumentOutOfRangeException("_blockState", _blockState, null);
		}
		string text3 = Localization.Get(text2, false, null);
		_button.ToolTip = string.Format(text, text3);
	}

	// Token: 0x04005E01 RID: 24065
	public XUiC_PlayersList PlayersList;

	// Token: 0x04005E02 RID: 24066
	public int EntityId;

	// Token: 0x04005E03 RID: 24067
	public PersistentPlayerData PlayerData;

	// Token: 0x04005E04 RID: 24068
	public XUiC_PlayerName PlayerName;

	// Token: 0x04005E05 RID: 24069
	public XUiV_Label ZombieKillsText;

	// Token: 0x04005E06 RID: 24070
	public XUiV_Label PlayerKillsText;

	// Token: 0x04005E07 RID: 24071
	public XUiV_Label DeathsText;

	// Token: 0x04005E08 RID: 24072
	public XUiV_Label LevelText;

	// Token: 0x04005E09 RID: 24073
	public XUiV_Label PingText;

	// Token: 0x04005E0A RID: 24074
	public XUiV_Label GamestageText;

	// Token: 0x04005E0B RID: 24075
	public XUiV_Sprite AdminSprite;

	// Token: 0x04005E0C RID: 24076
	public XUiV_Sprite TwitchSprite;

	// Token: 0x04005E0D RID: 24077
	public XUiV_Sprite TwitchDisabledSprite;

	// Token: 0x04005E0E RID: 24078
	public XUiV_Label labelPartyIcon;

	// Token: 0x04005E0F RID: 24079
	public XUiV_Label labelAllyIcon;

	// Token: 0x04005E10 RID: 24080
	public XUiV_Label labelShowOnMap;

	// Token: 0x04005E11 RID: 24081
	public bool IsFriend;

	// Token: 0x04005E12 RID: 24082
	public XUiV_Button buttonShowOnMap;

	// Token: 0x04005E13 RID: 24083
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOffline;

	// Token: 0x04005E14 RID: 24084
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_isLocalPlayer;

	// Token: 0x04005E15 RID: 24085
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PlayersListEntry.EnumPartyStatus m_partyStatus;

	// Token: 0x04005E16 RID: 24086
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite rowBG;

	// Token: 0x04005E17 RID: 24087
	public XUiV_Button Voice;

	// Token: 0x04005E18 RID: 24088
	public XUiV_Button Chat;

	// Token: 0x04005E19 RID: 24089
	public XUiV_Label DistanceToFriend;

	// Token: 0x04005E1A RID: 24090
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button buttonPartyIcon;

	// Token: 0x04005E1B RID: 24091
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button buttonAllyIcon;

	// Token: 0x04005E1C RID: 24092
	public XUiV_Button buttonReportPlayer;

	// Token: 0x04005E1D RID: 24093
	[PublicizedFrom(EAccessModifier.Private)]
	public Color enabledColor;

	// Token: 0x04005E1E RID: 24094
	[PublicizedFrom(EAccessModifier.Private)]
	public Color disabledColor;

	// Token: 0x04005E1F RID: 24095
	[PublicizedFrom(EAccessModifier.Private)]
	public Color alternatingColor;

	// Token: 0x04005E20 RID: 24096
	[PublicizedFrom(EAccessModifier.Private)]
	public static string sentKeyword = "";

	// Token: 0x04005E21 RID: 24097
	[PublicizedFrom(EAccessModifier.Private)]
	public static string receivedKeyword = "";

	// Token: 0x04005E22 RID: 24098
	[PublicizedFrom(EAccessModifier.Private)]
	public static string naKeyword = "";

	// Token: 0x04005E23 RID: 24099
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTime;

	// Token: 0x02000FB8 RID: 4024
	public enum EnumTrackStatus
	{
		// Token: 0x04005E25 RID: 24101
		Hidden,
		// Token: 0x04005E26 RID: 24102
		NotTracked,
		// Token: 0x04005E27 RID: 24103
		Tracked
	}

	// Token: 0x02000FB9 RID: 4025
	public enum EnumPartyStatus
	{
		// Token: 0x04005E29 RID: 24105
		LocalPlayer_InParty,
		// Token: 0x04005E2A RID: 24106
		LocalPlayer_InPartyAsLead,
		// Token: 0x04005E2B RID: 24107
		LocalPlayer_NoParty,
		// Token: 0x04005E2C RID: 24108
		LocalPlayer_Received,
		// Token: 0x04005E2D RID: 24109
		OtherPlayer_InParty,
		// Token: 0x04005E2E RID: 24110
		OtherPlayer_InPartyIsLead,
		// Token: 0x04005E2F RID: 24111
		OtherPlayer_InPartyAsLead,
		// Token: 0x04005E30 RID: 24112
		OtherPlayer_NoParty,
		// Token: 0x04005E31 RID: 24113
		OtherPlayer_NoPartyAsLead,
		// Token: 0x04005E32 RID: 24114
		OtherPlayer_PartyFullAsLead,
		// Token: 0x04005E33 RID: 24115
		OtherPlayer_Sent,
		// Token: 0x04005E34 RID: 24116
		Offline
	}
}
