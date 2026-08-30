using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000337 RID: 823
[Preserve]
public class XUiC_DiscordBlockedUsersList : XUiC_List<XUiC_DiscordBlockedUsersList.Entry>
{
	// Token: 0x060017DF RID: 6111 RVA: 0x00088E13 File Offset: 0x00087013
	public override void Init()
	{
		base.Init();
		DiscordManager.Instance.FriendsListChanged += this.discordFriendsListChanged;
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x00088E31 File Offset: 0x00087031
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.FriendsListChanged -= this.discordFriendsListChanged;
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x00088E4F File Offset: 0x0008704F
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordFriendsListChanged()
	{
		this.UpdateList();
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x00088E58 File Offset: 0x00087058
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x00088E7C File Offset: 0x0008707C
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		this.users.Clear();
		DiscordManager.Instance.GetBlockedUsers(this.users);
		foreach (DiscordManager.DiscordUser discordUser in this.users)
		{
			XUiC_DiscordBlockedUsersList.Entry entry = new XUiC_DiscordBlockedUsersList.Entry(discordUser);
			this.allEntries.Add(entry);
		}
		this.users.Clear();
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x00088F1C File Offset: 0x0008711C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000F6E RID: 3950
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<DiscordManager.DiscordUser> users = new HashSet<DiscordManager.DiscordUser>();

	// Token: 0x02000338 RID: 824
	[Preserve]
	public class Entry : XUiListEntry<XUiC_DiscordBlockedUsersList.Entry>
	{
		// Token: 0x060017E6 RID: 6118 RVA: 0x00088F4B File Offset: 0x0008714B
		public Entry(DiscordManager.DiscordUser _discordUser)
		{
			this.User = _discordUser;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00088F5A File Offset: 0x0008715A
		public override int CompareTo(XUiC_DiscordBlockedUsersList.Entry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			if (_otherEntry.User == null)
			{
				return -1;
			}
			if (this.User == null)
			{
				return 1;
			}
			return string.Compare(this.User.DisplayName, _otherEntry.User.DisplayName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00088F91 File Offset: 0x00087191
		public override bool MatchesSearch(string _searchString)
		{
			if (string.IsNullOrEmpty(_searchString))
			{
				return true;
			}
			DiscordManager.DiscordUser user = this.User;
			return user != null && user.DisplayName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04000F6F RID: 3951
		public readonly DiscordManager.DiscordUser User;
	}

	// Token: 0x02000339 RID: 825
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DiscordBlockedUsersListEntryController : XUiC_List<XUiC_DiscordBlockedUsersList.Entry>.XUiC_ListEntry
	{
		// Token: 0x060017E9 RID: 6121 RVA: 0x00088FB4 File Offset: 0x000871B4
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
					XUiC_DiscordBlockedUsersList.Entry entry = base.GetEntry();
					DiscordManager.DiscordUser user = entry.User;
					XUiC_PopupMenu popupMenuWindow = this.xui.PopupMenuWindow;
					popupMenuWindow.Setup(new Vector2i(0, -26), Localization.Get("xuiDiscordUserActions", false, null));
					popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordAddGameFriend", false, null), "ui_game_symbol_add_game_friend", true, null, delegate(XUiC_PopupMenuItem.Entry _)
					{
						user.SendFriendRequest(true);
					}));
					string text = Localization.Get("xuiDiscordAddDiscordFriend", false, null);
					string text2 = "ui_game_symbol_add_discord_friend";
					DiscordManager.DiscordUser localUser = DiscordManager.Instance.LocalUser;
					popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(text, text2, localUser == null || !localUser.IsProvisionalAccount, null, delegate(XUiC_PopupMenuItem.Entry _)
					{
						user.SendFriendRequest(false);
					}));
					popupMenuWindow.AddItem(new XUiC_PopupMenuItem.Entry(Localization.Get("xuiDiscordUnblockUser", false, null), "ui_game_symbol_modded", true, null, delegate(XUiC_PopupMenuItem.Entry _)
					{
						user.UnblockUser();
					}));
					popupMenuWindow.Show();
				};
			}
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0008900D File Offset: 0x0008720D
		public override void SetEntry(XUiC_DiscordBlockedUsersList.Entry _data)
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

		// Token: 0x060017EB RID: 6123 RVA: 0x00089042 File Offset: 0x00087242
		[XuiXmlBinding("displayname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDisplayName()
		{
			XUiC_DiscordBlockedUsersList.Entry entryData = this.entryData;
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

		// Token: 0x04000F70 RID: 3952
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiV_Texture avatarTexture;
	}
}
