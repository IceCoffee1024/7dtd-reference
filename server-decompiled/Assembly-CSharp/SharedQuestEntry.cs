using System;
using UnityEngine;

// Token: 0x020009D7 RID: 2519
public class SharedQuestEntry
{
	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x06004B28 RID: 19240 RVA: 0x001D3D0B File Offset: 0x001D1F0B
	public QuestClass QuestClass
	{
		get
		{
			return QuestClass.GetQuest(this.QuestID);
		}
	}

	// Token: 0x06004B29 RID: 19241 RVA: 0x001D3D18 File Offset: 0x001D1F18
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

	// Token: 0x06004B2A RID: 19242 RVA: 0x001D3E0C File Offset: 0x001D200C
	public SharedQuestEntry Clone()
	{
		return new SharedQuestEntry(this.QuestCode, this.QuestID, this.POIName, this.Position, this.Size, this.ReturnPos, this.SharedByPlayerID, this.QuestGiverID, this.Quest.OwnerJournal, this.Quest);
	}

	// Token: 0x04003AFA RID: 15098
	public int QuestCode;

	// Token: 0x04003AFB RID: 15099
	public string QuestID;

	// Token: 0x04003AFC RID: 15100
	public string POIName;

	// Token: 0x04003AFD RID: 15101
	public Vector3 Position;

	// Token: 0x04003AFE RID: 15102
	public Vector3 Size;

	// Token: 0x04003AFF RID: 15103
	public Vector3 ReturnPos;

	// Token: 0x04003B00 RID: 15104
	public int SharedByPlayerID = -1;

	// Token: 0x04003B01 RID: 15105
	public int QuestGiverID = -1;

	// Token: 0x04003B02 RID: 15106
	public Quest Quest;
}
