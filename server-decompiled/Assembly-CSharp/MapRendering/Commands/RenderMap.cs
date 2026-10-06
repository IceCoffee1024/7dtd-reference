using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace MapRendering.Commands
{
	// Token: 0x02001AE2 RID: 6882
	[Preserve]
	public class RenderMap : ConsoleCmdAbstract
	{
		// Token: 0x0600CF98 RID: 53144 RVA: 0x004B3972 File Offset: 0x004B1B72
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "render the current map to a file";
		}

		// Token: 0x0600CF99 RID: 53145 RVA: 0x004B3979 File Offset: 0x004B1B79
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "rendermap" };
		}

		// Token: 0x0600CF9A RID: 53146 RVA: 0x004B3989 File Offset: 0x004B1B89
		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (!MapRenderer.HasInstance)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Renderer not enabled");
				return;
			}
			MapRenderer.Instance.RenderFullMap();
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Render map done");
		}
	}
}
