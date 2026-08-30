using System;
using UnityEngine.Scripting;

// Token: 0x020005F5 RID: 1525
[Preserve]
public class ItemClassArmor : ItemClass
{
	// Token: 0x060031C1 RID: 12737 RVA: 0x00143B59 File Offset: 0x00141D59
	public override bool KeepOnDeath()
	{
		return this.keepOnDeath;
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x00143B64 File Offset: 0x00141D64
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

	// Token: 0x060031C3 RID: 12739 RVA: 0x00143C44 File Offset: 0x00141E44
	public override bool CanEquip()
	{
		return (World.TemperatureSurvival || World.StormFrequency != 0f || this.EquipSlot < EquipmentSlots.ClothingHead || this.EquipSlot > EquipmentSlots.ClothingFeet) && (World.BiomeProgressionEnabled || this.EquipSlot < EquipmentSlots.BiomeBadge || this.EquipSlot > EquipmentSlots.BiomeBadge4);
	}

	// Token: 0x04002737 RID: 10039
	public const string PropEquipSlot = "EquipSlot";

	// Token: 0x04002738 RID: 10040
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropArmorGroup = "ArmorGroup";

	// Token: 0x04002739 RID: 10041
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropIsCosmetic = "IsCosmetic";

	// Token: 0x0400273A RID: 10042
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropKeepOnDeath = "KeepOnDeath";

	// Token: 0x0400273B RID: 10043
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropAllowUnEquip = "AllowUnEquip";

	// Token: 0x0400273C RID: 10044
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropAutoEquip = "AutoEquip";

	// Token: 0x0400273D RID: 10045
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropReplaceByTag = "ReplaceByTag";

	// Token: 0x0400273E RID: 10046
	public EquipmentSlots EquipSlot = EquipmentSlots.Count;

	// Token: 0x0400273F RID: 10047
	public string[] ArmorGroup;

	// Token: 0x04002740 RID: 10048
	public bool IsCosmetic = true;

	// Token: 0x04002741 RID: 10049
	public int CosmeticID = -1;

	// Token: 0x04002742 RID: 10050
	[PublicizedFrom(EAccessModifier.Private)]
	public bool keepOnDeath;

	// Token: 0x04002743 RID: 10051
	public bool AllowUnEquip = true;

	// Token: 0x04002744 RID: 10052
	public bool AutoEquip;

	// Token: 0x04002745 RID: 10053
	public string ReplaceByTag;
}
