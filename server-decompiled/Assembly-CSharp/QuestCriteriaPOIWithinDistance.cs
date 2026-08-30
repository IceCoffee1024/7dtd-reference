using System;

// Token: 0x02000975 RID: 2421
public class QuestCriteriaPOIWithinDistance : BaseQuestCriteria
{
	// Token: 0x060046D1 RID: 18129 RVA: 0x001BEEC4 File Offset: 0x001BD0C4
	public override bool CheckForQuestGiver(EntityNPC entity)
	{
		int num = 0;
		int.TryParse(this.Value, out num);
		return false;
	}
}
