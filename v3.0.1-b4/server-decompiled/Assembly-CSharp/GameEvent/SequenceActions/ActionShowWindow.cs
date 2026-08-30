using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199C RID: 6556
	[Preserve]
	public class ActionShowWindow : ActionBaseClientAction
	{
		// Token: 0x0600C816 RID: 51222 RVA: 0x004996A8 File Offset: 0x004978A8
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.PlayerUI.windowManager.Open(this.window, true);
			}
		}

		// Token: 0x0600C817 RID: 51223 RVA: 0x004996D6 File Offset: 0x004978D6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionShowWindow.PropWindow, ref this.window);
		}

		// Token: 0x0600C818 RID: 51224 RVA: 0x004996F0 File Offset: 0x004978F0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionShowWindow
			{
				targetGroup = this.targetGroup,
				window = this.window
			};
		}

		// Token: 0x040097C1 RID: 38849
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropWindow = "window";

		// Token: 0x040097C2 RID: 38850
		[PublicizedFrom(EAccessModifier.Private)]
		public string window = "";
	}
}
