using System;

// Token: 0x0200097C RID: 2428
public class QuestCriteriaLevel : BaseQuestCriteria
{
	// Token: 0x06004674 RID: 18036 RVA: 0x001BF3F4 File Offset: 0x001BD5F4
	public override bool CheckForPlayer(EntityPlayer player)
	{
		int num = 0;
		return int.TryParse(this.Value, out num) && player.Progression.Level >= num;
	}
}
