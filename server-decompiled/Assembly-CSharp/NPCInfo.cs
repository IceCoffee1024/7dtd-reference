using System;
using System.Collections.Generic;
using MusicUtils.Enums;

// Token: 0x020008A8 RID: 2216
public class NPCInfo
{
	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x06004056 RID: 16470 RVA: 0x00191FFC File Offset: 0x001901FC
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

	// Token: 0x06004058 RID: 16472 RVA: 0x00192045 File Offset: 0x00190245
	public static void InitStatic()
	{
		NPCInfo.npcInfoList = new Dictionary<string, NPCInfo>();
	}

	// Token: 0x06004059 RID: 16473 RVA: 0x00192051 File Offset: 0x00190251
	public void Init()
	{
		NPCInfo.npcInfoList[this.Id] = this;
	}

	// Token: 0x0600405A RID: 16474 RVA: 0x00192064 File Offset: 0x00190264
	public static void Cleanup()
	{
		NPCInfo.npcInfoList = null;
	}

	// Token: 0x040033E0 RID: 13280
	public static Dictionary<string, NPCInfo> npcInfoList;

	// Token: 0x040033E1 RID: 13281
	public string Id;

	// Token: 0x040033E2 RID: 13282
	public string Name;

	// Token: 0x040033E3 RID: 13283
	public string Faction;

	// Token: 0x040033E4 RID: 13284
	public string Portrait;

	// Token: 0x040033E5 RID: 13285
	public string LocalizationID;

	// Token: 0x040033E6 RID: 13286
	public string VoiceSet = "";

	// Token: 0x040033E7 RID: 13287
	public NPCInfo.StanceTypes CurrentStance;

	// Token: 0x040033E8 RID: 13288
	public SectionType DmsSectionType;

	// Token: 0x040033E9 RID: 13289
	public byte QuestFaction;

	// Token: 0x040033EA RID: 13290
	public string QuestListName = "trader_quests";

	// Token: 0x040033EB RID: 13291
	public int TraderID = -1;

	// Token: 0x040033EC RID: 13292
	public string DialogID;

	// Token: 0x020008A9 RID: 2217
	public enum StanceTypes
	{
		// Token: 0x040033EE RID: 13294
		None,
		// Token: 0x040033EF RID: 13295
		Like,
		// Token: 0x040033F0 RID: 13296
		Neutral,
		// Token: 0x040033F1 RID: 13297
		Dislike
	}
}
