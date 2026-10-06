using System;
using UnityEngine;

// Token: 0x020009F2 RID: 2546
public class SharedQuestEntry
{
	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x06004B7D RID: 19325 RVA: 0x001D777F File Offset: 0x001D597F
	public QuestClass QuestClass
	{
		get
		{
			return QuestClass.GetQuest(this.QuestID);
		}
	}

	// Token: 0x06004B7E RID: 19326 RVA: 0x001D778C File Offset: 0x001D598C
	public SharedQuestEntry(string questUniqueId, int questCode, string questID, string poiName, Vector3 position, Vector3 size, Vector3 returnPos, int sharedByPlayerID, int questGiverID, QuestJournal questJournal, Quest quest)
	{
		this.QuestUniqueId = questUniqueId;
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
		this.Quest.QuestUniqueId = questUniqueId;
		this.Quest.QuestCode = questCode;
		this.Quest.AddSharedLocation(position, size);
		if (!this.Quest.DataVariables.ContainsKey("POIName"))
		{
			this.Quest.DataVariables.Add("POIName", poiName);
		}
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x001D7894 File Offset: 0x001D5A94
	public SharedQuestEntry Clone()
	{
		return new SharedQuestEntry(this.QuestUniqueId, this.QuestCode, this.QuestID, this.POIName, this.Position, this.Size, this.ReturnPos, this.SharedByPlayerID, this.QuestGiverID, this.Quest.OwnerJournal, this.Quest);
	}

	// Token: 0x04003B97 RID: 15255
	public string QuestUniqueId;

	// Token: 0x04003B98 RID: 15256
	public int QuestCode;

	// Token: 0x04003B99 RID: 15257
	public string QuestID;

	// Token: 0x04003B9A RID: 15258
	public string POIName;

	// Token: 0x04003B9B RID: 15259
	public Vector3 Position;

	// Token: 0x04003B9C RID: 15260
	public Vector3 Size;

	// Token: 0x04003B9D RID: 15261
	public Vector3 ReturnPos;

	// Token: 0x04003B9E RID: 15262
	public int SharedByPlayerID = -1;

	// Token: 0x04003B9F RID: 15263
	public int QuestGiverID = -1;

	// Token: 0x04003BA0 RID: 15264
	public Quest Quest;
}
