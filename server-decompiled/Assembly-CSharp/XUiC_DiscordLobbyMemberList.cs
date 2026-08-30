using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000349 RID: 841
[Preserve]
public class XUiC_DiscordLobbyMemberList : XUiC_List<XUiC_DiscordLobbyMemberList.LobbyMember>
{
	// Token: 0x0600184A RID: 6218 RVA: 0x0008A884 File Offset: 0x00088A84
	public override void Init()
	{
		base.Init();
		DiscordManager.Instance.LobbyMembersChanged += this.InstanceOnLobbyMembersChanged;
		DiscordManager.Instance.CallChanged += this.InstanceOnCallChanged;
		DiscordManager.Instance.CallMembersChanged += this.InstanceOnCallMembersChanged;
		DiscordManager.Instance.VoiceStateChanged += this.InstanceOnVoiceStateChanged;
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x0008A8F0 File Offset: 0x00088AF0
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.LobbyMembersChanged -= this.InstanceOnLobbyMembersChanged;
		DiscordManager.Instance.CallChanged -= this.InstanceOnCallChanged;
		DiscordManager.Instance.CallMembersChanged -= this.InstanceOnCallMembersChanged;
		DiscordManager.Instance.VoiceStateChanged -= this.InstanceOnVoiceStateChanged;
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x0008A95B File Offset: 0x00088B5B
	[PublicizedFrom(EAccessModifier.Private)]
	public void InstanceOnLobbyMembersChanged(DiscordManager.LobbyInfo _lobby)
	{
		this.RebuildList(false);
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x0008A95B File Offset: 0x00088B5B
	[PublicizedFrom(EAccessModifier.Private)]
	public void InstanceOnCallChanged(DiscordManager.CallInfo _newCall)
	{
		this.RebuildList(false);
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x0008A95B File Offset: 0x00088B5B
	[PublicizedFrom(EAccessModifier.Private)]
	public void InstanceOnCallMembersChanged(DiscordManager.CallInfo _call)
	{
		this.RebuildList(false);
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x0008A964 File Offset: 0x00088B64
	[PublicizedFrom(EAccessModifier.Private)]
	public void InstanceOnVoiceStateChanged(bool _self, ulong _userId)
	{
		base.RefreshBindingsSelfAndChildren();
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x0008A96C File Offset: 0x00088B6C
	public override void RebuildList(bool _resetFilter = false)
	{
		this.allEntries.Clear();
		this.membersTempCopy.Clear();
		DiscordManager.LobbyInfo activeVoiceLobby = DiscordManager.Instance.ActiveVoiceLobby;
		if (activeVoiceLobby != null)
		{
			activeVoiceLobby.VoiceCall.GetMembers(this.membersTempCopy);
		}
		foreach (ulong num in this.membersTempCopy)
		{
			this.allEntries.Add(new XUiC_DiscordLobbyMemberList.LobbyMember(DiscordManager.Instance.GetUser(num)));
		}
		this.allEntries.Sort();
		base.RebuildList(_resetFilter);
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x0008AA1C File Offset: 0x00088C1C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000FA4 RID: 4004
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ulong> membersTempCopy = new List<ulong>();

	// Token: 0x0200034A RID: 842
	[Preserve]
	public class LobbyMember : XUiListEntry<XUiC_DiscordLobbyMemberList.LobbyMember>
	{
		// Token: 0x06001853 RID: 6227 RVA: 0x0008AA4B File Offset: 0x00088C4B
		public LobbyMember(DiscordManager.DiscordUser _discordUser)
		{
			this.User = _discordUser;
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x0008AA5C File Offset: 0x00088C5C
		public override int CompareTo(XUiC_DiscordLobbyMemberList.LobbyMember _otherEntry)
		{
			if (_otherEntry == null)
			{
				return -1;
			}
			if (this.User.IsLocalAccount)
			{
				return -1;
			}
			if (_otherEntry.User.IsLocalAccount)
			{
				return 1;
			}
			return string.Compare(this.User.DisplayName, _otherEntry.User.DisplayName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x0008AAA8 File Offset: 0x00088CA8
		public override bool MatchesSearch(string _searchString)
		{
			return this.User.DisplayName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04000FA5 RID: 4005
		public readonly DiscordManager.DiscordUser User;
	}

	// Token: 0x0200034B RID: 843
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DiscordLobbyMemberList.LobbyMember>.XUiC_ListEntry
	{
		// Token: 0x06001856 RID: 6230 RVA: 0x0008AABC File Offset: 0x00088CBC
		[XuiXmlBinding("is_self")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsSelf()
		{
			XUiC_DiscordLobbyMemberList.LobbyMember entryData = this.entryData;
			bool? flag;
			if (entryData == null)
			{
				flag = null;
			}
			else
			{
				DiscordManager.DiscordUser user = entryData.User;
				flag = ((user != null) ? new bool?(user.IsLocalAccount) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0008AB04 File Offset: 0x00088D04
		[XuiXmlBinding("displayname")]
		[PublicizedFrom(EAccessModifier.Private)]
		public string bindingDisplayName()
		{
			XUiC_DiscordLobbyMemberList.LobbyMember entryData = this.entryData;
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

		// Token: 0x06001858 RID: 6232 RVA: 0x0008AB30 File Offset: 0x00088D30
		[XuiXmlBinding("is_speaking")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingIsSpeaking()
		{
			XUiC_DiscordLobbyMemberList.LobbyMember entryData = this.entryData;
			bool? flag;
			if (entryData == null)
			{
				flag = null;
			}
			else
			{
				DiscordManager.DiscordUser user = entryData.User;
				flag = ((user != null) ? new bool?(user.IsSpeaking) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x0008AB78 File Offset: 0x00088D78
		[XuiXmlBinding("voice_muted")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingVoiceMuted()
		{
			XUiC_DiscordLobbyMemberList.LobbyMember entryData = this.entryData;
			bool? flag;
			if (entryData == null)
			{
				flag = null;
			}
			else
			{
				DiscordManager.DiscordUser user = entryData.User;
				flag = ((user != null) ? new bool?(user.IsMuted) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x0008ABC0 File Offset: 0x00088DC0
		[XuiXmlBinding("output_muted")]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool bindingOutputMuted()
		{
			XUiC_DiscordLobbyMemberList.LobbyMember entryData = this.entryData;
			bool? flag;
			if (entryData == null)
			{
				flag = null;
			}
			else
			{
				DiscordManager.DiscordUser user = entryData.User;
				flag = ((user != null) ? new bool?(user.IsDeafened) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}
}
