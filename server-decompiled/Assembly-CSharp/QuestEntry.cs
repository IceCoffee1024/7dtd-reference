using System;
using UnityEngine.Scripting;

// Token: 0x020004C2 RID: 1218
[Preserve]
public class QuestEntry
{
	// Token: 0x0600269F RID: 9887 RVA: 0x000EC0FC File Offset: 0x000EA2FC
	public QuestEntry(string questID, float prob, int startStage, int endStage, string cvar)
	{
		this.QuestID = questID;
		this.Prob = prob;
		this.StartStage = startStage;
		this.EndStage = endStage;
		this.QuestClass = QuestClass.GetQuest(this.QuestID);
		if (cvar != "")
		{
			this.CVar = cvar.Split(',', StringSplitOptions.None);
		}
	}

	// Token: 0x060026A0 RID: 9888 RVA: 0x000EC174 File Offset: 0x000EA374
	public bool CheckRequirement(EntityPlayer player)
	{
		if (!QuestJournal.BuriedQuestsEnabled && this.QuestClass.FilterTags.Test_AnySet(QuestEntry.buriedTag))
		{
			return false;
		}
		if (!QuestJournal.POIQuestsEnabled && this.QuestClass.FilterTags.Test_AnySet(QuestEntry.poiTag))
		{
			return false;
		}
		if (this.CVar == null)
		{
			return true;
		}
		for (int i = 0; i < this.CVar.Length; i++)
		{
			if (player.Buffs.GetCustomVar(this.CVar[i]) == 0f)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04001C9B RID: 7323
	public float Prob = 1f;

	// Token: 0x04001C9C RID: 7324
	public int StartStage = -1;

	// Token: 0x04001C9D RID: 7325
	public int EndStage = -1;

	// Token: 0x04001C9E RID: 7326
	public string QuestID;

	// Token: 0x04001C9F RID: 7327
	public QuestClass QuestClass;

	// Token: 0x04001CA0 RID: 7328
	public string[] CVar;

	// Token: 0x04001CA1 RID: 7329
	public static FastTags<TagGroup.Global> buriedTag = FastTags<TagGroup.Global>.Parse("buried");

	// Token: 0x04001CA2 RID: 7330
	public static FastTags<TagGroup.Global> poiTag = FastTags<TagGroup.Global>.Parse("poi");
}
