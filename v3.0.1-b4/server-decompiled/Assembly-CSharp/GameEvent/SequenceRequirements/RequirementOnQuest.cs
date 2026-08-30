using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200191A RID: 6426
	[Preserve]
	public class RequirementOnQuest : BaseRequirement
	{
		// Token: 0x0600C5BB RID: 50619 RVA: 0x0048B71C File Offset: 0x0048991C
		public override bool CanPerform(Entity target)
		{
			bool flag = false;
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer == null)
			{
				return false;
			}
			if (this.QuestID == "")
			{
				if (entityPlayer.QuestJournal.ActiveQuest != null || entityPlayer.QuestJournal.FindActiveQuest() != null)
				{
					flag = true;
				}
			}
			else if (entityPlayer.QuestJournal.FindActiveQuest(this.QuestID, -1) != null)
			{
				flag = true;
			}
			if (!this.Invert)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x0600C5BC RID: 50620 RVA: 0x0048B78B File Offset: 0x0048998B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementOnQuest.PropQuest, ref this.QuestID);
		}

		// Token: 0x0600C5BD RID: 50621 RVA: 0x0048B7A5 File Offset: 0x004899A5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementOnQuest
			{
				Invert = this.Invert,
				QuestID = this.QuestID
			};
		}

		// Token: 0x040094DB RID: 38107
		[PublicizedFrom(EAccessModifier.Private)]
		public string QuestID = "";

		// Token: 0x040094DC RID: 38108
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropQuest = "quest";
	}
}
