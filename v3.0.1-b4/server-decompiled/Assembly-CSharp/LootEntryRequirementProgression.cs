using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200061E RID: 1566
[Preserve]
public class LootEntryRequirementProgression : BaseOperationLootEntryRequirement
{
	// Token: 0x060032C4 RID: 12996 RVA: 0x0014A40B File Offset: 0x0014860B
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("name", ref this.progressionName);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x0014A444 File Offset: 0x00148644
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

	// Token: 0x060032C6 RID: 12998 RVA: 0x0014A47C File Offset: 0x0014867C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return StringParsers.ParseFloat(this.valueText, 0, -1, NumberStyles.Any);
	}

	// Token: 0x04002868 RID: 10344
	[PublicizedFrom(EAccessModifier.Protected)]
	public string progressionName;

	// Token: 0x04002869 RID: 10345
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
