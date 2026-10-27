using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200021A RID: 538
[Preserve]
public class ConsoleCmdFloatingOrigin : ConsoleCmdAbstract
{
	// Token: 0x06001037 RID: 4151 RVA: 0x00067467 File Offset: 0x00065667
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "floatingorigin", "fo" };
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "";
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x0600103A RID: 4154 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x0600103B RID: 4155 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x00067480 File Offset: 0x00065680
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 1 && _params[0] == "on")
		{
			if (Origin.Instance != null)
			{
				Origin.Instance.isAuto = true;
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Set floating origin to on");
			return;
		}
		if (_params.Count == 1 && _params[0] == "off")
		{
			if (Origin.Instance != null)
			{
				Origin.Instance.isAuto = false;
				if (GameManager.Instance.World != null)
				{
					Origin.Instance.Reposition(Vector3.zero);
				}
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Set floating origin to off");
			return;
		}
		if (Origin.Instance != null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Floating origin is " + (Origin.Instance.isAuto ? "on" : "off") + " and is at position " + Origin.position.ToCultureInvariantString());
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No FO instance!");
	}
}
