using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000297 RID: 663
[Preserve]
public class ConsoleCmdShowSpecular : ConsoleCmdAbstract
{
	// Token: 0x17000207 RID: 519
	// (get) Token: 0x0600133B RID: 4923 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x000765F1 File Offset: 0x000747F1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showspecular", "spec" };
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x00076609 File Offset: 0x00074809
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Specular) ? Polarizer.ViewEnums.Specular : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("showspecular " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Specular) ? "on" : "off"));
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x00076644 File Offset: 0x00074844
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of specular values in gBuffer";
	}
}
