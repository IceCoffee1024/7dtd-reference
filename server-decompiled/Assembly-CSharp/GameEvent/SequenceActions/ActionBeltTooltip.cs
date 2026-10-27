using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CE RID: 6606
	[Preserve]
	public class ActionBeltTooltip : ActionBaseClientAction
	{
		// Token: 0x0600CAA5 RID: 51877 RVA: 0x0049F06C File Offset: 0x0049D26C
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

		// Token: 0x0600CAA6 RID: 51878 RVA: 0x0049F0C8 File Offset: 0x0049D2C8
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

		// Token: 0x0600CAA7 RID: 51879 RVA: 0x0049F150 File Offset: 0x0049D350
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

		// Token: 0x040098D0 RID: 39120
		[PublicizedFrom(EAccessModifier.Protected)]
		public string textKey = "Sequence Complete";

		// Token: 0x040098D1 RID: 39121
		[PublicizedFrom(EAccessModifier.Protected)]
		public string text = "";

		// Token: 0x040098D2 RID: 39122
		[PublicizedFrom(EAccessModifier.Protected)]
		public string soundName = "";

		// Token: 0x040098D3 RID: 39123
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropText = "text";

		// Token: 0x040098D4 RID: 39124
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTextKey = "text_key";

		// Token: 0x040098D5 RID: 39125
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSound = "sound";
	}
}
