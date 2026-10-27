using System;
using System.Collections.Generic;

// Token: 0x020008C4 RID: 2244
public class TraderStageTemplateGroup
{
	// Token: 0x06004061 RID: 16481 RVA: 0x001948B4 File Offset: 0x00192AB4
	public bool IsWithin(int traderStage, int quality)
	{
		for (int i = 0; i < this.Templates.Count; i++)
		{
			if (this.Templates[i].IsWithin(traderStage, quality))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04003472 RID: 13426
	public string Name = "";

	// Token: 0x04003473 RID: 13427
	public List<TraderStageTemplate> Templates = new List<TraderStageTemplate>();
}
