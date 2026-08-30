using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200195A RID: 6490
	[Preserve]
	public class ActionFailQuest : ActionBaseClientAction
	{
		// Token: 0x0600C6F6 RID: 50934 RVA: 0x004929E4 File Offset: 0x00490BE4
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

		// Token: 0x0600C6F7 RID: 50935 RVA: 0x00492A8C File Offset: 0x00490C8C
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

		// Token: 0x0600C6F8 RID: 50936 RVA: 0x00492AE5 File Offset: 0x00490CE5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionFailQuest
			{
				QuestID = this.QuestID,
				RemoveQuest = this.RemoveQuest
			};
		}

		// Token: 0x04009653 RID: 38483
		public string QuestID = "";

		// Token: 0x04009654 RID: 38484
		public bool RemoveQuest;

		// Token: 0x04009655 RID: 38485
		public static string PropQuestID = "quest";

		// Token: 0x04009656 RID: 38486
		public static string PropRemoveQuest = "remove_quest";
	}
}
