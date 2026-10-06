using System;
using System.Collections.Generic;

// Token: 0x020009EC RID: 2540
public class NPCQuestData
{
	// Token: 0x04003B80 RID: 15232
	public Dictionary<int, NPCQuestData.PlayerQuestData> PlayerQuestList = new Dictionary<int, NPCQuestData.PlayerQuestData>();

	// Token: 0x020009ED RID: 2541
	public class PlayerQuestData
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06004B22 RID: 19234 RVA: 0x001D4F9A File Offset: 0x001D319A
		// (set) Token: 0x06004B23 RID: 19235 RVA: 0x001D4FA2 File Offset: 0x001D31A2
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

		// Token: 0x06004B24 RID: 19236 RVA: 0x001D4FCE File Offset: 0x001D31CE
		public PlayerQuestData(List<Quest> questList)
		{
			this.QuestList = questList;
		}

		// Token: 0x04003B81 RID: 15233
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Quest> questList;

		// Token: 0x04003B82 RID: 15234
		public ulong LastUpdate;
	}
}
