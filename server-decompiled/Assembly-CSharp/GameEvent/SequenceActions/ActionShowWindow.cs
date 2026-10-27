using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1F RID: 6687
	[Preserve]
	public class ActionShowWindow : ActionBaseClientAction
	{
		// Token: 0x0600CC09 RID: 52233 RVA: 0x004A79EC File Offset: 0x004A5BEC
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.PlayerUI.windowManager.Open(this.window, true);
			}
		}

		// Token: 0x0600CC0A RID: 52234 RVA: 0x004A7A1A File Offset: 0x004A5C1A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionShowWindow.PropWindow, ref this.window);
		}

		// Token: 0x0600CC0B RID: 52235 RVA: 0x004A7A34 File Offset: 0x004A5C34
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionShowWindow
			{
				targetGroup = this.targetGroup,
				window = this.window
			};
		}

		// Token: 0x04009A99 RID: 39577
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropWindow = "window";

		// Token: 0x04009A9A RID: 39578
		[PublicizedFrom(EAccessModifier.Private)]
		public string window = "";
	}
}
