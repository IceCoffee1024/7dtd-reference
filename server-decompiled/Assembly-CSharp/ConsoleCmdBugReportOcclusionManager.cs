using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001EF RID: 495
[Preserve]
public class ConsoleCmdBugReportOcclusionManager : ConsoleCmdAbstract
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000F44 RID: 3908 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00063789 File Offset: 0x00061989
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testoccreport", "toccr" };
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x000637A1 File Offset: 0x000619A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Test the occlusion manager self reporting to backtrace, requires Backtrace to be enabled at build creation";
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000F47 RID: 3911 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x000637A8 File Offset: 0x000619A8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
		{
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("testoccreport: Command unavailable without Backtrace.");
	}
}
