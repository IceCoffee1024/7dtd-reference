using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C2 RID: 6594
	[Preserve]
	public class ActionShowWindow : ActionBaseClientAction
	{
		// Token: 0x0600C99D RID: 51613 RVA: 0x0049FA00 File Offset: 0x0049DC00
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.PlayerUI.windowManager.Open(this.window, true);
			}
		}

		// Token: 0x0600C99E RID: 51614 RVA: 0x0049FA2E File Offset: 0x0049DC2E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionShowWindow.PropWindow, ref this.window);
		}

		// Token: 0x0600C99F RID: 51615 RVA: 0x0049FA48 File Offset: 0x0049DC48
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionShowWindow
			{
				targetGroup = this.targetGroup,
				window = this.window
			};
		}

		// Token: 0x040098EA RID: 39146
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropWindow = "window";

		// Token: 0x040098EB RID: 39147
		[PublicizedFrom(EAccessModifier.Private)]
		public string window = "";
	}
}
