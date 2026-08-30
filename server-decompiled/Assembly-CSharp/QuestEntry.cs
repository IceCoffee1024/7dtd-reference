using System;
using UnityEngine.Scripting;

// Token: 0x020004BE RID: 1214
[Preserve]
public class QuestEntry
{
	// Token: 0x0600267D RID: 9853 RVA: 0x000EB4A8 File Offset: 0x000E96A8
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

	// Token: 0x0600267E RID: 9854 RVA: 0x000EB520 File Offset: 0x000E9720
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

	// Token: 0x04001C79 RID: 7289
	public float Prob = 1f;

	// Token: 0x04001C7A RID: 7290
	public int StartStage = -1;

	// Token: 0x04001C7B RID: 7291
	public int EndStage = -1;

	// Token: 0x04001C7C RID: 7292
	public string QuestID;

	// Token: 0x04001C7D RID: 7293
	public QuestClass QuestClass;

	// Token: 0x04001C7E RID: 7294
	public string[] CVar;

	// Token: 0x04001C7F RID: 7295
	public static FastTags<TagGroup.Global> buriedTag = FastTags<TagGroup.Global>.Parse("buried");

	// Token: 0x04001C80 RID: 7296
	public static FastTags<TagGroup.Global> poiTag = FastTags<TagGroup.Global>.Parse("poi");
}
