using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021A RID: 538
[Preserve]
public class ConsoleCmdFov : ConsoleCmdAbstract
{
	// Token: 0x06001040 RID: 4160 RVA: 0x00067510 File Offset: 0x00065710
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "fov" };
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06001041 RID: 4161 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x00067520 File Offset: 0x00065720
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Camera field of view";
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x0003223F File Offset: 0x0003043F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x00067528 File Offset: 0x00065728
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		int num;
		if (_params.Count == 1 && int.TryParse(_params[0], out num))
		{
			GamePrefs.Set(EnumGamePrefs.OptionsGfxFOV, num);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Set FOV to " + num.ToString());
		}
	}
}
