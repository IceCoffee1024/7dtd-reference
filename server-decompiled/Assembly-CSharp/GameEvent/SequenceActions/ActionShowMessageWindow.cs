using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1E RID: 6686
	[Preserve]
	public class ActionShowMessageWindow : ActionBaseClientAction
	{
		// Token: 0x0600CC04 RID: 52228 RVA: 0x004A7923 File Offset: 0x004A5B23
		public override void OnClientPerform(Entity target)
		{
			if (target is EntityPlayerLocal)
			{
				XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Localization.Get(this.title, false, null), Localization.Get(this.message, false, null), "ui_game_symbol_pen", null, true, true, false);
			}
		}

		// Token: 0x0600CC05 RID: 52229 RVA: 0x004A795F File Offset: 0x004A5B5F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionShowMessageWindow.PropMessage, ref this.message);
			properties.ParseString(ActionShowMessageWindow.PropTitle, ref this.title);
		}

		// Token: 0x0600CC06 RID: 52230 RVA: 0x004A798A File Offset: 0x004A5B8A
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

		// Token: 0x04009A95 RID: 39573
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropMessage = "message";

		// Token: 0x04009A96 RID: 39574
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropTitle = "title";

		// Token: 0x04009A97 RID: 39575
		[PublicizedFrom(EAccessModifier.Private)]
		public string message = "";

		// Token: 0x04009A98 RID: 39576
		[PublicizedFrom(EAccessModifier.Private)]
		public string title = "";
	}
}
