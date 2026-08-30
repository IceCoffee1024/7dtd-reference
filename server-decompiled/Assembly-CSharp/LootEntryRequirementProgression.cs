using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000627 RID: 1575
[Preserve]
public class LootEntryRequirementProgression : BaseOperationLootEntryRequirement
{
	// Token: 0x06003317 RID: 13079 RVA: 0x0014C527 File Offset: 0x0014A727
	public override void Init(XElement e)
	{
		base.Init(e);
		e.ParseAttribute("name", ref this.progressionName);
		e.ParseAttribute("value", ref this.valueText);
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x0014C560 File Offset: 0x0014A760
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

	// Token: 0x06003319 RID: 13081 RVA: 0x0014C598 File Offset: 0x0014A798
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float RightSide(EntityPlayer player)
	{
		return StringParsers.ParseFloat(this.valueText, 0, -1, NumberStyles.Any);
	}

	// Token: 0x040028DB RID: 10459
	[PublicizedFrom(EAccessModifier.Protected)]
	public string progressionName;

	// Token: 0x040028DC RID: 10460
	[PublicizedFrom(EAccessModifier.Protected)]
	public string valueText;
}
