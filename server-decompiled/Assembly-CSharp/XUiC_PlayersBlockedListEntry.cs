using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E3A RID: 3642
[Preserve]
public class XUiC_PlayersBlockedListEntry : XUiC_PlayersBlockedListEntryBase
{
	// Token: 0x06006F89 RID: 28553 RVA: 0x002B44B0 File Offset: 0x002B26B0
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

	// Token: 0x040054AB RID: 21675
	[XuiBindComponent("unblockBtn", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnUnblockPlayer;
}
