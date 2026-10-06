using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200026C RID: 620
[Preserve]
public class ConsoleCmdPrefabEditor : ConsoleCmdAbstract
{
	// Token: 0x0600124C RID: 4684 RVA: 0x00072E5E File Offset: 0x0007105E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "prefabeditor", "prefabedit", "predit" };
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x0600124D RID: 4685 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600124E RID: 4686 RVA: 0x00072E7E File Offset: 0x0007107E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Open the Prefab Editor";
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x00072E85 File Offset: 0x00071085
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "prefabeditor";
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x00072E8C File Offset: 0x0007108C
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

	// Token: 0x04000D2E RID: 3374
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_connectingToPrefabEditor;
}
