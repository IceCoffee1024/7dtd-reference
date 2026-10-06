using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001F0 RID: 496
[Preserve]
public class ConsoleCmdBugReportOcclusionManager : ConsoleCmdAbstract
{
	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000F37 RID: 3895 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x00063779 File Offset: 0x00061979
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testoccreport", "toccr" };
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00063791 File Offset: 0x00061991
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Test the occlusion manager self reporting to backtrace, requires Backtrace to be enabled at build creation";
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000F3A RID: 3898 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x00063798 File Offset: 0x00061998
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
		{
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("testoccreport: Command unavailable without Backtrace.");
	}
}
