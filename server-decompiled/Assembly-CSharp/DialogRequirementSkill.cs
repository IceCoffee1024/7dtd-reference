using System;
using UnityEngine.Scripting;

// Token: 0x02000302 RID: 770
[Preserve]
public class DialogRequirementSkill : BaseDialogRequirement
{
	// Token: 0x1700026F RID: 623
	// (get) Token: 0x060015C3 RID: 5571 RVA: 0x00081CD4 File Offset: 0x0007FED4
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Skill;
		}
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x00081CD8 File Offset: 0x0007FED8
	public override string GetRequiredDescription(EntityPlayer player)
	{
		ProgressionValue progressionValue = player.Progression.GetProgressionValue(base.ID);
		return string.Format("({0} {1})", Localization.Get(progressionValue.ProgressionClass.NameKey, false, null), Convert.ToInt32(base.Value));
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x00081D23 File Offset: 0x0007FF23
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return player.Progression.GetProgressionValue(base.ID).Level > Convert.ToInt32(base.Value);
	}
}
