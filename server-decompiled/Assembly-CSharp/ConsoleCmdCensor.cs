using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001F2 RID: 498
[Preserve]
public class ConsoleCmdCensor : ConsoleCmdAbstract
{
	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000F5F RID: 3935 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x00063BA3 File Offset: 0x00061DA3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testCensor", "tcc" };
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x00063BBB File Offset: 0x00061DBB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Censorship testing toggle.";
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000F62 RID: 3938 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00063BC2 File Offset: 0x00061DC2
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
