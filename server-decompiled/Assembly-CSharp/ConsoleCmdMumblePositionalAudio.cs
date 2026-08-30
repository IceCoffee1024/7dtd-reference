using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000245 RID: 581
[Preserve]
public class ConsoleCmdMumblePositionalAudio : ConsoleCmdAbstract
{
	// Token: 0x06001172 RID: 4466 RVA: 0x0006E665 File Offset: 0x0006C865
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "mumblepositionalaudio", "mpa" };
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x0006E67D File Offset: 0x0006C87D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Mumble Positional Audio related tools";
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x0006E684 File Offset: 0x0006C884
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\r\n\t\t\t|Usage:\r\n\t\t\t|  1. mpa enable\r\n\t\t\t|  2. mpa disable\r\n\t\t\t|  3. mpa reinit\r\n\t\t\t".Unindent(true);
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06001175 RID: 4469 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001176 RID: 4470 RVA: 0x0006E694 File Offset: 0x0006C894
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count != 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Wrong number of arguments, expected 1, found " + _params.Count.ToString() + ".");
			return;
		}
		if (_params[0].EqualsCaseInsensitive("enable"))
		{
			if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance != null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Positional Audio already enabled");
				return;
			}
			MumblePositionalAudio.Init();
			return;
		}
		else if (_params[0].EqualsCaseInsensitive("disable"))
		{
			if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance == null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Positional Audio already disabled");
				return;
			}
			MumblePositionalAudio.Destroy();
			return;
		}
		else if (_params[0].EqualsCaseInsensitive("reinit"))
		{
			if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance == null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Positional Audio not enabled");
				return;
			}
			SingletonMonoBehaviour<MumblePositionalAudio>.Instance.ReinitShm();
			return;
		}
		else
		{
			if (!_params[0].EqualsCaseInsensitive("uiversion"))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid subcommand");
				return;
			}
			if (SingletonMonoBehaviour<MumblePositionalAudio>.Instance == null)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Positional Audio not enabled");
				return;
			}
			SingletonMonoBehaviour<MumblePositionalAudio>.Instance.printUiVersion();
			return;
		}
	}
}
