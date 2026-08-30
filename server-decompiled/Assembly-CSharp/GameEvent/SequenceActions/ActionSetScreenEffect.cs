using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BD RID: 6589
	[Preserve]
	public class ActionSetScreenEffect : ActionBaseClientAction
	{
		// Token: 0x0600C984 RID: 51588 RVA: 0x0049F574 File Offset: 0x0049D774
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.ScreenEffectManager.SetScreenEffect(this.screenEffect, GameEventManager.GetFloatValue(entityPlayerLocal, this.intensityText, 0f), GameEventManager.GetFloatValue(entityPlayerLocal, this.fadeTimeText, 0f));
			}
		}

		// Token: 0x0600C985 RID: 51589 RVA: 0x0049F5BE File Offset: 0x0049D7BE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetScreenEffect.PropScreenEffect, ref this.screenEffect);
			properties.ParseString(ActionSetScreenEffect.PropIntensity, ref this.intensityText);
			properties.ParseString(ActionSetScreenEffect.PropFadeTime, ref this.fadeTimeText);
		}

		// Token: 0x0600C986 RID: 51590 RVA: 0x0049F5FA File Offset: 0x0049D7FA
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

		// Token: 0x040098D2 RID: 39122
		[PublicizedFrom(EAccessModifier.Protected)]
		public string screenEffect = "";

		// Token: 0x040098D3 RID: 39123
		[PublicizedFrom(EAccessModifier.Protected)]
		public string intensityText;

		// Token: 0x040098D4 RID: 39124
		[PublicizedFrom(EAccessModifier.Protected)]
		public string fadeTimeText;

		// Token: 0x040098D5 RID: 39125
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropScreenEffect = "screen_effect";

		// Token: 0x040098D6 RID: 39126
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIntensity = "intensity";

		// Token: 0x040098D7 RID: 39127
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFadeTime = "fade_time";
	}
}
