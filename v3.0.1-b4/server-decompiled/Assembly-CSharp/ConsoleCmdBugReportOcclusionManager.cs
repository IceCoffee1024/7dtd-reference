using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001ED RID: 493
[Preserve]
public class ConsoleCmdBugReportOcclusionManager : ConsoleCmdAbstract
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x00063241 File Offset: 0x00061441
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testoccreport", "toccr" };
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x00063259 File Offset: 0x00061459
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Test the occlusion manager self reporting to backtrace, requires Backtrace to be enabled at build creation";
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000F3C RID: 3900 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00063260 File Offset: 0x00061460
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
		{
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("testoccreport: Command unavailable without Backtrace.");
	}
}
