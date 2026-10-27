using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000292 RID: 658
[Preserve]
public class ConsoleCmdShowAlbedo : ConsoleCmdAbstract
{
	// Token: 0x170001FF RID: 511
	// (get) Token: 0x0600131D RID: 4893 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x0600131E RID: 4894 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00076129 File Offset: 0x00074329
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showalbedo", "albedo" };
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x00076141 File Offset: 0x00074341
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Albedo) ? Polarizer.ViewEnums.Albedo : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("showalbedo " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Albedo) ? "on" : "off"));
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x0007617C File Offset: 0x0007437C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of albedo in gBuffer";
	}
}
