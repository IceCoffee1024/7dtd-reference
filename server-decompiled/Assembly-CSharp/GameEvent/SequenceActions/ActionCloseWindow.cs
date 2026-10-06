using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A0 RID: 6560
	[Preserve]
	public class ActionCloseWindow : ActionBaseClientAction
	{
		// Token: 0x0600C9A0 RID: 51616 RVA: 0x0049B920 File Offset: 0x00499B20
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				if (this.windowName == "")
				{
					entityPlayerLocal.PlayerUI.windowManager.CloseAllOpenModalWindows(null, false);
					return;
				}
				entityPlayerLocal.PlayerUI.windowManager.Close(this.windowName);
			}
		}

		// Token: 0x0600C9A1 RID: 51617 RVA: 0x0049B973 File Offset: 0x00499B73
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionCloseWindow.PropWindow, ref this.windowName);
		}

		// Token: 0x0600C9A2 RID: 51618 RVA: 0x0049B98D File Offset: 0x00499B8D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCloseWindow
			{
				targetGroup = this.targetGroup,
				windowName = this.windowName
			};
		}

		// Token: 0x040097F3 RID: 38899
		[PublicizedFrom(EAccessModifier.Protected)]
		public string windowName = "";

		// Token: 0x040097F4 RID: 38900
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropWindow = "window";
	}
}
