using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000282 RID: 642
[Preserve]
public class ConsoleCmdSetTargetFps : ConsoleCmdAbstract
{
	// Token: 0x170001FC RID: 508
	// (get) Token: 0x060012DD RID: 4829 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x060012DE RID: 4830 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x00074D33 File Offset: 0x00072F33
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "settargetfps" };
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x00074D43 File Offset: 0x00072F43
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Set the target FPS the game should run at (upper limit)";
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x00074D4A File Offset: 0x00072F4A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Set the target FPS the game should run at (upper limit).\nUsage:\n  1. settargetfps\n  2. settargetfps <fps>\n1. gets the current target FPS.\n2. sets the target FPS to the given integer value, 0 disables the FPS limiter.";
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x00074D54 File Offset: 0x00072F54
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			int targetFrameRate = Application.targetFrameRate;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output((targetFrameRate > 0) ? string.Format("Current FPS limit is {0}", targetFrameRate) : "FPS limiter is currently disabled");
			return;
		}
		if (_params.Count != 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Wrong number of arguments, expected 0 or 1, found {0}.", _params.Count));
			return;
		}
		int num;
		if (!int.TryParse(_params[0], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("\"" + _params[0] + "\" is not a valid integer.");
			return;
		}
		if (num < 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("FPS must be >= 0");
			return;
		}
		Application.targetFrameRate = ((num == 0) ? (-1) : num);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output((num > 0) ? string.Format("Set FPS limit to {0}", num) : "Disabled target FPS limiter");
	}
}
