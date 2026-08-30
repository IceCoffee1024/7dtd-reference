using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000247 RID: 583
[Preserve]
public class ConsoleCmdNetworkServer : ConsoleCmdAbstract
{
	// Token: 0x0600117C RID: 4476 RVA: 0x0006E7EA File Offset: 0x0006C9EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "networkserver", "nets" };
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x0600117D RID: 4477 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x0006E802 File Offset: 0x0006CA02
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Server side network commands";
	}

	// Token: 0x0600117F RID: 4479 RVA: 0x0006E809 File Offset: 0x0006CA09
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\nlatencysim <min> <max> - sets simulation in millisecs (0 min disables)\npacketlosssim <chance> - sets simulation in percent (0 - 50)";
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x0006E810 File Offset: 0x0006CA10
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		string text = _params[0].ToLower();
		if (text == "ls" || text == "latencysim")
		{
			int num = 0;
			int num2 = 100;
			if (_params.Count >= 2)
			{
				int.TryParse(_params[1], out num);
			}
			if (_params.Count >= 3)
			{
				int.TryParse(_params[2], out num2);
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SetLatencySimulation(num > 0, num, num2);
			return;
		}
		if (!(text == "pls") && !(text == "packetlosssim"))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown command " + text + ".");
			return;
		}
		int num3 = 0;
		if (_params.Count >= 2)
		{
			int.TryParse(_params[1], out num3);
		}
		if (num3 > 50)
		{
			num3 = 50;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SetPacketLossSimulation(num3 > 0, num3);
	}
}
