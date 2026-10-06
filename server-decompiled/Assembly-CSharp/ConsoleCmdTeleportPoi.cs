using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002B0 RID: 688
[Preserve]
public class ConsoleCmdTeleportPoi : ConsoleCmdAbstract
{
	// Token: 0x060013CB RID: 5067 RVA: 0x00078D60 File Offset: 0x00076F60
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "tppoi" };
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x060013CC RID: 5068 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x00078D70 File Offset: 0x00076F70
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Open POI Teleporter window";
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x00078D78 File Offset: 0x00076F78
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!_senderInfo.IsLocalGame)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on game clients");
			return;
		}
		if (_params.Count == 0)
		{
			GUIWindowConsole.Close();
			LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.Open(XUiC_PoiTeleportMenu.ID, true);
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Not implemented yet");
	}
}
