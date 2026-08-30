using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A9 RID: 681
[Preserve]
public class ConsoleCmdTeleportPoi : ConsoleCmdAbstract
{
	// Token: 0x060013BC RID: 5052 RVA: 0x00078450 File Offset: 0x00076650
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "tppoi" };
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x060013BD RID: 5053 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x00078460 File Offset: 0x00076660
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Open POI Teleporter window";
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x00078468 File Offset: 0x00076668
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
