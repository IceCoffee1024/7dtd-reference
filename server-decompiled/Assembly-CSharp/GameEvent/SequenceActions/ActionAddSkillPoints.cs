using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001962 RID: 6498
	[Preserve]
	public class ActionAddSkillPoints : ActionBaseClientAction
	{
		// Token: 0x0600C7F2 RID: 51186 RVA: 0x00495310 File Offset: 0x00493510
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				int intValue = GameEventManager.GetIntValue(entityPlayer, this.skillPointsText, 1);
				if (intValue <= 0)
				{
					return;
				}
				entityPlayer.Progression.SkillPoints += intValue;
			}
		}

		// Token: 0x0600C7F3 RID: 51187 RVA: 0x0049534D File Offset: 0x0049354D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddSkillPoints.PropSkillPoints, ref this.skillPointsText);
		}

		// Token: 0x0600C7F4 RID: 51188 RVA: 0x00495367 File Offset: 0x00493567
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddSkillPoints
			{
				skillPointsText = this.skillPointsText
			};
		}

		// Token: 0x040096BC RID: 38588
		[PublicizedFrom(EAccessModifier.Protected)]
		public string skillPointsText = "";

		// Token: 0x040096BD RID: 38589
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillPoints = "skill_points";
	}
}
