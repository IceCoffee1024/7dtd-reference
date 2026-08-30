using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000206 RID: 518
[Preserve]
public class ConsoleCmdDebuffPlayer : ConsoleCmdAbstract
{
	// Token: 0x06000FD2 RID: 4050 RVA: 0x0006626F File Offset: 0x0006446F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Remove a buff from a player";
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x00066276 File Offset: 0x00064476
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n   debuffplayer <player name / steam id / entity id> <buff name>\nRemove the given buff from the player given by the player name or entity id (as given by e.g. \"lpi\").";
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x0006627D File Offset: 0x0006447D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debuffplayer" };
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x00066290 File Offset: 0x00064490
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
