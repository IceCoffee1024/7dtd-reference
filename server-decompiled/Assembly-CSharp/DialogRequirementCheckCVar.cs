using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x020002FB RID: 763
[Preserve]
public class DialogRequirementCheckCVar : BaseDialogRequirement
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x060015AC RID: 5548 RVA: 0x000816C6 File Offset: 0x0007F8C6
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.CVar;
		}
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x000816CA File Offset: 0x0007F8CA
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		int num = (int)player.GetCVar(base.ID);
		LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
		return num == StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
	}
}
