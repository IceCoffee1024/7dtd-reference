using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E16 RID: 3606
[Preserve]
public class XUiC_PlayersBlockedListEntry : XUiC_PlayersBlockedListEntryBase
{
	// Token: 0x06006EF1 RID: 28401 RVA: 0x002ADBEC File Offset: 0x002ABDEC
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

	// Token: 0x040053D0 RID: 21456
	[XuiBindComponent("unblockBtn", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnUnblockPlayer;
}
