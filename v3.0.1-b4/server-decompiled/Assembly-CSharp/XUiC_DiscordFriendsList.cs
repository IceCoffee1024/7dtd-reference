using System;
using System.Collections.Generic;
using Discord.Sdk;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200033B RID: 827
[Preserve]
public class XUiC_DiscordFriendsList : XUiC_List<XUiC_DiscordFriendsList.FriendEntry>
{
	// Token: 0x060017F2 RID: 6130 RVA: 0x00089188 File Offset: 0x00087388
	public override void Init()
	{
		base.Init();
		DiscordManager.Instance.FriendsListChanged += this.discordFriendsListChanged;
		DiscordManager.Instance.StatusChanged += this.discordStatusChanged;
		for (int i = 0; i < 4; i++)
		{
			XUiC_DiscordFriendsList.sectionsOpened[i] = i != 3;
		}
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x000891E1 File Offset: 0x000873E1
	[XuiBindEvent("OnPress", "btnDiscordLinkAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordLinkAccount_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_DiscordLogin.Open(delegate
		{
		}, true, false, false, false, false);
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x0008921B File Offset: 0x0008741B
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.FriendsListChanged -= this.discordFriendsListChanged;
		DiscordManager.Instance.StatusChanged -= this.discordStatusChanged;
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x0008924F File Offset: 0x0008744F
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordFriendsListChanged()
	{
		this.UpdateList();
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x0008924F File Offset: 0x0008744F
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _obj)
	{
		this.UpdateList();
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x00089257 File Offset: 0x00087457
	public void ToggleSectionVisibility(XUiC_DiscordFriendsList.FriendEntry.ESection _section)
	{
		XUiC_DiscordFriendsList.sectionsOpened[(int)_section] = !XUiC_DiscordFriendsList.sectionsOpened[(int)_section];
		this.UpdateList();
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x00089270 File Offset: 0x00087470
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x00089294 File Offset: 0x00087494
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		Array.Clear(XUiC_DiscordFriendsList.sectionUserCounts, 0, XUiC_DiscordFriendsList.sectionUserCounts.Length);
		this.users.Clear();
		DiscordManager.Instance.GetFriends(this.users);
		DiscordManager.Instance.GetInServer(this.users);
		foreach (DiscordManager.DiscordUser discordUser in this.users)
		{
			XUiC_DiscordFriendsList.FriendEntry friendEntry = new XUiC_DiscordFriendsList.FriendEntry(discordUser);
			this.allEntries.Add(friendEntry);
			XUiC_DiscordFriendsList.sectionUserCounts[(int)friendEntry.SectionType]++;
		}
		this.users.Clear();
		this.allEntries.Add(this.entryHeaderInServer);
		this.allEntries.Add(this.entryHeaderInGame);
		this.allEntries.Add(this.entryHeaderOnline);
		this.allEntries.Add(this.entryHeaderOffline);
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x000893B0 File Offset: 0x000875B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FilterResults(string _textMatch)
	{
		base.FilterResults(_textMatch);
		for (int i = 0; i < 4; i++)
		{
			if (!XUiC_DiscordFriendsList.sectionsOpened[i])
			{
				XUiC_DiscordFriendsList.FriendEntry.ESection sectionType = (XUiC_DiscordFriendsList.FriendEntry.ESection)i;
				this.filteredEntries.RemoveAll((XUiC_DiscordFriendsList.FriendEntry _entry) => _entry.EntryType == XUiC_DiscordFriendsList.FriendEntry.EEntryType.User && _entry.SectionType == sectionType);
			}
		}
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x00089400 File Offset: 0x00087600
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "self_activity")
		{
			_value = DiscordManager.Instance.Presence.JoinableActivitySet.ToString();
			return true;
		}
		if (_bindingName == "discord_is_ready")
		{
			_value = DiscordManager.Instance.IsReady.ToString();
			return true;
		}
		if (_bindingName == "discord_supports_full_accounts")
		{
			_value = DiscordManager.SupportsFullAccounts.ToString();
			return true;
		}
		if (!(_bindingName == "discordaccountlinked"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		DiscordManager.DiscordUser localUser = DiscordManager.Instance.LocalUser;
		_value = (localUser == null || !localUser.IsProvisionalAccount).ToString();
		return true;
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x000894B2 File Offset: 0x000876B2
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000F72 RID: 3954
	[XuiBindComponent("btnDiscordLinkAccount", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordLinkAccount;

	// Token: 0x04000F73 RID: 3955
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool[] sectionsOpened = new bool[4];

	// Token: 0x04000F74 RID: 3956
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int[] sectionUserCounts = new int[4];

	// Token: 0x04000F75 RID: 3957
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<DiscordManager.DiscordUser> users = new HashSet<DiscordManager.DiscordUser>();

	// Token: 0x04000F76 RID: 3958
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderInServer = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.InServer);

	// Token: 0x04000F77 RID: 3959
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderInGame = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.OnlineInGame);

	// Token: 0x04000F78 RID: 3960
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderOnline = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.Online);

	// Token: 0x04000F79 RID: 3961
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderOffline = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.Offline);

	// Token: 0x0200033C RID: 828
	[Preserve]
	public class FriendEntry : XUiListEntry<XUiC_DiscordFriendsList.FriendEntry>
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x00089536 File Offset: 0x00087736
		public XUiC_DiscordFriendsList.FriendEntry.EEntryType EntryType { get; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001800 RID: 6144 RVA: 0x0008953E File Offset: 0x0008773E
		public XUiC_DiscordFriendsList.FriendEntry.ESection SectionType
		{
			get
			{
				if (this.EntryType != XUiC_DiscordFriendsList.FriendEntry.EEntryType.SectionHeader)
				{
					return this.getUserSectionType();
				}
				return this.sectionHeaderType;
			}
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x00089558 File Offset: 0x00087758
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_DiscordFriendsList.FriendEntry.ESection getUserSectionType()
		{
			if (this.User == null)
			{
				return XUiC_DiscordFriendsList.FriendEntry.ESection.Offline;
			}
			if (this.User.InSameSession)
			{
				return XUiC_DiscordFriendsList.FriendEntry.ESection.InServer;
			}
			if (this.User.InGame)
			{
				return XUiC_DiscordFriendsList.FriendEntry.ESection.OnlineInGame;
			}
			XUiC_DiscordFriendsList.FriendEntry.ESection esection;
			switch (this.User.DiscordState)
			{
			case StatusType.Online:
				esection = XUiC_DiscordFriendsList.FriendEntry.ESection.Online;
				break;
			case StatusType.Offline:
				esection = XUiC_DiscordFriendsList.FriendEntry.ESection.Offline;
				break;
			case StatusType.Blocked:
				esection = XUiC_DiscordFriendsList.FriendEntry.ESection.Online;
				break;
			case StatusType.Idle:
				esection = XUiC_DiscordFriendsList.FriendEntry.ESection.Online;
				break;
			case StatusType.Dnd:
				esection = XUiC_DiscordFriendsList.FriendEntry.ESection.Online;
				break;
			case StatusType.Invisible:
				esection = XUiC_DiscordFriendsList.FriendEntry.ESection.Online;
				break;
			case StatusType.Streaming:
				esection = XUiC_DiscordFriendsList.FriendEntry.ESection.Online;
				break;
			case StatusType.Unknown:
				esection = XUiC_DiscordFriendsList.FriendEntry.ESection.Offline;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return esection;
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x000895E8 File Offset: 0x000877E8
		public FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection _sectionHeaderType)
		{
			this.EntryType = XUiC_DiscordFriendsList.FriendEntry.EEntryType.SectionHeader;
			this.sectionHeaderType = _sectionHeaderType;
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x000895FE File Offset: 0x000877FE
		public FriendEntry(DiscordManager.DiscordUser _discordUser)
		{
			this.User = _discordUser;
			this.EntryType = XUiC_DiscordFriendsList.FriendEntry.EEntryType.User;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00089614 File Offset: 0x00087814
		public override int CompareTo(XUiC_DiscordFriendsList.FriendEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			int num = this.SectionType.CompareTo(_otherEntry.SectionType);
			if (num != 0)
			{
				return num;
			}
			num = this.EntryType.CompareTo(_otherEntry.EntryType);
			if (num != 0)
			{
				return num;
			}
			if (_otherEntry.User == null)
			{
				return -1;
			}
			if (this.User == null)
			{
				return 1;
			}
			num = -this.User.InSameSession.CompareTo(_otherEntry.User.InSameSession);
			if (num != 0)
			{
				return num;
			}
			num = -this.User.IsFriend.CompareTo(_otherEntry.User.IsFriend);
			if (num != 0)
			{
				return num;
			}
			num = this.User.DiscordState.CompareTo(_otherEntry.User.DiscordState);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(this.User.DisplayName, _otherEntry.User.DisplayName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0008971A File Offset: 0x0008791A
		public override bool MatchesSearch(string _searchString)
		{
			if (this.EntryType == XUiC_DiscordFriendsList.FriendEntry.EEntryType.SectionHeader)
			{
				return true;
			}
			if (string.IsNullOrEmpty(_searchString))
			{
				return true;
			}
			DiscordManager.DiscordUser user = this.User;
			return user != null && user.DisplayName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04000F7B RID: 3963
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_DiscordFriendsList.FriendEntry.ESection sectionHeaderType;

		// Token: 0x04000F7C RID: 3964
		public readonly DiscordManager.DiscordUser User;

		// Token: 0x0200033D RID: 829
		public enum EEntryType
		{
			// Token: 0x04000F7E RID: 3966
			SectionHeader,
			// Token: 0x04000F7F RID: 3967
			User
		}

		// Token: 0x0200033E RID: 830
		public enum ESection
		{
			// Token: 0x04000F81 RID: 3969
			InServer,
			// Token: 0x04000F82 RID: 3970
			OnlineInGame,
			// Token: 0x04000F83 RID: 3971
			Online,
			// Token: 0x04000F84 RID: 3972
			Offline,
			// Token: 0x04000F85 RID: 3973
			Count
		}
	}

	// Token: 0x0200033F RID: 831
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DiscordFriendsListEntryController : XUiC_List<XUiC_DiscordFriendsList.FriendEntry>.XUiC_ListEntry
	{
		// Token: 0x06001806 RID: 6150 RVA: 0x00089748 File Offset: 0x00087948
		public override void Init()
		{
			base.Init();
			XUiController childById = base.GetChildById("avatar");
			XUiV_Texture xuiV_Texture = ((childById != null) ? childById.ViewComponent : null) as XUiV_Texture;
			if (xuiV_Texture != null)
			{
				this.avatarTexture = xuiV_Texture;
			}
			if (base.ViewComponent.EventOnPress)
			{
				base.OnPress += delegate(XUiController _, int _)
				{
					XUiC_DiscordFriendsList.FriendEntry entry = base.GetEntry();
					if (entry.EntryType == XUiC_DiscordFriendsList.FriendEntry.EEntryType.SectionHeader)
					{
						((XUiC_DiscordFriendsList)this.list).ToggleSectionVisibility(entry.SectionType);
						return;
					}
					bool isPrimaryUI = this.xui.playerUI.isPrimaryUI;
					DiscordManager.DiscordUser user = entry.User;
					XUiC_PopupMenu popupMenuWindow = this.xui.PopupMenuWindow;
					popupMenuWindow.Setup(new Vector2i(0, -26), Localization.Get("xuiDiscordUserActions", false, null));
					if (!isPrimaryUI)
					{
						popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordSendMessage", false, null), "ui_game_symbol_invite", GameManager.Instance.World != null, null, delegate(XUiC_PopupMenuItem.Entry _)
						{
							LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
							uiforPrimaryPlayer.entityPlayer.AimingGun = false;
							uiforPrimaryPlayer.windowManager.Open(XUiC_Chat.ID, true);
							XUiC_Chat.SetChatTarget(uiforPrimaryPlayer.xui, EChatType.Discord, base.GetEntry().User.ID.ToString());
						}));
					}
					popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordSendJoinRequest", false, null), "ui_game_symbol_send_join_request", user.JoinableActivity && !user.InSameSession, null, delegate(XUiC_PopupMenuItem.Entry _)
					{
						user.SendJoinRequest();
					}));
					if (!isPrimaryUI)
					{
						popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordSendInvite", false, null), "ui_game_symbol_send_invite", DiscordManager.Instance.Presence.JoinableActivitySet && !user.InSameSession, null, delegate(XUiC_PopupMenuItem.Entry _)
						{
							user.SendInvite();
						}));
					}
					if (!isPrimaryUI)
					{
						popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordAddGameFriend", false, null), "ui_game_symbol_add_game_friend", !user.IsFriend && user.GameRelationship != RelationshipType.PendingOutgoing, null, delegate(XUiC_PopupMenuItem.Entry _)
						{
							user.SendFriendRequest(true);
						}));
						XUiC_PopupMenu xuiC_PopupMenu = popupMenuWindow;
						string text = Localization.Get("xuiDiscordAddDiscordFriend", false, null);
						string text2 = "ui_game_symbol_add_discord_friend";
						bool flag;
						if (user.DiscordRelationship != RelationshipType.Friend && user.DiscordRelationship != RelationshipType.PendingOutgoing)
						{
							DiscordManager.DiscordUser localUser = DiscordManager.Instance.LocalUser;
							flag = localUser == null || !localUser.IsProvisionalAccount;
						}
						else
						{
							flag = false;
						}
						xuiC_PopupMenu.AddItem(new XUiC_PopupMenuItem.Entry(text, text2, flag, null, delegate(XUiC_PopupMenuItem.Entry _)
						{
							user.SendFriendRequest(false);
						}));
					}
					popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordRemoveFriend", false, null), "ui_game_symbol_x", user.IsFriend, null, delegate(XUiC_PopupMenuItem.Entry _)
					{
						XUiC_MessageBoxWindowGroup.ShowOkCancel(this.xui, Localization.Get("xuiDiscordRemoveFriendConfirmationTitle", false, null), string.Format(Localization.Get("xuiDiscordRemoveFriendConfirmationText", false, null), user.DisplayName), "", new Action(user.RemoveFriend), null, false, false, false);
					}));
					popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordBlockUser", false, null), "ui_game_symbol_player_block", user.DiscordRelationship != RelationshipType.Blocked, null, delegate(XUiC_PopupMenuItem.Entry _)
					{
						XUiC_MessageBoxWindowGroup.ShowOkCancel(this.xui, Localization.Get("xuiDiscordBlockUserConfirmationTitle", false, null), string.Format(Localization.Get("xuiDiscordBlockUserConfirmationText", false, null), user.DisplayName), "", new Action(user.BlockUser), null, false, false, false);
					}));
					if (!isPrimaryUI)
					{
						popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordUserVolume", false, null), "ui_game_symbol_noise", user.Volume, user.InCurrentVoice, null, delegate(XUiC_PopupMenuItem.Entry _, double _value)
						{
							Log.Out(string.Format("[Discord UI] New output volume for user {0}: {1}", user.DisplayName, _value));
							user.Volume = _value;
						}));
					}
					popupMenuWindow.Show();
				};
			}
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000897A1 File Offset: 0x000879A1
		public override void SetEntry(XUiC_DiscordFriendsList.FriendEntry _data)
		{
			base.SetEntry(_data);
			if (this.avatarTexture == null)
			{
				return;
			}
			XUiV_TextureBased xuiV_TextureBased = this.avatarTexture;
			Texture texture;
			if (_data == null)
			{
				texture = null;
			}
			else
			{
				DiscordManager.DiscordUser user = _data.User;
				texture = ((user != null) ? user.Avatar : null);
			}
			xuiV_TextureBased.Texture = texture;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x000897D6 File Offset: 0x000879D6
		[XuiXmlBinding("displayname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDisplayName()
		{
			XUiC_DiscordFriendsList.FriendEntry entryData = this.entryData;
			string text;
			if (entryData == null)
			{
				text = null;
			}
			else
			{
				DiscordManager.DiscordUser user = entryData.User;
				text = ((user != null) ? user.DisplayName : null);
			}
			return text ?? "";
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00089800 File Offset: 0x00087A00
		[XuiXmlBinding("discordstate_icon")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDiscordStateIcon()
		{
			XUiC_DiscordFriendsList.FriendEntry entryData = this.entryData;
			if (((entryData != null) ? entryData.User : null) == null)
			{
				return "";
			}
			string text;
			switch (this.entryData.User.DiscordState)
			{
			case StatusType.Online:
				text = "discord_status_available";
				break;
			case StatusType.Offline:
				text = "discord_status_offline";
				break;
			case StatusType.Blocked:
				text = "";
				break;
			case StatusType.Idle:
				text = "discord_status_idle";
				break;
			case StatusType.Dnd:
				text = "discord_status_dnd";
				break;
			case StatusType.Invisible:
				text = "";
				break;
			case StatusType.Streaming:
				text = "";
				break;
			case StatusType.Unknown:
				text = "";
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return text;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000898A8 File Offset: 0x00087AA8
		[XuiXmlBinding("statustext")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingStatusText()
		{
			XUiC_DiscordFriendsList.FriendEntry entryData = this.entryData;
			DiscordManager.DiscordUser discordUser = ((entryData != null) ? entryData.User : null);
			if (discordUser == null)
			{
				return "";
			}
			if (discordUser.InSameSession)
			{
				Activity activity = discordUser.Activity;
				string text;
				if (activity == null)
				{
					text = null;
				}
				else
				{
					ActivityAssets activityAssets = activity.Assets();
					text = ((activityAssets != null) ? activityAssets.LargeText() : null);
				}
				string text2 = text;
				string text3 = Localization.Get("xuiDiscordSameSession", false, null);
				if (!string.IsNullOrEmpty(text2))
				{
					return text3 + " - " + text2;
				}
				return text3;
			}
			else
			{
				if (discordUser.InGame)
				{
					return discordUser.Activity.Details() ?? "";
				}
				return discordUser.DiscordStateLocalized;
			}
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0008993F File Offset: 0x00087B3F
		[XuiXmlBinding("entry_type")]
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_DiscordFriendsList.FriendEntry.EEntryType bindingEntryType()
		{
			XUiC_DiscordFriendsList.FriendEntry entryData = this.entryData;
			if (entryData == null)
			{
				return XUiC_DiscordFriendsList.FriendEntry.EEntryType.SectionHeader;
			}
			return entryData.EntryType;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00089952 File Offset: 0x00087B52
		[XuiXmlBinding("section_type")]
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_DiscordFriendsList.FriendEntry.ESection bindingSectionType()
		{
			XUiC_DiscordFriendsList.FriendEntry entryData = this.entryData;
			if (entryData == null)
			{
				return XUiC_DiscordFriendsList.FriendEntry.ESection.Offline;
			}
			return entryData.SectionType;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x00089968 File Offset: 0x00087B68
		[XuiXmlBinding("has_joinable_activity")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingHasJoinableActivity()
		{
			XUiC_DiscordFriendsList.FriendEntry entryData = this.entryData;
			bool? flag;
			if (entryData == null)
			{
				flag = null;
			}
			else
			{
				DiscordManager.DiscordUser user = entryData.User;
				flag = ((user != null) ? new bool?(user.JoinableActivity) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x000899B0 File Offset: 0x00087BB0
		[XuiXmlBinding("section_open")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSectionOpen()
		{
			return this.entryData == null || XUiC_DiscordFriendsList.sectionsOpened[(int)this.entryData.SectionType];
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x000899CD File Offset: 0x00087BCD
		[XuiXmlBinding("section_user_count")]
		[PublicizedFrom(EAccessModifier.Private)]
		public int bindingUserCount()
		{
			if (this.entryData != null)
			{
				return XUiC_DiscordFriendsList.sectionUserCounts[(int)this.entryData.SectionType];
			}
			return 0;
		}

		// Token: 0x04000F86 RID: 3974
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiV_Texture avatarTexture;
	}
}
