using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199D RID: 6557
	[Preserve]
	public class ActionBeltTooltip : ActionBaseClientAction
	{
		// Token: 0x0600C991 RID: 51601 RVA: 0x0049B56C File Offset: 0x0049976C
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

		// Token: 0x0600C992 RID: 51602 RVA: 0x0049B5C8 File Offset: 0x004997C8
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

		// Token: 0x0600C993 RID: 51603 RVA: 0x0049B650 File Offset: 0x00499850
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

		// Token: 0x040097E7 RID: 38887
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "Sequence Complete";

		// Token: 0x040097E8 RID: 38888
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x040097E9 RID: 38889
		[PublicizedFrom(EAccessModifier.Protected)]
		public string soundName = "";

		// Token: 0x040097EA RID: 38890
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x040097EB RID: 38891
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";

		// Token: 0x040097EC RID: 38892
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSound = "sound";
	}
}
