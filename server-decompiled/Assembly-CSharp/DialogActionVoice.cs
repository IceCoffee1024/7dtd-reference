using System;
using UnityEngine.Scripting;

// Token: 0x020002EB RID: 747
[Preserve]
public class DialogActionVoice : BaseDialogAction
{
	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001560 RID: 5472 RVA: 0x000809CD File Offset: 0x0007EBCD
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.Voice;
		}
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x000809D0 File Offset: 0x0007EBD0
	public override void PerformAction(EntityPlayer player)
	{
		LocalPlayerUI.primaryUI.xui.Dialog.Respondent.PlayVoiceSetEntry(base.ID, player, true, true);
	}

	// Token: 0x04000E56 RID: 3670
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
