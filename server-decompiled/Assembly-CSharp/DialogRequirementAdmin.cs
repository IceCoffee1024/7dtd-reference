using System;
using UnityEngine.Scripting;

// Token: 0x020002F8 RID: 760
[Preserve]
public class DialogRequirementAdmin : BaseDialogRequirement
{
	// Token: 0x17000266 RID: 614
	// (get) Token: 0x060015A1 RID: 5537 RVA: 0x00060491 File Offset: 0x0005E691
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Admin;
		}
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x00081620 File Offset: 0x0007F820
	public override void SetupRequirement()
	{
		string text = Localization.Get("RequirementAdmin_keyword", false, null);
		base.Description = text;
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x00081641 File Offset: 0x0007F841
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
	}

	// Token: 0x04000E9D RID: 3741
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
