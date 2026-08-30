using System;
using UnityEngine;

// Token: 0x020009E8 RID: 2536
public class SharedQuestEntry
{
	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x06004BAE RID: 19374 RVA: 0x001D64F3 File Offset: 0x001D46F3
	public QuestClass QuestClass
	{
		get
		{
			return QuestClass.GetQuest(this.QuestID);
		}
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x001D6500 File Offset: 0x001D4700
	public SharedQuestEntry(int questCode, string questID, string poiName, Vector3 position, Vector3 size, Vector3 returnPos, int sharedByPlayerID, int questGiverID, QuestJournal questJournal, Quest quest)
	{
		this.QuestCode = questCode;
		this.QuestID = questID;
		this.POIName = poiName;
		this.Position = position;
		this.Size = size;
		this.ReturnPos = returnPos;
		this.SharedByPlayerID = sharedByPlayerID;
		this.QuestGiverID = questGiverID;
		this.Quest = ((quest == null) ? QuestClass.CreateQuest(questID) : quest.Clone());
		this.Quest.OwnerJournal = questJournal;
		this.Quest.SetupSharedQuest();
		this.Quest.SharedOwnerID = sharedByPlayerID;
		this.Quest.QuestGiverID = questGiverID;
		this.Quest.QuestCode = questCode;
		this.Quest.AddSharedLocation(position, size);
		if (!this.Quest.DataVariables.ContainsKey("POIName"))
		{
			this.Quest.DataVariables.Add("POIName", poiName);
		}
	}

	// Token: 0x06004BB0 RID: 19376 RVA: 0x001D65F4 File Offset: 0x001D47F4
	public SharedQuestEntry Clone()
	{
		return new SharedQuestEntry(this.QuestCode, this.QuestID, this.POIName, this.Position, this.Size, this.ReturnPos, this.SharedByPlayerID, this.QuestGiverID, this.Quest.OwnerJournal, this.Quest);
	}

	// Token: 0x04003B8B RID: 15243
	public int QuestCode;

	// Token: 0x04003B8C RID: 15244
	public string QuestID;

	// Token: 0x04003B8D RID: 15245
	public string POIName;

	// Token: 0x04003B8E RID: 15246
	public Vector3 Position;

	// Token: 0x04003B8F RID: 15247
	public Vector3 Size;

	// Token: 0x04003B90 RID: 15248
	public Vector3 ReturnPos;

	// Token: 0x04003B91 RID: 15249
	public int SharedByPlayerID = -1;

	// Token: 0x04003B92 RID: 15250
	public int QuestGiverID = -1;

	// Token: 0x04003B93 RID: 15251
	public Quest Quest;
}
