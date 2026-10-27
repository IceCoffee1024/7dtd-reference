using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200021C RID: 540
[Preserve]
public class ConsoleCmdFov : ConsoleCmdAbstract
{
	// Token: 0x06001044 RID: 4164 RVA: 0x000676E0 File Offset: 0x000658E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "fov" };
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06001045 RID: 4165 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x000676F0 File Offset: 0x000658F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Camera field of view";
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x000676F8 File Offset: 0x000658F8
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
