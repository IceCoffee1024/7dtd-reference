using System;
using UnityEngine.Scripting;

// Token: 0x020002F0 RID: 752
[Preserve]
public class DialogRequirementBuff : BaseDialogRequirement
{
	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06001591 RID: 5521 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Buff;
		}
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x00080AD4 File Offset: 0x0007ECD4
	public override void SetupRequirement()
	{
		string text = Localization.Get("RequirementBuff_keyword", false, null);
		base.Description = string.Format("{0} {1}", text, BuffManager.GetBuff(base.ID).Name);
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return false;
	}

	// Token: 0x04000E84 RID: 3716
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
