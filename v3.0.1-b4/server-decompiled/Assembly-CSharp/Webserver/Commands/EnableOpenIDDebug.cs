using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Webserver.Commands
{
	// Token: 0x02001AE5 RID: 6885
	[Preserve]
	public class EnableOpenIDDebug : ConsoleCmdAbstract
	{
		// Token: 0x0600CE7E RID: 52862 RVA: 0x004B5527 File Offset: 0x004B3727
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string getDescription()
		{
			return "enable/disable OpenID debugging";
		}

		// Token: 0x0600CE7F RID: 52863 RVA: 0x004B552E File Offset: 0x004B372E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string[] getCommands()
		{
			return new string[] { "openiddebug" };
		}

		// Token: 0x0600CE80 RID: 52864 RVA: 0x004B5540 File Offset: 0x004B3740
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
