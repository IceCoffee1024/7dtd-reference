using System;
using System.Collections.Generic;

// Token: 0x020009FD RID: 2557
public class QuestList
{
	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x06004BA4 RID: 19364 RVA: 0x001D89D5 File Offset: 0x001D6BD5
	// (set) Token: 0x06004BA5 RID: 19365 RVA: 0x001D89DD File Offset: 0x001D6BDD
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004BA6 RID: 19366 RVA: 0x001D89E6 File Offset: 0x001D6BE6
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestList(string id)
	{
		this.ID = id;
	}

	// Token: 0x06004BA7 RID: 19367 RVA: 0x001D8A00 File Offset: 0x001D6C00
	public static QuestList NewList(string id)
	{
		if (QuestList.s_QuestLists.ContainsKey(id))
		{
			return null;
		}
		QuestList questList = new QuestList(id.ToLower());
		QuestList.s_QuestLists[id] = questList;
		return questList;
	}

	// Token: 0x06004BA8 RID: 19368 RVA: 0x001D8A35 File Offset: 0x001D6C35
	[PublicizedFrom(EAccessModifier.Internal)]
	public static QuestList GetQuest(string questListID)
	{
		if (!QuestList.s_QuestLists.ContainsKey(questListID))
		{
			return null;
		}
		return QuestList.s_QuestLists[questListID];
	}

	// Token: 0x04003BC2 RID: 15298
	public static Dictionary<string, QuestList> s_QuestLists = new CaseInsensitiveStringDictionary<QuestList>();

	// Token: 0x04003BC4 RID: 15300
	public List<QuestEntry> Quests = new List<QuestEntry>();
}
