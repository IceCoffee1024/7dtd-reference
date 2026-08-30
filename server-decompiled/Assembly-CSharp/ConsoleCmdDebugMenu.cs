using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000207 RID: 519
[Preserve]
public class ConsoleCmdDebugMenu : ConsoleCmdAbstract
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x000660DC File Offset: 0x000642DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugmenu", "dm" };
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x000660F4 File Offset: 0x000642F4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GamePrefs.Set(EnumGamePrefs.DebugMenuEnabled, !GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled));
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("debugmenu " + (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) ? "on" : "off"));
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x00066130 File Offset: 0x00064330
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables the debugmenu ";
	}
}
