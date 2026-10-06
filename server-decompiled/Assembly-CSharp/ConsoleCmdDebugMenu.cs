using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200020A RID: 522
[Preserve]
public class ConsoleCmdDebugMenu : ConsoleCmdAbstract
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x00066614 File Offset: 0x00064814
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugmenu", "dm" };
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0006662C File Offset: 0x0006482C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GamePrefs.Set(EnumGamePrefs.DebugMenuEnabled, !GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled));
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("debugmenu " + (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) ? "on" : "off"));
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x00066668 File Offset: 0x00064868
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables the debugmenu ";
	}
}
