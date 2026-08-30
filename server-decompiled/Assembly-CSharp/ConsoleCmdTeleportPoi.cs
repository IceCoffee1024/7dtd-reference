using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002AD RID: 685
[Preserve]
public class ConsoleCmdTeleportPoi : ConsoleCmdAbstract
{
	// Token: 0x060013CE RID: 5070 RVA: 0x00078AAC File Offset: 0x00076CAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "tppoi" };
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x060013CF RID: 5071 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x00078ABC File Offset: 0x00076CBC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Open POI Teleporter window";
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00078AC4 File Offset: 0x00076CC4
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
