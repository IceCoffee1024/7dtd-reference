using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000630 RID: 1584
[Preserve]
public class LootEntryRequirementProgression : BaseOperationLootEntryRequirement
{
	// Token: 0x06003368 RID: 13160 RVA: 0x0014DA8B File Offset: 0x0014BC8B
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("name", ref this.progressionName);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x0014DAC4 File Offset: 0x0014BCC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float LeftSide(EntityPlayer player)
	{
		if (player != null)
		{
			ProgressionValue progressionValue = player.Progression.GetProgressionValue(this.progressionName);
			if (progressionValue != null)
			{
				return progressionValue.GetCalculatedLevel(player);
			}
		}
		return 0f;
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x0014DAFC File Offset: 0x0014BCFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return StringParsers.ParseFloat(this.valueText, 0, -1, NumberStyles.Any);
	}

	// Token: 0x040028EF RID: 10479
	[PublicizedFrom(EAccessModifier.Protected)]
	public string progressionName;

	// Token: 0x040028F0 RID: 10480
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
