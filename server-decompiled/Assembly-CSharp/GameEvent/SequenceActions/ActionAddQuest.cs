using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198C RID: 6540
	[Preserve]
	public class ActionAddQuest : ActionBaseClientAction
	{
		// Token: 0x0600C940 RID: 51520 RVA: 0x004995F8 File Offset: 0x004977F8
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				Quest quest = QuestClass.CreateQuest(this.QuestID);
				entityPlayer.QuestJournal.AddQuest(quest, Quest.QuestSource.ChallengeSystem, this.Notify);
			}
		}

		// Token: 0x0600C941 RID: 51521 RVA: 0x0049962E File Offset: 0x0049782E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddQuest.PropQuestID, ref this.QuestID);
			properties.ParseBool(ActionAddQuest.PropNotify, ref this.Notify);
		}

		// Token: 0x0600C942 RID: 51522 RVA: 0x00499659 File Offset: 0x00497859
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

		// Token: 0x04009778 RID: 38776
		public string QuestID;

		// Token: 0x04009779 RID: 38777
		public bool Notify = true;

		// Token: 0x0400977A RID: 38778
		public static string PropQuestID = "quest";

		// Token: 0x0400977B RID: 38779
		public static string PropNotify = "notify";
	}
}
