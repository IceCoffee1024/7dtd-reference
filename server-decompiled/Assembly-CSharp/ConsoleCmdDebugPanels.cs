using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200020A RID: 522
[Preserve]
public class ConsoleCmdDebugPanels : ConsoleCmdAbstract
{
	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x0006667F File Offset: 0x0006487F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugpanels" };
	}

	// Token: 0x06000FEB RID: 4075 RVA: 0x0006668F File Offset: 0x0006488F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "debugpanels - toggle display, enabling also enables the debug menu\r\ndebugpanels [names] - set these panels active and disable all others, uses the short name of the panel (e.g. debugpanels Ply Ge Sp)";
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x00066698 File Offset: 0x00064898
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		GamePrefs.Set(EnumGamePrefs.DebugMenuEnabled, true);
		NGuiWdwDebugPanels nguiWdwDebugPanels = global::UnityEngine.Object.FindAnyObjectByType<NGuiWdwDebugPanels>();
		if (nguiWdwDebugPanels == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Cannot find debug panel controller");
			return;
		}
		if (_params.Count == 0)
		{
			nguiWdwDebugPanels.ToggleDisplay();
			return;
		}
		nguiWdwDebugPanels.ShowGeneralData();
		nguiWdwDebugPanels.SetActivePanels(_params.ToArray());
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x000666ED File Offset: 0x000648ED
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "allows usage of debug display panels (F3 menu) via command console";
	}
}
