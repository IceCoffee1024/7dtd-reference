using System;
using System.Collections.Generic;

// Token: 0x020002EC RID: 748
public abstract class BaseDialogRequirement
{
	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06001578 RID: 5496 RVA: 0x00080A1C File Offset: 0x0007EC1C
	// (set) Token: 0x06001579 RID: 5497 RVA: 0x00080A24 File Offset: 0x0007EC24
	public string ID { get; set; }

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x0600157A RID: 5498 RVA: 0x00080A2D File Offset: 0x0007EC2D
	// (set) Token: 0x0600157B RID: 5499 RVA: 0x00080A35 File Offset: 0x0007EC35
	public string Value { get; set; }

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x0600157C RID: 5500 RVA: 0x00080A3E File Offset: 0x0007EC3E
	// (set) Token: 0x0600157D RID: 5501 RVA: 0x00080A46 File Offset: 0x0007EC46
	public string Tag { get; set; }

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x0600157E RID: 5502 RVA: 0x00080A4F File Offset: 0x0007EC4F
	// (set) Token: 0x0600157F RID: 5503 RVA: 0x00080A57 File Offset: 0x0007EC57
	public Dialog Owner { get; set; }

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06001580 RID: 5504 RVA: 0x00080A60 File Offset: 0x0007EC60
	// (set) Token: 0x06001581 RID: 5505 RVA: 0x00080A68 File Offset: 0x0007EC68
	public BaseDialogRequirement.RequirementVisibilityTypes RequirementVisibilityType { get; set; }

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06001582 RID: 5506 RVA: 0x00080A71 File Offset: 0x0007EC71
	// (set) Token: 0x06001583 RID: 5507 RVA: 0x00080A79 File Offset: 0x0007EC79
	public string Description
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06001584 RID: 5508 RVA: 0x00080A82 File Offset: 0x0007EC82
	// (set) Token: 0x06001585 RID: 5509 RVA: 0x00080A8A File Offset: 0x0007EC8A
	public string StatusText
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual List<string> GetRequirementIDTypes()
	{
		return null;
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06001587 RID: 5511 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Buff;
		}
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x00031E8B File Offset: 0x0003008B
	public virtual string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupRequirement()
	{
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return false;
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual BaseDialogRequirement Clone()
	{
		return null;
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseDialogRequirement()
	{
	}

	// Token: 0x020002ED RID: 749
	public enum RequirementTypes
	{
		// Token: 0x04000E74 RID: 3700
		Buff,
		// Token: 0x04000E75 RID: 3701
		QuestStatus,
		// Token: 0x04000E76 RID: 3702
		QuestsAvailable,
		// Token: 0x04000E77 RID: 3703
		QuestTier,
		// Token: 0x04000E78 RID: 3704
		QuestTierHighest,
		// Token: 0x04000E79 RID: 3705
		QuestEditorTag,
		// Token: 0x04000E7A RID: 3706
		Skill,
		// Token: 0x04000E7B RID: 3707
		Admin,
		// Token: 0x04000E7C RID: 3708
		DroneState,
		// Token: 0x04000E7D RID: 3709
		DroneStateExclude,
		// Token: 0x04000E7E RID: 3710
		CVar,
		// Token: 0x04000E7F RID: 3711
		CanTrade
	}

	// Token: 0x020002EE RID: 750
	public enum RequirementVisibilityTypes
	{
		// Token: 0x04000E81 RID: 3713
		AlternateText,
		// Token: 0x04000E82 RID: 3714
		Hide
	}
}
