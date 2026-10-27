using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E45 RID: 3653
[Preserve]
public class XUiC_PlayersBlockedListEntry : XUiC_PlayersBlockedListEntryBase
{
	// Token: 0x06006FAE RID: 28590 RVA: 0x002B5A1C File Offset: 0x002B3C1C
	[XuiBindEvent("OnPress", "btnUnblockPlayer")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void unblockPlayerPressed(XUiController _sender, int _mouseButton)
	{
		if (base.PlayerId == null)
		{
			return;
		}
		BlockedPlayerList.ListEntry playerStateInfo = BlockedPlayerList.Instance.GetPlayerStateInfo(base.PlayerId);
		if (playerStateInfo == null || !playerStateInfo.ResolvedOnce)
		{
			return;
		}
		playerStateInfo.SetBlockState(false);
		this.BlockList.IsDirty = true;
	}

	// Token: 0x040054D0 RID: 21712
	[XuiBindComponent("unblockBtn", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnUnblockPlayer;
}
