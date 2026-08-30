using System;
using UnityEngine.Scripting;

// Token: 0x020002E4 RID: 740
[Preserve]
public class DialogActionVoice : BaseDialogAction
{
	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06001550 RID: 5456 RVA: 0x00080065 File Offset: 0x0007E265
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.Voice;
		}
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x00080068 File Offset: 0x0007E268
	public override void PerformAction(EntityPlayer player)
	{
		LocalPlayerUI.primaryUI.xui.Dialog.Respondent.PlayVoiceSetEntry(base.ID, player, true, true);
	}

	// Token: 0x04000E4C RID: 3660
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
