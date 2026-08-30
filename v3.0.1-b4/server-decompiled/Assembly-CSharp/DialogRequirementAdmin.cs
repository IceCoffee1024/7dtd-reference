using System;
using UnityEngine.Scripting;

// Token: 0x020002EF RID: 751
[Preserve]
public class DialogRequirementAdmin : BaseDialogRequirement
{
	// Token: 0x1700026D RID: 621
	// (get) Token: 0x0600158D RID: 5517 RVA: 0x0005FC95 File Offset: 0x0005DE95
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Admin;
		}
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x00080A94 File Offset: 0x0007EC94
	public override void SetupRequirement()
	{
		string text = Localization.Get("RequirementAdmin_keyword", false, null);
		base.Description = text;
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x00080AB5 File Offset: 0x0007ECB5
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
	}

	// Token: 0x04000E83 RID: 3715
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
