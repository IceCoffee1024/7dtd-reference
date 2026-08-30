using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000209 RID: 521
[Preserve]
public class ConsoleCmdDebugMenu : ConsoleCmdAbstract
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x00066624 File Offset: 0x00064824
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugmenu", "dm" };
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x0006663C File Offset: 0x0006483C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GamePrefs.Set(EnumGamePrefs.DebugMenuEnabled, !GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled));
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("debugmenu " + (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled) ? "on" : "off"));
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x00066678 File Offset: 0x00064878
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables the debugmenu ";
	}
}
