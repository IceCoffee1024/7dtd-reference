using System;
using UnityEngine;

// Token: 0x020009FC RID: 2556
public class SharedQuestEntry
{
	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x06004BA1 RID: 19361 RVA: 0x001D885F File Offset: 0x001D6A5F
	public QuestClass QuestClass
	{
		get
		{
			return QuestClass.GetQuest(this.QuestID);
		}
	}

	// Token: 0x06004BA2 RID: 19362 RVA: 0x001D886C File Offset: 0x001D6A6C
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
		this.Quest.DataVariables.TryAdd("POIName", Localization.Get(poiName, false, null));
		this.Quest.DataVariables.TryAdd("POINameKey", poiName);
	}

	// Token: 0x06004BA3 RID: 19363 RVA: 0x001D897C File Offset: 0x001D6B7C
	public SharedQuestEntry Clone()
	{
		return new SharedQuestEntry(this.QuestUniqueId, this.QuestCode, this.QuestID, this.POIName, this.Position, this.Size, this.ReturnPos, this.SharedByPlayerID, this.QuestGiverID, this.Quest.OwnerJournal, this.Quest);
	}

	// Token: 0x04003BB8 RID: 15288
	public string QuestUniqueId;

	// Token: 0x04003BB9 RID: 15289
	public int QuestCode;

	// Token: 0x04003BBA RID: 15290
	public string QuestID;

	// Token: 0x04003BBB RID: 15291
	public string POIName;

	// Token: 0x04003BBC RID: 15292
	public Vector3 Position;

	// Token: 0x04003BBD RID: 15293
	public Vector3 Size;

	// Token: 0x04003BBE RID: 15294
	public Vector3 ReturnPos;

	// Token: 0x04003BBF RID: 15295
	public int SharedByPlayerID = -1;

	// Token: 0x04003BC0 RID: 15296
	public int QuestGiverID = -1;

	// Token: 0x04003BC1 RID: 15297
	public Quest Quest;
}
