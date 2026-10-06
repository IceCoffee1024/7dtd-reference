using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020002F8 RID: 760
[Preserve]
public class DialogRequirementCanTrade : BaseDialogRequirement
{
	// Token: 0x17000268 RID: 616
	// (get) Token: 0x060015A5 RID: 5541 RVA: 0x0008148A File Offset: 0x0007F68A
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.CanTrade;
		}
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x0008148E File Offset: 0x0007F68E
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return SandboxOptionManager.GetBool(global::SandboxOptions.SandboxOptions.TradersEnabled);
	}
}
