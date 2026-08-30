using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001971 RID: 6513
	[Preserve]
	public class ActionBeltTooltip : ActionBaseClientAction
	{
		// Token: 0x0600C839 RID: 51257 RVA: 0x00497090 File Offset: 0x00495290
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

		// Token: 0x0600C83A RID: 51258 RVA: 0x004970EC File Offset: 0x004952EC
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

		// Token: 0x0600C83B RID: 51259 RVA: 0x00497174 File Offset: 0x00495374
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

		// Token: 0x04009721 RID: 38689
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "Sequence Complete";

		// Token: 0x04009722 RID: 38690
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x04009723 RID: 38691
		[PublicizedFrom(EAccessModifier.Protected)]
		public string soundName = "";

		// Token: 0x04009724 RID: 38692
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x04009725 RID: 38693
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";

		// Token: 0x04009726 RID: 38694
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSound = "sound";
	}
}
