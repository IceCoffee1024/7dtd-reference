using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000626 RID: 1574
[Preserve]
public class LootEntryRequirementCVar : BaseOperationLootEntryRequirement
{
	// Token: 0x06003313 RID: 13075 RVA: 0x0014C4B2 File Offset: 0x0014A6B2
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("cvar", ref this.cvar);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x0014C4E9 File Offset: 0x0014A6E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			return player.Buffs.GetCustomVar(this.cvar);
		}
		return 0f;
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x0014C50B File Offset: 0x0014A70B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return StringParsers.ParseFloat(this.valueText, 0, -1, NumberStyles.Any);
	}

	// Token: 0x040028D9 RID: 10457
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvar;

	// Token: 0x040028DA RID: 10458
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
