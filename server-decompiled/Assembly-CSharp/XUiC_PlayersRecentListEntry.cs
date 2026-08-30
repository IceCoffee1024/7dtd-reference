using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E2D RID: 3629
[Preserve]
public class XUiC_PlayersRecentListEntry : XUiC_PlayersBlockedListEntryBase
{
	// Token: 0x17000C8B RID: 3211
	// (get) Token: 0x06006F9B RID: 28571 RVA: 0x002B11FC File Offset: 0x002AF3FC
	// (set) Token: 0x06006F9C RID: 28572 RVA: 0x002B120D File Offset: 0x002AF40D
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

	// Token: 0x17000C8C RID: 3212
	// (get) Token: 0x06006F9D RID: 28573 RVA: 0x002B121D File Offset: 0x002AF41D
	// (set) Token: 0x06006F9E RID: 28574 RVA: 0x002B1225 File Offset: 0x002AF425
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

	// Token: 0x06006F9F RID: 28575 RVA: 0x002B1238 File Offset: 0x002AF438
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

	// Token: 0x06006FA0 RID: 28576 RVA: 0x002B1288 File Offset: 0x002AF488
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

	// Token: 0x06006FA1 RID: 28577 RVA: 0x002B130B File Offset: 0x002AF50B
	[XuiBindEvent("OnPress", "btnViewProfile")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void bntViewProfilePressed(XUiController _sender, int _mouseButton)
	{
		if (this.PlayerName.CanShowProfile())
		{
			this.PlayerName.ShowProfile();
		}
	}

	// Token: 0x04005479 RID: 21625
	[XuiBindComponent("lastSeen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblLastSeenTime;

	// Token: 0x0400547A RID: 21626
	[XuiBindComponent("blockBtn", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBlockPlayer;

	// Token: 0x0400547B RID: 21627
	[XuiBindComponent("btnViewProfile", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnViewProfile;

	// Token: 0x0400547C RID: 21628
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastSeenAge;

	// Token: 0x0400547D RID: 21629
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canShowProfile;
}
