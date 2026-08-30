using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Webserver.Commands
{
	// Token: 0x02001B0C RID: 6924
	[Preserve]
	public class EnableOpenIDDebug : ConsoleCmdAbstract
	{
		// Token: 0x0600D007 RID: 53255 RVA: 0x004BB91B File Offset: 0x004B9B1B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "enable/disable OpenID debugging";
		}

		// Token: 0x0600D008 RID: 53256 RVA: 0x004BB922 File Offset: 0x004B9B22
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "openiddebug" };
		}

		// Token: 0x0600D009 RID: 53257 RVA: 0x004BB934 File Offset: 0x004B9B34
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
