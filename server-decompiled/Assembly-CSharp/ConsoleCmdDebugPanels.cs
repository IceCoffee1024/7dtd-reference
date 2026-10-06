using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200020B RID: 523
[Preserve]
public class ConsoleCmdDebugPanels : ConsoleCmdAbstract
{
	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000FDC RID: 4060 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x0006666F File Offset: 0x0006486F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugpanels" };
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x0006667F File Offset: 0x0006487F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "debugpanels - toggle display, enabling also enables the debug menu\r\ndebugpanels [names] - set these panels active and disable all others, uses the short name of the panel (e.g. debugpanels Ply Ge Sp)";
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x00066688 File Offset: 0x00064888
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

	// Token: 0x06000FE0 RID: 4064 RVA: 0x000666DD File Offset: 0x000648DD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "allows usage of debug display panels (F3 menu) via command console";
	}
}
