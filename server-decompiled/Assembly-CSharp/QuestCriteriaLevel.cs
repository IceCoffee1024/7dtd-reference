using System;

// Token: 0x02000986 RID: 2438
public class QuestCriteriaLevel : BaseQuestCriteria
{
	// Token: 0x06004697 RID: 18071 RVA: 0x001C0478 File Offset: 0x001BE678
	public override bool CheckForPlayer(EntityPlayer player)
	{
		int num = 0;
		return int.TryParse(this.Value, out num) && player.Progression.Level >= num;
	}
}
