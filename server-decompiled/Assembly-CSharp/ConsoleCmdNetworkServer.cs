using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200024B RID: 587
[Preserve]
public class ConsoleCmdNetworkServer : ConsoleCmdAbstract
{
	// Token: 0x0600118E RID: 4494 RVA: 0x0006EE46 File Offset: 0x0006D046
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "networkserver", "nets" };
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x0600118F RID: 4495 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001190 RID: 4496 RVA: 0x0006EE5E File Offset: 0x0006D05E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Server side network commands";
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x0006EE65 File Offset: 0x0006D065
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\nlatencysim <min> <max> - sets simulation in millisecs (0 min disables)\npacketlosssim <chance> - sets simulation in percent (0 - 50)";
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x0006EE6C File Offset: 0x0006D06C
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
