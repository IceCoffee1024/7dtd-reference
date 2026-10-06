using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EE RID: 6638
	[Preserve]
	public class ActionShowWindow : ActionBaseClientAction
	{
		// Token: 0x0600CAF5 RID: 51957 RVA: 0x004A3EEC File Offset: 0x004A20EC
		public override void OnClientPerform(Entity target)
		{
			EntityPlayerLocal entityPlayerLocal = target as EntityPlayerLocal;
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.PlayerUI.windowManager.Open(this.window, true);
			}
		}

		// Token: 0x0600CAF6 RID: 51958 RVA: 0x004A3F1A File Offset: 0x004A211A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionShowWindow.PropWindow, ref this.window);
		}

		// Token: 0x0600CAF7 RID: 51959 RVA: 0x004A3F34 File Offset: 0x004A2134
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionShowWindow
			{
				targetGroup = this.targetGroup,
				window = this.window
			};
		}

		// Token: 0x040099B0 RID: 39344
		[PublicizedFrom(EAccessModifier.Private)]
		public static string PropWindow = "window";

		// Token: 0x040099B1 RID: 39345
		[PublicizedFrom(EAccessModifier.Private)]
		public string window = "";
	}
}
