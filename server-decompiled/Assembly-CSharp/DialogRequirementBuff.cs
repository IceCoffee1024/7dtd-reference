using System;
using UnityEngine.Scripting;

// Token: 0x020002F9 RID: 761
[Preserve]
public class DialogRequirementBuff : BaseDialogRequirement
{
	// Token: 0x17000267 RID: 615
	// (get) Token: 0x060015A5 RID: 5541 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Buff;
		}
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x00081660 File Offset: 0x0007F860
	public override void SetupRequirement()
	{
		string text = Localization.Get("RequirementBuff_keyword", false, null);
		base.Description = string.Format("{0} {1}", text, BuffManager.GetBuff(base.ID).Name);
	}

	// Token: 0x060015A7 RID: 5543 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return false;
	}

	// Token: 0x04000E9E RID: 3742
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
