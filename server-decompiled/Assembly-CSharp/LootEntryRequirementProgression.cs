using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000629 RID: 1577
[Preserve]
public class LootEntryRequirementProgression : BaseOperationLootEntryRequirement
{
	// Token: 0x0600335A RID: 13146 RVA: 0x0014CE97 File Offset: 0x0014B097
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("name", ref this.progressionName);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x0600335B RID: 13147 RVA: 0x0014CED0 File Offset: 0x0014B0D0
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

	// Token: 0x0600335C RID: 13148 RVA: 0x0014CF08 File Offset: 0x0014B108
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return StringParsers.ParseFloat(this.valueText, 0, -1, NumberStyles.Any);
	}

	// Token: 0x040028D4 RID: 10452
	[PublicizedFrom(EAccessModifier.Protected)]
	public string progressionName;

	// Token: 0x040028D5 RID: 10453
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
