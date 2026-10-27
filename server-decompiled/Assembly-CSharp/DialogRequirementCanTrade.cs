using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020002FA RID: 762
[Preserve]
public class DialogRequirementCanTrade : BaseDialogRequirement
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x060015A9 RID: 5545 RVA: 0x000816AE File Offset: 0x0007F8AE
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.CanTrade;
		}
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x000816B2 File Offset: 0x0007F8B2
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return SandboxOptionManager.GetBool(global::SandboxOptions.SandboxOptions.TradersEnabled);
	}
}
