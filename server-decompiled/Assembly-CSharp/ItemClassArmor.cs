using System;
using UnityEngine.Scripting;

// Token: 0x020005F6 RID: 1526
[Preserve]
public class ItemClassArmor : ItemClass
{
	// Token: 0x06003209 RID: 12809 RVA: 0x001445AD File Offset: 0x001427AD
	public override bool KeepOnDeath()
	{
		return this.keepOnDeath;
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x001445B8 File Offset: 0x001427B8
	public override void Init()
	{
		base.Init();
		string text = "";
		this.Properties.ParseString("ArmorGroup", ref text);
		this.ArmorGroup = text.Split(',', StringSplitOptions.None);
		if (this.Properties.Values.ContainsKey("EquipSlot"))
		{
			this.EquipSlot = EnumUtils.Parse<EquipmentSlots>(this.Properties.Values["EquipSlot"], false);
		}
		this.Properties.ParseBool("IsCosmetic", ref this.IsCosmetic);
		this.Properties.ParseBool("KeepOnDeath", ref this.keepOnDeath);
		this.Properties.ParseBool("AllowUnEquip", ref this.AllowUnEquip);
		this.Properties.ParseBool("AutoEquip", ref this.AutoEquip);
		this.Properties.ParseString("ReplaceByTag", ref this.ReplaceByTag);
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x00144698 File Offset: 0x00142898
	public override bool CanEquip()
	{
		return (World.TemperatureSurvival || World.StormFrequency != 0f || this.EquipSlot.SlotType() != EquipmentSlotType.Clothing) && (World.BiomeProgressionEnabled || this.EquipSlot.SlotType() != EquipmentSlotType.BiomeGear);
	}

	// Token: 0x04002730 RID: 10032
	public const string PropEquipSlot = "EquipSlot";

	// Token: 0x04002731 RID: 10033
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropArmorGroup = "ArmorGroup";

	// Token: 0x04002732 RID: 10034
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropIsCosmetic = "IsCosmetic";

	// Token: 0x04002733 RID: 10035
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropKeepOnDeath = "KeepOnDeath";

	// Token: 0x04002734 RID: 10036
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropAllowUnEquip = "AllowUnEquip";

	// Token: 0x04002735 RID: 10037
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropAutoEquip = "AutoEquip";

	// Token: 0x04002736 RID: 10038
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropReplaceByTag = "ReplaceByTag";

	// Token: 0x04002737 RID: 10039
	public EquipmentSlots EquipSlot = EquipmentSlots.Count;

	// Token: 0x04002738 RID: 10040
	public string[] ArmorGroup;

	// Token: 0x04002739 RID: 10041
	public bool IsCosmetic = true;

	// Token: 0x0400273A RID: 10042
	public int CosmeticID = -1;

	// Token: 0x0400273B RID: 10043
	[PublicizedFrom(EAccessModifier.Private)]
	public bool keepOnDeath;

	// Token: 0x0400273C RID: 10044
	public bool AllowUnEquip = true;

	// Token: 0x0400273D RID: 10045
	public bool AutoEquip;

	// Token: 0x0400273E RID: 10046
	public string ReplaceByTag;
}
