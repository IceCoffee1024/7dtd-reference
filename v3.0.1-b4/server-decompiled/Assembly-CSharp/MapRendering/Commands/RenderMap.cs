using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace MapRendering.Commands
{
	// Token: 0x02001A8F RID: 6799
	[Preserve]
	public class RenderMap : ConsoleCmdAbstract
	{
		// Token: 0x0600CCB7 RID: 52407 RVA: 0x004A90B2 File Offset: 0x004A72B2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "render the current map to a file";
		}

		// Token: 0x0600CCB8 RID: 52408 RVA: 0x004A90B9 File Offset: 0x004A72B9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "rendermap" };
		}

		// Token: 0x0600CCB9 RID: 52409 RVA: 0x004A90C9 File Offset: 0x004A72C9
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
