using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace MapRendering.Commands
{
	// Token: 0x02001A8E RID: 6798
	[Preserve]
	public class EnableRendering : ConsoleCmdAbstract
	{
		// Token: 0x0600CCB2 RID: 52402 RVA: 0x004A8FFC File Offset: 0x004A71FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Disable live map rendering";
		}

		// Token: 0x0600CCB3 RID: 52403 RVA: 0x004A9003 File Offset: 0x004A7203
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "\n\t\t\t|Usage:\n\t\t\t|  1. enablerendering\n\t\t\t|  2. enablerendering <0/1>\n\t\t\t|1. Show current state of renderer\n\t\t\t|2. Disable/enable renderer\n\t\t\t|NOTE: This command can only turn the renderer off, it can not turn it on if it is not enabled in the serverconfig!\n\t\t\t".Unindent(true);
		}

		// Token: 0x0600CCB4 RID: 52404 RVA: 0x004A9010 File Offset: 0x004A7210
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "enablerendering" };
		}

		// Token: 0x0600CCB5 RID: 52405 RVA: 0x004A9020 File Offset: 0x004A7220
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
