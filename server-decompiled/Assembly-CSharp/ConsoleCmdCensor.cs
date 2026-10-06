using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001F3 RID: 499
[Preserve]
public class ConsoleCmdCensor : ConsoleCmdAbstract
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000F52 RID: 3922 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x00063B93 File Offset: 0x00061D93
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testCensor", "tcc" };
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x00063BAB File Offset: 0x00061DAB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Censorship testing toggle.";
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000F55 RID: 3925 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00063BB2 File Offset: 0x00061DB2
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
		{
			return;
		}
		if (_params.Count == 0)
		{
			GameManager.DebugCensorship = !GameManager.DebugCensorship;
			Log.Out("Censor testing enabled: " + GameManager.DebugCensorship.ToString());
			return;
		}
	}
}
