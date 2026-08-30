using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020002F1 RID: 753
[Preserve]
public class DialogRequirementCanTrade : BaseDialogRequirement
{
	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06001595 RID: 5525 RVA: 0x00080B22 File Offset: 0x0007ED22
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.CanTrade;
		}
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x00080B26 File Offset: 0x0007ED26
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return SandboxOptionManager.GetBool(global::SandboxOptions.SandboxOptions.TradersEnabled);
	}
}
