using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E18 RID: 3608
[Preserve]
public class XUiC_PlayersRecentListEntry : XUiC_PlayersBlockedListEntryBase
{
	// Token: 0x17000C72 RID: 3186
	// (get) Token: 0x06006EFB RID: 28411 RVA: 0x002ADCE8 File Offset: 0x002ABEE8
	// (set) Token: 0x06006EFC RID: 28412 RVA: 0x002ADCF9 File Offset: 0x002ABEF9
	[XuiXmlBinding("lastseenage")]
	public string LastSeenAge
	{
		get
		{
			return this.lastSeenAge ?? "";
		}
		set
		{
			this.lastSeenAge = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x06006EFD RID: 28413 RVA: 0x002ADD09 File Offset: 0x002ABF09
	// (set) Token: 0x06006EFE RID: 28414 RVA: 0x002ADD11 File Offset: 0x002ABF11
	[XuiXmlBinding("canshowprofile")]
	public bool CanShowProfile
	{
		get
		{
			return this.canShowProfile;
		}
		set
		{
			this.canShowProfile = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006EFF RID: 28415 RVA: 0x002ADD24 File Offset: 0x002ABF24
	public override void UpdateEntry(PlatformUserIdentifierAbs _playerId)
	{
		base.UpdateEntry(_playerId);
		if (base.HasEntry)
		{
			BlockedPlayerList.ListEntry playerStateInfo = BlockedPlayerList.Instance.GetPlayerStateInfo(_playerId);
			this.LastSeenAge = Utils.DescribeTimeSince(DateTime.UtcNow, playerStateInfo.LastSeen);
			this.CanShowProfile = this.PlayerName.CanShowProfile();
		}
	}

	// Token: 0x06006F00 RID: 28416 RVA: 0x002ADD74 File Offset: 0x002ABF74
	[XuiBindEvent("OnPress", "btnBlockPlayer")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void blockPlayerPressed(XUiController _sender, int _mouseButton)
	{
		if (base.PlayerId == null || this.PlayerName.CanShowProfile())
		{
			return;
		}
		BlockedPlayerList.ListEntry playerStateInfo = BlockedPlayerList.Instance.GetPlayerStateInfo(base.PlayerId);
		if (playerStateInfo != null && playerStateInfo.ResolvedOnce)
		{
			ValueTuple<bool, string> valueTuple = playerStateInfo.SetBlockState(true);
			bool item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (item)
			{
				this.BlockList.IsDirty = true;
				return;
			}
			if (!string.IsNullOrEmpty(item2))
			{
				this.BlockList.DisplayMessage(Localization.Get("xuiBlockedPlayersCantAddHeader", false, null), item2);
			}
		}
	}

	// Token: 0x06006F01 RID: 28417 RVA: 0x002ADDF7 File Offset: 0x002ABFF7
	[XuiBindEvent("OnPress", "btnViewProfile")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void bntViewProfilePressed(XUiController _sender, int _mouseButton)
	{
		if (this.PlayerName.CanShowProfile())
		{
			this.PlayerName.ShowProfile();
		}
	}

	// Token: 0x040053D5 RID: 21461
	[XuiBindComponent("lastSeen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblLastSeenTime;

	// Token: 0x040053D6 RID: 21462
	[XuiBindComponent("blockBtn", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBlockPlayer;

	// Token: 0x040053D7 RID: 21463
	[XuiBindComponent("btnViewProfile", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnViewProfile;

	// Token: 0x040053D8 RID: 21464
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastSeenAge;

	// Token: 0x040053D9 RID: 21465
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canShowProfile;
}
