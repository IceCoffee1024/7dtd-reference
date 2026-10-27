using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020001F2 RID: 498
[Preserve]
public class ConsoleCmdBugReportOcclusionManager : ConsoleCmdAbstract
{
	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000F3B RID: 3899 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x00063A3D File Offset: 0x00061C3D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testoccreport", "toccr" };
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00063A55 File Offset: 0x00061C55
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Test the occlusion manager self reporting to backtrace, requires Backtrace to be enabled at build creation";
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000F3E RID: 3902 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool AllowedInMainMenu
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x00063A5C File Offset: 0x00061C5C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
		{
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("testoccreport: Command unavailable without Backtrace.");
	}
}
