using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200062F RID: 1583
[Preserve]
public class LootEntryRequirementCVar : BaseOperationLootEntryRequirement
{
	// Token: 0x06003364 RID: 13156 RVA: 0x0014DA16 File Offset: 0x0014BC16
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("cvar", ref this.cvar);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x0014DA4D File Offset: 0x0014BC4D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			return player.Buffs.GetCustomVar(this.cvar);
		}
		return 0f;
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x0014DA6F File Offset: 0x0014BC6F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return StringParsers.ParseFloat(this.valueText, 0, -1, NumberStyles.Any);
	}

	// Token: 0x040028ED RID: 10477
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvar;

	// Token: 0x040028EE RID: 10478
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
