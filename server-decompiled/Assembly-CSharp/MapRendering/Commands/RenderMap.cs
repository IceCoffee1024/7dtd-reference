using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace MapRendering.Commands
{
	// Token: 0x02001AB6 RID: 6838
	[Preserve]
	public class RenderMap : ConsoleCmdAbstract
	{
		// Token: 0x0600CE40 RID: 52800 RVA: 0x004AF4A6 File Offset: 0x004AD6A6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "render the current map to a file";
		}

		// Token: 0x0600CE41 RID: 52801 RVA: 0x004AF4AD File Offset: 0x004AD6AD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "rendermap" };
		}

		// Token: 0x0600CE42 RID: 52802 RVA: 0x004AF4BD File Offset: 0x004AD6BD
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
