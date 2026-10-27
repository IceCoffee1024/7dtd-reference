using System;
using System.Collections.Generic;
using DynamicMusic;
using UnityEngine.Scripting;

// Token: 0x02000213 RID: 531
[Preserve]
public class ConsoleCmdDMS : ConsoleCmdAbstract
{
	// Token: 0x06001008 RID: 4104 RVA: 0x00066D98 File Offset: 0x00064F98
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "dms" };
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06001009 RID: 4105 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x0600100A RID: 4106 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x00066DA8 File Offset: 0x00064FA8
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

	// Token: 0x0600100C RID: 4108 RVA: 0x00066E30 File Offset: 0x00065030
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gives control over Dynamic Music functionality.";
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x00066E37 File Offset: 0x00065037
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "No commands available for dms at the moment.";
	}
}
