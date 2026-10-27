using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001F5 RID: 501
[Preserve]
public class ConsoleCmdCensor : ConsoleCmdAbstract
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000F56 RID: 3926 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x00063E57 File Offset: 0x00062057
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testCensor", "tcc" };
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x00063E6F File Offset: 0x0006206F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Censorship testing toggle.";
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00063E76 File Offset: 0x00062076
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
