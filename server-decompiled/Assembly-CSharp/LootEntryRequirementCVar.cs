using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200061D RID: 1565
[Preserve]
public class LootEntryRequirementCVar : BaseOperationLootEntryRequirement
{
	// Token: 0x060032C0 RID: 12992 RVA: 0x0014A396 File Offset: 0x00148596
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("cvar", ref this.cvar);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x060032C1 RID: 12993 RVA: 0x0014A3CD File Offset: 0x001485CD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			return player.Buffs.GetCustomVar(this.cvar);
		}
		return 0f;
	}

	// Token: 0x060032C2 RID: 12994 RVA: 0x0014A3EF File Offset: 0x001485EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return StringParsers.ParseFloat(this.valueText, 0, -1, NumberStyles.Any);
	}

	// Token: 0x04002866 RID: 10342
	[PublicizedFrom(EAccessModifier.Protected)]
	public string cvar;

	// Token: 0x04002867 RID: 10343
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
