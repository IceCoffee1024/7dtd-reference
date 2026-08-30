using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200028E RID: 654
[Preserve]
public class ConsoleCmdShowSpecular : ConsoleCmdAbstract
{
	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06001328 RID: 4904 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x00075B11 File Offset: 0x00073D11
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showspecular", "spec" };
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x00075B29 File Offset: 0x00073D29
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Specular) ? Polarizer.ViewEnums.Specular : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("showspecular " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Specular) ? "on" : "off"));
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x00075B64 File Offset: 0x00073D64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of specular values in gBuffer";
	}
}
