using System;

// Token: 0x02000976 RID: 2422
public class QuestCriteriaLevel : BaseQuestCriteria
{
	// Token: 0x060046D3 RID: 18131 RVA: 0x001BEEEC File Offset: 0x001BD0EC
	public override bool CheckForPlayer(EntityPlayer player)
	{
		int num = 0;
		return int.TryParse(this.Value, out num) && player.Progression.Level >= num;
	}
}
