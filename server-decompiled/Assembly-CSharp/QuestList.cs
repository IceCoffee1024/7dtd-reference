using System;
using System.Collections.Generic;

// Token: 0x020009F3 RID: 2547
public class QuestList
{
	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x06004B80 RID: 19328 RVA: 0x001D78ED File Offset: 0x001D5AED
	// (set) Token: 0x06004B81 RID: 19329 RVA: 0x001D78F5 File Offset: 0x001D5AF5
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004B82 RID: 19330 RVA: 0x001D78FE File Offset: 0x001D5AFE
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestList(string id)
	{
		this.ID = id;
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x001D7918 File Offset: 0x001D5B18
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

	// Token: 0x06004B84 RID: 19332 RVA: 0x001D794D File Offset: 0x001D5B4D
	[PublicizedFrom(EAccessModifier.Internal)]
	public static QuestList GetQuest(string questListID)
	{
		if (!QuestList.s_QuestLists.ContainsKey(questListID))
		{
			return null;
		}
		return QuestList.s_QuestLists[questListID];
	}

	// Token: 0x04003BA1 RID: 15265
	public static Dictionary<string, QuestList> s_QuestLists = new CaseInsensitiveStringDictionary<QuestList>();

	// Token: 0x04003BA3 RID: 15267
	public List<QuestEntry> Quests = new List<QuestEntry>();
}
