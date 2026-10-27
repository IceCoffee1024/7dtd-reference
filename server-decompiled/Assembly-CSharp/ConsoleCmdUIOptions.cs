using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002BD RID: 701
[Preserve]
public class ConsoleCmdUIOptions : ConsoleCmdAbstract
{
	// Token: 0x06001410 RID: 5136 RVA: 0x0007A39C File Offset: 0x0007859C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "uioptions", "uio" };
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06001411 RID: 5137 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06001412 RID: 5138 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06001413 RID: 5139 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x0007A3B4 File Offset: 0x000785B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Allows overriding of some options that control the presentation of the UI";
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x0007A3BB File Offset: 0x000785BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\noptionsvideowindow <value> - set the options window to use for video settings\n[no parameters] - toggles video settings between simplified and detailed modes\n";
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x0007A3C4 File Offset: 0x000785C4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			UIOptions.OptionsVideoWindow = ((UIOptions.OptionsVideoWindow == OptionsVideoWindowMode.Simplified) ? OptionsVideoWindowMode.Detailed : OptionsVideoWindowMode.Simplified);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Set UIOptions.OptionsVideoWindow: {0}", UIOptions.OptionsVideoWindow));
			return;
		}
		if (_params[0].ToLowerInvariant() == "optionsvideowindow")
		{
			bool flag = false;
			if (_params.Count > 1)
			{
				OptionsVideoWindowMode optionsVideoWindowMode;
				if (EnumUtils.TryParse<OptionsVideoWindowMode>(_params[1], out optionsVideoWindowMode, true))
				{
					UIOptions.OptionsVideoWindow = optionsVideoWindowMode;
					flag = true;
				}
				else
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown window type " + _params[1]);
				}
			}
			if (!flag)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Valid values: " + string.Join<OptionsVideoWindowMode>(',', EnumUtils.Values<OptionsVideoWindowMode>()));
			}
		}
	}
}
