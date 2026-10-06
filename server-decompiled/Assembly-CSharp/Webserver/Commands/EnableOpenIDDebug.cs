using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Webserver.Commands
{
	// Token: 0x02001B38 RID: 6968
	[Preserve]
	public class EnableOpenIDDebug : ConsoleCmdAbstract
	{
		// Token: 0x0600D15F RID: 53599 RVA: 0x004BFDE7 File Offset: 0x004BDFE7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "enable/disable OpenID debugging";
		}

		// Token: 0x0600D160 RID: 53600 RVA: 0x004BFDEE File Offset: 0x004BDFEE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "openiddebug" };
		}

		// Token: 0x0600D161 RID: 53601 RVA: 0x004BFE00 File Offset: 0x004BE000
		public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count != 1)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Current state: {0}", OpenID.debugOpenId));
				return;
			}
			OpenID.debugOpenId = _params[0].Equals("1");
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Set OpenID debugging to {0}", _params[0].Equals("1")));
		}
	}
}
