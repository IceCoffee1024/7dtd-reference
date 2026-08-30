using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BC RID: 6588
	[Preserve]
	public class ActionAddPlayerLevel : ActionBaseClientAction
	{
		// Token: 0x0600C97F RID: 51583 RVA: 0x0049F4E4 File Offset: 0x0049D6E4
		public override void OnClientPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				int intValue = GameEventManager.GetIntValue(entityPlayer, this.addedLevelsText, 1);
				for (int i = 0; i < intValue; i++)
				{
					entityPlayer.Progression.AddLevelExp(entityPlayer.Progression.ExpToNextLevel, "_xpOther", Progression.XPTypes.Other, false, i == intValue - 1, -1, null);
				}
			}
		}

		// Token: 0x0600C980 RID: 51584 RVA: 0x0049F53B File Offset: 0x0049D73B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPlayerLevel.PropNewLevel, ref this.addedLevelsText);
		}

		// Token: 0x0600C981 RID: 51585 RVA: 0x0049F555 File Offset: 0x0049D755
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddPlayerLevel
			{
				addedLevelsText = this.addedLevelsText
			};
		}

		// Token: 0x040098D0 RID: 39120
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addedLevelsText;

		// Token: 0x040098D1 RID: 39121
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewLevel = "levels";
	}
}
