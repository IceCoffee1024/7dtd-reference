using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001EC RID: 492
[Preserve]
public class ConsoleCmdBuffPlayer : ConsoleCmdAbstract
{
	// Token: 0x06000F34 RID: 3892 RVA: 0x00063198 File Offset: 0x00061398
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Apply a buff to a player";
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x0006319F File Offset: 0x0006139F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n   buffplayer <player name / steam id / entity id> <buff name>\nApply the given buff to the player given by the player name or entity id (as given by e.g. \"lpi\").";
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x000631A6 File Offset: 0x000613A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "buffplayer" };
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x000631B8 File Offset: 0x000613B8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 2)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid arguments, requires a target player and a buff name");
			ConsoleCmdBuff.PrintAvailableBuffNames();
			return;
		}
		string text = _params[1];
		ClientInfo clientInfo = ConsoleHelper.ParseParamIdOrName(_params[0], true, false);
		if (clientInfo != null)
		{
			clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConsoleCmdClient>().Setup("buff " + text, true));
			return;
		}
		if (_senderInfo.IsLocalGame)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Use the \"buff\" command for the local player.");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Playername or entity ID not found.");
	}
}
