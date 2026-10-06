using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019ED RID: 6637
	[Preserve]
	public class ActionShowMessageWindow : ActionBaseClientAction
	{
		// Token: 0x0600CAF0 RID: 51952 RVA: 0x004A3E23 File Offset: 0x004A2023
		public override void OnClientPerform(Entity target)
		{
			if (target is EntityPlayerLocal)
			{
				XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
			}
		}

		// Token: 0x0600CAF1 RID: 51953 RVA: 0x004A3E5F File Offset: 0x004A205F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionShowMessageWindow.PropMessage, ref this.message);
			properties.ParseString(ActionShowMessageWindow.PropTitle, ref this.title);
		}

		// Token: 0x0600CAF2 RID: 51954 RVA: 0x004A3E8A File Offset: 0x004A208A
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

		// Token: 0x040099AC RID: 39340
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropMessage = "message";

		// Token: 0x040099AD RID: 39341
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropTitle = "title";

		// Token: 0x040099AE RID: 39342
		[PublicizedFrom(EAccessModifier.Private)]
		public string message = "";

		// Token: 0x040099AF RID: 39343
		[PublicizedFrom(EAccessModifier.Private)]
		public string title = "";
	}
}
