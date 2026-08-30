using System;
using System.Collections.Generic;

// Token: 0x020009D8 RID: 2520
public class QuestList
{
	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06004B2B RID: 19243 RVA: 0x001D3E5F File Offset: 0x001D205F
	// (set) Token: 0x06004B2C RID: 19244 RVA: 0x001D3E67 File Offset: 0x001D2067
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004B2D RID: 19245 RVA: 0x001D3E70 File Offset: 0x001D2070
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestList(string id)
	{
		this.ID = id;
	}

	// Token: 0x06004B2E RID: 19246 RVA: 0x001D3E8C File Offset: 0x001D208C
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

	// Token: 0x06004B2F RID: 19247 RVA: 0x001D3EC1 File Offset: 0x001D20C1
	[PublicizedFrom(EAccessModifier.Internal)]
	public static QuestList GetQuest(string questListID)
	{
		if (!QuestList.s_QuestLists.ContainsKey(questListID))
		{
			return null;
		}
		return QuestList.s_QuestLists[questListID];
	}

	// Token: 0x04003B03 RID: 15107
	public static Dictionary<string, QuestList> s_QuestLists = new CaseInsensitiveStringDictionary<QuestList>();

	// Token: 0x04003B05 RID: 15109
	public List<QuestEntry> Quests = new List<QuestEntry>();
}
