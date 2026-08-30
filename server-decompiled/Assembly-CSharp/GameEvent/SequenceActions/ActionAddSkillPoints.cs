using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200193C RID: 6460
	[Preserve]
	public class ActionAddSkillPoints : ActionBaseClientAction
	{
		// Token: 0x0600C66B RID: 50795 RVA: 0x0048EFC4 File Offset: 0x0048D1C4
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

		// Token: 0x0600C66C RID: 50796 RVA: 0x0048F001 File Offset: 0x0048D201
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddSkillPoints.PropSkillPoints, ref this.skillPointsText);
		}

		// Token: 0x0600C66D RID: 50797 RVA: 0x0048F01B File Offset: 0x0048D21B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddSkillPoints
			{
				skillPointsText = this.skillPointsText
			};
		}

		// Token: 0x04009593 RID: 38291
		[PublicizedFrom(EAccessModifier.Protected)]
		public string skillPointsText = "";

		// Token: 0x04009594 RID: 38292
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillPoints = "skill_points";
	}
}
