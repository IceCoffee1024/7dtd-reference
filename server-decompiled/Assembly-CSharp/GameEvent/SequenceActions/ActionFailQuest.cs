using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AC RID: 6572
	[Preserve]
	public class ActionFailQuest : ActionBaseClientAction
	{
		// Token: 0x0600C9D5 RID: 51669 RVA: 0x0049D1F0 File Offset: 0x0049B3F0
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

		// Token: 0x0600C9D6 RID: 51670 RVA: 0x0049D298 File Offset: 0x0049B498
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

		// Token: 0x0600C9D7 RID: 51671 RVA: 0x0049D2F1 File Offset: 0x0049B4F1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionFailQuest
			{
				QuestID = this.QuestID,
				RemoveQuest = this.RemoveQuest
			};
		}

		// Token: 0x04009842 RID: 38978
		public string QuestID = "";

		// Token: 0x04009843 RID: 38979
		public bool RemoveQuest;

		// Token: 0x04009844 RID: 38980
		public static string PropQuestID = "quest";

		// Token: 0x04009845 RID: 38981
		public static string PropRemoveQuest = "remove_quest";
	}
}
