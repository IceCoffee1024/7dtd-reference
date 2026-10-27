using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FC7 RID: 4039
[Preserve]
public class XUiC_PlayersListEntry : XUiController
{
	// Token: 0x17000EF3 RID: 3827
	// (set) Token: 0x06007EF3 RID: 32499 RVA: 0x003056D9 File Offset: 0x003038D9
	public bool ShowOnMapEnabled
	{
		set
		{
			this.buttonShowOnMap.Enabled = value;
			this.buttonShowOnMap.IsVisible = value;
			this.labelShowOnMap.IsVisible = !value;
		}
	}

	// Token: 0x17000EF4 RID: 3828
	// (set) Token: 0x06007EF4 RID: 32500 RVA: 0x00305704 File Offset: 0x00303904
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

	// Token: 0x17000EF5 RID: 3829
	// (get) Token: 0x06007EF5 RID: 32501 RVA: 0x003057D5 File Offset: 0x003039D5
	// (set) Token: 0x06007EF6 RID: 32502 RVA: 0x003057DD File Offset: 0x003039DD
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

	// Token: 0x17000EF6 RID: 3830
	// (get) Token: 0x06007EF7 RID: 32503 RVA: 0x003057EC File Offset: 0x003039EC
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

	// Token: 0x17000EF7 RID: 3831
	// (get) Token: 0x06007EF8 RID: 32504 RVA: 0x00305818 File Offset: 0x00303A18
	// (set) Token: 0x06007EF9 RID: 32505 RVA: 0x00305820 File Offset: 0x00303A20
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

	// Token: 0x17000EF8 RID: 3832
	// (set) Token: 0x06007EFA RID: 32506 RVA: 0x0030582F File Offset: 0x00303A2F
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

	// Token: 0x17000EF9 RID: 3833
	// (get) Token: 0x06007EFB RID: 32507 RVA: 0x00305845 File Offset: 0x00303A45
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

	// Token: 0x17000EFA RID: 3834
	// (get) Token: 0x06007EFC RID: 32508 RVA: 0x0030586E File Offset: 0x00303A6E
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

	// Token: 0x17000EFB RID: 3835
	// (get) Token: 0x06007EFD RID: 32509 RVA: 0x00305897 File Offset: 0x00303A97
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

	// Token: 0x06007EFE RID: 32510 RVA: 0x003058C0 File Offset: 0x00303AC0
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

	// Token: 0x06007EFF RID: 32511 RVA: 0x00305BE6 File Offset: 0x00303DE6
	[PublicizedFrom(EAccessModifier.Private)]
	public void Shutdown()
	{
		this.xui.OnShutdown -= this.Shutdown;
		PlatformUserManager.BlockedStateChanged -= this.playerBlockStateChanged;
	}

	// Token: 0x06007F00 RID: 32512 RVA: 0x00305C10 File Offset: 0x00303E10
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

	// Token: 0x06007F01 RID: 32513 RVA: 0x00305CB5 File Offset: 0x00303EB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveAlly_ItemClicked(XUiC_PopupMenuItem.Entry entry)
	{
		if (this.PlayerData != null)
		{
			this.PlayersList.RemoveInvitePress(this.PlayerData);
		}
	}

	// Token: 0x06007F02 RID: 32514 RVA: 0x00305CD0 File Offset: 0x00303ED0
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

	// Token: 0x06007F03 RID: 32515 RVA: 0x00305FD4 File Offset: 0x003041D4
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

	// Token: 0x06007F04 RID: 32516 RVA: 0x00306044 File Offset: 0x00304244
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

	// Token: 0x06007F05 RID: 32517 RVA: 0x003060B4 File Offset: 0x003042B4
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

	// Token: 0x06007F06 RID: 32518 RVA: 0x0030610E File Offset: 0x0030430E
	[PublicizedFrom(EAccessModifier.Private)]
	public void onReportPlayerPress(XUiController _sender, int _mouseButton)
	{
		if (PlatformManager.MultiPlatform.PlayerReporting != null && this.PlayerData != null)
		{
			XUiC_ReportPlayer.Open(this.PlayerData.PlayerData, EnumReportCategory.None, "");
		}
	}

	// Token: 0x06007F07 RID: 32519 RVA: 0x0030613C File Offset: 0x0030433C
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

	// Token: 0x06007F08 RID: 32520 RVA: 0x00306210 File Offset: 0x00304410
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

	// Token: 0x06007F09 RID: 32521 RVA: 0x003064B4 File Offset: 0x003046B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void onShowOnMapPress(XUiController _sender, int _mouseButton)
	{
		this.PlayersList.ShowOnMap(this.EntityId);
	}

	// Token: 0x06007F0A RID: 32522 RVA: 0x003064C8 File Offset: 0x003046C8
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

	// Token: 0x06007F0B RID: 32523 RVA: 0x00306515 File Offset: 0x00304715
	[PublicizedFrom(EAccessModifier.Private)]
	public void textChatButtonOnPress(XUiController _sender, int _mouseButton)
	{
		this.blockButtonPressed(EBlockType.TextChat);
	}

	// Token: 0x06007F0C RID: 32524 RVA: 0x0030651E File Offset: 0x0030471E
	[PublicizedFrom(EAccessModifier.Private)]
	public void voiceChatButtonOnPress(XUiController _sender, int _mouseButton)
	{
		this.blockButtonPressed(EBlockType.VoiceChat);
	}

	// Token: 0x06007F0D RID: 32525 RVA: 0x00306528 File Offset: 0x00304728
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

	// Token: 0x06007F0E RID: 32526 RVA: 0x00306568 File Offset: 0x00304768
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

	// Token: 0x06007F0F RID: 32527 RVA: 0x00306614 File Offset: 0x00304814
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

	// Token: 0x04005E3C RID: 24124
	public XUiC_PlayersList PlayersList;

	// Token: 0x04005E3D RID: 24125
	public int EntityId;

	// Token: 0x04005E3E RID: 24126
	public PersistentPlayerData PlayerData;

	// Token: 0x04005E3F RID: 24127
	public XUiC_PlayerName PlayerName;

	// Token: 0x04005E40 RID: 24128
	public XUiV_Label ZombieKillsText;

	// Token: 0x04005E41 RID: 24129
	public XUiV_Label PlayerKillsText;

	// Token: 0x04005E42 RID: 24130
	public XUiV_Label DeathsText;

	// Token: 0x04005E43 RID: 24131
	public XUiV_Label LevelText;

	// Token: 0x04005E44 RID: 24132
	public XUiV_Label PingText;

	// Token: 0x04005E45 RID: 24133
	public XUiV_Label GamestageText;

	// Token: 0x04005E46 RID: 24134
	public XUiV_Sprite AdminSprite;

	// Token: 0x04005E47 RID: 24135
	public XUiV_Sprite TwitchSprite;

	// Token: 0x04005E48 RID: 24136
	public XUiV_Sprite TwitchDisabledSprite;

	// Token: 0x04005E49 RID: 24137
	public XUiV_Label labelPartyIcon;

	// Token: 0x04005E4A RID: 24138
	public XUiV_Label labelAllyIcon;

	// Token: 0x04005E4B RID: 24139
	public XUiV_Label labelShowOnMap;

	// Token: 0x04005E4C RID: 24140
	public bool IsFriend;

	// Token: 0x04005E4D RID: 24141
	public XUiV_Button buttonShowOnMap;

	// Token: 0x04005E4E RID: 24142
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOffline;

	// Token: 0x04005E4F RID: 24143
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_isLocalPlayer;

	// Token: 0x04005E50 RID: 24144
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PlayersListEntry.EnumPartyStatus m_partyStatus;

	// Token: 0x04005E51 RID: 24145
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite rowBG;

	// Token: 0x04005E52 RID: 24146
	public XUiV_Button Voice;

	// Token: 0x04005E53 RID: 24147
	public XUiV_Button Chat;

	// Token: 0x04005E54 RID: 24148
	public XUiV_Label DistanceToFriend;

	// Token: 0x04005E55 RID: 24149
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button buttonPartyIcon;

	// Token: 0x04005E56 RID: 24150
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button buttonAllyIcon;

	// Token: 0x04005E57 RID: 24151
	public XUiV_Button buttonReportPlayer;

	// Token: 0x04005E58 RID: 24152
	[PublicizedFrom(EAccessModifier.Private)]
	public Color enabledColor;

	// Token: 0x04005E59 RID: 24153
	[PublicizedFrom(EAccessModifier.Private)]
	public Color disabledColor;

	// Token: 0x04005E5A RID: 24154
	[PublicizedFrom(EAccessModifier.Private)]
	public Color alternatingColor;

	// Token: 0x04005E5B RID: 24155
	[PublicizedFrom(EAccessModifier.Private)]
	public static string sentKeyword = "";

	// Token: 0x04005E5C RID: 24156
	[PublicizedFrom(EAccessModifier.Private)]
	public static string receivedKeyword = "";

	// Token: 0x04005E5D RID: 24157
	[PublicizedFrom(EAccessModifier.Private)]
	public static string naKeyword = "";

	// Token: 0x04005E5E RID: 24158
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTime;

	// Token: 0x02000FC8 RID: 4040
	public enum EnumTrackStatus
	{
		// Token: 0x04005E60 RID: 24160
		Hidden,
		// Token: 0x04005E61 RID: 24161
		NotTracked,
		// Token: 0x04005E62 RID: 24162
		Tracked
	}

	// Token: 0x02000FC9 RID: 4041
	public enum EnumPartyStatus
	{
		// Token: 0x04005E64 RID: 24164
		LocalPlayer_InParty,
		// Token: 0x04005E65 RID: 24165
		LocalPlayer_InPartyAsLead,
		// Token: 0x04005E66 RID: 24166
		LocalPlayer_NoParty,
		// Token: 0x04005E67 RID: 24167
		LocalPlayer_Received,
		// Token: 0x04005E68 RID: 24168
		OtherPlayer_InParty,
		// Token: 0x04005E69 RID: 24169
		OtherPlayer_InPartyIsLead,
		// Token: 0x04005E6A RID: 24170
		OtherPlayer_InPartyAsLead,
		// Token: 0x04005E6B RID: 24171
		OtherPlayer_NoParty,
		// Token: 0x04005E6C RID: 24172
		OtherPlayer_NoPartyAsLead,
		// Token: 0x04005E6D RID: 24173
		OtherPlayer_PartyFullAsLead,
		// Token: 0x04005E6E RID: 24174
		OtherPlayer_Sent,
		// Token: 0x04005E6F RID: 24175
		Offline
	}
}
