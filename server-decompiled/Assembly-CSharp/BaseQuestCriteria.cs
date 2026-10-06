using System;
using UnityEngine.Scripting;

// Token: 0x02000979 RID: 2425
[Preserve]
public class BaseQuestCriteria
{
	// Token: 0x0600466E RID: 18030 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleVariables()
	{
	}

	// Token: 0x0600466F RID: 18031 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CheckForQuestGiver(EntityNPC entity)
	{
		return true;
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CheckForPlayer(EntityPlayer player)
	{
		return true;
	}

	// Token: 0x040038E4 RID: 14564
	public string ID;

	// Token: 0x040038E5 RID: 14565
	public string Value;

	// Token: 0x040038E6 RID: 14566
	public QuestClass OwnerQuestClass;

	// Token: 0x040038E7 RID: 14567
	public BaseQuestCriteria.CriteriaTypes CriteriaType;

	// Token: 0x0200097A RID: 2426
	public enum CriteriaTypes
	{
		// Token: 0x040038E9 RID: 14569
		QuestGiver,
		// Token: 0x040038EA RID: 14570
		Player
	}
}
