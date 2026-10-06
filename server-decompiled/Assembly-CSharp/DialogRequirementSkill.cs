using System;
using UnityEngine.Scripting;

// Token: 0x02000300 RID: 768
[Preserve]
public class DialogRequirementSkill : BaseDialogRequirement
{
	// Token: 0x1700026F RID: 623
	// (get) Token: 0x060015BF RID: 5567 RVA: 0x00081AB0 File Offset: 0x0007FCB0
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Skill;
		}
	}

	// Token: 0x060015C0 RID: 5568 RVA: 0x00081AB4 File Offset: 0x0007FCB4
	public override string GetRequiredDescription(EntityPlayer player)
	{
		ProgressionValue progressionValue = player.Progression.GetProgressionValue(base.ID);
		return string.Format("({0} {1})", Localization.Get(progressionValue.ProgressionClass.NameKey, false, null), Convert.ToInt32(base.Value));
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x00081AFF File Offset: 0x0007FCFF
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return player.Progression.GetProgressionValue(base.ID).Level > Convert.ToInt32(base.Value);
	}
}
