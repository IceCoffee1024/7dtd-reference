using System;
using System.Collections.Generic;

// Token: 0x020008B5 RID: 2229
public class TraderStageTemplateGroup
{
	// Token: 0x060040A8 RID: 16552 RVA: 0x00193708 File Offset: 0x00191908
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

	// Token: 0x0400345E RID: 13406
	public string Name = "";

	// Token: 0x0400345F RID: 13407
	public List<TraderStageTemplate> Templates = new List<TraderStageTemplate>();
}
