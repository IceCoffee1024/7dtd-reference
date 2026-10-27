using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200026E RID: 622
[Preserve]
public class ConsoleCmdPrefabEditor : ConsoleCmdAbstract
{
	// Token: 0x06001250 RID: 4688 RVA: 0x0007302E File Offset: 0x0007122E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "prefabeditor", "prefabedit", "predit" };
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06001251 RID: 4689 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x0007304E File Offset: 0x0007124E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Open the Prefab Editor";
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x00073055 File Offset: 0x00071255
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "prefabeditor";
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x0007305C File Offset: 0x0007125C
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

	// Token: 0x04000D2F RID: 3375
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_connectingToPrefabEditor;
}
