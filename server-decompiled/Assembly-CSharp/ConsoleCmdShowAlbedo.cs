using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000290 RID: 656
[Preserve]
public class ConsoleCmdShowAlbedo : ConsoleCmdAbstract
{
	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06001319 RID: 4889 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x0600131A RID: 4890 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x00075F59 File Offset: 0x00074159
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "showalbedo", "albedo" };
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x00075F71 File Offset: 0x00074171
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		Polarizer.SetDebugView((Polarizer.GetDebugView() != Polarizer.ViewEnums.Albedo) ? Polarizer.ViewEnums.Albedo : Polarizer.ViewEnums.None);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("showalbedo " + ((Polarizer.GetDebugView() == Polarizer.ViewEnums.Albedo) ? "on" : "off"));
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00075FAC File Offset: 0x000741AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables display of albedo in gBuffer";
	}
}
