using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001974 RID: 6516
	[Preserve]
	public class ActionCloseWindow : ActionBaseClientAction
	{
		// Token: 0x0600C848 RID: 51272 RVA: 0x00497444 File Offset: 0x00495644
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

		// Token: 0x0600C849 RID: 51273 RVA: 0x00497497 File Offset: 0x00495697
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionCloseWindow.PropWindow, ref this.windowName);
		}

		// Token: 0x0600C84A RID: 51274 RVA: 0x004974B1 File Offset: 0x004956B1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCloseWindow
			{
				targetGroup = this.targetGroup,
				windowName = this.windowName
			};
		}

		// Token: 0x0400972D RID: 38701
		[PublicizedFrom(EAccessModifier.Protected)]
		public string windowName = "";

		// Token: 0x0400972E RID: 38702
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropWindow = "window";
	}
}
