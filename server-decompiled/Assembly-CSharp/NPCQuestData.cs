using System;
using System.Collections.Generic;

// Token: 0x020009F6 RID: 2550
public class NPCQuestData
{
	// Token: 0x04003BA1 RID: 15265
	public Dictionary<int, NPCQuestData.PlayerQuestData> PlayerQuestList = new Dictionary<int, NPCQuestData.PlayerQuestData>();

	// Token: 0x020009F7 RID: 2551
	public class PlayerQuestData
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06004B46 RID: 19270 RVA: 0x001D607A File Offset: 0x001D427A
		// (set) Token: 0x06004B47 RID: 19271 RVA: 0x001D6082 File Offset: 0x001D4282
		public List<Quest> QuestList
		{
			get
			{
				return this.questList;
			}
			set
			{
				this.questList = value;
				this.LastUpdate = GameManager.Instance.World.GetWorldTime() / 24000UL * 24000UL;
			}
		}

		// Token: 0x06004B48 RID: 19272 RVA: 0x001D60AE File Offset: 0x001D42AE
		public PlayerQuestData(List<Quest> questList)
		{
			this.QuestList = questList;
		}

		// Token: 0x04003BA2 RID: 15266
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Quest> questList;

		// Token: 0x04003BA3 RID: 15267
		public ulong LastUpdate;
	}
}
