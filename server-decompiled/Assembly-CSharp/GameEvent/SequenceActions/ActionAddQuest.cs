using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200193A RID: 6458
	[Preserve]
	public class ActionAddQuest : ActionBaseClientAction
	{
		// Token: 0x0600C661 RID: 50785 RVA: 0x0048EDC0 File Offset: 0x0048CFC0
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				Quest quest = QuestClass.CreateQuest(this.QuestID);
				entityPlayer.QuestJournal.AddQuest(quest, this.Notify);
			}
		}

		// Token: 0x0600C662 RID: 50786 RVA: 0x0048EDF5 File Offset: 0x0048CFF5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddQuest.PropQuestID, ref this.QuestID);
			properties.ParseBool(ActionAddQuest.PropNotify, ref this.Notify);
		}

		// Token: 0x0600C663 RID: 50787 RVA: 0x0048EE20 File Offset: 0x0048D020
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

		// Token: 0x04009589 RID: 38281
		public string QuestID;

		// Token: 0x0400958A RID: 38282
		public bool Notify = true;

		// Token: 0x0400958B RID: 38283
		public static string PropQuestID = "quest";

		// Token: 0x0400958C RID: 38284
		public static string PropNotify = "notify";
	}
}
