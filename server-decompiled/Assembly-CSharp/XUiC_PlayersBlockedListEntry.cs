using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E2B RID: 3627
[Preserve]
public class XUiC_PlayersBlockedListEntry : XUiC_PlayersBlockedListEntryBase
{
	// Token: 0x06006F91 RID: 28561 RVA: 0x002B1100 File Offset: 0x002AF300
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

	// Token: 0x04005474 RID: 21620
	[XuiBindComponent("unblockBtn", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnUnblockPlayer;
}
