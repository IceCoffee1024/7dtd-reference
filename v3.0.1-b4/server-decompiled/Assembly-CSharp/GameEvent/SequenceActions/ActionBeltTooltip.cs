using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200194B RID: 6475
	[Preserve]
	public class ActionBeltTooltip : ActionBaseClientAction
	{
		// Token: 0x0600C6B2 RID: 50866 RVA: 0x00490D44 File Offset: 0x0048EF44
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				if (this.soundName != "")
				{
					GameManager.ShowTooltip(entityPlayerLocal, this.text, false, false, 0f);
					return;
				}
				GameManager.ShowTooltip(entityPlayerLocal, this.text, "", this.soundName, null, false, false, 0f);
			}
		}

		// Token: 0x0600C6B3 RID: 50867 RVA: 0x00490DA0 File Offset: 0x0048EFA0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionBeltTooltip.PropText))
			{
				this.text = properties.Values[ActionBeltTooltip.PropText];
			}
			if (properties.Values.ContainsKey(ActionBeltTooltip.PropTextKey))
			{
				this.textKey = properties.Values[ActionBeltTooltip.PropTextKey];
				this.text = Localization.Get(this.textKey, false, null);
			}
			properties.ParseString(ActionBeltTooltip.PropSound, ref this.soundName);
		}

		// Token: 0x0600C6B4 RID: 50868 RVA: 0x00490E28 File Offset: 0x0048F028
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBeltTooltip
			{
				targetGroup = this.targetGroup,
				textKey = this.textKey,
				text = this.text,
				soundName = this.soundName
			};
		}

		// Token: 0x040095F8 RID: 38392
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "Sequence Complete";

		// Token: 0x040095F9 RID: 38393
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x040095FA RID: 38394
		[PublicizedFrom(EAccessModifier.Protected)]
		public string soundName = "";

		// Token: 0x040095FB RID: 38395
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x040095FC RID: 38396
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";

		// Token: 0x040095FD RID: 38397
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSound = "sound";
	}
}
