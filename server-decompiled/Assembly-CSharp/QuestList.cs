using System;
using System.Collections.Generic;

// Token: 0x020009E9 RID: 2537
public class QuestList
{
	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x001D6647 File Offset: 0x001D4847
	// (set) Token: 0x06004BB2 RID: 19378 RVA: 0x001D664F File Offset: 0x001D484F
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x001D6658 File Offset: 0x001D4858
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestList(string id)
	{
		this.ID = id;
	}

	// Token: 0x06004BB4 RID: 19380 RVA: 0x001D6674 File Offset: 0x001D4874
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

	// Token: 0x06004BB5 RID: 19381 RVA: 0x001D66A9 File Offset: 0x001D48A9
	[PublicizedFrom(EAccessModifier.Internal)]
	public static QuestList GetQuest(string questListID)
	{
		if (!QuestList.s_QuestLists.ContainsKey(questListID))
		{
			return null;
		}
		return QuestList.s_QuestLists[questListID];
	}

	// Token: 0x04003B94 RID: 15252
	public static Dictionary<string, QuestList> s_QuestLists = new CaseInsensitiveStringDictionary<QuestList>();

	// Token: 0x04003B96 RID: 15254
	public List<QuestEntry> Quests = new List<QuestEntry>();
}
