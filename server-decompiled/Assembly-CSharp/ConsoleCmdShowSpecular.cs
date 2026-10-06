using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000295 RID: 661
[Preserve]
public class ConsoleCmdShowSpecular : ConsoleCmdAbstract
{
	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06001337 RID: 4919 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x00076421 File Offset: 0x00074621
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showspecular", "spec" };
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x00076439 File Offset: 0x00074639
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Specular) ? Polarizer.ViewEnums.Specular : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("showspecular " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Specular) ? "on" : "off"));
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x00076474 File Offset: 0x00074674
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of specular values in gBuffer";
	}
}
