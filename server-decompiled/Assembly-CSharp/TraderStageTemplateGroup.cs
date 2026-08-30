using System;
using System.Collections.Generic;

// Token: 0x020008A9 RID: 2217
public class TraderStageTemplateGroup
{
	// Token: 0x06004043 RID: 16451 RVA: 0x00191630 File Offset: 0x0018F830
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

	// Token: 0x040033DD RID: 13277
	public string Name = "";

	// Token: 0x040033DE RID: 13278
	public List<TraderStageTemplate> Templates = new List<TraderStageTemplate>();
}
