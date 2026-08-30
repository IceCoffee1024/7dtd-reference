using System;
using UnityEngine.Scripting;

// Token: 0x020002F9 RID: 761
[Preserve]
public class DialogRequirementSkill : BaseDialogRequirement
{
	// Token: 0x17000276 RID: 630
	// (get) Token: 0x060015AF RID: 5551 RVA: 0x00081148 File Offset: 0x0007F348
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Skill;
		}
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x0008114C File Offset: 0x0007F34C
	public override string GetRequiredDescription(EntityPlayer player)
	{
		ProgressionValue progressionValue = player.Progression.GetProgressionValue(base.ID);
		return string.Format("({0} {1})", Localization.Get(progressionValue.ProgressionClass.NameKey, false, null), Convert.ToInt32(base.Value));
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x00081197 File Offset: 0x0007F397
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return player.Progression.GetProgressionValue(base.ID).Level > Convert.ToInt32(base.Value);
	}
}
