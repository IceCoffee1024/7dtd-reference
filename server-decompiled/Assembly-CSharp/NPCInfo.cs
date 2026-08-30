using System;
using System.Collections.Generic;
using MusicUtils.Enums;

// Token: 0x0200089C RID: 2204
public class NPCInfo
{
	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x0018FF2C File Offset: 0x0018E12C
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

	// Token: 0x06003FF3 RID: 16371 RVA: 0x0018FF75 File Offset: 0x0018E175
	public static void InitStatic()
	{
		NPCInfo.npcInfoList = new Dictionary<string, NPCInfo>();
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x0018FF81 File Offset: 0x0018E181
	public void Init()
	{
		NPCInfo.npcInfoList[this.Id] = this;
	}

	// Token: 0x06003FF5 RID: 16373 RVA: 0x0018FF94 File Offset: 0x0018E194
	public static void Cleanup()
	{
		NPCInfo.npcInfoList = null;
	}

	// Token: 0x0400335F RID: 13151
	public static Dictionary<string, NPCInfo> npcInfoList;

	// Token: 0x04003360 RID: 13152
	public string Id;

	// Token: 0x04003361 RID: 13153
	public string Name;

	// Token: 0x04003362 RID: 13154
	public string Faction;

	// Token: 0x04003363 RID: 13155
	public string Portrait;

	// Token: 0x04003364 RID: 13156
	public string LocalizationID;

	// Token: 0x04003365 RID: 13157
	public string VoiceSet = "";

	// Token: 0x04003366 RID: 13158
	public NPCInfo.StanceTypes CurrentStance;

	// Token: 0x04003367 RID: 13159
	public SectionType DmsSectionType;

	// Token: 0x04003368 RID: 13160
	public byte QuestFaction;

	// Token: 0x04003369 RID: 13161
	public string QuestListName = "trader_quests";

	// Token: 0x0400336A RID: 13162
	public int TraderID = -1;

	// Token: 0x0400336B RID: 13163
	public string DialogID;

	// Token: 0x0200089D RID: 2205
	public enum StanceTypes
	{
		// Token: 0x0400336D RID: 13165
		None,
		// Token: 0x0400336E RID: 13166
		Like,
		// Token: 0x0400336F RID: 13167
		Neutral,
		// Token: 0x04003370 RID: 13168
		Dislike
	}
}
