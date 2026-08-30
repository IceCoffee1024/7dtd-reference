using System;
using System.Collections.Generic;

// Token: 0x020009E2 RID: 2530
public class NPCQuestData
{
	// Token: 0x04003B74 RID: 15220
	public Dictionary<int, NPCQuestData.PlayerQuestData> PlayerQuestList = new Dictionary<int, NPCQuestData.PlayerQuestData>();

	// Token: 0x020009E3 RID: 2531
	public class PlayerQuestData
	{
		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06004B53 RID: 19283 RVA: 0x001D3D56 File Offset: 0x001D1F56
		// (set) Token: 0x06004B54 RID: 19284 RVA: 0x001D3D5E File Offset: 0x001D1F5E
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

		// Token: 0x06004B55 RID: 19285 RVA: 0x001D3D8A File Offset: 0x001D1F8A
		public PlayerQuestData(List<Quest> questList)
		{
			this.QuestList = questList;
		}

		// Token: 0x04003B75 RID: 15221
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Quest> questList;

		// Token: 0x04003B76 RID: 15222
		public ulong LastUpdate;
	}
}
