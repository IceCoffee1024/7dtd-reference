using System;

// Token: 0x0200097B RID: 2427
public class QuestCriteriaPOIWithinDistance : BaseQuestCriteria
{
	// Token: 0x06004672 RID: 18034 RVA: 0x001BF3CC File Offset: 0x001BD5CC
	public override bool CheckForQuestGiver(EntityNPC entity)
	{
		int num = 0;
		int.TryParse(this.Value, out num);
		return false;
	}
}
