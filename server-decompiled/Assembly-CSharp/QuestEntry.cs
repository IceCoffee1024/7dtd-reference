using System;
using UnityEngine.Scripting;

// Token: 0x020004CA RID: 1226
[Preserve]
public class QuestEntry
{
	// Token: 0x0600269C RID: 9884 RVA: 0x000EC604 File Offset: 0x000EA804
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

	// Token: 0x0600269D RID: 9885 RVA: 0x000EC67C File Offset: 0x000EA87C
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

	// Token: 0x04001CA8 RID: 7336
	public float Prob = 1f;

	// Token: 0x04001CA9 RID: 7337
	public int StartStage = -1;

	// Token: 0x04001CAA RID: 7338
	public int EndStage = -1;

	// Token: 0x04001CAB RID: 7339
	public string QuestID;

	// Token: 0x04001CAC RID: 7340
	public QuestClass QuestClass;

	// Token: 0x04001CAD RID: 7341
	public string[] CVar;

	// Token: 0x04001CAE RID: 7342
	public static FastTags<TagGroup.Global> buriedTag = FastTags<TagGroup.Global>.Parse("buried");

	// Token: 0x04001CAF RID: 7343
	public static FastTags<TagGroup.Global> poiTag = FastTags<TagGroup.Global>.Parse("poi");
}
