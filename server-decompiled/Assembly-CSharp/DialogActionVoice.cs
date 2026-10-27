using System;
using UnityEngine.Scripting;

// Token: 0x020002ED RID: 749
[Preserve]
public class DialogActionVoice : BaseDialogAction
{
	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001564 RID: 5476 RVA: 0x00080BF2 File Offset: 0x0007EDF2
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.Voice;
		}
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x00080BF5 File Offset: 0x0007EDF5
	public override void PerformAction(EntityPlayer player)
	{
		LocalPlayerUI.primaryUI.xui.Dialog.Respondent.PlayVoiceSetEntry(base.ID, player, true, true);
	}

	// Token: 0x04000E66 RID: 3686
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
