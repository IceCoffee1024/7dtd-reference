using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200196C RID: 6508
	[Preserve]
	public class RequirementOnQuest : BaseRequirement
	{
		// Token: 0x0600C89A RID: 51354 RVA: 0x00495F54 File Offset: 0x00494154
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

		// Token: 0x0600C89B RID: 51355 RVA: 0x00495FC3 File Offset: 0x004941C3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementOnQuest.PropQuest, ref this.QuestID);
		}

		// Token: 0x0600C89C RID: 51356 RVA: 0x00495FDD File Offset: 0x004941DD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementOnQuest
			{
				Invert = this.Invert,
				QuestID = this.QuestID
			};
		}

		// Token: 0x040096CA RID: 38602
		[PublicizedFrom(EAccessModifier.Private)]
		public string QuestID = "";

		// Token: 0x040096CB RID: 38603
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropQuest = "quest";
	}
}
