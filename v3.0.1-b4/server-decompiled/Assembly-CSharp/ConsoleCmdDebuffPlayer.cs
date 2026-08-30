using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000204 RID: 516
[Preserve]
public class ConsoleCmdDebuffPlayer : ConsoleCmdAbstract
{
	// Token: 0x06000FC7 RID: 4039 RVA: 0x00065D27 File Offset: 0x00063F27
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Remove a buff from a player";
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x00065D2E File Offset: 0x00063F2E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n   debuffplayer <player name / steam id / entity id> <buff name>\nRemove the given buff from the player given by the player name or entity id (as given by e.g. \"lpi\").";
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x00065D35 File Offset: 0x00063F35
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debuffplayer" };
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x00065D48 File Offset: 0x00063F48
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
			clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConsoleCmdClient>().Setup("debuff " + text, true));
			return;
		}
		if (_senderInfo.IsLocalGame)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Use the \"debuff\" command for the local player.");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Playername or entity ID not found.");
	}
}
