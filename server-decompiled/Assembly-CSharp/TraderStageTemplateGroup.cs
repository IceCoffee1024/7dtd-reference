using System;
using System.Collections.Generic;

// Token: 0x020008BA RID: 2234
public class TraderStageTemplateGroup
{
	// Token: 0x0600403E RID: 16446 RVA: 0x0019380C File Offset: 0x00191A0C
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

	// Token: 0x04003452 RID: 13394
	public string Name = "";

	// Token: 0x04003453 RID: 13395
	public List<TraderStageTemplate> Templates = new List<TraderStageTemplate>();
}
