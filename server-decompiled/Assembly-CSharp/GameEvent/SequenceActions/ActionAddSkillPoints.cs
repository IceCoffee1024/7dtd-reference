using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198E RID: 6542
	[Preserve]
	public class ActionAddSkillPoints : ActionBaseClientAction
	{
		// Token: 0x0600C94A RID: 51530 RVA: 0x00499800 File Offset: 0x00497A00
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

		// Token: 0x0600C94B RID: 51531 RVA: 0x0049983D File Offset: 0x00497A3D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddSkillPoints.PropSkillPoints, ref this.skillPointsText);
		}

		// Token: 0x0600C94C RID: 51532 RVA: 0x00499857 File Offset: 0x00497A57
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddSkillPoints
			{
				skillPointsText = this.skillPointsText
			};
		}

		// Token: 0x04009782 RID: 38786
		[PublicizedFrom(EAccessModifier.Protected)]
		public string skillPointsText = "";

		// Token: 0x04009783 RID: 38787
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillPoints = "skill_points";
	}
}
