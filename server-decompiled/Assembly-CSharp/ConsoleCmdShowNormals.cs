using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000291 RID: 657
[Preserve]
public class ConsoleCmdShowNormals : ConsoleCmdAbstract
{
	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06001335 RID: 4917 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x00076113 File Offset: 0x00074313
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "shownormals", "norms" };
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x0007612B File Offset: 0x0007432B
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Normals) ? Polarizer.ViewEnums.Normals : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("shownormals " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Normals) ? "on" : "off"));
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x00076166 File Offset: 0x00074366
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of normal maps in gBuffer";
	}
}
