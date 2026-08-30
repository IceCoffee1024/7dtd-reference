using System;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F8F RID: 3983
[Preserve]
public class XUiC_PlayersListEntry : XUiController
{
	// Token: 0x17000E88 RID: 3720
	// (set) Token: 0x06007DA3 RID: 32163 RVA: 0x002FC411 File Offset: 0x002FA611
	public bool ShowOnMapEnabled
	{
		set
		{
			this.buttonShowOnMap.Enabled = value;
			this.buttonShowOnMap.IsVisible = value;
			this.labelShowOnMap.IsVisible = !value;
		}
	}

	// Token: 0x17000E89 RID: 3721
	// (set) Token: 0x06007DA4 RID: 32164 RVA: 0x002FC43C File Offset: 0x002FA63C
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

	// Token: 0x17000E8A RID: 3722
	// (get) Token: 0x06007DA5 RID: 32165 RVA: 0x002FC50D File Offset: 0x002FA70D
	// (set) Token: 0x06007DA6 RID: 32166 RVA: 0x002FC515 File Offset: 0x002FA715
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

	// Token: 0x17000E8B RID: 3723
	// (get) Token: 0x06007DA7 RID: 32167 RVA: 0x002FC524 File Offset: 0x002FA724
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

	// Token: 0x17000E8C RID: 3724
	// (get) Token: 0x06007DA8 RID: 32168 RVA: 0x002FC550 File Offset: 0x002FA750
	// (set) Token: 0x06007DA9 RID: 32169 RVA: 0x002FC558 File Offset: 0x002FA758
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

	// Token: 0x17000E8D RID: 3725
	// (set) Token: 0x06007DAA RID: 32170 RVA: 0x002FC567 File Offset: 0x002FA767
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

	// Token: 0x17000E8E RID: 3726
	// (get) Token: 0x06007DAB RID: 32171 RVA: 0x002FC57D File Offset: 0x002FA77D
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

	// Token: 0x17000E8F RID: 3727
	// (get) Token: 0x06007DAC RID: 32172 RVA: 0x002FC5A6 File Offset: 0x002FA7A6
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

	// Token: 0x17000E90 RID: 3728
	// (get) Token: 0x06007DAD RID: 32173 RVA: 0x002FC5CF File Offset: 0x002FA7CF
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

	// Token: 0x06007DAE RID: 32174 RVA: 0x002FC5F8 File Offset: 0x002FA7F8
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

	// Token: 0x06007DAF RID: 32175 RVA: 0x002FC91E File Offset: 0x002FAB1E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Shutdown()
	{
		this.xui.OnShutdown -= this.Shutdown;
		PlatformUserManager.BlockedStateChanged -= this.playerBlockStateChanged;
	}

	// Token: 0x06007DB0 RID: 32176 RVA: 0x002FC948 File Offset: 0x002FAB48
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

	// Token: 0x06007DB1 RID: 32177 RVA: 0x002FC9ED File Offset: 0x002FABED
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveAlly_ItemClicked(XUiC_PopupMenuItem.Entry entry)
	{
		if (this.PlayerData != null)
		{
			this.PlayersList.RemoveInvitePress(this.PlayerData);
		}
	}

	// Token: 0x06007DB2 RID: 32178 RVA: 0x002FCA08 File Offset: 0x002FAC08
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

	// Token: 0x06007DB3 RID: 32179 RVA: 0x002FCD0C File Offset: 0x002FAF0C
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

	// Token: 0x06007DB4 RID: 32180 RVA: 0x002FCD7C File Offset: 0x002FAF7C
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

	// Token: 0x06007DB5 RID: 32181 RVA: 0x002FCDEC File Offset: 0x002FAFEC
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

	// Token: 0x06007DB6 RID: 32182 RVA: 0x002FCE46 File Offset: 0x002FB046
	[PublicizedFrom(EAccessModifier.Private)]
	public void onReportPlayerPress(XUiController _sender, int _mouseButton)
	{
		if (PlatformManager.MultiPlatform.PlayerReporting != null && this.PlayerData != null)
		{
			XUiC_ReportPlayer.Open(this.PlayerData.PlayerData, EnumReportCategory.None, "");
		}
	}

	// Token: 0x06007DB7 RID: 32183 RVA: 0x002FCE74 File Offset: 0x002FB074
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

	// Token: 0x06007DB8 RID: 32184 RVA: 0x002FCF48 File Offset: 0x002FB148
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

	// Token: 0x06007DB9 RID: 32185 RVA: 0x002FD1EC File Offset: 0x002FB3EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void onShowOnMapPress(XUiController _sender, int _mouseButton)
	{
		this.PlayersList.ShowOnMap(this.EntityId);
	}

	// Token: 0x06007DBA RID: 32186 RVA: 0x002FD200 File Offset: 0x002FB400
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

	// Token: 0x06007DBB RID: 32187 RVA: 0x002FD24D File Offset: 0x002FB44D
	[PublicizedFrom(EAccessModifier.Private)]
	public void textChatButtonOnPress(XUiController _sender, int _mouseButton)
	{
		this.blockButtonPressed(EBlockType.TextChat);
	}

	// Token: 0x06007DBC RID: 32188 RVA: 0x002FD256 File Offset: 0x002FB456
	[PublicizedFrom(EAccessModifier.Private)]
	public void voiceChatButtonOnPress(XUiController _sender, int _mouseButton)
	{
		this.blockButtonPressed(EBlockType.VoiceChat);
	}

	// Token: 0x06007DBD RID: 32189 RVA: 0x002FD260 File Offset: 0x002FB460
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

	// Token: 0x06007DBE RID: 32190 RVA: 0x002FD2A0 File Offset: 0x002FB4A0
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

	// Token: 0x06007DBF RID: 32191 RVA: 0x002FD34C File Offset: 0x002FB54C
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

	// Token: 0x04005D11 RID: 23825
	public XUiC_PlayersList PlayersList;

	// Token: 0x04005D12 RID: 23826
	public int EntityId;

	// Token: 0x04005D13 RID: 23827
	public PersistentPlayerData PlayerData;

	// Token: 0x04005D14 RID: 23828
	public XUiC_PlayerName PlayerName;

	// Token: 0x04005D15 RID: 23829
	public XUiV_Label ZombieKillsText;

	// Token: 0x04005D16 RID: 23830
	public XUiV_Label PlayerKillsText;

	// Token: 0x04005D17 RID: 23831
	public XUiV_Label DeathsText;

	// Token: 0x04005D18 RID: 23832
	public XUiV_Label LevelText;

	// Token: 0x04005D19 RID: 23833
	public XUiV_Label PingText;

	// Token: 0x04005D1A RID: 23834
	public XUiV_Label GamestageText;

	// Token: 0x04005D1B RID: 23835
	public XUiV_Sprite AdminSprite;

	// Token: 0x04005D1C RID: 23836
	public XUiV_Sprite TwitchSprite;

	// Token: 0x04005D1D RID: 23837
	public XUiV_Sprite TwitchDisabledSprite;

	// Token: 0x04005D1E RID: 23838
	public XUiV_Label labelPartyIcon;

	// Token: 0x04005D1F RID: 23839
	public XUiV_Label labelAllyIcon;

	// Token: 0x04005D20 RID: 23840
	public XUiV_Label labelShowOnMap;

	// Token: 0x04005D21 RID: 23841
	public bool IsFriend;

	// Token: 0x04005D22 RID: 23842
	public XUiV_Button buttonShowOnMap;

	// Token: 0x04005D23 RID: 23843
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOffline;

	// Token: 0x04005D24 RID: 23844
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_isLocalPlayer;

	// Token: 0x04005D25 RID: 23845
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PlayersListEntry.EnumPartyStatus m_partyStatus;

	// Token: 0x04005D26 RID: 23846
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite rowBG;

	// Token: 0x04005D27 RID: 23847
	public XUiV_Button Voice;

	// Token: 0x04005D28 RID: 23848
	public XUiV_Button Chat;

	// Token: 0x04005D29 RID: 23849
	public XUiV_Label DistanceToFriend;

	// Token: 0x04005D2A RID: 23850
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button buttonPartyIcon;

	// Token: 0x04005D2B RID: 23851
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button buttonAllyIcon;

	// Token: 0x04005D2C RID: 23852
	public XUiV_Button buttonReportPlayer;

	// Token: 0x04005D2D RID: 23853
	[PublicizedFrom(EAccessModifier.Private)]
	public Color enabledColor;

	// Token: 0x04005D2E RID: 23854
	[PublicizedFrom(EAccessModifier.Private)]
	public Color disabledColor;

	// Token: 0x04005D2F RID: 23855
	[PublicizedFrom(EAccessModifier.Private)]
	public Color alternatingColor;

	// Token: 0x04005D30 RID: 23856
	[PublicizedFrom(EAccessModifier.Private)]
	public static string sentKeyword = "";

	// Token: 0x04005D31 RID: 23857
	[PublicizedFrom(EAccessModifier.Private)]
	public static string receivedKeyword = "";

	// Token: 0x04005D32 RID: 23858
	[PublicizedFrom(EAccessModifier.Private)]
	public static string naKeyword = "";

	// Token: 0x04005D33 RID: 23859
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTime;

	// Token: 0x02000F90 RID: 3984
	public enum EnumTrackStatus
	{
		// Token: 0x04005D35 RID: 23861
		Hidden,
		// Token: 0x04005D36 RID: 23862
		NotTracked,
		// Token: 0x04005D37 RID: 23863
		Tracked
	}

	// Token: 0x02000F91 RID: 3985
	public enum EnumPartyStatus
	{
		// Token: 0x04005D39 RID: 23865
		LocalPlayer_InParty,
		// Token: 0x04005D3A RID: 23866
		LocalPlayer_InPartyAsLead,
		// Token: 0x04005D3B RID: 23867
		LocalPlayer_NoParty,
		// Token: 0x04005D3C RID: 23868
		LocalPlayer_Received,
		// Token: 0x04005D3D RID: 23869
		OtherPlayer_InParty,
		// Token: 0x04005D3E RID: 23870
		OtherPlayer_InPartyIsLead,
		// Token: 0x04005D3F RID: 23871
		OtherPlayer_InPartyAsLead,
		// Token: 0x04005D40 RID: 23872
		OtherPlayer_NoParty,
		// Token: 0x04005D41 RID: 23873
		OtherPlayer_NoPartyAsLead,
		// Token: 0x04005D42 RID: 23874
		OtherPlayer_PartyFullAsLead,
		// Token: 0x04005D43 RID: 23875
		OtherPlayer_Sent,
		// Token: 0x04005D44 RID: 23876
		Offline
	}
}
