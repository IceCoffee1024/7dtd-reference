using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200199D RID: 6557
	[Preserve]
	public class RequirementOnQuest : BaseRequirement
	{
		// Token: 0x0600C9AE RID: 51630 RVA: 0x00499948 File Offset: 0x00497B48
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

		// Token: 0x0600C9AF RID: 51631 RVA: 0x004999B7 File Offset: 0x00497BB7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementOnQuest.PropQuest, ref this.QuestID);
		}

		// Token: 0x0600C9B0 RID: 51632 RVA: 0x004999D1 File Offset: 0x00497BD1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementOnQuest
			{
				Invert = this.Invert,
				QuestID = this.QuestID
			};
		}

		// Token: 0x040097B3 RID: 38835
		[PublicizedFrom(EAccessModifier.Private)]
		public string QuestID = "";

		// Token: 0x040097B4 RID: 38836
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropQuest = "quest";
	}
}
