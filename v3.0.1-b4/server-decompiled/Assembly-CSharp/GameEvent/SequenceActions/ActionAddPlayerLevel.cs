using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001996 RID: 6550
	[Preserve]
	public class ActionAddPlayerLevel : ActionBaseClientAction
	{
		// Token: 0x0600C7F8 RID: 51192 RVA: 0x0049918C File Offset: 0x0049738C
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

		// Token: 0x0600C7F9 RID: 51193 RVA: 0x004991E3 File Offset: 0x004973E3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPlayerLevel.PropNewLevel, ref this.addedLevelsText);
		}

		// Token: 0x0600C7FA RID: 51194 RVA: 0x004991FD File Offset: 0x004973FD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddPlayerLevel
			{
				addedLevelsText = this.addedLevelsText
			};
		}

		// Token: 0x040097A7 RID: 38823
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addedLevelsText;

		// Token: 0x040097A8 RID: 38824
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewLevel = "levels";
	}
}
