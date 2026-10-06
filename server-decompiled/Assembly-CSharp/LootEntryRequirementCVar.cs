using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000628 RID: 1576
[Preserve]
public class LootEntryRequirementCVar : BaseOperationLootEntryRequirement
{
	// Token: 0x06003356 RID: 13142 RVA: 0x0014CE22 File Offset: 0x0014B022
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("cvar", ref this.cvar);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x0014CE59 File Offset: 0x0014B059
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			return player.Buffs.GetCustomVar(this.cvar);
		}
		return 0f;
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x0014CE7B File Offset: 0x0014B07B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return StringParsers.ParseFloat(this.valueText, 0, -1, NumberStyles.Any);
	}

	// Token: 0x040028D2 RID: 10450
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvar;

	// Token: 0x040028D3 RID: 10451
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
