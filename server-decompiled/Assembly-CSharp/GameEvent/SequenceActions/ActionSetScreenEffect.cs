using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E9 RID: 6633
	[Preserve]
	public class ActionSetScreenEffect : ActionBaseClientAction
	{
		// Token: 0x0600CADC RID: 51932 RVA: 0x004A3A60 File Offset: 0x004A1C60
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.ScreenEffectManager.SetScreenEffect(this.screenEffect, GameEventManager.GetFloatValue(entityPlayerLocal, this.intensityText, 0f), GameEventManager.GetFloatValue(entityPlayerLocal, this.fadeTimeText, 0f));
			}
		}

		// Token: 0x0600CADD RID: 51933 RVA: 0x004A3AAA File Offset: 0x004A1CAA
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetScreenEffect.PropScreenEffect, ref this.screenEffect);
			properties.ParseString(ActionSetScreenEffect.PropIntensity, ref this.intensityText);
			properties.ParseString(ActionSetScreenEffect.PropFadeTime, ref this.fadeTimeText);
		}

		// Token: 0x0600CADE RID: 51934 RVA: 0x004A3AE6 File Offset: 0x004A1CE6
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

		// Token: 0x04009998 RID: 39320
		[PublicizedFrom(EAccessModifier.Protected)]
		public string screenEffect = "";

		// Token: 0x04009999 RID: 39321
		[PublicizedFrom(EAccessModifier.Protected)]
		public string intensityText;

		// Token: 0x0400999A RID: 39322
		[PublicizedFrom(EAccessModifier.Protected)]
		public string fadeTimeText;

		// Token: 0x0400999B RID: 39323
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropScreenEffect = "screen_effect";

		// Token: 0x0400999C RID: 39324
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIntensity = "intensity";

		// Token: 0x0400999D RID: 39325
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFadeTime = "fade_time";
	}
}
