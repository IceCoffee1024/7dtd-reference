using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001F0 RID: 496
[Preserve]
public class ConsoleCmdCensor : ConsoleCmdAbstract
{
	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x0006365B File Offset: 0x0006185B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testCensor", "tcc" };
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00063673 File Offset: 0x00061873
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Censorship testing toggle.";
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x0006367A File Offset: 0x0006187A
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
