using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x020002F9 RID: 761
[Preserve]
public class DialogRequirementCheckCVar : BaseDialogRequirement
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x060015A8 RID: 5544 RVA: 0x000814A2 File Offset: 0x0007F6A2
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.CVar;
		}
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x000814A6 File Offset: 0x0007F6A6
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		int num = (int)player.GetCVar(base.ID);
		LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
		return num == StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
	}
}
