using System;

// Token: 0x02000985 RID: 2437
public class QuestCriteriaPOIWithinDistance : BaseQuestCriteria
{
	// Token: 0x06004695 RID: 18069 RVA: 0x001C0450 File Offset: 0x001BE650
	public override bool CheckForQuestGiver(EntityNPC entity)
	{
		int num = 0;
		int.TryParse(this.Value, out num);
		return false;
	}
}
