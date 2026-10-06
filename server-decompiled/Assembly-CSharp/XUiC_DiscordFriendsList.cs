using System;
using System.Collections.Generic;
using Discord.Sdk;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000342 RID: 834
[Preserve]
public class XUiC_DiscordFriendsList : XUiC_List<XUiC_DiscordFriendsList.FriendEntry>
{
	// Token: 0x06001802 RID: 6146 RVA: 0x00089AA8 File Offset: 0x00087CA8
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

	// Token: 0x06001803 RID: 6147 RVA: 0x00089B01 File Offset: 0x00087D01
	[XuiBindEvent("OnPress", "btnDiscordLinkAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordLinkAccount_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_DiscordLogin.Open(delegate
		{
		}, true, false, false, false, (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent());
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x00089B41 File Offset: 0x00087D41
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.FriendsListChanged -= this.discordFriendsListChanged;
		DiscordManager.Instance.StatusChanged -= this.discordStatusChanged;
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x00089B75 File Offset: 0x00087D75
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordFriendsListChanged()
	{
		this.UpdateList();
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x00089B75 File Offset: 0x00087D75
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _obj)
	{
		this.UpdateList();
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x00089B7D File Offset: 0x00087D7D
	public void ToggleSectionVisibility(XUiC_DiscordFriendsList.FriendEntry.ESection _section)
	{
		XUiC_DiscordFriendsList.sectionsOpened[(int)_section] = !XUiC_DiscordFriendsList.sectionsOpened[(int)_section];
		this.UpdateList();
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x00089B98 File Offset: 0x00087D98
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x00089BBC File Offset: 0x00087DBC
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

	// Token: 0x0600180A RID: 6154 RVA: 0x00089CD8 File Offset: 0x00087ED8
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

	// Token: 0x0600180B RID: 6155 RVA: 0x00089D28 File Offset: 0x00087F28
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

	// Token: 0x0600180C RID: 6156 RVA: 0x00089DDA File Offset: 0x00087FDA
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000F7C RID: 3964
	[XuiBindComponent("btnDiscordLinkAccount", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordLinkAccount;

	// Token: 0x04000F7D RID: 3965
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool[] sectionsOpened = new bool[4];

	// Token: 0x04000F7E RID: 3966
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int[] sectionUserCounts = new int[4];

	// Token: 0x04000F7F RID: 3967
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<DiscordManager.DiscordUser> users = new HashSet<DiscordManager.DiscordUser>();

	// Token: 0x04000F80 RID: 3968
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderInServer = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.InServer);

	// Token: 0x04000F81 RID: 3969
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderInGame = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.OnlineInGame);

	// Token: 0x04000F82 RID: 3970
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderOnline = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.Online);

	// Token: 0x04000F83 RID: 3971
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderOffline = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.Offline);

	// Token: 0x02000343 RID: 835
	[Preserve]
	public class FriendEntry : XUiListEntry<XUiC_DiscordFriendsList.FriendEntry>
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x00089E5E File Offset: 0x0008805E
		public XUiC_DiscordFriendsList.FriendEntry.EEntryType EntryType { get; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x00089E66 File Offset: 0x00088066
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

		// Token: 0x06001811 RID: 6161 RVA: 0x00089E80 File Offset: 0x00088080
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

		// Token: 0x06001812 RID: 6162 RVA: 0x00089F10 File Offset: 0x00088110
		public FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection _sectionHeaderType)
		{
			this.EntryType = XUiC_DiscordFriendsList.FriendEntry.EEntryType.SectionHeader;
			this.sectionHeaderType = _sectionHeaderType;
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00089F26 File Offset: 0x00088126
		public FriendEntry(DiscordManager.DiscordUser _discordUser)
		{
			this.User = _discordUser;
			this.EntryType = XUiC_DiscordFriendsList.FriendEntry.EEntryType.User;
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00089F3C File Offset: 0x0008813C
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

		// Token: 0x06001815 RID: 6165 RVA: 0x0008A042 File Offset: 0x00088242
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

		// Token: 0x04000F85 RID: 3973
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_DiscordFriendsList.FriendEntry.ESection sectionHeaderType;

		// Token: 0x04000F86 RID: 3974
		public readonly DiscordManager.DiscordUser User;

		// Token: 0x02000344 RID: 836
		public enum EEntryType
		{
			// Token: 0x04000F88 RID: 3976
			SectionHeader,
			// Token: 0x04000F89 RID: 3977
			User
		}

		// Token: 0x02000345 RID: 837
		public enum ESection
		{
			// Token: 0x04000F8B RID: 3979
			InServer,
			// Token: 0x04000F8C RID: 3980
			OnlineInGame,
			// Token: 0x04000F8D RID: 3981
			Online,
			// Token: 0x04000F8E RID: 3982
			Offline,
			// Token: 0x04000F8F RID: 3983
			Count
		}
	}

	// Token: 0x02000346 RID: 838
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DiscordFriendsListEntryController : XUiC_List<XUiC_DiscordFriendsList.FriendEntry>.XUiC_ListEntry
	{
		// Token: 0x06001816 RID: 6166 RVA: 0x0008A070 File Offset: 0x00088270
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

		// Token: 0x06001817 RID: 6167 RVA: 0x0008A0C9 File Offset: 0x000882C9
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

		// Token: 0x06001818 RID: 6168 RVA: 0x0008A0FE File Offset: 0x000882FE
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

		// Token: 0x06001819 RID: 6169 RVA: 0x0008A128 File Offset: 0x00088328
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

		// Token: 0x0600181A RID: 6170 RVA: 0x0008A1D0 File Offset: 0x000883D0
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

		// Token: 0x0600181B RID: 6171 RVA: 0x0008A267 File Offset: 0x00088467
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

		// Token: 0x0600181C RID: 6172 RVA: 0x0008A27A File Offset: 0x0008847A
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

		// Token: 0x0600181D RID: 6173 RVA: 0x0008A290 File Offset: 0x00088490
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

		// Token: 0x0600181E RID: 6174 RVA: 0x0008A2D8 File Offset: 0x000884D8
		[XuiXmlBinding("section_open")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSectionOpen()
		{
			return this.entryData == null || XUiC_DiscordFriendsList.sectionsOpened[(int)this.entryData.SectionType];
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0008A2F5 File Offset: 0x000884F5
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

		// Token: 0x04000F90 RID: 3984
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiV_Texture avatarTexture;
	}
}
