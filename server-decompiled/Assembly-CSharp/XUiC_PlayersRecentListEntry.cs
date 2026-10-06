using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E3C RID: 3644
[Preserve]
public class XUiC_PlayersRecentListEntry : XUiC_PlayersBlockedListEntryBase
{
	// Token: 0x17000CA6 RID: 3238
	// (get) Token: 0x06006F93 RID: 28563 RVA: 0x002B45AC File Offset: 0x002B27AC
	// (set) Token: 0x06006F94 RID: 28564 RVA: 0x002B45BD File Offset: 0x002B27BD
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

	// Token: 0x17000CA7 RID: 3239
	// (get) Token: 0x06006F95 RID: 28565 RVA: 0x002B45CD File Offset: 0x002B27CD
	// (set) Token: 0x06006F96 RID: 28566 RVA: 0x002B45D5 File Offset: 0x002B27D5
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

	// Token: 0x06006F97 RID: 28567 RVA: 0x002B45E8 File Offset: 0x002B27E8
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

	// Token: 0x06006F98 RID: 28568 RVA: 0x002B4638 File Offset: 0x002B2838
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

	// Token: 0x06006F99 RID: 28569 RVA: 0x002B46BB File Offset: 0x002B28BB
	[XuiBindEvent("OnPress", "btnViewProfile")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void bntViewProfilePressed(XUiController _sender, int _mouseButton)
	{
		if (this.PlayerName.CanShowProfile())
		{
			this.PlayerName.ShowProfile();
		}
	}

	// Token: 0x040054B0 RID: 21680
	[XuiBindComponent("lastSeen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblLastSeenTime;

	// Token: 0x040054B1 RID: 21681
	[XuiBindComponent("blockBtn", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBlockPlayer;

	// Token: 0x040054B2 RID: 21682
	[XuiBindComponent("btnViewProfile", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnViewProfile;

	// Token: 0x040054B3 RID: 21683
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastSeenAge;

	// Token: 0x040054B4 RID: 21684
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canShowProfile;
}
