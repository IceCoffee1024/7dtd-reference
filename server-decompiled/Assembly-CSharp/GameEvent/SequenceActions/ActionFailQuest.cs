using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001980 RID: 6528
	[Preserve]
	public class ActionFailQuest : ActionBaseClientAction
	{
		// Token: 0x0600C87D RID: 51325 RVA: 0x00498D2C File Offset: 0x00496F2C
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				if (this.QuestID == "")
				{
					Quest quest = entityPlayer.QuestJournal.ActiveQuest;
					if (quest == null)
					{
						quest = entityPlayer.QuestJournal.FindActiveQuest();
					}
					if (quest != null)
					{
						quest.CloseQuest(Quest.QuestState.Failed, null);
						entityPlayer.QuestJournal.ActiveQuest = null;
						if (this.RemoveQuest)
						{
							entityPlayer.QuestJournal.ForceRemoveQuest(quest);
							return;
						}
					}
				}
				else
				{
					Quest quest2 = entityPlayer.QuestJournal.FindActiveQuest(this.QuestID, -1);
					if (quest2 != null)
					{
						quest2.CloseQuest(Quest.QuestState.Failed, null);
						if (this.RemoveQuest)
						{
							entityPlayer.QuestJournal.ForceRemoveQuest(quest2);
						}
					}
				}
			}
		}

		// Token: 0x0600C87E RID: 51326 RVA: 0x00498DD4 File Offset: 0x00496FD4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionFailQuest.PropQuestID, ref this.QuestID);
			properties.ParseBool(ActionFailQuest.PropRemoveQuest, ref this.RemoveQuest);
			if (this.QuestID != "")
			{
				this.QuestID = this.QuestID.ToLower();
			}
		}

		// Token: 0x0600C87F RID: 51327 RVA: 0x00498E2D File Offset: 0x0049702D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionFailQuest
			{
				QuestID = this.QuestID,
				RemoveQuest = this.RemoveQuest
			};
		}

		// Token: 0x0400977C RID: 38780
		public string QuestID = "";

		// Token: 0x0400977D RID: 38781
		public bool RemoveQuest;

		// Token: 0x0400977E RID: 38782
		public static string PropQuestID = "quest";

		// Token: 0x0400977F RID: 38783
		public static string PropRemoveQuest = "remove_quest";
	}
}
