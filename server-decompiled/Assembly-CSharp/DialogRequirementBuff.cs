using System;
using UnityEngine.Scripting;

// Token: 0x020002F7 RID: 759
[Preserve]
public class DialogRequirementBuff : BaseDialogRequirement
{
	// Token: 0x17000267 RID: 615
	// (get) Token: 0x060015A1 RID: 5537 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Buff;
		}
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x0008143C File Offset: 0x0007F63C
	public override void SetupRequirement()
	{
		string text = Localization.Get("RequirementBuff_keyword", false, null);
		base.Description = string.Format("{0} {1}", text, BuffManager.GetBuff(base.ID).Name);
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return false;
	}

	// Token: 0x04000E8E RID: 3726
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
