using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001997 RID: 6551
	[Preserve]
	public class ActionSetScreenEffect : ActionBaseClientAction
	{
		// Token: 0x0600C7FD RID: 51197 RVA: 0x0049921C File Offset: 0x0049741C
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.ScreenEffectManager.SetScreenEffect(this.screenEffect, GameEventManager.GetFloatValue(entityPlayerLocal, this.intensityText, 0f), GameEventManager.GetFloatValue(entityPlayerLocal, this.fadeTimeText, 0f));
			}
		}

		// Token: 0x0600C7FE RID: 51198 RVA: 0x00499266 File Offset: 0x00497466
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetScreenEffect.PropScreenEffect, ref this.screenEffect);
			properties.ParseString(ActionSetScreenEffect.PropIntensity, ref this.intensityText);
			properties.ParseString(ActionSetScreenEffect.PropFadeTime, ref this.fadeTimeText);
		}

		// Token: 0x0600C7FF RID: 51199 RVA: 0x004992A2 File Offset: 0x004974A2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetScreenEffect
			{
				screenEffect = this.screenEffect,
				intensityText = this.intensityText,
				fadeTimeText = this.fadeTimeText,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040097A9 RID: 38825
		[PublicizedFrom(EAccessModifier.Protected)]
		public string screenEffect = "";

		// Token: 0x040097AA RID: 38826
		[PublicizedFrom(EAccessModifier.Protected)]
		public string intensityText;

		// Token: 0x040097AB RID: 38827
		[PublicizedFrom(EAccessModifier.Protected)]
		public string fadeTimeText;

		// Token: 0x040097AC RID: 38828
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropScreenEffect = "screen_effect";

		// Token: 0x040097AD RID: 38829
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIntensity = "intensity";

		// Token: 0x040097AE RID: 38830
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFadeTime = "fade_time";
	}
}
