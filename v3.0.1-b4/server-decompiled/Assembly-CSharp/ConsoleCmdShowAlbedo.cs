using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000289 RID: 649
[Preserve]
public class ConsoleCmdShowAlbedo : ConsoleCmdAbstract
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x0600130A RID: 4874 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x0600130B RID: 4875 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x00075649 File Offset: 0x00073849
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showalbedo", "albedo" };
	}

	// Token: 0x0600130D RID: 4877 RVA: 0x00075661 File Offset: 0x00073861
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Albedo) ? Polarizer.ViewEnums.Albedo : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("showalbedo " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Albedo) ? "on" : "off"));
	}

	// Token: 0x0600130E RID: 4878 RVA: 0x0007569C File Offset: 0x0007389C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of albedo in gBuffer";
	}
}
