using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DD RID: 6621
	[Preserve]
	public class ActionFailQuest : ActionBaseClientAction
	{
		// Token: 0x0600CAE9 RID: 51945 RVA: 0x004A0CF0 File Offset: 0x0049EEF0
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

		// Token: 0x0600CAEA RID: 51946 RVA: 0x004A0D98 File Offset: 0x0049EF98
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

		// Token: 0x0600CAEB RID: 51947 RVA: 0x004A0DF1 File Offset: 0x0049EFF1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionFailQuest
			{
				QuestID = this.QuestID,
				RemoveQuest = this.RemoveQuest
			};
		}

		// Token: 0x0400992B RID: 39211
		public string QuestID = "";

		// Token: 0x0400992C RID: 39212
		public bool RemoveQuest;

		// Token: 0x0400992D RID: 39213
		public static string PropQuestID = "quest";

		// Token: 0x0400992E RID: 39214
		public static string PropRemoveQuest = "remove_quest";
	}
}
