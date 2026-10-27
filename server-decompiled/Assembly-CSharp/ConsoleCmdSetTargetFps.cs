using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200028B RID: 651
[Preserve]
public class ConsoleCmdSetTargetFps : ConsoleCmdAbstract
{
	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x060012F0 RID: 4848 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x060012F1 RID: 4849 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x00075813 File Offset: 0x00073A13
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "settargetfps" };
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x00075823 File Offset: 0x00073A23
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Set the target FPS the game should run at (upper limit)";
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0007582A File Offset: 0x00073A2A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Set the target FPS the game should run at (upper limit).\nUsage:\n  1. settargetfps\n  2. settargetfps <fps>\n1. gets the current target FPS.\n2. sets the target FPS to the given integer value, 0 disables the FPS limiter.";
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x00075834 File Offset: 0x00073A34
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
