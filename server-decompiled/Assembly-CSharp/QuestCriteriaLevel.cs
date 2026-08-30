using System;

// Token: 0x02000965 RID: 2405
public class QuestCriteriaLevel : BaseQuestCriteria
{
	// Token: 0x0600464D RID: 17997 RVA: 0x001BC6F8 File Offset: 0x001BA8F8
	public override bool CheckForPlayer(EntityPlayer player)
	{
		int num = 0;
		return int.TryParse(this.Value, out num) && player.Progression.Level >= num;
	}
}
