using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000217 RID: 535
[Preserve]
public class ConsoleCmdFov : ConsoleCmdAbstract
{
	// Token: 0x06001042 RID: 4162 RVA: 0x00066FD8 File Offset: 0x000651D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "fov" };
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x06001043 RID: 4163 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x00066FE8 File Offset: 0x000651E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Camera field of view";
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x00066FF0 File Offset: 0x000651F0
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
