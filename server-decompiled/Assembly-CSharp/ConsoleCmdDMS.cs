using System;
using System.Collections.Generic;
using DynamicMusic;
using UnityEngine.Scripting;

// Token: 0x02000210 RID: 528
[Preserve]
public class ConsoleCmdDMS : ConsoleCmdAbstract
{
	// Token: 0x06001011 RID: 4113 RVA: 0x00066BD8 File Offset: 0x00064DD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "dms" };
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06001012 RID: 4114 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06001013 RID: 4115 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x00066BE8 File Offset: 0x00064DE8
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

	// Token: 0x06001015 RID: 4117 RVA: 0x00066C70 File Offset: 0x00064E70
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gives control over Dynamic Music functionality.";
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x00066C77 File Offset: 0x00064E77
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "No commands available for dms at the moment.";
	}
}
