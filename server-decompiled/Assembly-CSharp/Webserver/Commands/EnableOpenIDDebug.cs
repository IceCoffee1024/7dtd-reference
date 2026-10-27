using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Webserver.Commands
{
	// Token: 0x02001B69 RID: 7017
	[Preserve]
	public class EnableOpenIDDebug : ConsoleCmdAbstract
	{
		// Token: 0x0600D273 RID: 53875 RVA: 0x004C390F File Offset: 0x004C1B0F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "enable/disable OpenID debugging";
		}

		// Token: 0x0600D274 RID: 53876 RVA: 0x004C3916 File Offset: 0x004C1B16
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "openiddebug" };
		}

		// Token: 0x0600D275 RID: 53877 RVA: 0x004C3928 File Offset: 0x004C1B28
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
