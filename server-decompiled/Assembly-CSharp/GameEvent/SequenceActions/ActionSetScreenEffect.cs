using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1A RID: 6682
	[Preserve]
	public class ActionSetScreenEffect : ActionBaseClientAction
	{
		// Token: 0x0600CBF0 RID: 52208 RVA: 0x004A7560 File Offset: 0x004A5760
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.ScreenEffectManager.SetScreenEffect(this.screenEffect, GameEventManager.GetFloatValue(entityPlayerLocal, this.intensityText, 0f), GameEventManager.GetFloatValue(entityPlayerLocal, this.fadeTimeText, 0f));
			}
		}

		// Token: 0x0600CBF1 RID: 52209 RVA: 0x004A75AA File Offset: 0x004A57AA
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetScreenEffect.PropScreenEffect, ref this.screenEffect);
			properties.ParseString(ActionSetScreenEffect.PropIntensity, ref this.intensityText);
			properties.ParseString(ActionSetScreenEffect.PropFadeTime, ref this.fadeTimeText);
		}

		// Token: 0x0600CBF2 RID: 52210 RVA: 0x004A75E6 File Offset: 0x004A57E6
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

		// Token: 0x04009A81 RID: 39553
		[PublicizedFrom(EAccessModifier.Protected)]
		public string screenEffect = "";

		// Token: 0x04009A82 RID: 39554
		[PublicizedFrom(EAccessModifier.Protected)]
		public string intensityText;

		// Token: 0x04009A83 RID: 39555
		[PublicizedFrom(EAccessModifier.Protected)]
		public string fadeTimeText;

		// Token: 0x04009A84 RID: 39556
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropScreenEffect = "screen_effect";

		// Token: 0x04009A85 RID: 39557
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIntensity = "intensity";

		// Token: 0x04009A86 RID: 39558
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFadeTime = "fade_time";
	}
}
