using System;
using System.Collections.Generic;
using Discord.Sdk;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200034C RID: 844
[Preserve]
public class XUiC_DiscordPendingList : XUiC_List<XUiC_DiscordPendingList.PendingEntry>
{
	// Token: 0x06001879 RID: 6265 RVA: 0x0008AD88 File Offset: 0x00088F88
	public override void Init()
	{
		base.Init();
		DiscordManager.Instance.StatusChanged += this.discordStatusChanged;
		DiscordManager.Instance.RelationshipChanged += this.discordRelationshipChanged;
		DiscordManager.Instance.ActivityInviteReceived += this.discordActivityInviteReceived;
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x0008ADE0 File Offset: 0x00088FE0
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.StatusChanged -= this.discordStatusChanged;
		DiscordManager.Instance.RelationshipChanged -= this.discordRelationshipChanged;
		DiscordManager.Instance.ActivityInviteReceived -= this.discordActivityInviteReceived;
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x0008AE35 File Offset: 0x00089035
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		if (_status == DiscordManager.EDiscordStatus.Disconnected)
		{
			this.UpdateList();
		}
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x0008AE41 File Offset: 0x00089041
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordActivityInviteReceived(DiscordManager.DiscordUser _user, bool _cleared, ActivityActionTypes _type)
	{
		this.UpdateList();
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x0008AE41 File Offset: 0x00089041
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordRelationshipChanged(DiscordManager.DiscordUser _user)
	{
		this.UpdateList();
	}

	// Token: 0x0600187E RID: 6270 RVA: 0x0008AE4C File Offset: 0x0008904C
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x0008AE70 File Offset: 0x00089070
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		this.users.Clear();
		DiscordManager.Instance.GetUsersWithPendingAction(this.users);
		foreach (DiscordManager.DiscordUser discordUser in this.users)
		{
			if (discordUser.PendingFriendRequest)
			{
				this.allEntries.Add(new XUiC_DiscordPendingList.PendingEntry(discordUser, (discordUser.GameRelationship == RelationshipType.PendingIncoming) ? XUiC_DiscordPendingList.PendingEntry.EEntryType.FriendRequestGame : XUiC_DiscordPendingList.PendingEntry.EEntryType.FriendRequestDiscord));
			}
			if (discordUser.PendingIncomingJoinRequest)
			{
				this.allEntries.Add(new XUiC_DiscordPendingList.PendingEntry(discordUser, XUiC_DiscordPendingList.PendingEntry.EEntryType.JoinRequest));
			}
			if (discordUser.PendingIncomingInvite)
			{
				this.allEntries.Add(new XUiC_DiscordPendingList.PendingEntry(discordUser, XUiC_DiscordPendingList.PendingEntry.EEntryType.Invite));
			}
		}
		this.users.Clear();
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x0008AF5C File Offset: 0x0008915C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x0008AF66 File Offset: 0x00089166
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000FA6 RID: 4006
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<DiscordManager.DiscordUser> users = new HashSet<DiscordManager.DiscordUser>();

	// Token: 0x0200034D RID: 845
	[Preserve]
	public class PendingEntry : XUiListEntry<XUiC_DiscordPendingList.PendingEntry>
	{
		// Token: 0x06001883 RID: 6275 RVA: 0x0008AF95 File Offset: 0x00089195
		public PendingEntry(DiscordManager.DiscordUser _discordUser, XUiC_DiscordPendingList.PendingEntry.EEntryType _type)
		{
			this.User = _discordUser;
			this.EntryType = _type;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0008AFAC File Offset: 0x000891AC
		public override int CompareTo(XUiC_DiscordPendingList.PendingEntry _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			int num = this.EntryType.CompareTo(_otherEntry.EntryType);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(this.User.DisplayName, _otherEntry.User.DisplayName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0008AFFC File Offset: 0x000891FC
		public override bool MatchesSearch(string _searchString)
		{
			if (string.IsNullOrEmpty(_searchString))
			{
				return true;
			}
			DiscordManager.DiscordUser user = this.User;
			return user != null && user.DisplayName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04000FA7 RID: 4007
		public readonly XUiC_DiscordPendingList.PendingEntry.EEntryType EntryType;

		// Token: 0x04000FA8 RID: 4008
		public readonly DiscordManager.DiscordUser User;

		// Token: 0x0200034E RID: 846
		public enum EEntryType
		{
			// Token: 0x04000FAA RID: 4010
			JoinRequest,
			// Token: 0x04000FAB RID: 4011
			Invite,
			// Token: 0x04000FAC RID: 4012
			FriendRequestGame,
			// Token: 0x04000FAD RID: 4013
			FriendRequestDiscord
		}
	}

	// Token: 0x0200034F RID: 847
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DiscordPendingListEntryController : XUiC_List<XUiC_DiscordPendingList.PendingEntry>.XUiC_ListEntry
	{
		// Token: 0x06001886 RID: 6278 RVA: 0x0008B020 File Offset: 0x00089220
		public override void Init()
		{
			base.Init();
			XUiController childById = base.GetChildById("avatar");
			XUiV_Texture xuiV_Texture = ((childById != null) ? childById.ViewComponent : null) as XUiV_Texture;
			if (xuiV_Texture != null)
			{
				this.avatarTexture = xuiV_Texture;
			}
			XUiController childById2 = base.GetChildById("btnAccept");
			XUiV_Button xuiV_Button = ((childById2 != null) ? childById2.ViewComponent : null) as XUiV_Button;
			if (xuiV_Button != null)
			{
				xuiV_Button.Controller.OnPress += delegate(XUiController _, int _)
				{
					XUiC_DiscordPendingList.PendingEntry entry = base.GetEntry();
					DiscordManager.DiscordUser user = entry.User;
					switch (entry.EntryType)
					{
					case XUiC_DiscordPendingList.PendingEntry.EEntryType.JoinRequest:
						user.SendInvite();
						return;
					case XUiC_DiscordPendingList.PendingEntry.EEntryType.Invite:
						user.AcceptInvite(null);
						return;
					case XUiC_DiscordPendingList.PendingEntry.EEntryType.FriendRequestGame:
					case XUiC_DiscordPendingList.PendingEntry.EEntryType.FriendRequestDiscord:
						user.SendFriendRequest(entry.EntryType == XUiC_DiscordPendingList.PendingEntry.EEntryType.FriendRequestGame);
						return;
					default:
						throw new ArgumentOutOfRangeException();
					}
				};
			}
			XUiController childById3 = base.GetChildById("btnDecline");
			XUiV_Button xuiV_Button2 = ((childById3 != null) ? childById3.ViewComponent : null) as XUiV_Button;
			if (xuiV_Button2 != null)
			{
				xuiV_Button2.Controller.OnPress += delegate(XUiController _, int _)
				{
					XUiC_DiscordPendingList.PendingEntry entry2 = base.GetEntry();
					DiscordManager.DiscordUser user2 = entry2.User;
					switch (entry2.EntryType)
					{
					case XUiC_DiscordPendingList.PendingEntry.EEntryType.JoinRequest:
						user2.DeclineJoinRequest();
						return;
					case XUiC_DiscordPendingList.PendingEntry.EEntryType.Invite:
						user2.DeclineInvite();
						return;
					case XUiC_DiscordPendingList.PendingEntry.EEntryType.FriendRequestGame:
					case XUiC_DiscordPendingList.PendingEntry.EEntryType.FriendRequestDiscord:
						user2.DeclineFriendRequest(entry2.EntryType == XUiC_DiscordPendingList.PendingEntry.EEntryType.FriendRequestGame);
						return;
					default:
						throw new ArgumentOutOfRangeException();
					}
				};
			}
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0008B0C8 File Offset: 0x000892C8
		public override void SetEntry(XUiC_DiscordPendingList.PendingEntry _data)
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

		// Token: 0x06001888 RID: 6280 RVA: 0x0008B0FD File Offset: 0x000892FD
		[XuiXmlBinding("displayname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDisplayName()
		{
			XUiC_DiscordPendingList.PendingEntry entryData = this.entryData;
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

		// Token: 0x06001889 RID: 6281 RVA: 0x0008B126 File Offset: 0x00089326
		[XuiXmlBinding("entry_type")]
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiC_DiscordPendingList.PendingEntry.EEntryType bindingEntryType()
		{
			XUiC_DiscordPendingList.PendingEntry entryData = this.entryData;
			if (entryData == null)
			{
				return XUiC_DiscordPendingList.PendingEntry.EEntryType.Invite;
			}
			return entryData.EntryType;
		}

		// Token: 0x04000FAE RID: 4014
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiV_Texture avatarTexture;
	}
}
