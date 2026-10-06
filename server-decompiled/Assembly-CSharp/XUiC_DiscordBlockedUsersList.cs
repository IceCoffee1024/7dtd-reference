using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200033E RID: 830
[Preserve]
public class XUiC_DiscordBlockedUsersList : XUiC_List<XUiC_DiscordBlockedUsersList.Entry>
{
	// Token: 0x060017EF RID: 6127 RVA: 0x00089733 File Offset: 0x00087933
	public override void Init()
	{
		base.Init();
		DiscordManager.Instance.FriendsListChanged += this.discordFriendsListChanged;
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x00089751 File Offset: 0x00087951
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.FriendsListChanged -= this.discordFriendsListChanged;
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x0008976F File Offset: 0x0008796F
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordFriendsListChanged()
	{
		this.UpdateList();
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x00089778 File Offset: 0x00087978
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x0008979C File Offset: 0x0008799C
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

	// Token: 0x060017F4 RID: 6132 RVA: 0x0008983C File Offset: 0x00087A3C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000F78 RID: 3960
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<DiscordManager.DiscordUser> users = new HashSet<DiscordManager.DiscordUser>();

	// Token: 0x0200033F RID: 831
	[Preserve]
	public class Entry : XUiListEntry<XUiC_DiscordBlockedUsersList.Entry>
	{
		// Token: 0x060017F6 RID: 6134 RVA: 0x0008986B File Offset: 0x00087A6B
		public Entry(DiscordManager.DiscordUser _discordUser)
		{
			this.User = _discordUser;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0008987A File Offset: 0x00087A7A
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

		// Token: 0x060017F8 RID: 6136 RVA: 0x000898B1 File Offset: 0x00087AB1
		public override bool MatchesSearch(string _searchString)
		{
			if (string.IsNullOrEmpty(_searchString))
			{
				return true;
			}
			DiscordManager.DiscordUser user = this.User;
			return user != null && user.DisplayName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04000F79 RID: 3961
		public readonly DiscordManager.DiscordUser User;
	}

	// Token: 0x02000340 RID: 832
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DiscordBlockedUsersListEntryController : XUiC_List<XUiC_DiscordBlockedUsersList.Entry>.XUiC_ListEntry
	{
		// Token: 0x060017F9 RID: 6137 RVA: 0x000898D4 File Offset: 0x00087AD4
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

		// Token: 0x060017FA RID: 6138 RVA: 0x0008992D File Offset: 0x00087B2D
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

		// Token: 0x060017FB RID: 6139 RVA: 0x00089962 File Offset: 0x00087B62
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

		// Token: 0x04000F7A RID: 3962
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiV_Texture avatarTexture;
	}
}
