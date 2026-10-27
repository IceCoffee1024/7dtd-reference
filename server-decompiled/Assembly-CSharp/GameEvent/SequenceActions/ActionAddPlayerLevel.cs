using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A19 RID: 6681
	[Preserve]
	public class ActionAddPlayerLevel : ActionBaseClientAction
	{
		// Token: 0x0600CBEB RID: 52203 RVA: 0x004A74D0 File Offset: 0x004A56D0
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

		// Token: 0x0600CBEC RID: 52204 RVA: 0x004A7527 File Offset: 0x004A5727
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPlayerLevel.PropNewLevel, ref this.addedLevelsText);
		}

		// Token: 0x0600CBED RID: 52205 RVA: 0x004A7541 File Offset: 0x004A5741
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddPlayerLevel
			{
				addedLevelsText = this.addedLevelsText
			};
		}

		// Token: 0x04009A7F RID: 39551
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addedLevelsText;

		// Token: 0x04009A80 RID: 39552
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewLevel = "levels";
	}
}
