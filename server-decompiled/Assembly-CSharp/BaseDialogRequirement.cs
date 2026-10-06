using System;
using System.Collections.Generic;

// Token: 0x020002F3 RID: 755
public abstract class BaseDialogRequirement
{
	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06001588 RID: 5512 RVA: 0x00081384 File Offset: 0x0007F584
	// (set) Token: 0x06001589 RID: 5513 RVA: 0x0008138C File Offset: 0x0007F58C
	public string ID { get; set; }

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x0600158A RID: 5514 RVA: 0x00081395 File Offset: 0x0007F595
	// (set) Token: 0x0600158B RID: 5515 RVA: 0x0008139D File Offset: 0x0007F59D
	public string Value { get; set; }

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x0600158C RID: 5516 RVA: 0x000813A6 File Offset: 0x0007F5A6
	// (set) Token: 0x0600158D RID: 5517 RVA: 0x000813AE File Offset: 0x0007F5AE
	public string Tag { get; set; }

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x0600158E RID: 5518 RVA: 0x000813B7 File Offset: 0x0007F5B7
	// (set) Token: 0x0600158F RID: 5519 RVA: 0x000813BF File Offset: 0x0007F5BF
	public Dialog Owner { get; set; }

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06001590 RID: 5520 RVA: 0x000813C8 File Offset: 0x0007F5C8
	// (set) Token: 0x06001591 RID: 5521 RVA: 0x000813D0 File Offset: 0x0007F5D0
	public BaseDialogRequirement.RequirementVisibilityTypes RequirementVisibilityType { get; set; }

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06001592 RID: 5522 RVA: 0x000813D9 File Offset: 0x0007F5D9
	// (set) Token: 0x06001593 RID: 5523 RVA: 0x000813E1 File Offset: 0x0007F5E1
	public string Description
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06001594 RID: 5524 RVA: 0x000813EA File Offset: 0x0007F5EA
	// (set) Token: 0x06001595 RID: 5525 RVA: 0x000813F2 File Offset: 0x0007F5F2
	public string StatusText
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual List<string> GetRequirementIDTypes()
	{
		return null;
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06001597 RID: 5527 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Buff;
		}
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x0003223F File Offset: 0x0003043F
	public virtual string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupRequirement()
	{
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return false;
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseDialogRequirement Clone()
	{
		return null;
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseDialogRequirement()
	{
	}

	// Token: 0x020002F4 RID: 756
	public enum RequirementTypes
	{
		// Token: 0x04000E7E RID: 3710
		Buff,
		// Token: 0x04000E7F RID: 3711
		QuestStatus,
		// Token: 0x04000E80 RID: 3712
		QuestsAvailable,
		// Token: 0x04000E81 RID: 3713
		QuestTier,
		// Token: 0x04000E82 RID: 3714
		QuestTierHighest,
		// Token: 0x04000E83 RID: 3715
		QuestEditorTag,
		// Token: 0x04000E84 RID: 3716
		Skill,
		// Token: 0x04000E85 RID: 3717
		Admin,
		// Token: 0x04000E86 RID: 3718
		DroneState,
		// Token: 0x04000E87 RID: 3719
		DroneStateExclude,
		// Token: 0x04000E88 RID: 3720
		CVar,
		// Token: 0x04000E89 RID: 3721
		CanTrade
	}

	// Token: 0x020002F5 RID: 757
	public enum RequirementVisibilityTypes
	{
		// Token: 0x04000E8B RID: 3723
		AlternateText,
		// Token: 0x04000E8C RID: 3724
		Hide
	}
}
