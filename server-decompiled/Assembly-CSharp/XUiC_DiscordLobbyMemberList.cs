using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200034C RID: 844
[Preserve]
public class XUiC_DiscordLobbyMemberList : XUiC_List<XUiC_DiscordLobbyMemberList.LobbyMember>
{
	// Token: 0x06001845 RID: 6213 RVA: 0x0008AB04 File Offset: 0x00088D04
	public override void Init()
	{
		base.Init();
		DiscordManager.Instance.LobbyMembersChanged += this.InstanceOnLobbyMembersChanged;
		DiscordManager.Instance.CallChanged += this.InstanceOnCallChanged;
		DiscordManager.Instance.CallMembersChanged += this.InstanceOnCallMembersChanged;
		DiscordManager.Instance.VoiceStateChanged += this.InstanceOnVoiceStateChanged;
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x0008AB70 File Offset: 0x00088D70
	public override void Cleanup()
	{
		base.Cleanup();
		DiscordManager.Instance.LobbyMembersChanged -= this.InstanceOnLobbyMembersChanged;
		DiscordManager.Instance.CallChanged -= this.InstanceOnCallChanged;
		DiscordManager.Instance.CallMembersChanged -= this.InstanceOnCallMembersChanged;
		DiscordManager.Instance.VoiceStateChanged -= this.InstanceOnVoiceStateChanged;
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x0008ABDB File Offset: 0x00088DDB
	[PublicizedFrom(EAccessModifier.Private)]
	public void InstanceOnLobbyMembersChanged(DiscordManager.LobbyInfo _lobby)
	{
		this.RebuildList(false);
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x0008ABDB File Offset: 0x00088DDB
	[PublicizedFrom(EAccessModifier.Private)]
	public void InstanceOnCallChanged(DiscordManager.CallInfo _newCall)
	{
		this.RebuildList(false);
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x0008ABDB File Offset: 0x00088DDB
	[PublicizedFrom(EAccessModifier.Private)]
	public void InstanceOnCallMembersChanged(DiscordManager.CallInfo _call)
	{
		this.RebuildList(false);
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x0008ABE4 File Offset: 0x00088DE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void InstanceOnVoiceStateChanged(bool _self, ulong _userId)
	{
		base.RefreshBindingsSelfAndChildren();
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x0008ABEC File Offset: 0x00088DEC
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

	// Token: 0x0600184C RID: 6220 RVA: 0x0008AC9C File Offset: 0x00088E9C
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.allEntries.Count == 0)
		{
			this.RebuildList(false);
		}
	}

	// Token: 0x04000F9D RID: 3997
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ulong> membersTempCopy = new List<ulong>();

	// Token: 0x0200034D RID: 845
	[Preserve]
	public class LobbyMember : XUiListEntry<XUiC_DiscordLobbyMemberList.LobbyMember>
	{
		// Token: 0x0600184E RID: 6222 RVA: 0x0008ACCB File Offset: 0x00088ECB
		public LobbyMember(DiscordManager.DiscordUser _discordUser)
		{
			this.User = _discordUser;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0008ACDC File Offset: 0x00088EDC
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

		// Token: 0x06001850 RID: 6224 RVA: 0x0008AD28 File Offset: 0x00088F28
		public override bool MatchesSearch(string _searchString)
		{
			return this.User.DisplayName.ContainsCaseInsensitive(_searchString);
		}

		// Token: 0x04000F9E RID: 3998
		public readonly DiscordManager.DiscordUser User;
	}

	// Token: 0x0200034E RID: 846
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public class EntryController : XUiC_List<XUiC_DiscordLobbyMemberList.LobbyMember>.XUiC_ListEntry
	{
		// Token: 0x06001851 RID: 6225 RVA: 0x0008AD3C File Offset: 0x00088F3C
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

		// Token: 0x06001852 RID: 6226 RVA: 0x0008AD84 File Offset: 0x00088F84
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

		// Token: 0x06001853 RID: 6227 RVA: 0x0008ADB0 File Offset: 0x00088FB0
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

		// Token: 0x06001854 RID: 6228 RVA: 0x0008ADF8 File Offset: 0x00088FF8
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

		// Token: 0x06001855 RID: 6229 RVA: 0x0008AE40 File Offset: 0x00089040
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
