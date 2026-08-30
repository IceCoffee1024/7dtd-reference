using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001940 RID: 6464
	[Preserve]
	public class RequirementOnQuest : BaseRequirement
	{
		// Token: 0x0600C742 RID: 51010 RVA: 0x00491A68 File Offset: 0x0048FC68
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

		// Token: 0x0600C743 RID: 51011 RVA: 0x00491AD7 File Offset: 0x0048FCD7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementOnQuest.PropQuest, ref this.QuestID);
		}

		// Token: 0x0600C744 RID: 51012 RVA: 0x00491AF1 File Offset: 0x0048FCF1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementOnQuest
			{
				Invert = this.Invert,
				QuestID = this.QuestID
			};
		}

		// Token: 0x04009604 RID: 38404
		[PublicizedFrom(EAccessModifier.Private)]
		public string QuestID = "";

		// Token: 0x04009605 RID: 38405
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropQuest = "quest";
	}
}
