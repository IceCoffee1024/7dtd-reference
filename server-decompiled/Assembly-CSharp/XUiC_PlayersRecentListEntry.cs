using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E47 RID: 3655
[Preserve]
public class XUiC_PlayersRecentListEntry : XUiC_PlayersBlockedListEntryBase
{
	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x06006FB8 RID: 28600 RVA: 0x002B5B18 File Offset: 0x002B3D18
	// (set) Token: 0x06006FB9 RID: 28601 RVA: 0x002B5B29 File Offset: 0x002B3D29
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

	// Token: 0x17000CAD RID: 3245
	// (get) Token: 0x06006FBA RID: 28602 RVA: 0x002B5B39 File Offset: 0x002B3D39
	// (set) Token: 0x06006FBB RID: 28603 RVA: 0x002B5B41 File Offset: 0x002B3D41
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

	// Token: 0x06006FBC RID: 28604 RVA: 0x002B5B54 File Offset: 0x002B3D54
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

	// Token: 0x06006FBD RID: 28605 RVA: 0x002B5BA4 File Offset: 0x002B3DA4
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

	// Token: 0x06006FBE RID: 28606 RVA: 0x002B5C27 File Offset: 0x002B3E27
	[XuiBindEvent("OnPress", "btnViewProfile")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void bntViewProfilePressed(XUiController _sender, int _mouseButton)
	{
		if (this.PlayerName.CanShowProfile())
		{
			this.PlayerName.ShowProfile();
		}
	}

	// Token: 0x040054D5 RID: 21717
	[XuiBindComponent("lastSeen", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Label lblLastSeenTime;

	// Token: 0x040054D6 RID: 21718
	[XuiBindComponent("blockBtn", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBlockPlayer;

	// Token: 0x040054D7 RID: 21719
	[XuiBindComponent("btnViewProfile", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnViewProfile;

	// Token: 0x040054D8 RID: 21720
	[PublicizedFrom(EAccessModifier.Private)]
	public string lastSeenAge;

	// Token: 0x040054D9 RID: 21721
	[PublicizedFrom(EAccessModifier.Private)]
	public bool canShowProfile;
}
