using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000296 RID: 662
[Preserve]
public class ConsoleCmdShowNormals : ConsoleCmdAbstract
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06001336 RID: 4918 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x00076597 File Offset: 0x00074797
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shownormals", "norms" };
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x000765AF File Offset: 0x000747AF
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Normals) ? Polarizer.ViewEnums.Normals : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("shownormals " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Normals) ? "on" : "off"));
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x000765EA File Offset: 0x000747EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of normal maps in gBuffer";
	}
}
