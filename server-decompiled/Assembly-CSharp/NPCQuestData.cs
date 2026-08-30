using System;
using System.Collections.Generic;

// Token: 0x020009D1 RID: 2513
public class NPCQuestData
{
	// Token: 0x04003AE3 RID: 15075
	public Dictionary<int, NPCQuestData.PlayerQuestData> PlayerQuestList = new Dictionary<int, NPCQuestData.PlayerQuestData>();

	// Token: 0x020009D2 RID: 2514
	public class PlayerQuestData
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06004ACD RID: 19149 RVA: 0x001D156E File Offset: 0x001CF76E
		// (set) Token: 0x06004ACE RID: 19150 RVA: 0x001D1576 File Offset: 0x001CF776
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

		// Token: 0x06004ACF RID: 19151 RVA: 0x001D15A2 File Offset: 0x001CF7A2
		public PlayerQuestData(List<Quest> questList)
		{
			this.QuestList = questList;
		}

		// Token: 0x04003AE4 RID: 15076
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Quest> questList;

		// Token: 0x04003AE5 RID: 15077
		public ulong LastUpdate;
	}
}
