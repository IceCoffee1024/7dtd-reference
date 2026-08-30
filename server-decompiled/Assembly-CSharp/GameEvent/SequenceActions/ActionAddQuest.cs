using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001960 RID: 6496
	[Preserve]
	public class ActionAddQuest : ActionBaseClientAction
	{
		// Token: 0x0600C7E8 RID: 51176 RVA: 0x0049510C File Offset: 0x0049330C
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				Quest quest = QuestClass.CreateQuest(this.QuestID);
				entityPlayer.QuestJournal.AddQuest(quest, this.Notify);
			}
		}

		// Token: 0x0600C7E9 RID: 51177 RVA: 0x00495141 File Offset: 0x00493341
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddQuest.PropQuestID, ref this.QuestID);
			properties.ParseBool(ActionAddQuest.PropNotify, ref this.Notify);
		}

		// Token: 0x0600C7EA RID: 51178 RVA: 0x0049516C File Offset: 0x0049336C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddQuest
			{
				targetGroup = this.targetGroup,
				QuestID = this.QuestID,
				Notify = this.Notify
			};
		}

		// Token: 0x040096B2 RID: 38578
		public string QuestID;

		// Token: 0x040096B3 RID: 38579
		public bool Notify = true;

		// Token: 0x040096B4 RID: 38580
		public static string PropQuestID = "quest";

		// Token: 0x040096B5 RID: 38581
		public static string PropNotify = "notify";
	}
}
