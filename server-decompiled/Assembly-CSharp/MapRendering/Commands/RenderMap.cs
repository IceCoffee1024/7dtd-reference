using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace MapRendering.Commands
{
	// Token: 0x02001B13 RID: 6931
	[Preserve]
	public class RenderMap : ConsoleCmdAbstract
	{
		// Token: 0x0600D0AC RID: 53420 RVA: 0x004B746E File Offset: 0x004B566E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "render the current map to a file";
		}

		// Token: 0x0600D0AD RID: 53421 RVA: 0x004B7475 File Offset: 0x004B5675
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "rendermap" };
		}

		// Token: 0x0600D0AE RID: 53422 RVA: 0x004B7485 File Offset: 0x004B5685
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
