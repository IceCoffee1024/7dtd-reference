using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000265 RID: 613
[Preserve]
public class ConsoleCmdPrefabEditor : ConsoleCmdAbstract
{
	// Token: 0x0600123D RID: 4669 RVA: 0x0007254E File Offset: 0x0007074E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "prefabeditor", "prefabedit", "predit" };
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x0600123E RID: 4670 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x0007256E File Offset: 0x0007076E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Open the Prefab Editor";
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x00072575 File Offset: 0x00070775
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "prefabeditor";
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x0007257C File Offset: 0x0007077C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (PrefabEditModeManager.Instance.IsActive())
		{
			Log.Out("You are already in the prefab editor.");
			return;
		}
		if (!GameManager.Instance.IsSafeToConnect())
		{
			Log.Warning("Please return to the main menu before using this command.");
			return;
		}
		GameUtils.StartSinglePrefabEditingWithMessage(null);
	}

	// Token: 0x04000D24 RID: 3364
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_connectingToPrefabEditor;
}
