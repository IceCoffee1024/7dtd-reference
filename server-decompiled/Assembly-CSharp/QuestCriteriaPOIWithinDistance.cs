using System;

// Token: 0x02000964 RID: 2404
public class QuestCriteriaPOIWithinDistance : BaseQuestCriteria
{
	// Token: 0x0600464B RID: 17995 RVA: 0x001BC6D0 File Offset: 0x001BA8D0
	public override bool CheckForQuestGiver(EntityNPC entity)
	{
		int num = 0;
		int.TryParse(this.Value, out num);
		return false;
	}
}
