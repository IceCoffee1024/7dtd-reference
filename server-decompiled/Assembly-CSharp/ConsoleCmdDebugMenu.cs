using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200020C RID: 524
[Preserve]
public class ConsoleCmdDebugMenu : ConsoleCmdAbstract
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000FDB RID: 4059 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x000667E4 File Offset: 0x000649E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugmenu", "dm" };
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x000667FC File Offset: 0x000649FC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GamePrefs.Set(EnumGamePrefs.DebugMenuEnabled, !GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled));
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("debugmenu " + (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) ? "on" : "off"));
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x00066838 File Offset: 0x00064A38
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables the debugmenu ";
	}
}
