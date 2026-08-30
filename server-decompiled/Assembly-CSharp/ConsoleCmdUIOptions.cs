using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002B8 RID: 696
[Preserve]
public class ConsoleCmdUIOptions : ConsoleCmdAbstract
{
	// Token: 0x0600140F RID: 5135 RVA: 0x00079F18 File Offset: 0x00078118
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "uioptions", "uio" };
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06001410 RID: 5136 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06001411 RID: 5137 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06001412 RID: 5138 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x00079F30 File Offset: 0x00078130
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Allows overriding of some options that control the presentation of the UI";
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x00079F37 File Offset: 0x00078137
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\noptionsvideowindow <value> - set the options window to use for video settings\n[no parameters] - toggles video settings between simplified and detailed modes\n";
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x00079F40 File Offset: 0x00078140
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
