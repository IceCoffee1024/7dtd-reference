using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000294 RID: 660
[Preserve]
public class ConsoleCmdShowNormals : ConsoleCmdAbstract
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06001332 RID: 4914 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x000763C7 File Offset: 0x000745C7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shownormals", "norms" };
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x000763DF File Offset: 0x000745DF
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Normals) ? Polarizer.ViewEnums.Normals : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("shownormals " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Normals) ? "on" : "off"));
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x0007641A File Offset: 0x0007461A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of normal maps in gBuffer";
	}
}
