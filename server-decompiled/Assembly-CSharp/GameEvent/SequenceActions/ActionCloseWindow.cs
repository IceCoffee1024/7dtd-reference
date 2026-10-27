using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D1 RID: 6609
	[Preserve]
	public class ActionCloseWindow : ActionBaseClientAction
	{
		// Token: 0x0600CAB4 RID: 51892 RVA: 0x0049F420 File Offset: 0x0049D620
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

		// Token: 0x0600CAB5 RID: 51893 RVA: 0x0049F473 File Offset: 0x0049D673
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionCloseWindow.PropWindow, ref this.windowName);
		}

		// Token: 0x0600CAB6 RID: 51894 RVA: 0x0049F48D File Offset: 0x0049D68D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCloseWindow
			{
				targetGroup = this.targetGroup,
				windowName = this.windowName
			};
		}

		// Token: 0x040098DC RID: 39132
		[PublicizedFrom(EAccessModifier.Protected)]
		public string windowName = "";

		// Token: 0x040098DD RID: 39133
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropWindow = "window";
	}
}
