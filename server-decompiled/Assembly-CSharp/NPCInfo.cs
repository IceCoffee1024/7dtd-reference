using System;
using System.Collections.Generic;
using MusicUtils.Enums;

// Token: 0x020008AD RID: 2221
public class NPCInfo
{
	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06003FEC RID: 16364 RVA: 0x00192100 File Offset: 0x00190300
	public List<QuestEntry> Quests
	{
		get
		{
			QuestList quest = QuestList.GetQuest(this.QuestListName);
			if (quest != null)
			{
				return quest.Quests;
			}
			return null;
		}
	}

	// Token: 0x06003FEE RID: 16366 RVA: 0x00192149 File Offset: 0x00190349
	public static void InitStatic()
	{
		NPCInfo.npcInfoList = new Dictionary<string, NPCInfo>();
	}

	// Token: 0x06003FEF RID: 16367 RVA: 0x00192155 File Offset: 0x00190355
	public void Init()
	{
		NPCInfo.npcInfoList[this.Id] = this;
	}

	// Token: 0x06003FF0 RID: 16368 RVA: 0x00192168 File Offset: 0x00190368
	public static void Cleanup()
	{
		NPCInfo.npcInfoList = null;
	}

	// Token: 0x040033D4 RID: 13268
	public static Dictionary<string, NPCInfo> npcInfoList;

	// Token: 0x040033D5 RID: 13269
	public string Id;

	// Token: 0x040033D6 RID: 13270
	public string Name;

	// Token: 0x040033D7 RID: 13271
	public string Faction;

	// Token: 0x040033D8 RID: 13272
	public string Portrait;

	// Token: 0x040033D9 RID: 13273
	public string LocalizationID;

	// Token: 0x040033DA RID: 13274
	public string VoiceSet = "";

	// Token: 0x040033DB RID: 13275
	public NPCInfo.StanceTypes CurrentStance;

	// Token: 0x040033DC RID: 13276
	public SectionType DmsSectionType;

	// Token: 0x040033DD RID: 13277
	public byte QuestFaction;

	// Token: 0x040033DE RID: 13278
	public string QuestListName = "trader_quests";

	// Token: 0x040033DF RID: 13279
	public int TraderID = -1;

	// Token: 0x040033E0 RID: 13280
	public string DialogID;

	// Token: 0x020008AE RID: 2222
	public enum StanceTypes
	{
		// Token: 0x040033E2 RID: 13282
		None,
		// Token: 0x040033E3 RID: 13283
		Like,
		// Token: 0x040033E4 RID: 13284
		Neutral,
		// Token: 0x040033E5 RID: 13285
		Dislike
	}
}
