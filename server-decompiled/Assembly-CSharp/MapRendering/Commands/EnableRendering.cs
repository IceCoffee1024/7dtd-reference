using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace MapRendering.Commands
{
	// Token: 0x02001AE1 RID: 6881
	[Preserve]
	public class EnableRendering : ConsoleCmdAbstract
	{
		// Token: 0x0600CF93 RID: 53139 RVA: 0x004B38BC File Offset: 0x004B1ABC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Disable live map rendering";
		}

		// Token: 0x0600CF94 RID: 53140 RVA: 0x004B38C3 File Offset: 0x004B1AC3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "\n\t\t\t|Usage:\n\t\t\t|  1. enablerendering\n\t\t\t|  2. enablerendering <0/1>\n\t\t\t|1. Show current state of renderer\n\t\t\t|2. Disable/enable renderer\n\t\t\t|NOTE: This command can only turn the renderer off, it can not turn it on if it is not enabled in the serverconfig!\n\t\t\t".Unindent(true);
		}

		// Token: 0x0600CF95 RID: 53141 RVA: 0x004B38D0 File Offset: 0x004B1AD0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "enablerendering" };
		}

		// Token: 0x0600CF96 RID: 53142 RVA: 0x004B38E0 File Offset: 0x004B1AE0
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
