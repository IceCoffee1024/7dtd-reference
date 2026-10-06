using System;
using UnityEngine.Scripting;

// Token: 0x020004C8 RID: 1224
[Preserve]
public class QuestEntry
{
	// Token: 0x06002695 RID: 9877 RVA: 0x000EC1F4 File Offset: 0x000EA3F4
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

	// Token: 0x06002696 RID: 9878 RVA: 0x000EC26C File Offset: 0x000EA46C
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

	// Token: 0x04001C98 RID: 7320
	public float Prob = 1f;

	// Token: 0x04001C99 RID: 7321
	public int StartStage = -1;

	// Token: 0x04001C9A RID: 7322
	public int EndStage = -1;

	// Token: 0x04001C9B RID: 7323
	public string QuestID;

	// Token: 0x04001C9C RID: 7324
	public QuestClass QuestClass;

	// Token: 0x04001C9D RID: 7325
	public string[] CVar;

	// Token: 0x04001C9E RID: 7326
	public static FastTags<TagGroup.Global> buriedTag = FastTags<TagGroup.Global>.Parse("buried");

	// Token: 0x04001C9F RID: 7327
	public static FastTags<TagGroup.Global> poiTag = FastTags<TagGroup.Global>.Parse("poi");
}
