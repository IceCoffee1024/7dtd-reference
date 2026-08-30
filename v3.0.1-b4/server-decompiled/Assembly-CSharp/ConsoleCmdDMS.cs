using System;
using System.Collections.Generic;
using DynamicMusic;
using UnityEngine.Scripting;

// Token: 0x0200020E RID: 526
[Preserve]
public class ConsoleCmdDMS : ConsoleCmdAbstract
{
	// Token: 0x06001006 RID: 4102 RVA: 0x00066690 File Offset: 0x00064890
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "dms" };
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06001007 RID: 4103 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06001008 RID: 4104 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x000666A0 File Offset: 0x000648A0
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

	// Token: 0x0600100A RID: 4106 RVA: 0x00066728 File Offset: 0x00064928
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gives control over Dynamic Music functionality.";
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0006672F File Offset: 0x0006492F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "No commands available for dms at the moment.";
	}
}
