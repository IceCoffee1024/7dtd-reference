using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BD RID: 6589
	[Preserve]
	public class ActionAddQuest : ActionBaseClientAction
	{
		// Token: 0x0600CA54 RID: 51796 RVA: 0x0049CFEC File Offset: 0x0049B1EC
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				Quest quest = QuestClass.CreateQuest(this.QuestID);
				entityPlayer.QuestJournal.AddQuest(quest, Quest.QuestSource.ChallengeSystem, this.Notify);
			}
		}

		// Token: 0x0600CA55 RID: 51797 RVA: 0x0049D022 File Offset: 0x0049B222
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddQuest.PropQuestID, ref this.QuestID);
			properties.ParseBool(ActionAddQuest.PropNotify, ref this.Notify);
		}

		// Token: 0x0600CA56 RID: 51798 RVA: 0x0049D04D File Offset: 0x0049B24D
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

		// Token: 0x04009861 RID: 39009
		public string QuestID;

		// Token: 0x04009862 RID: 39010
		public bool Notify = true;

		// Token: 0x04009863 RID: 39011
		public static string PropQuestID = "quest";

		// Token: 0x04009864 RID: 39012
		public static string PropNotify = "notify";
	}
}
