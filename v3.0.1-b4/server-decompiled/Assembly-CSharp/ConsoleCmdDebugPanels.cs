using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000208 RID: 520
[Preserve]
public class ConsoleCmdDebugPanels : ConsoleCmdAbstract
{
	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x00066137 File Offset: 0x00064337
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debugpanels" };
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x00066147 File Offset: 0x00064347
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "debugpanels - toggle display, enabling also enables the debug menu\r\ndebugpanels [names] - set these panels active and disable all others, uses the short name of the panel (e.g. debugpanels Ply Ge Sp)";
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x00066150 File Offset: 0x00064350
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

	// Token: 0x06000FE2 RID: 4066 RVA: 0x000661A5 File Offset: 0x000643A5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "allows usage of debug display panels (F3 menu) via command console";
	}
}
