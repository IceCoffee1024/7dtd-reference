using System;
using UnityEngine.Scripting;

// Token: 0x020005F0 RID: 1520
[Preserve]
public class ItemClassArmor : ItemClass
{
	// Token: 0x06003191 RID: 12689 RVA: 0x00142A89 File Offset: 0x00140C89
	public override bool KeepOnDeath()
	{
		return this.keepOnDeath;
	}

	// Token: 0x06003192 RID: 12690 RVA: 0x00142A94 File Offset: 0x00140C94
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

	// Token: 0x06003193 RID: 12691 RVA: 0x00142B74 File Offset: 0x00140D74
	public override bool CanEquip()
	{
		return (World.TemperatureSurvival || World.StormFrequency != 0f || this.EquipSlot < EquipmentSlots.ClothingHead || this.EquipSlot > EquipmentSlots.ClothingFeet) && (World.BiomeProgressionEnabled || this.EquipSlot < EquipmentSlots.BiomeBadge || this.EquipSlot > EquipmentSlots.BiomeBadge4);
	}

	// Token: 0x04002701 RID: 9985
	public const string PropEquipSlot = "EquipSlot";

	// Token: 0x04002702 RID: 9986
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropArmorGroup = "ArmorGroup";

	// Token: 0x04002703 RID: 9987
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropIsCosmetic = "IsCosmetic";

	// Token: 0x04002704 RID: 9988
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropKeepOnDeath = "KeepOnDeath";

	// Token: 0x04002705 RID: 9989
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropAllowUnEquip = "AllowUnEquip";

	// Token: 0x04002706 RID: 9990
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropAutoEquip = "AutoEquip";

	// Token: 0x04002707 RID: 9991
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PropReplaceByTag = "ReplaceByTag";

	// Token: 0x04002708 RID: 9992
	public EquipmentSlots EquipSlot = EquipmentSlots.Count;

	// Token: 0x04002709 RID: 9993
	public string[] ArmorGroup;

	// Token: 0x0400270A RID: 9994
	public bool IsCosmetic = true;

	// Token: 0x0400270B RID: 9995
	public int CosmeticID = -1;

	// Token: 0x0400270C RID: 9996
	[PublicizedFrom(EAccessModifier.Private)]
	public bool keepOnDeath;

	// Token: 0x0400270D RID: 9997
	public bool AllowUnEquip = true;

	// Token: 0x0400270E RID: 9998
	public bool AutoEquip;

	// Token: 0x0400270F RID: 9999
	public string ReplaceByTag;
}
