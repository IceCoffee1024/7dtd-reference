using System;
using UnityEngine.Scripting;

// Token: 0x02000962 RID: 2402
[Preserve]
public class BaseQuestCriteria
{
	// Token: 0x06004647 RID: 17991 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleVariables()
	{
	}

	// Token: 0x06004648 RID: 17992 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CheckForQuestGiver(EntityNPC entity)
	{
		return true;
	}

	// Token: 0x06004649 RID: 17993 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CheckForPlayer(EntityPlayer player)
	{
		return true;
	}

	// Token: 0x04003857 RID: 14423
	public string ID;

	// Token: 0x04003858 RID: 14424
	public string Value;

	// Token: 0x04003859 RID: 14425
	public QuestClass OwnerQuestClass;

	// Token: 0x0400385A RID: 14426
	public BaseQuestCriteria.CriteriaTypes CriteriaType;

	// Token: 0x02000963 RID: 2403
	public enum CriteriaTypes
	{
		// Token: 0x0400385C RID: 14428
		QuestGiver,
		// Token: 0x0400385D RID: 14429
		Player
	}
}
