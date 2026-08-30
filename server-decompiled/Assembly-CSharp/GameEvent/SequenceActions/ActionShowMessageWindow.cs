using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C1 RID: 6593
	[Preserve]
	public class ActionShowMessageWindow : ActionBaseClientAction
	{
		// Token: 0x0600C998 RID: 51608 RVA: 0x0049F937 File Offset: 0x0049DB37
		public override void OnClientPerform(Entity target)
		{
			if (target is EntityPlayerLocal)
			{
				XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
			}
		}

		// Token: 0x0600C999 RID: 51609 RVA: 0x0049F973 File Offset: 0x0049DB73
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionShowMessageWindow.PropMessage, ref this.message);
			properties.ParseString(ActionShowMessageWindow.PropTitle, ref this.title);
		}

		// Token: 0x0600C99A RID: 51610 RVA: 0x0049F99E File Offset: 0x0049DB9E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionShowMessageWindow
			{
				targetGroup = this.targetGroup,
				message = this.message,
				title = this.title
			};
		}

		// Token: 0x040098E6 RID: 39142
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropMessage = "message";

		// Token: 0x040098E7 RID: 39143
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropTitle = "title";

		// Token: 0x040098E8 RID: 39144
		[PublicizedFrom(EAccessModifier.Private)]
		public string message = "";

		// Token: 0x040098E9 RID: 39145
		[PublicizedFrom(EAccessModifier.Private)]
		public string title = "";
	}
}
