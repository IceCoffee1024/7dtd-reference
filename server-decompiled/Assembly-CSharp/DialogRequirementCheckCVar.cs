using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x020002F2 RID: 754
[Preserve]
public class DialogRequirementCheckCVar : BaseDialogRequirement
{
	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06001598 RID: 5528 RVA: 0x00080B3A File Offset: 0x0007ED3A
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.CVar;
		}
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x00080B3E File Offset: 0x0007ED3E
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		int num = (int)player.GetCVar(base.ID);
		LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
		return num == StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
	}
}
