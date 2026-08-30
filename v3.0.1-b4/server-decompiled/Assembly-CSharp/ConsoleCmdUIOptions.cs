using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002B4 RID: 692
[Preserve]
public class ConsoleCmdUIOptions : ConsoleCmdAbstract
{
	// Token: 0x060013FD RID: 5117 RVA: 0x000798BC File Offset: 0x00077ABC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "uioptions", "uio" };
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x060013FE RID: 5118 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x060013FF RID: 5119 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06001400 RID: 5120 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x000798D4 File Offset: 0x00077AD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Allows overriding of some options that control the presentation of the UI";
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x000798DB File Offset: 0x00077ADB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Commands:\noptionsvideowindow <value> - set the options window to use for video settings\n[no parameters] - toggles video settings between simplified and detailed modes\n";
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x000798E4 File Offset: 0x00077AE4
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
