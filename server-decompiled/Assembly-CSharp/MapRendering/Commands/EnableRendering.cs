using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace MapRendering.Commands
{
	// Token: 0x02001B12 RID: 6930
	[Preserve]
	public class EnableRendering : ConsoleCmdAbstract
	{
		// Token: 0x0600D0A7 RID: 53415 RVA: 0x004B73B8 File Offset: 0x004B55B8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Disable live map rendering";
		}

		// Token: 0x0600D0A8 RID: 53416 RVA: 0x004B73BF File Offset: 0x004B55BF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "\r\n\t\t\t|Usage:\r\n\t\t\t|  1. enablerendering\r\n\t\t\t|  2. enablerendering <0/1>\r\n\t\t\t|1. Show current state of renderer\r\n\t\t\t|2. Disable/enable renderer\r\n\t\t\t|NOTE: This command can only turn the renderer off, it can not turn it on if it is not enabled in the serverconfig!\r\n\t\t\t".Unindent(true);
		}

		// Token: 0x0600D0A9 RID: 53417 RVA: 0x004B73CC File Offset: 0x004B55CC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "enablerendering" };
		}

		// Token: 0x0600D0AA RID: 53418 RVA: 0x004B73DC File Offset: 0x004B55DC
		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count != 1)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Current state: {0}{1}", MapRenderer.Enabled && MapRenderer.renderingEnabled, (!MapRenderer.Enabled) ? " (disabled by serverconfig!)" : ""));
				return;
			}
			MapRenderer.renderingEnabled = _params[0].Equals("1");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Set live map rendering to {0}", _params[0].Equals("1")));
		}
	}
}
