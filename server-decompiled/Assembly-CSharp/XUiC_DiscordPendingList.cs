using System;
using System.Collections.Generic;
using Discord.Sdk;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000353 RID: 851
[Preserve]
public class XUiC_DiscordPendingList : XUiC_List<XUiC_DiscordPendingList.PendingEntry>
{
	// Token: 0x06001891 RID: 6289 RVA: 0x0008B748 File Offset: 0x00089948
	public override void Init()
	{
		base.Init();
		DiscordManager.Instance.StatusChanged += this.discordStatusChanged;
		DiscordManager.Instance.RelationshipChanged += this.discordRelationshipChanged;
		DiscordManager.Instance.ActivityInviteReceived += this.discordActivityInviteReceived;
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x0008B7A0 File Offset: 0x000899A0
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.StatusChanged -= this.discordStatusChanged;
		DiscordManager.Instance.RelationshipChanged -= this.discordRelationshipChanged;
		DiscordManager.Instance.ActivityInviteReceived -= this.discordActivityInviteReceived;
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x0008B7F5 File Offset: 0x000899F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordStatusChanged(DiscordManager.EDiscordStatus _status)
	{
		if (_status == DiscordManager.EDiscordStatus.Disconnected)
		{
			this.UpdateList();
		}
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x0008B801 File Offset: 0x00089A01
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordActivityInviteReceived(DiscordManager.DiscordUser _user, bool _cleared, ActivityActionTypes _type)
	{
		this.UpdateList();
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x0008B801 File Offset: 0x00089A01
	[PublicizedFrom(EAccessModifier.Private)]
	public void discordRelationshipChanged(DiscordManager.DiscordUser _user)
	{
		this.UpdateList();
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x0008B80C File Offset: 0x00089A0C
	public void UpdateList()
	{
		int page = base.Page;
		this.RebuildList(false);
		base.Page = page;
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x0008B830 File Offset: 0x00089A30
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

	// Token: 0x06001898 RID: 6296 RVA: 0x0008B91C File Offset: 0x00089B1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x0008B926 File Offset: 0x00089B26
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000FB4 RID: 4020
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<DiscordManager.DiscordUser> users = new HashSet<DiscordManager.DiscordUser>();

	// Token: 0x02000354 RID: 852
	[Preserve]
	public class PendingEntry : XUiListEntry<XUiC_DiscordPendingList.PendingEntry>
	{
		// Token: 0x0600189B RID: 6299 RVA: 0x0008B955 File Offset: 0x00089B55
		public PendingEntry(DiscordManager.DiscordUser _discordUser, XUiC_DiscordPendingList.PendingEntry.EEntryType _type)
		{
			this.User = _discordUser;
			this.EntryType = _type;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0008B96C File Offset: 0x00089B6C
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

		// Token: 0x0600189D RID: 6301 RVA: 0x0008B9BC File Offset: 0x00089BBC
		public override bool MatchesSearch(string _searchString)
		{
			if (string.IsNullOrEmpty(_searchString))
			{
				return true;
			}
			DiscordManager.DiscordUser user = this.User;
			return user != null && user.DisplayName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04000FB5 RID: 4021
		public readonly XUiC_DiscordPendingList.PendingEntry.EEntryType EntryType;

		// Token: 0x04000FB6 RID: 4022
		public readonly DiscordManager.DiscordUser User;

		// Token: 0x02000355 RID: 853
		public enum EEntryType
		{
			// Token: 0x04000FB8 RID: 4024
			JoinRequest,
			// Token: 0x04000FB9 RID: 4025
			Invite,
			// Token: 0x04000FBA RID: 4026
			FriendRequestGame,
			// Token: 0x04000FBB RID: 4027
			FriendRequestDiscord
		}
	}

	// Token: 0x02000356 RID: 854
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DiscordPendingListEntryController : XUiC_List<XUiC_DiscordPendingList.PendingEntry>.XUiC_ListEntry
	{
		// Token: 0x0600189E RID: 6302 RVA: 0x0008B9E0 File Offset: 0x00089BE0
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

		// Token: 0x0600189F RID: 6303 RVA: 0x0008BA88 File Offset: 0x00089C88
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

		// Token: 0x060018A0 RID: 6304 RVA: 0x0008BABD File Offset: 0x00089CBD
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

		// Token: 0x060018A1 RID: 6305 RVA: 0x0008BAE6 File Offset: 0x00089CE6
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

		// Token: 0x04000FBC RID: 4028
		[PublicizedFrom(EAccessModifier.Private)]
		public XUiV_Texture avatarTexture;
	}
}
