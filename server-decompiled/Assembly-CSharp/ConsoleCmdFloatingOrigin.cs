using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000215 RID: 533
[Preserve]
public class ConsoleCmdFloatingOrigin : ConsoleCmdAbstract
{
	// Token: 0x06001035 RID: 4149 RVA: 0x00066D5F File Offset: 0x00064F5F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "floatingorigin", "fo" };
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "";
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06001038 RID: 4152 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06001039 RID: 4153 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x00066D78 File Offset: 0x00064F78
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
