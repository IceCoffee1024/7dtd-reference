using System;
using UnityEngine.Scripting;

// Token: 0x020002F6 RID: 758
[Preserve]
public class DialogRequirementAdmin : BaseDialogRequirement
{
	// Token: 0x17000266 RID: 614
	// (get) Token: 0x0600159D RID: 5533 RVA: 0x000601CD File Offset: 0x0005E3CD
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Admin;
		}
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x000813FC File Offset: 0x0007F5FC
	public override void SetupRequirement()
	{
		string text = Localization.Get("RequirementAdmin_keyword", false, null);
		base.Description = text;
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x0008141D File Offset: 0x0007F61D
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
	}

	// Token: 0x04000E8D RID: 3725
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
