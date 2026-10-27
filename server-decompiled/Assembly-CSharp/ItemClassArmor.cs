using System;
using UnityEngine.Scripting;

// Token: 0x020005FD RID: 1533
[Preserve]
public class ItemClassArmor : ItemClass
{
	// Token: 0x06003217 RID: 12823 RVA: 0x001451A1 File Offset: 0x001433A1
	public override bool KeepOnDeath()
	{
		return this.keepOnDeath;
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x001451AC File Offset: 0x001433AC
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

	// Token: 0x06003219 RID: 12825 RVA: 0x0014528C File Offset: 0x0014348C
	public override bool CanEquip()
	{
		return (World.TemperatureSurvival || World.StormFrequency != 0f || this.EquipSlot.SlotType() != EquipmentSlotType.Clothing) && (World.BiomeProgressionEnabled || this.EquipSlot.SlotType() != EquipmentSlotType.BiomeGear);
	}

	// Token: 0x0400274B RID: 10059
	public const string PropEquipSlot = "EquipSlot";

	// Token: 0x0400274C RID: 10060
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropArmorGroup = "ArmorGroup";

	// Token: 0x0400274D RID: 10061
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropIsCosmetic = "IsCosmetic";

	// Token: 0x0400274E RID: 10062
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropKeepOnDeath = "KeepOnDeath";

	// Token: 0x0400274F RID: 10063
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropAllowUnEquip = "AllowUnEquip";

	// Token: 0x04002750 RID: 10064
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropAutoEquip = "AutoEquip";

	// Token: 0x04002751 RID: 10065
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropReplaceByTag = "ReplaceByTag";

	// Token: 0x04002752 RID: 10066
	public EquipmentSlots EquipSlot = EquipmentSlots.Count;

	// Token: 0x04002753 RID: 10067
	public string[] ArmorGroup;

	// Token: 0x04002754 RID: 10068
	public bool IsCosmetic = true;

	// Token: 0x04002755 RID: 10069
	public int CosmeticID = -1;

	// Token: 0x04002756 RID: 10070
	[PublicizedFrom(EAccessModifier.Private)]
	public bool keepOnDeath;

	// Token: 0x04002757 RID: 10071
	public bool AllowUnEquip = true;

	// Token: 0x04002758 RID: 10072
	public bool AutoEquip;

	// Token: 0x04002759 RID: 10073
	public string ReplaceByTag;
}
