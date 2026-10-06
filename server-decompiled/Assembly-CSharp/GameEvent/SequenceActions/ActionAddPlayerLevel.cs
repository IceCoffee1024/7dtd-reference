using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E8 RID: 6632
	[Preserve]
	public class ActionAddPlayerLevel : ActionBaseClientAction
	{
		// Token: 0x0600CAD7 RID: 51927 RVA: 0x004A39D0 File Offset: 0x004A1BD0
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

		// Token: 0x0600CAD8 RID: 51928 RVA: 0x004A3A27 File Offset: 0x004A1C27
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPlayerLevel.PropNewLevel, ref this.addedLevelsText);
		}

		// Token: 0x0600CAD9 RID: 51929 RVA: 0x004A3A41 File Offset: 0x004A1C41
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddPlayerLevel
			{
				addedLevelsText = this.addedLevelsText
			};
		}

		// Token: 0x04009996 RID: 39318
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addedLevelsText;

		// Token: 0x04009997 RID: 39319
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewLevel = "levels";
	}
}
