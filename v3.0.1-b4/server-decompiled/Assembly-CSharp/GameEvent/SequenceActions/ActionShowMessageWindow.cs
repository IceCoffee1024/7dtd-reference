using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199B RID: 6555
	[Preserve]
	public class ActionShowMessageWindow : ActionBaseClientAction
	{
		// Token: 0x0600C811 RID: 51217 RVA: 0x004995DF File Offset: 0x004977DF
		public override void OnClientPerform(Entity target)
		{
			if (target is EntityPlayerLocal)
			{
				XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
			}
		}

		// Token: 0x0600C812 RID: 51218 RVA: 0x0049961B File Offset: 0x0049781B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionShowMessageWindow.PropMessage, ref this.message);
			properties.ParseString(ActionShowMessageWindow.PropTitle, ref this.title);
		}

		// Token: 0x0600C813 RID: 51219 RVA: 0x00499646 File Offset: 0x00497846
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

		// Token: 0x040097BD RID: 38845
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropMessage = "message";

		// Token: 0x040097BE RID: 38846
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropTitle = "title";

		// Token: 0x040097BF RID: 38847
		[PublicizedFrom(EAccessModifier.Private)]
		public string message = "";

		// Token: 0x040097C0 RID: 38848
		[PublicizedFrom(EAccessModifier.Private)]
		public string title = "";
	}
}
