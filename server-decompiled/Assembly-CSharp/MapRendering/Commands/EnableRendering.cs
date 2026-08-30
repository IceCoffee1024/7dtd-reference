using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace MapRendering.Commands
{
	// Token: 0x02001AB5 RID: 6837
	[Preserve]
	public class EnableRendering : ConsoleCmdAbstract
	{
		// Token: 0x0600CE3B RID: 52795 RVA: 0x004AF3F0 File Offset: 0x004AD5F0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "Disable live map rendering";
		}

		// Token: 0x0600CE3C RID: 52796 RVA: 0x004AF3F7 File Offset: 0x004AD5F7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getHelp()
		{
			return "\n\t\t\t|Usage:\n\t\t\t|  1. enablerendering\n\t\t\t|  2. enablerendering <0/1>\n\t\t\t|1. Show current state of renderer\n\t\t\t|2. Disable/enable renderer\n\t\t\t|NOTE: This command can only turn the renderer off, it can not turn it on if it is not enabled in the serverconfig!\n\t\t\t".Unindent(true);
		}

		// Token: 0x0600CE3D RID: 52797 RVA: 0x004AF404 File Offset: 0x004AD604
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "enablerendering" };
		}

		// Token: 0x0600CE3E RID: 52798 RVA: 0x004AF414 File Offset: 0x004AD614
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
