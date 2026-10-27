using System;
using System.Collections.Generic;
using MusicUtils.Enums;

// Token: 0x020008B7 RID: 2231
public class NPCInfo
{
	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x0600400F RID: 16399 RVA: 0x001931A8 File Offset: 0x001913A8
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

	// Token: 0x06004011 RID: 16401 RVA: 0x001931F1 File Offset: 0x001913F1
	public static void InitStatic()
	{
		NPCInfo.npcInfoList = new Dictionary<string, NPCInfo>();
	}

	// Token: 0x06004012 RID: 16402 RVA: 0x001931FD File Offset: 0x001913FD
	public void Init()
	{
		NPCInfo.npcInfoList[this.Id] = this;
	}

	// Token: 0x06004013 RID: 16403 RVA: 0x00193210 File Offset: 0x00191410
	public static void Cleanup()
	{
		NPCInfo.npcInfoList = null;
	}

	// Token: 0x040033F4 RID: 13300
	public static Dictionary<string, NPCInfo> npcInfoList;

	// Token: 0x040033F5 RID: 13301
	public string Id;

	// Token: 0x040033F6 RID: 13302
	public string Name;

	// Token: 0x040033F7 RID: 13303
	public string Faction;

	// Token: 0x040033F8 RID: 13304
	public string Portrait;

	// Token: 0x040033F9 RID: 13305
	public string LocalizationID;

	// Token: 0x040033FA RID: 13306
	public string VoiceSet = "";

	// Token: 0x040033FB RID: 13307
	public NPCInfo.StanceTypes CurrentStance;

	// Token: 0x040033FC RID: 13308
	public SectionType DmsSectionType;

	// Token: 0x040033FD RID: 13309
	public byte QuestFaction;

	// Token: 0x040033FE RID: 13310
	public string QuestListName = "trader_quests";

	// Token: 0x040033FF RID: 13311
	public int TraderID = -1;

	// Token: 0x04003400 RID: 13312
	public string DialogID;

	// Token: 0x020008B8 RID: 2232
	public enum StanceTypes
	{
		// Token: 0x04003402 RID: 13314
		None,
		// Token: 0x04003403 RID: 13315
		Like,
		// Token: 0x04003404 RID: 13316
		Neutral,
		// Token: 0x04003405 RID: 13317
		Dislike
	}
}
