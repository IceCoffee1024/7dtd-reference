using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000219 RID: 537
[Preserve]
public class ConsoleCmdFov : ConsoleCmdAbstract
{
	// Token: 0x0600104D RID: 4173 RVA: 0x00067520 File Offset: 0x00065720
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "fov" };
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x0600104E RID: 4174 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x00067530 File Offset: 0x00065730
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Camera field of view";
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x00032147 File Offset: 0x00030347
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x00067538 File Offset: 0x00065738
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
