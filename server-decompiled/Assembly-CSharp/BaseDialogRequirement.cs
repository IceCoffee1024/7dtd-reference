using System;
using System.Collections.Generic;

// Token: 0x020002F5 RID: 757
public abstract class BaseDialogRequirement
{
	// Token: 0x1700025E RID: 606
	// (get) Token: 0x0600158C RID: 5516 RVA: 0x000815A8 File Offset: 0x0007F7A8
	// (set) Token: 0x0600158D RID: 5517 RVA: 0x000815B0 File Offset: 0x0007F7B0
	public string ID { get; set; }

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x0600158E RID: 5518 RVA: 0x000815B9 File Offset: 0x0007F7B9
	// (set) Token: 0x0600158F RID: 5519 RVA: 0x000815C1 File Offset: 0x0007F7C1
	public string Value { get; set; }

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06001590 RID: 5520 RVA: 0x000815CA File Offset: 0x0007F7CA
	// (set) Token: 0x06001591 RID: 5521 RVA: 0x000815D2 File Offset: 0x0007F7D2
	public string Tag { get; set; }

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06001592 RID: 5522 RVA: 0x000815DB File Offset: 0x0007F7DB
	// (set) Token: 0x06001593 RID: 5523 RVA: 0x000815E3 File Offset: 0x0007F7E3
	public Dialog Owner { get; set; }

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06001594 RID: 5524 RVA: 0x000815EC File Offset: 0x0007F7EC
	// (set) Token: 0x06001595 RID: 5525 RVA: 0x000815F4 File Offset: 0x0007F7F4
	public BaseDialogRequirement.RequirementVisibilityTypes RequirementVisibilityType { get; set; }

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06001596 RID: 5526 RVA: 0x000815FD File Offset: 0x0007F7FD
	// (set) Token: 0x06001597 RID: 5527 RVA: 0x00081605 File Offset: 0x0007F805
	public string Description
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06001598 RID: 5528 RVA: 0x0008160E File Offset: 0x0007F80E
	// (set) Token: 0x06001599 RID: 5529 RVA: 0x00081616 File Offset: 0x0007F816
	public string StatusText
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual List<string> GetRequirementIDTypes()
	{
		return null;
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x0600159B RID: 5531 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.Buff;
		}
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x000321FF File Offset: 0x000303FF
	public virtual string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupRequirement()
	{
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		return false;
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseDialogRequirement Clone()
	{
		return null;
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public BaseDialogRequirement()
	{
	}

	// Token: 0x020002F6 RID: 758
	public enum RequirementTypes
	{
		// Token: 0x04000E8E RID: 3726
		Buff,
		// Token: 0x04000E8F RID: 3727
		QuestStatus,
		// Token: 0x04000E90 RID: 3728
		QuestsAvailable,
		// Token: 0x04000E91 RID: 3729
		QuestTier,
		// Token: 0x04000E92 RID: 3730
		QuestTierHighest,
		// Token: 0x04000E93 RID: 3731
		QuestEditorTag,
		// Token: 0x04000E94 RID: 3732
		Skill,
		// Token: 0x04000E95 RID: 3733
		Admin,
		// Token: 0x04000E96 RID: 3734
		DroneState,
		// Token: 0x04000E97 RID: 3735
		DroneStateExclude,
		// Token: 0x04000E98 RID: 3736
		CVar,
		// Token: 0x04000E99 RID: 3737
		CanTrade
	}

	// Token: 0x020002F7 RID: 759
	public enum RequirementVisibilityTypes
	{
		// Token: 0x04000E9B RID: 3739
		AlternateText,
		// Token: 0x04000E9C RID: 3740
		Hide
	}
}
