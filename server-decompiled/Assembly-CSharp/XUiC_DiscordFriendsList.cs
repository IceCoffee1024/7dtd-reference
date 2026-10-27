using System;
using System.Collections.Generic;
using Discord.Sdk;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000344 RID: 836
[Preserve]
public class XUiC_DiscordFriendsList : XUiC_List<XUiC_DiscordFriendsList.FriendEntry>
{
	// Token: 0x06001806 RID: 6150 RVA: 0x00089CDC File Offset: 0x00087EDC
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

	// Token: 0x06001807 RID: 6151 RVA: 0x00089D35 File Offset: 0x00087F35
	[XuiBindEvent("OnPress", "btnDiscordLinkAccount")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnDiscordLinkAccount_OnPress(XUiController _sender, int _mouseButton)
	{
		XUiC_DiscordLogin.Open(delegate
		{
		}, true, false, false, false, (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent());
		DiscordManager.Instance.AuthManager.LoginDiscordUser();
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x00089D75 File Offset: 0x00087F75
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.FriendsListChanged -= this.discordFriendsListChanged;
		DiscordManager.Instance.StatusChanged -= this.discordStatusChanged;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x00089DA9 File Offset: 0x00087FA9
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordFriendsListChanged()
	{
		this.UpdateList();
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x00089DA9 File Offset: 0x00087FA9
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _obj)
	{
		this.UpdateList();
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x00089DB1 File Offset: 0x00087FB1
	public void ToggleSectionVisibility(XUiC_DiscordFriendsList.FriendEntry.ESection _section)
	{
		XUiC_DiscordFriendsList.sectionsOpened[(int)_section] = !XUiC_DiscordFriendsList.sectionsOpened[(int)_section];
		this.UpdateList();
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x00089DCC File Offset: 0x00087FCC
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x00089DF0 File Offset: 0x00087FF0
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

	// Token: 0x0600180E RID: 6158 RVA: 0x00089F0C File Offset: 0x0008810C
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

	// Token: 0x0600180F RID: 6159 RVA: 0x00089F5C File Offset: 0x0008815C
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

	// Token: 0x06001810 RID: 6160 RVA: 0x0008A00E File Offset: 0x0008820E
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000F8C RID: 3980
	[XuiBindComponent("btnDiscordLinkAccount", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDiscordLinkAccount;

	// Token: 0x04000F8D RID: 3981
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool[] sectionsOpened = new bool[4];

	// Token: 0x04000F8E RID: 3982
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int[] sectionUserCounts = new int[4];

	// Token: 0x04000F8F RID: 3983
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<DiscordManager.DiscordUser> users = new HashSet<DiscordManager.DiscordUser>();

	// Token: 0x04000F90 RID: 3984
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderInServer = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.InServer);

	// Token: 0x04000F91 RID: 3985
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderInGame = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.OnlineInGame);

	// Token: 0x04000F92 RID: 3986
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderOnline = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.Online);

	// Token: 0x04000F93 RID: 3987
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_DiscordFriendsList.FriendEntry entryHeaderOffline = new XUiC_DiscordFriendsList.FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection.Offline);

	// Token: 0x02000345 RID: 837
	[Preserve]
	public class FriendEntry : XUiListEntry<XUiC_DiscordFriendsList.FriendEntry>
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x0008A092 File Offset: 0x00088292
		public XUiC_DiscordFriendsList.FriendEntry.EEntryType EntryType { get; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x0008A09A File Offset: 0x0008829A
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

		// Token: 0x06001815 RID: 6165 RVA: 0x0008A0B4 File Offset: 0x000882B4
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

		// Token: 0x06001816 RID: 6166 RVA: 0x0008A144 File Offset: 0x00088344
		public FriendEntry(XUiC_DiscordFriendsList.FriendEntry.ESection _sectionHeaderType)
		{
			this.EntryType = XUiC_DiscordFriendsList.FriendEntry.EEntryType.SectionHeader;
			this.sectionHeaderType = _sectionHeaderType;
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0008A15A File Offset: 0x0008835A
		public FriendEntry(DiscordManager.DiscordUser _discordUser)
		{
			this.User = _discordUser;
			this.EntryType = XUiC_DiscordFriendsList.FriendEntry.EEntryType.User;
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0008A170 File Offset: 0x00088370
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

		// Token: 0x06001819 RID: 6169 RVA: 0x0008A276 File Offset: 0x00088476
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

		// Token: 0x04000F95 RID: 3989
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_DiscordFriendsList.FriendEntry.ESection sectionHeaderType;

		// Token: 0x04000F96 RID: 3990
		public readonly DiscordManager.DiscordUser User;

		// Token: 0x02000346 RID: 838
		public enum EEntryType
		{
			// Token: 0x04000F98 RID: 3992
			SectionHeader,
			// Token: 0x04000F99 RID: 3993
			User
		}

		// Token: 0x02000347 RID: 839
		public enum ESection
		{
			// Token: 0x04000F9B RID: 3995
			InServer,
			// Token: 0x04000F9C RID: 3996
			OnlineInGame,
			// Token: 0x04000F9D RID: 3997
			Online,
			// Token: 0x04000F9E RID: 3998
			Offline,
			// Token: 0x04000F9F RID: 3999
			Count
		}
	}

	// Token: 0x02000348 RID: 840
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DiscordFriendsListEntryController : XUiC_List<XUiC_DiscordFriendsList.FriendEntry>.XUiC_ListEntry
	{
		// Token: 0x0600181A RID: 6170 RVA: 0x0008A2A4 File Offset: 0x000884A4
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

		// Token: 0x0600181B RID: 6171 RVA: 0x0008A2FD File Offset: 0x000884FD
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

		// Token: 0x0600181C RID: 6172 RVA: 0x0008A332 File Offset: 0x00088532
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

		// Token: 0x0600181D RID: 6173 RVA: 0x0008A35C File Offset: 0x0008855C
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

		// Token: 0x0600181E RID: 6174 RVA: 0x0008A404 File Offset: 0x00088604
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

		// Token: 0x0600181F RID: 6175 RVA: 0x0008A49B File Offset: 0x0008869B
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

		// Token: 0x06001820 RID: 6176 RVA: 0x0008A4AE File Offset: 0x000886AE
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

		// Token: 0x06001821 RID: 6177 RVA: 0x0008A4C4 File Offset: 0x000886C4
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

		// Token: 0x06001822 RID: 6178 RVA: 0x0008A50C File Offset: 0x0008870C
		[XuiXmlBinding("section_open")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingSectionOpen()
		{
			return this.entryData == null || XUiC_DiscordFriendsList.sectionsOpened[(int)this.entryData.SectionType];
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0008A529 File Offset: 0x00088729
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

		// Token: 0x04000FA0 RID: 4000
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiV_Texture avatarTexture;
	}
}
