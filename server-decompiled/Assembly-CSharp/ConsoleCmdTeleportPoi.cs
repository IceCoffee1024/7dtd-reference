using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002B2 RID: 690
[Preserve]
public class ConsoleCmdTeleportPoi : ConsoleCmdAbstract
{
	// Token: 0x060013CF RID: 5071 RVA: 0x00078F30 File Offset: 0x00077130
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "tppoi" };
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x060013D0 RID: 5072 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00078F40 File Offset: 0x00077140
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Open POI Teleporter window";
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x00078F48 File Offset: 0x00077148
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
