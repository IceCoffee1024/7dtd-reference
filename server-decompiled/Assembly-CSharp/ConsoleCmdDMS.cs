using System;
using System.Collections.Generic;
using DynamicMusic;
using UnityEngine.Scripting;

// Token: 0x02000211 RID: 529
[Preserve]
public class ConsoleCmdDMS : ConsoleCmdAbstract
{
	// Token: 0x06001004 RID: 4100 RVA: 0x00066BC8 File Offset: 0x00064DC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "dms" };
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06001005 RID: 4101 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06001006 RID: 4102 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x00066BD8 File Offset: 0x00064DD8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count <= 0)
		{
			Log.Out("a parameter is required to run a dms command. Call 'help dms' to see the list of available parameters.");
			return;
		}
		if (!(_params[0].ToLower() == "state"))
		{
			Log.Out(string.Format("{0} is not a known parameter for 'dms'", _params[0]));
			return;
		}
		Conductor dmsConductor = GameManager.Instance.World.dmsConductor;
		if (dmsConductor != null)
		{
			Log.Out(string.Format("dms exists with current state ${0}", dmsConductor.CurrentSectionType));
			return;
		}
		Log.Out("dms does not currently exist");
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x00066C60 File Offset: 0x00064E60
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gives control over Dynamic Music functionality.";
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x00066C67 File Offset: 0x00064E67
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "No commands available for dms at the moment.";
	}
}
