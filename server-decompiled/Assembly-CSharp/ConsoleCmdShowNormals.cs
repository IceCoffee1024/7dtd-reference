using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200028D RID: 653
[Preserve]
public class ConsoleCmdShowNormals : ConsoleCmdAbstract
{
	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06001323 RID: 4899 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x00075AB7 File Offset: 0x00073CB7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shownormals", "norms" };
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x00075ACF File Offset: 0x00073CCF
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Normals) ? Polarizer.ViewEnums.Normals : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("shownormals " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Normals) ? "on" : "off"));
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x00075B0A File Offset: 0x00073D0A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of normal maps in gBuffer";
	}
}
