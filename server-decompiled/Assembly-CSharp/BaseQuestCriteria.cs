using System;
using UnityEngine.Scripting;

// Token: 0x02000983 RID: 2435
[Preserve]
public class BaseQuestCriteria
{
	// Token: 0x06004691 RID: 18065 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void HandleVariables()
	{
	}

	// Token: 0x06004692 RID: 18066 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CheckForQuestGiver(EntityNPC entity)
	{
		return true;
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CheckForPlayer(EntityPlayer player)
	{
		return true;
	}

	// Token: 0x04003904 RID: 14596
	public string ID;

	// Token: 0x04003905 RID: 14597
	public string Value;

	// Token: 0x04003906 RID: 14598
	public QuestClass OwnerQuestClass;

	// Token: 0x04003907 RID: 14599
	public BaseQuestCriteria.CriteriaTypes CriteriaType;

	// Token: 0x02000984 RID: 2436
	public enum CriteriaTypes
	{
		// Token: 0x04003909 RID: 14601
		QuestGiver,
		// Token: 0x0400390A RID: 14602
		Player
	}
}
